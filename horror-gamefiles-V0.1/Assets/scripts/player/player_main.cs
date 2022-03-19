using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class player_main : MonoBehaviour
{
    [Header("functional options: ")]
    [SerializeField]private bool canUseHeadBob;
    [SerializeField]private bool canCrouch;
    [SerializeField]private bool canSprint;
    [SerializeField]private bool canUseFlashlight;
    [SerializeField]private bool useFootSteps;
    [SerializeField]private bool canPause;
    [SerializeField]private bool canUseTime;

    [Header("player settings:")]
    public float mouseXSens;
    public float mouseYSens;
    [SerializeField]private float normalSpeed;
    [SerializeField]private float sprintSpeed;
    [SerializeField]private float crouchSpeed;
    [SerializeField]private float crouchSize;
    [SerializeField]private float normalSize;
    [SerializeField]private float uncrouchSpeed;
    [SerializeField]private float gravityValue;

    [Header("controls: ")]
    [SerializeField]private KeyCode K_flashlight;
    [SerializeField]private KeyCode k_sprint;
    [SerializeField]private KeyCode k_pause;

    [Header("set componenets: ")]
    [SerializeField]private GameObject flashLight;
    [SerializeField]private CharacterController playerController;
    [SerializeField]private GameObject pauseMenu;
    [SerializeField]private GameObject settingsMenu;
    [SerializeField]private TMP_Text time;

    [Header("footsteps: ")]
    [SerializeField]private float baseStepSpeed;
    [SerializeField]private float sprintStepSpeed;
    [SerializeField]private AudioSource footstepSource = default;
    [SerializeField]private AudioClip[] footstepAudio = default;
    private float footStepTimer = 0;
    private float GetCurrentOffset => isSprinting ? sprintStepSpeed : baseStepSpeed;

    [Header("headBob: ")]
    [SerializeField]private float walkBobSpeed;
    [SerializeField]private float walkBobAmount;
    [SerializeField]private float sprintBobSpeed;
    [SerializeField]private float sprintBobAmount;

    private float defaultYpos = 0;
    private float timer;
    private float mouseX;
    private float mouseY;
    private float horizontal;
    private float vertical;
    private float xRotation = 0f;
    private Vector3 velocity;
    private bool flashLightState = false;
    private bool isSprinting;
    private Transform playerCamera;
    private float speed;
    private float startTime;
    private float seconds;
    private float minutes;
    private float days;
    private bool started;


    //public
    [HideInInspector]
    public bool paused;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCamera = Camera.main.transform;
        defaultYpos = playerCamera.transform.localPosition.y;

        //time
        startTime = Time.time;
        started = true;
    }

    private void Update() 
    {

        if(useFootSteps)
        {
            handle_Footsteps();
        }
        if(canUseHeadBob)
        {
            handle_headBob();
        }
        if(canCrouch)
        {
            handle_Crouch();
        }
        if(canUseFlashlight)
        {
            handle_Flashlight();
        }
        if(canSprint)
        {
            handle_Sprint();
        }
        if(canPause)
        {
            handle_pauseMenu();
        }
        if(canUseTime && started)
        {
            handle_Time();
        }
        gravity();
        mouseLook();
        movement();
        inputs();
    }

    private void movement()
    {
        //wasd movement:
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        playerController.Move(move * speed * Time.deltaTime);

    }
    private void gravity()
    {
        //gravity
        if(playerController.isGrounded && velocity.y < 0)
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
        if(!playerController.isGrounded) return;
        if(horizontal == 0 && vertical == 0) return;


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
    private void handle_headBob()
    {
        if(!playerController.isGrounded) return;
        if(horizontal == 0 & vertical == 0) return;

        timer += Time.deltaTime * (isSprinting ? sprintBobSpeed : walkBobSpeed);
        playerCamera.transform.localPosition = new Vector3(
            playerCamera.transform.localPosition.x, 
            defaultYpos + Mathf.Sin(timer) * (isSprinting ? sprintBobAmount : walkBobAmount),
            playerCamera.transform.localPosition.z
        );
    }
    private void handle_Crouch()
    {
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
    private void handle_Sprint()
    {
        //sprint:
        if(Input.GetKey(k_sprint))
        {
            speed = sprintSpeed;
            isSprinting = true;
        }
        else
        {
            speed = normalSpeed;
            isSprinting = false;
        }
    }
    private void handle_Flashlight()
    {
        //flashLight:
        if(Input.GetKeyDown(K_flashlight) && flashLightState == false)
        {
            flashLight.SetActive(true);
            flashLightState = true;
        }
        else if(Input.GetKeyDown(KeyCode.F) && flashLightState == true)
        {
            flashLight.SetActive(false);
            flashLightState = false;
        }
    }
    private void handle_pauseMenu()
    {
        //pause
        if(Input.GetKeyDown(k_pause) && !paused)
        {
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            paused = true;
        }
        //unpause
        else if(Input.GetKeyDown(k_pause) && paused)
        {
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            paused = false;
        }
    }
    private void handle_Time()
    {
        seconds = Time.time - startTime;
        seconds = Mathf.Round(seconds);
        if(seconds >= 60)
        {
            minutes += 1;
            seconds = 0;
            startTime = Time.time;
        }
        if(minutes >= 60)
        {
            days += 1;
            minutes = 0;
        }
        time.text = days.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    private void inputs()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseXSens * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseYSens * Time.deltaTime;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }
}
