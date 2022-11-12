using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class read_enemy_stat : MonoBehaviour
{

    public enemy_main stats;

    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);

        this.GetComponent<TextMeshPro>().text = stats.health  + " / " + stats.max_health;

        //percnate health change shits

    }
}
