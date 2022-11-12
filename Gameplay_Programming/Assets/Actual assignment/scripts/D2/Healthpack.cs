using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Healthpack : Parent_consumable
{
    public override void genName()
    {
        memory_saved_obj = Instantiate(text_pop, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
        memory_saved_obj.GetComponent<TextMeshPro>().text = "Health pack";
        Showing_name = true;
    }

    public override void destroyName()
    {
        Showing_name = false;
        Destroy(memory_saved_obj);
    }

    private void Update()
    {
        if (Showing_name)
        {
            memory_saved_obj.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        }
    }

    public override void Action() { player.HealthGaincall(); }

}
