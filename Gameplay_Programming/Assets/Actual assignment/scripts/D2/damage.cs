using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    private player_stats player;


    private void Awake()
    {
        player = FindObjectOfType<player_stats>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 3)
            player.health -= 30;
    }

}
