using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndController : MonoBehaviour
{
    [SerializeField] TMP_Text title;
    [SerializeField] GameObject buttonContinue;
    [SerializeField] GameObject buttonRestart;

    public void setTitleTo()
    {

        title.text = "You Lost..";
        buttonContinue.SetActive(false);
        buttonRestart.SetActive(true);

    }
}
