using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slime_missile : enemy_main
{
    // Start is called before the first frame update
    public GameObject missile;


    private void Awake() {   player = GameObject.FindGameObjectsWithTag("Player")[0];

        text.gameObject.transform.GetChild(1).transform.GetComponent<Text>().text = "Artillery";
    }



    public override void attacking()
    {

        //we could have it turn always 

        if (Vector3.Distance(this.transform.position, player.transform.position) >= 30)
        {
            action_state = state.WONDERING;// this is wehre the call to anim could be
            action_rate = 0.1f;
        }
        else 
        {
            var adj_pos = new Vector3(this.transform.position.x, this.transform.position.y + 1.4f, this.transform.position.z);

            GameObject missile_obj = Instantiate(missile, adj_pos, this.transform.rotation);

            missile_obj.GetComponent<Missile>().player = player;
        }
    }

    public override void seeking() {}

    public override void wondering()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) < 30)
        {
            action_state = state.ATTACKING;
            action_rate = 2f;
        }
    }



    public override void on_death()
    {
        
    }

}
