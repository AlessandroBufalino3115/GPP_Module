using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotating : MonoBehaviour
{

    public GameObject parent;
    public Transform middle_player;

    private Vector3 positiongg;
    int layerMask_obj = 1 << 11;

    [Range(0.0000001f, 0.0001f)]
    public float interpolationRatio;

    [Range(0.01f, 1)]
    public float sensitivity = 1;

    [Range(5f, 30)]
    public float radius = 3;
    public float curr_radius = 3;

    private float phase;


    private void coll_hit() 
    {
        bool hit_somthing = false;

        float saved_closest = radius;

        var new_pos = new Vector3(middle_player.position.x, middle_player.position.y - 1, middle_player.position.z);

        RaycastHit hit;
        var targetForward = this.transform.position - middle_player.transform.position;

        if (Physics.Raycast(new_pos, targetForward , out hit, radius, layerMask_obj))
        {


            hit_somthing = true;
            
            if (hit.distance <= saved_closest)
                saved_closest = hit.distance;
        }

        if (!hit_somthing) 
        {
            curr_radius = radius;
        }
        else 
        {
            curr_radius = saved_closest;
        }
        if (curr_radius <= 3.5f)
        {
            curr_radius = 3.5f;
        }
    }

    private void FixedUpdate()
    {

        coll_hit();



        positiongg = parent.transform.position;

        var radians = 2 * Mathf.PI + phase;

        var vertrical = Mathf.Sin(radians);
        var horizontal = Mathf.Cos(radians);

        var spawnDir = new Vector3(horizontal, 0, vertrical);

        var spawnPos = positiongg + spawnDir * curr_radius;

        this.transform.position = new Vector3(spawnPos.x, spawnPos.y + Mathf.PI , spawnPos.z);
    }

    public void CallMove(float x) 
    {
        phase += x* sensitivity;
    }

    public void QuickTurn(float dir) 
    { 
        if (dir > 0) 
        {
            phase += 30;
        }
        else 
        {
            phase -= 30;
        }
    }
}
