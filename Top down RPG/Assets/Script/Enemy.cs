using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    //Eperience

    public int xpValue = 1;

    //Logic
    public float triggerLength = 1; //if the distance in between the enemy and the player is 
                                    //less or equal than 1m so he will starts chasing you.
    
    public float chaseLength = 5; //this logic will give the enemy the length how long
                                  //enemy is  chasing you, the result will be 5 meters.
    
    private bool chasing; //if they chasing you right now.
    
    private bool collidingWithPlayer;// its to know whether or not the enemy is
                                     // chasing the player right now
    private Transform playerTransform; //
    
    private Vector3 startingPosition;

    //Hitbox
    private ContactFilter2D filter;
    private BoxCollider2D hitBox; //The hitBox will be the head of the enemy and
                                  //the small enemy will be the face of the enmy that
                                  //player can actual hit.

    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        //this Getchild will get the children below the small enemy  GameObject and
        //get that component.
        hitBox = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }
    protected void FixedUpdate()
    {
        //Is the player in range?
        if(Vector3.Distance(playerTransform.position,startingPosition) <chaseLength)
        {
            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
            {
                chasing = true;
            }
            //chasing = Vector3.Distance(playerTransform.transform.position, startingPosition) < triggerLength;

            if(chasing)
            {
                if(!collidingWithPlayer)
                {
                    UpdateMotor((playerTransform.transform.position - transform.position).normalized);
                }

            }
            else
            {
                UpdateMotor(startingPosition - transform.position);
            }
        }
        else
        {
            UpdateMotor(startingPosition - transform.position);
            chasing = false;
        }


        //Check overLap
        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
            {
                continue;
            }
            if(hits[i].tag == "Fighter" && hits[i].name == "Player")
            {
                collidingWithPlayer = true;
            }



            //the arrays is not cleaned up, so we do it ourself;

            hits[i] = null;
        }
    }

    protected override void Death()
    {
        base.Death();
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+ " + xpValue + "xp", 30
                                        , Color.magenta, transform.position, Vector3.up * 40, 1.0f);
    }
}
