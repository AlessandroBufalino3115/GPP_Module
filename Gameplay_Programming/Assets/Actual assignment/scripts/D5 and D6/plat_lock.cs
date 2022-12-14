using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plat_lock : MonoBehaviour
{
    private GameObject Player;


    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player) 
        {
            Player.transform.parent = transform;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = null;
        }
    }
    
}
