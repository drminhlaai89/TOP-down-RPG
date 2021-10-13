using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    [HideInInspector]
    private Transform lootAt;
    public float boundX = 0.15f;
    public float boundY = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        lootAt = GameObject.Find("Player").transform;
    }

    // LateUpdate is called once per frame after player doing action
    void LateUpdate()
    {
        Vector3 delta = Vector3.zero;

        //this is to check if we're inside the bounds on the X axis.
        float deltaX = lootAt.position.x - transform.position.x;

        if (deltaX > boundX || deltaX < -boundX)
        {
            if (transform.position.x <lootAt.position.x)
            {
                delta.x = deltaX - boundX;
            }
            else
            {
                delta.x = deltaX + boundX;
            }
        }

        //this is to check if we're inside the bounds on the y axis.
        float deltaY = lootAt.position.y - transform.position.y;

        if (deltaY > boundY || deltaY < -boundY)
        {
            if (transform.position.y < lootAt.position.y)
            {
                delta.y = deltaY - boundY;
            }
            else
            {
                delta.y = deltaY + boundY;
            }
        }

        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
