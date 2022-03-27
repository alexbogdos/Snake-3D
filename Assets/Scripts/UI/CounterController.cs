using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CounterController : MonoBehaviour
{
    [SerializeField] TMP_Text count;
    [SerializeField] Animator animator;
    
    public void SetCount(string _count) 
    {
        count.text = _count;
        animator.SetTrigger("changed");
    }
}
