using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slime_enemy : MonoBehaviour
{

    //ineedto fix whatever is going on with the locking on
    // have a look at the fightihgn
    // implement the diff type into the enemy



    private Renderer cubeRenderer;
    private Rigidbody m_Rigidbody;

    public float action_rate = 2f;
    public float last_damage = 0.0f;

    private float split_left = 5;

    private int damage = 5;
    private bool damaged;

    private player_stats stats;
    private GameObject player;

    public GameObject slime;

    private void Awake()
    {
      stats = FindObjectOfType<player_stats>();
        cubeRenderer = GetComponent<Renderer>();
        m_Rigidbody = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }


    public enum state
    {
        SEEKING,
        WONDERING,
        ATTACKING
    }
    public state action_state;
    

    


    // Start is called before the first frame update
    void Start()
    {
        //stats = FindObjectOfType<player_stats>();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        action_state = state.WONDERING;
    }

    // Update is called once per frame
    void Update()
    {
     
        


        if (Time.time > action_rate + last_damage)
        {
            last_damage = Time.time;





            if (Vector3.Distance(this.transform.position, player.transform.position) < 5 && action_state == state.SEEKING)
            {
                action_state = state.ATTACKING;
            }
            else if (action_state == state.ATTACKING && Vector3.Distance(this.transform.position, player.transform.position) > 5)
            {

                action_state = state.SEEKING;
            }



            switch (action_state)
            {

                case state.SEEKING:

                    Vector3 look_at_pos = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);

                    this.transform.LookAt(look_at_pos);


                    cubeRenderer.material.SetColor("_Color", Color.blue);
                    m_Rigidbody.AddForce(transform.up * 250);
                    m_Rigidbody.AddForce(transform.forward * 200);





                    break;

                case state.WONDERING:


                   this.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);


                    cubeRenderer.material.SetColor("_Color", Color.green);

                    m_Rigidbody.AddForce(transform.up * 250);
                    m_Rigidbody.AddForce(transform.forward * 200);


                    break;

                case state.ATTACKING:

                    cubeRenderer.material.SetColor("_Color", Color.red);

                   // stats.take_damage(5f * split_left);


                    player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    player.GetComponent<Rigidbody>().AddForce(this.transform.forward * (200 * split_left/5));
                    
                    break;
            }

        }
    }


    public void Set_stats(float splits) 
    {
        split_left = splits;
        this.transform.localScale = new Vector3(splits / 5, splits / 5, splits / 5);
    }


    private void OnDestroy()
    {// this is where we spit
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {

            action_state = state.SEEKING;
            action_rate = 2f;

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player") 
        {

            action_rate = 4f;
            action_state = state.WONDERING;

        }
    }



  public void on_Death() 
    {
        if (split_left == 4 || split_left == 5)
        {
            var num = Random.Range(2, 4);

            for (int i = 0; i < num; i++)
            {

                var new_slime = Instantiate(slime, new Vector3(this.transform.position.x + Random.Range(-2, 2) , this.transform.position.y + Random.Range(0, 5) , this.transform.position.z + Random.Range(-2, 2)), Quaternion.Euler(0, Random.Range(0, 360), 0));

                new_slime.GetComponent<slime_enemy>().Set_stats(split_left -1);
                Debug.Log("spawned");
                m_Rigidbody.AddForce(transform.up * 250);
                m_Rigidbody.AddForce(transform.forward * 200);
            }

        }
        else if (split_left == 3 || split_left == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                var new_slime = Instantiate(slime, new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y + Random.Range(0, 5), this.transform.position.z + Random.Range(-2, 2)), Quaternion.Euler(0, Random.Range(0, 360), 0));

                new_slime.GetComponent<slime_enemy>().Set_stats(split_left - 1);
                Debug.Log("spawned");
                m_Rigidbody.AddForce(transform.up * 250);
                m_Rigidbody.AddForce(transform.forward * 200);
            }
        }
        else if (split_left == 1)
        {
            // just die
        }


        Destroy(gameObject);
    }


}
