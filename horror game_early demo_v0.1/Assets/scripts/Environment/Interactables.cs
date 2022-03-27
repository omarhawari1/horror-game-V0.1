using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactables : MonoBehaviour
{
    [SerializeField]private float rayLength = 5;
    [SerializeField]private float interactRadius;
    [SerializeField]private LayerMask layerMaskInteract;

    private doorController doorController;

    [SerializeField]private KeyCode interactKey = KeyCode.E;
    [SerializeField]private GameObject crosshair = null;

    [Header("flashlight: ")]
    [SerializeField]private GameObject flashlight;
    [Header("eventsManager: ")]
    [SerializeField] private eventsManager eventsManager;

    private void Update() 
    {
        if(Physics.SphereCast(transform.position, interactRadius, transform.TransformDirection(Vector3.forward), out RaycastHit hit, rayLength, layerMaskInteract))
        {
            crosshair.SetActive(true);
            if(Input.GetKeyDown(interactKey))
            {
                switch(hit.collider.tag)
                {
                    case "door":
                        hit.collider.transform.parent.gameObject.GetComponent<doorController>().playAnim();
                        break;
                    case "flashlight":
                        hit.collider.gameObject.SetActive(false);
                        gameObject.GetComponent<player_main>().canUseFlashlight = true;
                        gameObject.GetComponent<player_main>().flashLightState = true;
                        flashlight.SetActive(true);
                        break;
                    case "paper":
                        hit.collider.gameObject.SetActive(false);
                        break;
                    case "dvdPlayer":
                        eventsManager.dvdPlayer();
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
