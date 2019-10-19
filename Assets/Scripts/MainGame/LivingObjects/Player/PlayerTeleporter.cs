using MainGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    public float delay = 1f;

    public MapGenerator mapGenerator;

    public Animator isometricAnimator;

    private float sinkTime;
    private float riseUpTime;

    private bool isTeleporting;

    private void Start()
    {
        mapGenerator = GameObject.FindObjectOfType<MapGenerator>();
        isometricAnimator = GameObject.FindGameObjectWithTag("ModelPlayer").GetComponentInChildren<Animator>();

    }

    private IEnumerator SmoothTeleport(GameObject player)
    {
        if (isometricAnimator == null)
        {
            Debug.LogError("isometric animator could no be found");
        }

        AnimationClip[] clips = isometricAnimator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            switch (clip.name)
            {
                case "ModelSink": sinkTime = clip.length; break;
                case "ModelRiseUp": riseUpTime = clip.length; break;
            }
        }

        isometricAnimator.SetTrigger("isSink");

        //When the sink animation clip is over, deactive the player
        yield return new WaitForSeconds(sinkTime);

        gameObject.SetActive(false);

        mapGenerator.LayoutObjectAtRandom(player.transform);

        //yield return new WaitForSeconds(delay);

        gameObject.SetActive(true);
        //Start the rise up aniamtion clip
        isometricAnimator.SetTrigger("isRiseUp");

        //When the riseup aniamtion clip is over,set then handle input to true
        yield return new WaitForSeconds(riseUpTime);

        //Set isTeleporting is false
        isTeleporting = false;
    }

    public void Teleport(GameObject player)
    {
        if (player == null) return;
        isTeleporting = true;
        StartCoroutine(SmoothTeleport(player));
    }

    public bool GetState()
    {
        return isTeleporting;
    }
}
