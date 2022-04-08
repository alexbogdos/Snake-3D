using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : MonoBehaviour
{
    [SerializeField] CounterController counterController;
    [SerializeField] SnakeController snakeController;
    [SerializeField] GameObject ApplePrefab;
    [SerializeField] float audioDelay;

    AudioSource source;
    List<GameObject> Apples = new List<GameObject>();
    List<bool> Destroyed = new List<bool>();
    public int applesEaten = 0;
    int remainingApples = 0;

    void Start()
    {
        source = GetComponent<AudioSource>();
        SpawnApples();
    }

    void SpawnApples()
    {
        remainingApples = 3;

        Vector3 snakeTargetPosition = snakeController.targetPosition;
        List<List<(int, int)>> availableZones = new List<List<(int, int)>> {
            new List<(int, int)> {
                (-7,7), (-5,7), (-3,7), (-1,7),
                (-7,5), (-5,5), (-3,5), (-1,5),
                (-7,3), (-5,3), (-3,3), (-1,3),
                (-7,1), (-5,1), (-3,1), (-1,1)
            },
            new List<(int, int)> {
                (-7,-1), (-5,-1), (-3,-1), (-1,-1),
                (-7,-3), (-5,-3), (-3,-3), (-1,-3),
                (-7,-5), (-5,-5), (-3,-5), (-1,-5),
                (-7,-7), (-5,-7), (-3,-7), (-1,-7)
            },
            new List<(int, int)> {
                (7,7), (5,7), (3,7), (1,7),
                (7,5), (5,5), (3,5), (1,5),
                (7,3), (5,3), (3,3), (1,3),
                (7,1), (5,1), (3,1), (1,1)
            },
            new List<(int, int)> {
                (7,-7), (5,-7), (3,-7), (1,-7),
                (7,-5), (5,-5), (3,-5), (1,-5),
                (7,-3), (5,-3), (3,-3), (1,-3),
                (7,-1), (5,-1), (3,-1), (1,-1)
            }
         };

        int zone = checkCurrentZone(snakeTargetPosition);
        availableZones.RemoveAt(zone - 1);

        List<Vector3> spawnPositions = new List<Vector3>();
        for (int i = 1; i <= 3; i++)
        {
            System.Random rand = new System.Random();
            int zoneIndex = rand.Next(availableZones.Count - 1);
            List<(int, int)> positions = availableZones[zoneIndex];
            int positionIndex = rand.Next(positions.Count - 1);
            Vector3 position = new Vector3(x: positions[positionIndex].Item1, y: 0.3f, z: positions[positionIndex].Item2);
            spawnPositions.Add(position);

            availableZones.RemoveAt(zoneIndex);
        }

        foreach (Vector3 position in spawnPositions)
        {
            GameObject appleInstance = Instantiate(ApplePrefab, position, ApplePrefab.transform.rotation);
            Apples.Add(appleInstance);
            Destroyed.Add(false);
        }
    }

    int checkCurrentZone(Vector3 position)
    {
        int zone;
        bool inLeftZones = position.x < 0;
        bool inTopZones = position.z > 0;

        if (inLeftZones == true && inTopZones == true)
        {
            zone = 1;
        }
        else if (inLeftZones == true && inTopZones == false)
        {
            zone = 2;
        }
        else if (inLeftZones == false && inTopZones == true)
        {
            zone = 3;
        }
        else
        {
            zone = 4;
        }

        return zone;
    }


    public bool checkIfInPosition(Vector3 position)
    {
        int index = 0;
        foreach (var apple in Apples)
        {
            if (Destroyed[index] == false)
            {
                bool inX = apple.transform.position.x - 0.1 <= position.x && position.x <= apple.transform.position.x + 0.1;
                bool inZ = apple.transform.position.z - 0.1 <= position.z && position.z <= apple.transform.position.z + 0.1;

                if (inX && inZ)
                {
                    apple.GetComponent<Animator>().SetTrigger("Eaten");
                    Destroyed[index] = true;
                    applesEaten++;

                    counterController.SetCount(applesEaten.ToString());
                    source.PlayDelayed(audioDelay);

                    remainingApples--;
                    if (remainingApples == 0) 
                    {
                        SpawnApples();
                    }

                    StartCoroutine(DestroyApple(index, apple));

                    return (true);
                }
            }
            index++;
        }
        return (false);
    }

    IEnumerator DestroyApple(int index, GameObject apple)
    {
        yield return new WaitForSeconds(1f);
        Apples.RemoveAt(index);
        Destroyed.RemoveAt(index);
        Destroy(apple);
    }
}
