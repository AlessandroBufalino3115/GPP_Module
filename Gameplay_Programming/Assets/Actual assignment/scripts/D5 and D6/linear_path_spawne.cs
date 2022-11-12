using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linear_path_spawne : MonoBehaviour
{
    public Transform[] locations;


    [Range(0.05f, 4)]
    public float Spawnrate = 2f;
    private float lastSpawn = 0.0f;


    [Range(0.001f, 0.05f)]
    public float speed = 0.005f;

    [SerializeField]
    private GameObject platform;


    public bool comeback = false;

    void Start()
    {
        if (comeback) 
        {
            GameObject platform_spawned = Instantiate(platform, locations[0].position, transform.rotation);
            platform_spawned.GetComponent<linear_follower>().locations = locations;
            platform_spawned.GetComponent<linear_follower>().step = speed;

            platform_spawned.GetComponent<linear_follower>().comeback = comeback;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!comeback) 
        {
            if (Time.time > Spawnrate + lastSpawn)
            {
                GameObject platform_spawned = Instantiate(platform, locations[0].position, transform.rotation);


                platform_spawned.GetComponent<linear_follower>().locations = locations;
                platform_spawned.GetComponent<linear_follower>().step = speed;

                platform_spawned.GetComponent<linear_follower>().comeback = comeback;


                lastSpawn = Time.time;
            }
        }
    }
}
