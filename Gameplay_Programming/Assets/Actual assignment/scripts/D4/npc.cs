using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc : MonoBehaviour
{

    private Movement_controller move_cont;
    private GameManager manager;
    private CameraScript cam_script;

    public Transform look_pos;
    public Transform look_rot;
    private GameObject player;

    public bool interacted;
    public bool draw_text;

    public bool draw_text_sent;

    private int num_sentences = -1;
    public List<string> list_of_sentences = new List<string>();


    private void Awake()
    {
        move_cont = FindObjectOfType<Movement_controller>();
        manager = FindObjectOfType<GameManager>();
        cam_script = FindObjectOfType<CameraScript>();
    }

    void Update()
    {
        if (interacted)
        {
            if (move_cont.action)
            {
                num_sentences++;

                if (num_sentences > list_of_sentences.Count - 1)
                {
                    draw_text = true;
                    interacted = false;
                    num_sentences =-1;
                    cam_script.camTargetpos = manager.player_target_pos;

                    cam_script.camTargetrot = manager.player_target_rot;

                    cam_script.lockPlayer = true;
                    manager.curr_game_state = GameManager.game_state.GAME;
                }

                move_cont.action = false;
            }

        }


        if (draw_text && move_cont.action) 
        {
            interacted = true;
            draw_text = false;
            cam_script.lockPlayer = false;

            cam_script.camTargetrot = look_rot;
            cam_script.camTargetpos = look_pos;
            manager.curr_game_state = GameManager.game_state.CUTSCENE;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "player")
        {
            player = other.gameObject;
            draw_text = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "player")
        {
            draw_text = false;
        }
    }



    private void OnGUI()
    {
        if (draw_text) 
        {
            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height - 50, 300, 30), "Press the Interaction key");
            Vector3 look_at_pos = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);

            this.transform.LookAt(look_at_pos);
        }

        if (interacted)
        {
            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height - 150, 300, 100), list_of_sentences[num_sentences] + "\n\n (Press the Interaction key to continue)");
        }
    }
}
