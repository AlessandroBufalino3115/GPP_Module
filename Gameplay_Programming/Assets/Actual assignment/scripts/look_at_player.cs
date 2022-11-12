using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class look_at_player : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
