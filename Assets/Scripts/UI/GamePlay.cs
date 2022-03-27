using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject ButtonPaused;
    [SerializeField] GameObject ButtonUnpaused;
    bool paused = false;

    public void PauseGame()
    {
        if (Time.timeScale == 0) {
            return;
        }

        ButtonUnpaused.SetActive(false);
        ButtonPaused.SetActive(true);
        PausePanel.SetActive(true);

        paused = true;
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        ButtonPaused.SetActive(false);
        ButtonUnpaused.SetActive(true);
        PausePanel.SetActive(false);

        paused = false;
        Time.timeScale = 1f;
    }

    public void SetButtonAsPaused()
    {
        ButtonUnpaused.SetActive(false);
        ButtonPaused.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused == false)
            {
                PauseGame();
            }
            else
            {
                UnpauseGame();
            }
        }
    }
}
