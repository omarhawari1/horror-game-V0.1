using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class eventsManager : MonoBehaviour
{
    [Header("dvdPlayer")]
    [SerializeField]private GameObject TV;
    [Header("camera: ")]
    [SerializeField]private player_main player;
    [SerializeField]private float cameraRotSpeed;
    [SerializeField]private Transform cameraZoom;

    private bool lookAt = false;
    private float lastTime;

    public void dvdPlayer()
    {
        TV.SetActive(true);
        player.canMove = false;
        player.canLook = false;
        player.canUseHeadBob = false;
        player.useFootSteps = false;
        lookAt = true;

    }

    private void Update() 
    {
        if(lookAt)
        {
            Vector3 dir = TV.transform.position - Camera.main.transform.position;
            dir.y = 0f;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, lookRot, 0.002f * lastTime);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraZoom.position, 0.002f * lastTime);
            lastTime += Time.deltaTime;

            StartCoroutine(changeScene());
        }
    }

    private IEnumerator changeScene()
    {
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene("caveScene");
    }
}
