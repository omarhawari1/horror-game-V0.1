using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class lerpValue : MonoBehaviour
{
    [SerializeField]private RawImage color;
    [SerializeField]private float duration;
    [SerializeField]private float timeToStart;
    [SerializeField]private GameObject Object;
    [SerializeField]private player_main player;
    [SerializeField]private VideoPlayer videoPlayer;

    private void Start() 
    {
        player.canMove = false;
        player.canLook = false;
        player.canUseHeadBob = false;
        player.useFootSteps = false;
    }
    private void Update()
    {
        if(Time.timeSinceLevelLoad >= timeToStart)
        {
            color.color = Color.Lerp(color.color, new Color(color.color.r, color.color.g, color.color.b, 0), duration * Time.deltaTime);
            videoPlayer.SetDirectAudioVolume(0, color.color.a / 100);
            if(color.color.a <= 0.05f)
            {
                player.canMove = true;
                player.canLook = true;
                player.canUseHeadBob = true;
                player.useFootSteps = true;
                Object.SetActive(false);
            }
        }
    }
}
