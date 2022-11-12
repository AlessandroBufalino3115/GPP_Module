using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent_consumable : MonoBehaviour
{


    public Movement_controller move_contr;
    public player_stats player;
    public GameObject text_pop;

    public GameObject memory_saved_obj;
    public bool Showing_name;

    private void Awake()
    {
        move_contr = FindObjectOfType<Movement_controller>();
        player = FindObjectOfType<player_stats>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3) 
        {
            Action();
            if (Showing_name) 
            {
                Destroy(memory_saved_obj);
            }
            Destroy(this.gameObject);
        }
    }

    public virtual void genName() {}

    public virtual void destroyName() {}


    public virtual void Action() {}

}
