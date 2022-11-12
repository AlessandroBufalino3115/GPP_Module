using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linear_follower : MonoBehaviour
{


    public Transform[] locations;

    public int target_ind;
    public float step = 0.1f;

    public bool comeback;

    private bool dir = false;     // false forward      true back

    void Start()
    {
        target_ind = 1;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, locations[target_ind].position, step);

        if (Vector3.Distance(transform.position, locations[target_ind].position) <= 0.2f) 
        { 


            if (!comeback) 
            {

                if (target_ind + 1 == locations.Length)
                {
                    Destroy(gameObject);
                }
                else
                {

                    target_ind++;
                }
            }
            else 
            {

                if (!dir)  
                {
                    if (target_ind + 1 == locations.Length)
                    {
                        dir = !dir;
                    }
                    else
                    {

                        target_ind++;
                    }

                }
                else 
                {
                    if (target_ind - 1 == -1)
                    {
                        dir = !dir;
                    }
                    else
                    {

                        target_ind--;
                    }
                }
            }
        }
    }



    private void OnDestroy()
    {
        if (this.transform.childCount > 0)
        {
            this.gameObject.transform.GetChild(0).gameObject.transform.parent = null;
        }
    }



}
