﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobBase : agentScript {
    
    /// <summary>
    /// mobBase acts as the base class for all autonomous mobs in the game
    /// attack values
    /// allignment
    /// quick reference to map position
    /// move distance
    /// </summary>

    /// <summary>
    ///Stores mobds attack/damage value
    ///Can be used to modify when buffed/spawned under alternate circumstances.
    ///Can vary between mobs.
    /// </summary>
    private int attack;
    public int Attack
    {

        get { return this.attack; }

        set { this.attack = value; }

    }

    /// <summary>
    /// Tracks whether or not the unit has been selected by the player
    /// </summary>
    private bool selected;
    public bool Selected
    {

        get { return this.selected; }

        set { this.selected = value; }

    }

    private string type;
    public string Type
    {
        get { return this.type; }

        set { this.type = value; }
    }

    private int arrayPos;
    public int ArrayPos
    {
        get { return this.arrayPos; }

        set { this.arrayPos = value; }
    }

    protected Animator anim;

	// Use this for initialization
	public virtual new void Start () {
        base.Start();
        anim = GetComponent<Animator>();
        this.CanMove = true;
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
    }

    /// <summary>
    /// Make an attack against an enemy Mob
    /// </summary>
    public virtual void mobAttack(agentScript target) {

        Debug.Log(gameObject.name + " ATTACKING " + target.gameObject.name);
        this.gameObject.transform.rotation = Quaternion.LookRotation(((new Vector3(target.gameObject.transform.position.x, gameObject.transform.position.y, target.gameObject.transform.position.z)) - gameObject.transform.position).normalized); //rotates so agent is looking forward when moving
        anim.Play("Attack");
        target.takeDamage(Attack);
    }

    public override void takeDamage(int damageTaken)
    {
        base.takeDamage(damageTaken);
        anim.Play("Hit");
    }

}
