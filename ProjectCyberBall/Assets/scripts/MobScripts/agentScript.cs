using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agentScript : MonoBehaviour {


    //cube coord
    private int x;
    public int X
    {
        get { return this.x; }
    }

    private int y;
    public int Y
    {
        get { return this.y; }
    }

    private int z;
    public int Z
    {
        get { return this.z; }
    }

    /// <summary>
    /// Sets the location of the agent
    /// location is based in the cudic coordnate system (x, y, z)
    /// Makes sure that coordinates add up to 0, x + y + z = 0
    /// </summary>
    public void setLocation(int xPos, int yPos, int zPos) 
    {
        if (xPos + yPos + zPos == 0)
        {
            this.x = xPos;
            this.y = yPos;
            this.z = zPos;
        }
    }

    /// <summary>
    /// Tracks wether or not mob has ball in hand
    /// return true if mob has ball in hand
    /// return false if mob does not have ball in hand
    /// </summary>
    private bool hasBall;
    public bool HasBall
    {

        get { return this.hasBall; }

        set { this.hasBall = value; }

    }

    /// <summary>
    /// Stores mob health.
    /// </summary>
    private int health;
    public int Health
    {

        get { return this.health; }

        set { this.health = value; }

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Move the mob around the grid
    /// </summary>
    public virtual void Move(Hex newHex)
    {



    }

    public virtual void pickUpBall()
    {



    }
}
