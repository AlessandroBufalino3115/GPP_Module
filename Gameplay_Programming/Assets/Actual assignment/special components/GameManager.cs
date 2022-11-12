using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform player_target_rot;
    public Transform player_target_pos;
    public Canvas pause_screen;

    public enum game_state
    {
       PAUSE,
       GAME,
       MENU,
       CUTSCENE
    }
    public game_state curr_game_state;

    void Start()
    {
        curr_game_state = game_state.GAME;
    }

    void Update()
    {
        switch (curr_game_state) 
        {

            case game_state.PAUSE:
                pause_screen.enabled = true;
                break;


            case game_state.GAME:
                pause_screen.enabled = false;
                break;

            case game_state.MENU:

                break;
        }
    }

}
