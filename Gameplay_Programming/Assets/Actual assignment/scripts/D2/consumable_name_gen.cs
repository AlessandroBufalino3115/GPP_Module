using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class consumable_name_gen : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) 
        {
            Parent_consumable script = other.GetComponent<Parent_consumable>();
            script.genName();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            Parent_consumable script = other.GetComponent<Parent_consumable>();
            script.destroyName();
        }
    }
}
