using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_main : MonoBehaviour
{
    //SerializeField
    [SerializeField]private float mouseSens;
    [SerializeField]private float speed;
    [SerializeField]private float sprintSpeed;
    [SerializeField]private CharacterController playerController;
    [SerializeField]private GameObject playerModel;
    [SerializeField]private float crouchSize;
    [SerializeField]private float normalSize;


    //private
    private float mouseX;
    private float mouseY;
    private float horizontal;
    private float vertical;
    private float xRotation = 0f;
    private Transform playerCamera;


    //public

    private void Start() 
    {
        playerCamera = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() 
    {
        inputs();
        mouseLook();
        movement();

    }

    private void movement()
    {
        //wasd movement:
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        //check if sprint:
        if(Input.GetKey(KeyCode.LeftShift))
        {
            playerController.Move(move * sprintSpeed * Time.deltaTime);
        }
        else
        {
            playerController.Move(move * speed * Time.deltaTime);
        }


        //crouch:
        if(Input.GetKey(KeyCode.LeftControl))
        {
            playerController.height = crouchSize;
        }
        else
        {
            playerController.height = normalSize;
        }
    }

    private void mouseLook()
    {
        //rotate x-axis:
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);


        //rotate y-axis:
        transform.Rotate(Vector3.up * mouseX);
    }

    private void inputs()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }
}
