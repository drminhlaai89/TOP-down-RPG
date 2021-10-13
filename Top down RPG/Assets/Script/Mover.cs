using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    protected BoxCollider2D boxCollider;

    protected Vector3 moveDelta;

    protected RaycastHit2D hit;

    private Vector3 originalSize;

    public float ySpeed = 0.75f;
    public float xSpedd = 1.0f;
    // Start is called before the first frame update
    protected  virtual void Start()
    {
        originalSize = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();

        
    }

    // Update is called once per frame
    //void FixedUpdate() // call fixed update when doing collision and physic
    //{
    //    float x = Input.GetAxisRaw("Horizontal"); //GetAxisRaw using for 2d games, Left Right.
    //    float y = Input.GetAxisRaw("Vertical"); // Up down

        


    //}
    
    
    protected virtual void UpdateMotor(Vector3 input)
    {
        //Reset MoveDelta.
        moveDelta =new Vector3(input.x *xSpedd,input.y *ySpeed,0);

        //Swap sprite direction, wether you're going right or left.
        if (moveDelta.x > 0)
        {
            transform.localScale = originalSize; //by using input. the sprite will in the right
        }
        else if (moveDelta.x < 0)
        {
            transform.localScale = new Vector3(originalSize.x*-1, originalSize.y, originalSize.z); // by using input. The sprite will in 
                                                          // the left.
        }

        //Add push vector, if any
        moveDelta += pushDirection;

        //Reduce push force every frame, based off recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);


        //Make sure we can move in this direction, by casting a box there first, if the box
        // returns null, we're free to move.
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0 // angle
            , new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime)
            , LayerMask.GetMask("Blocking", "Actor"));


        if (hit.collider == null)
        {
            //Make this thing move!

            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
            //transform.position += moveDelta * Time.deltaTime;
        }
        //Origin             //size
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0 // angle
           , new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime)// distance
           , LayerMask.GetMask("Blocking", "Actor")); //layerMask.



        if (hit.collider == null)
        {
            //Make this thing move!

            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
            //transform.position += moveDelta * Time.deltaTime;
        }
    }
}
