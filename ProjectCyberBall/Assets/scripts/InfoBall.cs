using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBall : agentScript {

    private GameObject holder;
    public GameObject Holder
    {
        get { return holder; }
        set { holder = value; }
    }

    public float MovementSpeed
    {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    public virtual void Move(Hex newHex)
    {
        //Debug.Log("(" + newHex.X + ", " + newHex.Y + ", " + newHex.Z + ") (" + newHex.Row + ", " + newHex.Col + ")");

        movementPath.AddRange(mapLocal.pathfinding(standingHex, newHex));//list hex that agent needs to visit while headin to new location
        StandingHex = newHex;
        setLocation(newHex.X, newHex.Y, newHex.Z); //agent knows where it is

        return;
    }

    public virtual void dropBall()
    {
        if(holder != null)
        {
            holder = null;
            movementSpeed = 1f;
        }
    }

}
