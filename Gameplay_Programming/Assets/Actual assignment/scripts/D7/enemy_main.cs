using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy_main : MonoBehaviour
{

    public float health = 0;
    public float max_health = 0;

    public float action_rate = 0f;
    public float last_damage = 0.0f;

    public enum state
    {
        SEEKING,
        WONDERING,
        ATTACKING
    }
    public state action_state;

    public player_stats stats;
    public GameObject player;

    public Image Fill;  // assign in the editor the "Fill"
    public GameObject text;




    // Start is called before the first frame update
    void Start()
    {
        action_state = state.WONDERING;


    }

    private void Update()
    {

        //health -= Time.deltaTime;


        if (health > max_health)
        {
            health = max_health;
        }
        else if (health < 1)
        {
            on_death();
            Destroy(this.gameObject);
        }

        //Debug.Log(action_state);


        if (Time.time > action_rate + last_damage)
        {
            last_damage = Time.time;
            switch (action_state)
            {
                case state.SEEKING:
                    seeking();
                    break;

                case state.ATTACKING:
                    attacking();
                    break;

                case state.WONDERING:

                    wondering();
                    break;

            }
        }

        const_call();

        draw_text();

    }




    virtual public void seeking()
    { }

    virtual public void attacking()
    { }

    virtual public void wondering()
    { }

    virtual public void on_death()
    { }

    virtual public void const_call() 
    { }
    // Update is called once per frame



    public void draw_text()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) < 30 )
        {
            text.SetActive(true);

            Vector3 look_at_pos = new Vector3(player.transform.position.x, text.transform.position.y, player.transform.position.z);

            text.transform.LookAt(look_at_pos);


            float percentage = health / max_health;



            text.gameObject.transform.GetChild(0).transform.GetComponent<Slider>().value = percentage;
           // Debug.Log(percentage);

            if (percentage >= 0.79f) 
            {
                Fill.color = Color.green;
            }
            else if (percentage >= 0.45f    &&  percentage < 0.79f) 
            {
                Fill.color = Color.yellow;
                    }
            else if (percentage < 0.45f) 
            {
                Fill.color = Color.red;
            }





        }
        else if (Vector3.Distance(this.transform.position, player.transform.position) >= 30)
        {

            text.SetActive(false);
        }


    }


    public void damage(int dam)
    {

        health -= dam;
    }
}
