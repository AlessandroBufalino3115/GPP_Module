using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class call_init_anim : MonoBehaviour
{
    public initiate_anim init;

    public void call_reset() 
    {
        init.reset_cam_pos();
    }
}
