using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class route_spawner : MonoBehaviour
{

    public curve_route[] routes;
    private Vector3 primary_pos;

    [SerializeField]
    private GameObject platform;

    [SerializeField]
    private bool spawn_allowed = false;

    [SerializeField]
    private bool weight_movement = false;


    [Range(0.05f, 4)]
    public float Spawnrate = 2f;
    private float lastSpawn = 0.0f;

    //private Movement_controller move_cont;

    [Range(0.001f, 0.5f)]
    public float speed = 0.1f;

    private bool draw_text;


    public bool comeback = false;


    // Start is called before the first frame update
    void Start()
    {
        primary_pos = routes[0].Getpos(0);

        if (comeback) 
        {
            GameObject platform_spawned = Instantiate(platform, primary_pos, transform.rotation);


            platform_spawned.GetComponent<Route_follower>().routes = routes;
            platform_spawned.GetComponent<Route_follower>().speed = speed;
            platform_spawned.GetComponent<Route_follower>().Allow_weight_mod = weight_movement;
            platform_spawned.GetComponent<Route_follower>().comeback = comeback;

        }



    }

    // Update is called once per frame
    void Update()
    {
            if (!comeback) 
        { 
        
            if (Time.time > Spawnrate + lastSpawn)
            {
                GameObject platform_spawned = Instantiate(platform, primary_pos, transform.rotation);


                platform_spawned.GetComponent<Route_follower>().routes = routes;
                platform_spawned.GetComponent<Route_follower>().speed = speed;
                platform_spawned.GetComponent<Route_follower>().Allow_weight_mod = weight_movement;
                platform_spawned.GetComponent<Route_follower>().comeback = comeback;
                lastSpawn = Time.time;
            }


        }
    }




    private void OnTriggerStay(Collider other)
    {
       
    }





    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "player")
        {
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
            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height - 50, 300, 30), "Press the Interaction key to toggle the swith");
        }
    }

}
