using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider2D))] // RequireComponent only add one component once
                                            // when call it.
public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if(!isAlive)
        {
            return;
        }
        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitPointChange();
    }

    protected override void Death()
    {
        isAlive = false;
        GameManager.instance.deathMenuAnim.SetTrigger("show");
    }

    // Update is called once per frame
    void FixedUpdate() // call fixed update when doing collision and physic
    {
        float x = Input.GetAxisRaw("Horizontal"); //GetAxisRaw using for 2d games, Left Right.
        float y = Input.GetAxisRaw("Vertical"); // Up down

        if(isAlive)
        {
            UpdateMotor(new Vector3(x, y, 0));
        }
        


    }

    public void SwapSprite(int skinId)
    {
        GetComponent<SpriteRenderer>().sprite = GameManager.instance.playerSprites[skinId];
    }

    public void OnLevelUp()
    {
        maxHitpoint++;
        hitpoint = maxHitpoint;
    }

    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
        {
            OnLevelUp();
        }
    }

    public void Heal(int healingAmount)
    {
        

        if (hitpoint == maxHitpoint)
        {
            return;
        }
        hitpoint += healingAmount;

        if(hitpoint > maxHitpoint)
        {
            hitpoint = maxHitpoint;
        }
        GameManager.instance.ShowText(" + " + healingAmount.ToString() + "HP", 25, Color.green, transform.position, Vector3.up, 1.0f);
        GameManager.instance.OnHitPointChange();

    }

    public void Respawn()
    {
        Heal(maxHitpoint);
        isAlive = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
    }
}
