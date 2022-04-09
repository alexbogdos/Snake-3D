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
        System.Random rand = new System.Random();
        int rate = rand.Next(minValue: 2, maxValue: 6);
        remainingApples += rate;

        for (int i = 1; i <= rate; i++)
        {
            bool isUsed;
            bool isNearOthers = false;
            int zoneIndex;
            Vector3 position;
            int positionIndex;
            zoneIndex = rand.Next(availableZones.Count);
            List<(int, int)> positions = availableZones[zoneIndex];

            do
            {
                positionIndex = rand.Next(positions.Count - 1);
                position = new Vector3(x: positions[positionIndex].Item1, y: 0.3f, z: positions[positionIndex].Item2);

                isUsed = checkIfUsed(position);

                isNearOthers = checkIfNearOthers(position, spawnPositions);


            } while (!(isUsed == false && isNearOthers == false));

            spawnPositions.Add(position);
            availableZones[zoneIndex].RemoveAt(positionIndex);
        }

        foreach (Vector3 position in spawnPositions)
        {
            GameObject appleInstance = Instantiate(ApplePrefab, position, ApplePrefab.transform.rotation);
            Apples.Add(appleInstance);
            Destroyed.Add(false);
        }
    }

    bool checkIfNearOthers(Vector3 position, List<Vector3> list)
    {
        bool isNearOthers = false;
        int _index = 0;
        while (isNearOthers == false && _index < list.Count)
        {
            Vector3 pos = list[_index];

            Vector3 left = new Vector3(x: position.x - 2, z: position.z, y: position.y);
            Vector3 right = new Vector3(x: position.x + 2, z: position.z, y: position.y);
            Vector3 top = new Vector3(x: position.x, z: position.z + 2, y: position.y);
            Vector3 bottom = new Vector3(x: position.x, z: position.z - 2, y: position.y);

            if (pos == left || pos == right || pos == top || pos == bottom)
            {
                isNearOthers = true;
            }

            _index++;
        }

        if (isNearOthers == true)
        {
            return isNearOthers;
        }


        _index = 0;
        while (isNearOthers == false && _index < Apples.Count)
        {
            if (Destroyed[_index] == false)
            {
                Vector3 pos = Apples[_index].transform.position;

                Vector3 left = new Vector3(x: position.x - 2, z: position.z, y: position.y);
                Vector3 right = new Vector3(x: position.x + 2, z: position.z, y: position.y);
                Vector3 top = new Vector3(x: position.x, z: position.z + 2, y: position.y);
                Vector3 bottom = new Vector3(x: position.x, z: position.z - 2, y: position.y);

                if (pos == left || pos == right || pos == top || pos == bottom)
                {
                    isNearOthers = true;
                }
            }

            _index++;
        }

        return isNearOthers;
    }
    bool checkIfUsed(Vector3 position)
    {
        bool isUsed = false;
        int _index = 0;
        while (isUsed == false && _index < Apples.Count)
        {
            Vector3 pos = Apples[_index].transform.position;
            if (position == pos)
            {
                isUsed = true;
            }
            _index++;
        }
        return isUsed;
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


    public void saveData()
    {
        PlayerPrefs.SetInt("Score", applesEaten);

        if (PlayerPrefs.HasKey("HighScore"))
        {
            int highScore = PlayerPrefs.GetInt("HighScore");
            if (applesEaten > highScore)
            {
                PlayerPrefs.SetInt("HighScore", applesEaten);
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", applesEaten);
        }
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
                    StartCoroutine(DestroyEatenApples());

                    if (remainingApples == 1)
                    {
                        System.Random rand = new System.Random();
                        int posibility = rand.Next(10);
                        if (posibility <= 4)
                        {
                            SpawnApples();
                        }
                    }

                    if (remainingApples == 0)
                    {
                        SpawnApples();
                    }

                    return (true);
                }
            }
            index++;
        }
        return (false);
    }

    IEnumerator DestroyEatenApples()
    {
        yield return new WaitForSeconds(0.5f);
        for (int index = 0; index < Destroyed.Count; index++)
        {
            if (Destroyed[index] == true)
            {
                GameObject apple = Apples[index];
                Apples.RemoveAt(index);
                Destroyed.RemoveAt(index);
                Destroy(apple);
            }
        }
    }
}
