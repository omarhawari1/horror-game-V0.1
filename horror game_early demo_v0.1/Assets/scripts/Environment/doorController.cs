using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorController : MonoBehaviour
{
    private Animator doorAnim;
    private bool doorOpen = false;


    private void Awake() 
    {
        doorAnim = gameObject.GetComponent<Animator>();
    }

    public void playAnim()
    {
        if(!doorOpen)
        {
            doorAnim.Play("doorOpen", 0, 0f);
            doorOpen = true;
        }
        else
        {
            doorAnim.Play("doorClose", 0, 0f);
            doorOpen = false;
        }
    }
}
