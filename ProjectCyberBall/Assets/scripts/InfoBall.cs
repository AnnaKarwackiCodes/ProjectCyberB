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
	}

    public virtual void Move(Hex newHex)
    {
        //Debug.Log("(" + newHex.X + ", " + newHex.Y + ", " + newHex.Z + ") (" + newHex.Row + ", " + newHex.Col + ")");

        StandingHex = newHex;
        setLocation(newHex.X, newHex.Y, newHex.Z); //agent knows where it is
        GameObject g = gameController.theMap.getHex(newHex.X, newHex.Y, newHex.Z).gameObject;
        this.gameObject.transform.position = new Vector3(g.transform.position.x, g.transform.position.y + yOffset, g.transform.position.z);  //agent's gameObjects move to proper location


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
