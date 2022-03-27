using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : MonoBehaviour
{
    [SerializeField] CounterController counterController;
    List<GameObject> Apples = new List<GameObject>();
    List<bool> Destroyed = new List<bool>();
    int applesEaten = 0;

    void Start()
    {
        Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts)
        {
            if (t != null && t.gameObject != null && t.gameObject.tag == "Apple")
            {
                Apples.Add(t.gameObject);
                Destroyed.Add(false);
            }
        }
    }

    public (bool, bool) checkIfInPosition(Vector3 position)
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
                    return (true, applesEaten == Apples.Count);
                }
            }
            index++;
        }
        return (false, applesEaten == Apples.Count);
    }
}
