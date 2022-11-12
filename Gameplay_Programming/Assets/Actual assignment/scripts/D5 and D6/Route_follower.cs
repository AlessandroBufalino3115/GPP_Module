using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route_follower : MonoBehaviour
{
    public curve_route[] routes;

    int curr_route = 0;

    [Range(0.01f, 1f)]
    public float curr_t_value = 0;

    private float weigth = 0;

    public float speed = 0.2f;

    private bool active = false;

    public bool Allow_weight_mod;



    public bool comeback= false;

    private bool dir = false;   // false for     true back
    

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Allow_weight_mod) 
        {
            if (!active && weigth > 0)
            {
                weigth -= Time.deltaTime;
            }
            else if (!active && weigth < 0)
            {
                weigth = 0;
            }
            else if (active)
            {
                Debug.Log(weigth);
                weigth += Time.deltaTime * 2f;
            }

        }
       



        if (!comeback) 
        {
            curr_t_value += Time.deltaTime * speed;
            if (curr_t_value > 1)
            {
                curr_t_value = 0;

                curr_route++;

                if (curr_route > routes.Length - 1)
                {
                    Destroy(this.gameObject);
                }
            }
        }
        else
        {  // false for     true back

            if (dir) 
            {
                curr_t_value -= Time.deltaTime * speed;
                if (curr_t_value < 0)
                {
                    curr_t_value = 1;

                    curr_route--;

                    if (curr_route < 0)
                    {
                        curr_route = 0;
                        curr_t_value = 0;
                        dir = !dir;
                    }
                }

            }
            else 
            {
                curr_t_value += Time.deltaTime * speed;
                if (curr_t_value > 1)
                {
                    curr_t_value = 0;

                    curr_route++;

                    if (curr_route > routes.Length - 1)
                    {

                        curr_route = routes.Length - 1;
                        curr_t_value = 1;
                        dir = !dir;
                    }
                }

            }
        
        }


        Vector3 pos = routes[curr_route].Getpos(curr_t_value);

        this.transform.position = new Vector3(pos.x, pos.y - weigth, pos.z);

    }


    private void OnDestroy()
    {
        if (this.transform.childCount > 0) 
        {
            this.gameObject.transform.GetChild(0).gameObject.transform.parent = null;
        }
    }


    private GameObject Player;


    private void Awake()
    {
       // Player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = transform;
            active = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = null;
            active = false;
        }
    }

    


}
