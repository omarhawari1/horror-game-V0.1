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
    [SerializeField]private GameObject flashLight;
    [SerializeField]private Transform playerCamera;

    [Header("Audio:")]
    [SerializeField]private bool useFootSteps = true;

    [Header("footsteps: ")]
    [SerializeField]private float baseStepSpeed = 0.5f;
    [SerializeField]private float sprintStepMultiplier = 0.6f;
    [SerializeField]private AudioSource footstepSource = default;
    [SerializeField]private AudioClip[] footstepAudio = default;
    private float footStepTimer = 0;
    private float GetCurrentOffset => isSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;


    //private
    private float mouseX;
    private float mouseY;
    private float horizontal;
    private float vertical;
    private float xRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;
    private bool flashLightState = false;
    private bool isSprinting;


    //public

    private void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() 
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //flashLight:
        if(Input.GetKeyDown(KeyCode.F) && flashLightState == false)
        {
            flashLight.SetActive(true);
            flashLightState = true;
        }
        else if(Input.GetKeyDown(KeyCode.F) && flashLightState == true)
        {
            flashLight.SetActive(false);
            flashLightState = false;
        }

        if(useFootSteps)
        {
            handle_Footsteps();
        }

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
            isSprinting = true;
        }
        else
        {
            speed = normalSpeed;
            isSprinting = false;
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

    private void handle_Footsteps()
    {
        if(!isGrounded) return;
        if(horizontal == 0 & vertical == 0) return;

        footStepTimer -= Time.deltaTime;

        if(footStepTimer <= 0)
        {
            if(Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                switch(hit.collider.tag)
                {
                    case "footsteps/Concrete":
                    footstepSource.PlayOneShot(footstepAudio[Random.Range(0, footstepAudio.Length -1)]);
                        break;
                    default:
                    footstepSource.PlayOneShot(footstepAudio[Random.Range(0, footstepAudio.Length -1)]);
                        break;
                }
            }
            footStepTimer = GetCurrentOffset;
        }
    }

    private void inputs()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }
}
