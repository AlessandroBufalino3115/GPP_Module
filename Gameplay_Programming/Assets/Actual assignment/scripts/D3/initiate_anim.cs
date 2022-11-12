using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initiate_anim : MonoBehaviour
{
    private Movement_controller move_cont;
    private GameObject player;
    private GameObject player_body;

    private Animator player_anim;

    public Animator moving_anim;
    private GameManager manager;

    public Animator self_anim;


    public Transform ButtonTargetrot;
    public Transform ButtonTargetpos;


    private CameraScript cam_script;

    public Transform DoorTargetrot;
    public Transform DoorTargetpos;

    private float pLerp = 0.065f;
    private float rLerp = 0.015f;


    private bool draw_text;   
    private bool move_char;   

    private bool spent  = false;    



    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        cam_script = FindObjectOfType<CameraScript>();
    }

    void Update()
    {

        if (!spent)
        {
            if (draw_text && !move_char)
            {
                if (move_cont.action)
                {
                    manager.curr_game_state = GameManager.game_state.CUTSCENE;
                    move_char = true;

                    cam_script.camTargetpos = DoorTargetpos;
                    cam_script.camTargetrot = ButtonTargetrot;
                    cam_script.lockPlayer = false;
                }
            }

            if (move_char)
            {
                if (Vector3.Distance(player.transform.position, ButtonTargetpos.transform.position) >= 0.06f)
                {
                    player.transform.position = Vector3.Lerp(player.transform.position, ButtonTargetpos.position, pLerp);
                    player_body.transform.rotation = Quaternion.Lerp(player_body.transform.rotation, ButtonTargetrot.rotation, rLerp);
                }
                else
                {
                    player_body.transform.rotation = ButtonTargetrot.rotation;

                    move_char = !move_char;
                    move_cont.interaction_call();
                    self_anim.SetTrigger("press");

                    draw_text = false;
                    spent = true;
                }
            }
        }
    }

    public void reset_cam_pos()
    {
        manager.curr_game_state = GameManager.game_state.GAME;

        cam_script.camTargetpos = manager.player_target_pos;
        cam_script.camTargetrot = manager.player_target_rot;
        cam_script.lockPlayer = true;
        move_cont.action = false;
    }

    public void call_door_anim() 
    {
        cam_script.camTargetrot = DoorTargetrot;
        moving_anim.SetTrigger("open");
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!spent) 
        {

            if (other.name == "player")
            {
                player = other.gameObject;
            }


            if (other.name == "RPG-Character")
            {
                player_body = other.gameObject;
                move_cont = other.GetComponent<Movement_controller>();
                player_anim = other.GetComponent<Animator>();
                draw_text = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
            if (other.gameObject.layer == 3)
            {
                draw_text = false;
            }
    }

    private void OnGUI()
    {
        if (draw_text)
        GUI.Box(new Rect(Screen.width/2 - 150, Screen.height - 50, 300, 30), "press up on the D-pad to interact");
    }
}
