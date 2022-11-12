using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{


    public GameObject player;



    private float timer = 1f;
    private float time_spawn = 0f;


    private bool active = false;


    private void Start()
    {
        Destroy(gameObject, 10);
        time_spawn = Time.time + timer;
    }


    // Update is called once per frame
    void Update()
    {

        if (Time.time > time_spawn) 
        {

             var adjusted_player_pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

            var x = Quaternion.LookRotation(adjusted_player_pos - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, x, 59 * Time.deltaTime);


            this.transform.Translate(Vector3.forward * Time.deltaTime * 5);

        }
        else 
        {

            this.transform.rotation = Quaternion.Euler(-90,0 , 0);

            this.transform.Translate(Vector3.forward * Time.deltaTime * 5);
            // going up
        }
    }

 


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<player_stats>().take_damage(5);
            Debug.Log("caklk ndaihdaiod");
            
        }
        else 
        {

            Debug.Log("whwhwhhwwhwhwhwh");
        }


        Destroy(gameObject);
    }


}
