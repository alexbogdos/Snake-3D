using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonSetter : MonoBehaviour
{
    [SerializeField] SnakeController snakeController;
    [SerializeField] GameObject fpCamera;
    [SerializeField] GameObject tpCamera;
    void Awake()
    {
        int fp = 0;


        if (PlayerPrefs.HasKey("FirstPerson"))
        {
            fp = PlayerPrefs.GetInt("FirstPerson");
        }
        else
        {
            PlayerPrefs.SetInt("FirstPerson", 0);
        }

        if (fp == 1)
        {
            tpCamera.SetActive(false);
            fpCamera.SetActive(true);
            snakeController.firstPersonMode = true;
        }
        else if (fp == 0)
        {
            fpCamera.SetActive(false);
            tpCamera.SetActive(true);
            snakeController.firstPersonMode = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int fp = PlayerPrefs.GetInt("FirstPerson");

            if (fp == 1)
            {
                PlayerPrefs.SetInt("FirstPerson", 0);

                fpCamera.SetActive(false);
                tpCamera.SetActive(true);
                snakeController.firstPersonMode = false;
            }
            else if (fp == 0)
            {
                PlayerPrefs.SetInt("FirstPerson", 1);

                tpCamera.SetActive(false);
                fpCamera.SetActive(true);
                snakeController.firstPersonMode = true;
            }
        }
    }
}
