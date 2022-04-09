using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndController : MonoBehaviour
{
    [SerializeField] TMP_Text Score;
    [SerializeField] TMP_Text HighScore;

    public void setTitleTo()
    {

        Score.text = "Score: " + PlayerPrefs.GetInt("Score").ToString();
        HighScore.text = "HighScore: " + PlayerPrefs.GetInt("HighScore").ToString();

    }
}
