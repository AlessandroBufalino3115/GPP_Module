using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infantry_slime : enemy_main
{

    private float split_left = 5;
    public GameObject slime;

    private Renderer cubeRenderer;
    private Rigidbody m_Rigidbody;


    private float rate = 1f;
    private float time_passed = 0f;


    private void Awake()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
       stats = FindObjectOfType<player_stats>();
        cubeRenderer = GetComponent<Renderer>();
        m_Rigidbody = GetComponent<Rigidbody>();

        text.gameObject.transform.GetChild(1).transform.GetComponent<Text>().text = "Infantry";
    }

    public override void attacking()
    {
        cubeRenderer.material.SetColor("_Color", Color.red);

        stats.take_damage(2f * split_left);

        //player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //player.GetComponent<Rigidbody>().AddForce(this.transform.forward * (200 * split_left / 5));

        if (Vector3.Distance(this.transform.position, player.transform.position) >= 3)
        {
            action_state = state.SEEKING;
        }
    }

    public override void seeking()
    {

        Vector3 look_at_pos = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);

        this.transform.LookAt(look_at_pos);
        
        cubeRenderer.material.SetColor("_Color", Color.blue);
        m_Rigidbody.AddForce(transform.up * 250);
        m_Rigidbody.AddForce(transform.forward * 200);


       // Debug.Log(Vector3.Distance(this.transform.position, player.transform.position));

        
     if (Vector3.Distance(this.transform.position, player.transform.position) >= 20) 
        {
            action_state = state.WONDERING;
        }
    }

    public override void wondering()
    {

        this.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        cubeRenderer.material.SetColor("_Color", Color.green);

        m_Rigidbody.AddForce(transform.up * 250);
        m_Rigidbody.AddForce(transform.forward * 200);

 
        if (Vector3.Distance(this.transform.position, player.transform.position) < 20 )
        {
            action_state = state.SEEKING;
        }

    }

    public override void const_call()
    {

        time_passed += Time.deltaTime;
        health -= Time.deltaTime * 2.5f;
    }


    public void Set_stats(float splits)
    {
        split_left = splits;
        health = 100 / splits;
        max_health = 100 / splits;
        this.transform.localScale = new Vector3(splits / 5, splits / 5, splits / 5);
    }


    public override void on_death()
    {
        if (split_left == 4 || split_left == 5)
        {
            var num = Random.Range(2, 4);

            for (int i = 0; i < num; i++)
            {

                var new_slime = Instantiate(slime, new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y + Random.Range(0, 5), this.transform.position.z + Random.Range(-2, 2)), Quaternion.Euler(0, Random.Range(0, 360), 0));

                new_slime.GetComponent<Infantry_slime>().Set_stats(split_left - 1);
                
               // m_Rigidbody.AddForce(transform.up * 250);
                //m_Rigidbody.AddForce(transform.forward * 200);
            }

        }
        else if (split_left == 3 )
        {
            for (int i = 0; i < 2; i++)
            {
                var new_slime = Instantiate(slime, new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y + Random.Range(0, 5), this.transform.position.z + Random.Range(-2, 2)), Quaternion.Euler(0, Random.Range(0, 360), 0));

                new_slime.GetComponent<Infantry_slime>().Set_stats(split_left - 1);
                
               // m_Rigidbody.AddForce(transform.up * 250);
                //m_Rigidbody.AddForce(transform.forward * 200);
            }
        }
        else if (split_left == 1 || split_left == 2)
        {
            // just die
        }


        Destroy(gameObject);
    }




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (time_passed > rate) 
            {

                stats.take_damage(2f * split_left);
            }





        }
    }





}



