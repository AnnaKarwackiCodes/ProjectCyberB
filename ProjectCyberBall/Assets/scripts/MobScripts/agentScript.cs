using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agentScript : MonoBehaviour {

    public GameController gameController;

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
     /// the height off the hex that an agent's model will be
     /// </summary>
    private float yOffset = 0f;

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
       gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Move agent to hex
    /// </summary>
    public virtual void Move(Hex newHex)
    {
        if (gameController != null) //agent is in a game
        {
            if (gameController.theMap.hexExists(newHex.X, newHex.Y, newHex.Z))//hex exist to move to
            {
                gameController.theMap.getHex(x, y, z).occupant = null; //remove from start hex
                gameController.theMap.getHex(newHex.X, newHex.Y, newHex.Z).occupant = this; //new hex know something is now on it

                setLocation(newHex.X, newHex.Y, newHex.Z); //agent knows where it is
                GameObject g = gameController.theMap.getHex(newHex.X, newHex.Y, newHex.Z).gameObject;
                this.gameObject.transform.position = new Vector3(g.transform.position.x, g.transform.position.y + yOffset, g.transform.position.z);  //agent's gameObjects move to proper location
                return;
            }
            Debug.LogError("Hex: " + newHex + " does not exist");
            return;
        }
        Debug.LogError("game Controller not found");
    }

    public virtual void pickUpBall()
    {



    }
}
