using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDButtonsSetter : MonoBehaviour
{
    [SerializeField] GameObject ButtonsPanel;

    void Awake()
    {
        int fp = 0;


        if (PlayerPrefs.HasKey("HUDButtons"))
        {
            fp = PlayerPrefs.GetInt("HUDButtons");
        }
        else
        {
            PlayerPrefs.SetInt("HUDButtons", 0);
        }

        if (fp == 1)
        {
           ButtonsPanel.SetActive(true);
        }
        else if (fp == 0)
        {
            ButtonsPanel.SetActive(false);
        }
    }
}
