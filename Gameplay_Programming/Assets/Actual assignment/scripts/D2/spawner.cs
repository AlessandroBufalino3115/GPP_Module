using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{

    public GameObject spawn_point;
    public GameObject instantiate_obj;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        Instantiate(instantiate_obj, spawn_point.transform.position, Quaternion.identity);
    }

}
