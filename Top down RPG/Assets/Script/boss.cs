using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : Enemy
{
    public float[] fireballSpeed = { 2.5f , -2.5f};
    public float distance = 0.25f;
    public Transform[] fireBalls;

    

    private void Update()
    {
        for (int i = 0; i < fireBalls.Length; i++)
        {
            fireBalls[i].position = transform.position + new Vector3(-Mathf.Cos(Time.time * fireballSpeed[i]) * distance, -Mathf.Sin(Time.time * fireballSpeed[i]) * distance, 0);

        }

        

    }
}
