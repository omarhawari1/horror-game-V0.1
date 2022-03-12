using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactables : MonoBehaviour
{
    [SerializeField]private int rayLength = 5;
    [SerializeField]private LayerMask layerMaskInteract;

    private doorController doorController;

    [SerializeField]private KeyCode interactKey = KeyCode.E;
    [SerializeField]private GameObject crosshair = null;

    private void Update() 
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, rayLength, layerMaskInteract))
        {
            crosshair.SetActive(true);
            if(Input.GetKeyDown(interactKey))
            {
                switch(hit.collider.tag)
                {
                    case "door":
                        hit.collider.transform.parent.gameObject.GetComponent<doorController>().playAnim();
                        break;
                }
            }
        }
        else
        {
            crosshair.SetActive(false);
        }
    }
}
