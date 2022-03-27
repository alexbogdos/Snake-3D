using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject MainPanel;
    [SerializeField] GameObject SettingsPanel;
    [SerializeField] Toggle toggle;

    public void GoToPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToSettings()
    {
        MainPanel.SetActive(false);
        SettingsPanel.SetActive(true);

        if (PlayerPrefs.HasKey("FirstPerson"))
        {
            toggle.isOn = PlayerPrefs.GetInt("FirstPerson") == 1;
        }
        else
        {
            toggle.isOn = false;
        }

    }

    public void GoToMain()
    {
        SettingsPanel.SetActive(false);
        MainPanel.SetActive(true);
    }

    public void ChangeValue(bool fp)
    {
        if (fp == false)
        {
            PlayerPrefs.SetInt("FirstPerson", 0);
        }
        else
        {
            PlayerPrefs.SetInt("FirstPerson", 1);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
