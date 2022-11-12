using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class enemy_detection : MonoBehaviour
{

    public GameObject text_pop;
    public GameObject to_track;

    public Movement_controller move_cont;

    private List<GameObject> enemies_in_radius = new List<GameObject>();
    private List<GameObject> enemies_text_pop = new List<GameObject>();


    public bool locked = false;
    public int curr_index_lock = 0;


    [Range(5, 20)]
    public int radius =10;
    public SphereCollider sphere_coll;



    void Update()
    {
        sphere_coll.radius = radius;

        if (move_cont.follow == true) 
        {
            for (int i = 0; i < enemies_text_pop.Count; i++) 
            {

                if (i == curr_index_lock) 
                {
                    enemies_text_pop[i].GetComponent<TextMeshPro>().color = Color.red;
                }
                else 
                {
                    enemies_text_pop[i].GetComponent<TextMeshPro>().color = Color.white;
                }
            }
        }
    }

    public void New_enemy (int num)
    {
        curr_index_lock += num;
        if (curr_index_lock > enemies_in_radius.Count - 1) 
        {
            curr_index_lock = 0;
        }
        else if (curr_index_lock < 0) 
        {
            curr_index_lock = enemies_in_radius.Count - 1;
        }

        move_cont.enemy = enemies_in_radius[curr_index_lock];
    }


    public void Start_tracking() 
    {
        if (enemies_in_radius.Count > 0) 
        {
            curr_index_lock = 0;
            move_cont.enemy = enemies_in_radius[curr_index_lock];
        }
        else 
        {
            End_tracking();
        }
    }


    public void End_tracking() 
    {
        for (int i = 0; i < enemies_text_pop.Count; i++)
        {
                enemies_text_pop[i].GetComponent<TextMeshPro>().color = Color.white;
            
        }

        move_cont.follow = false;
        move_cont.enemy = null;
    }



    public int get_size() 
    {
        return enemies_in_radius.Count;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            enemies_in_radius.Add(other.gameObject);

            GameObject text = Instantiate(text_pop, new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 1f, other.gameObject.transform.position.z), Quaternion.identity);
            text.transform.parent = other.gameObject.transform;

            Enemy_type Type = other.gameObject.GetComponent<Enemy_type>();
            

            switch (Type.type) 
            {
                case Enemy_type.enemy_type.ARCHER:

                    text.GetComponent<TextMeshPro>().text = "ARCHER";
                    break;

                case Enemy_type.enemy_type.INFANTRY:

                    text.GetComponent<TextMeshPro>().text = "INFANTRY";
                    break;

                case Enemy_type.enemy_type.HORSEMAN:

                    text.GetComponent<TextMeshPro>().text = "HORSEMAN";
                    break;
            }
            enemies_text_pop.Add(text);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {

            for (int i = 0; i < enemies_in_radius.Count; i++)
            {

                if (other.gameObject == enemies_in_radius[i])
                {
                    End_tracking();
                    enemies_in_radius.RemoveAt(i);

                    Destroy(enemies_text_pop[i]);
                    enemies_text_pop.RemoveAt(i);

                }
            }
        }
    }
}
