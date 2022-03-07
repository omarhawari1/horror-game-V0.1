using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_main : MonoBehaviour
{
    //SerializeField
    [SerializeField]private float mouseSens;
    [SerializeField]private float speed;
    [SerializeField]private float sprintSpeed;
    [SerializeField]private float normalSpeed;
    [SerializeField]private CharacterController playerController;
    [SerializeField]private float crouchSize;
    [SerializeField]private float normalSize;
    [SerializeField]private float gravityValue;
    [SerializeField]private Transform groundCheck;
    [SerializeField]private float groundDistance;
    [SerializeField]private LayerMask groundMask;
    [SerializeField]private float uncrouchSpeed;
    [SerializeField]private float crouchSpeed;


    //private
    private float mouseX;
    private float mouseY;
    private float horizontal;
    private float vertical;
    private float xRotation = 0f;
    private Transform playerCamera;
    private Vector3 velocity;
    private bool isGrounded;


    //public

    private void Start() 
    {
        playerCamera = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() 
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        inputs();
        mouseLook();
        gravity();
        movement();
    }

    private void movement()
    {
        //wasd movement:
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        playerController.Move(move * speed * Time.deltaTime);

        //sprint:
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = normalSpeed;
        }


        //crouch:
        if(Input.GetKey(KeyCode.LeftControl))
        {
            playerController.height = crouchSize;
            speed = crouchSpeed;
        }
        else if(!Input.GetKey(KeyCode.LeftControl) && playerController.height >= crouchSize && playerController.height <= normalSize)
        {
            playerController.height += uncrouchSpeed;
            speed = normalSpeed;
        }
    }
    private void gravity()
    {
        //gravity
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravityValue * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);
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
