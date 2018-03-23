using System.Collections;
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
    private bool attack;
    private bool Attack
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
    private bool canMove;
    public bool CanMove
    {
        get { return this.canMove; }

        set { this.canMove = value; }
    }

	// Use this for initialization
	public virtual new void Start () {
        base.Start();
        canMove = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Make an attack against an enemy Mob
    /// </summary>
    public virtual void mobAttack(agentScript target) {



    }

}
