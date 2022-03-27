using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject MainPanel;
    [SerializeField] GameObject SettingsPanel;
    
    [SerializeField] Toggle toggleFPV;
    [SerializeField] Toggle toggleHUD;
    [SerializeField] Toggle togglePostPrc;

    public void GoToPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToSettings()
    {
        MainPanel.SetActive(false);
        SettingsPanel.SetActive(true);

        setToggleValue(toggleFPV, "FirstPerson");
        setToggleValue(toggleHUD, "HUDButtons");
        setToggleValue(togglePostPrc, "PostProcessing");

    }

  

    void setToggleValue(Toggle toggle, string prefsKey) 
    {
        if (PlayerPrefs.HasKey(prefsKey))
        {
            toggle.isOn = PlayerPrefs.GetInt(prefsKey) == 1;
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

    public void ChangeFPVvalue(bool fp)
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

    public void ChangeHUDvalue(bool fp)
    {
        if (fp == false)
        {
            PlayerPrefs.SetInt("HUDButtons", 0);
        }
        else
        {
            PlayerPrefs.SetInt("HUDButtons", 1);
        }
    }

    public void ChangePostProcessingvalue(bool fp)
    {
        if (fp == false)
        {
            PlayerPrefs.SetInt("PostProcessing", 0);
        }
        else
        {
            PlayerPrefs.SetInt("PostProcessing", 1);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
