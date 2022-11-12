using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform camTargetrot;
    public Transform middleCamTarget;
    public Transform camTargetpos;
    private float pLerp = 0.1f;
    public float rLerp = 0.01f;

    private GameManager manager;

    public bool lockPlayer = false;
    public GameObject player;
    public Movement_controller move_cont;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        if (!move_cont.follow)
        {
            transform.position = Vector3.Lerp(transform.position, camTargetpos.position, pLerp);

            if (lockPlayer)
            {
                this.transform.LookAt(middleCamTarget.transform);
            }
            else
            {
                if (manager.curr_game_state == GameManager.game_state.CUTSCENE)
                {
                    transform.LookAt(camTargetrot);
                }
                else
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, camTargetrot.rotation, rLerp);
                }
            }
        }
    }

}
