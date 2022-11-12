using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : Parent_consumable
{

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0.3f, 0));
    }
    public override void Action()
    {
        player.Coincall();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            var new_pos = new Vector3(other.transform.position.x, other.transform.position.y + 1.6f, other.transform.position.z);
           
            var rayDirection = new_pos - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit))
            {
                if (hit.transform.gameObject.layer == 3)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new_pos, 03 * Time.deltaTime);

                    if (Vector3.Distance(new_pos, transform.position) <= 0.1f)
                    {
                        player.Coincall();
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
