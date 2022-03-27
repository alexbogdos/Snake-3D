using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessingSetter : MonoBehaviour
{
    [SerializeField] GameObject PostProcessing;

    void Awake()
    {
        int fp = 0;


        if (PlayerPrefs.HasKey("PostProcessing"))
        {
            fp = PlayerPrefs.GetInt("PostProcessing");
        }
        else
        {
            PlayerPrefs.SetInt("PostProcessing", 0);
        }

        if (fp == 1)
        {
            PostProcessing.SetActive(true);
        }
        else if (fp == 0)
        {
            PostProcessing.SetActive(false);
        }
    }
}
