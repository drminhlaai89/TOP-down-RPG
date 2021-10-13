using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : Collidable
{
    // Damage struct

    public int[] damagePoint = { 1, 2, 3, 4, 5, 6, 7 };
    public float[] pushForce = { 2.0f, 2.2f, 2.5f, 3f, 3.2f, 3.6f, 4f };

    //Upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    //Swing

    private Animator anim;
    
    //parameters
    private float cooldown = 0.5f;
    private float lastSwing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    protected override void Start()
    {
        //this base have box collider 2d so we dont have make another get component 2d.
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        //this base update have the colliable work in every weapon.
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
           
        }
    }
    
    
    protected override void OnCollide(Collider2D coll)
    {
        if(coll.tag == "fighter")
        {
            if (coll.name == "Player")
                return;

            //Create a new damage object, then we'll send it to the fighter we've hit
            Damage dmg = new Damage()
            {
                damageAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForce = pushForce[weaponLevel]
            };

            coll.SendMessage("ReceiveDamage",dmg);
            //Debug.Log(coll.name);
        }
       
    }

    private void Swing()
    {

        anim.SetTrigger("Swing");
        
        
        
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];

        //Change stats
    } 
    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }

}
