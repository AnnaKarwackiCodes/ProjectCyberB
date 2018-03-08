using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agentScript : MonoBehaviour {

    /// <summary>
    /// agentScript is responsible for the inheritance of all major universal functions
    /// position saving
    /// holding a ball
    /// the health
    /// movement
    /// setting current hex
    /// </summary>

    private GameController gameController;
    //public GameController gameController;

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

    /// <summary>
    /// Tracks which hex the mob is currently standing on: [row, column]
    /// Stores position of hex in array.
    /// Can use physical hex position to position the mob
    /// </summary>
    private Hex standingHex;
    public Hex StandingHex
    {

        get { return this.standingHex; }

        set { this.standingHex = value; }

    }


    private int moveDistance;
    public int MoveDistance
    {

        get { return this.moveDistance; }

        set { this.moveDistance = value; }

    }

    /// <summary>
    /// Stores a local copy of the map for mobs to be able to see and use
    /// Does this need to be a property?
    /// No
    /// Is it a property?
    /// Yes
    /// Why?
    /// Because it can be, ergo it is.
    /// </summary>
    private Map mapLocal;
    public Map MapLocal
    {

        get { return this.mapLocal; }

        set { this.mapLocal = value; }

    }

    // Use this for initialization
    public virtual void Start () {
       Debug.Log("AGENT START");
       gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
       MapLocal = gameController.theMap;
	}

    void Awake()
    {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Move agent to hex
    /// </summary>
    public virtual void Move(Hex newHex)
    {
        if (StandingHex == null) Debug.Log("standing null");
        if (newHex == null) Debug.Log("newHex null");
        if (MapLocal == null) Debug.Log("map null");
        int dist = MapLocal.distanceBetween(standingHex, newHex);

        if (gameController != null) //agent is in a game
        {
            if (gameController.theMap.hexExists(newHex.X, newHex.Y, newHex.Z))//hex exist to move to
            {
                if (dist <= moveDistance)
                    {
                        gameController.theMap.getHex(x, y, z).occupant = null; //remove from start hex
                        gameController.theMap.getHex(newHex.X, newHex.Y, newHex.Z).occupant = this; //new hex know something is now on it
                        StandingHex = newHex;
                        setLocation(newHex.X, newHex.Y, newHex.Z); //agent knows where it is
                        GameObject g = gameController.theMap.getHex(newHex.X, newHex.Y, newHex.Z).gameObject;
                        this.gameObject.transform.position = new Vector3(g.transform.position.x, g.transform.position.y + yOffset, g.transform.position.z);  //agent's gameObjects move to proper location
                        return;
                    }
            }
            Debug.LogError("Hex: " + newHex + " does not exist");
            return;
        }
        Debug.LogError("game Controller not found");
    }

    public virtual void spawnIn(Hex newHex)
    {
        if (newHex == null) Debug.Log("dfbdskjbkds");
        if (gameController == null) Debug.Log("but why");
        //if (gameController.theMap == null) Debug.Log("fuck me");
        gameController.theMap.getHex(newHex.X, newHex.Y, newHex.Z).occupant = this;
        setLocation(newHex.X, newHex.Y, newHex.Z); //agent knows where it is
        GameObject g = gameController.theMap.getHex(newHex.X, newHex.Y, newHex.Z).gameObject;
        standingHex = newHex;
        this.gameObject.transform.position = new Vector3(g.transform.position.x, g.transform.position.y + yOffset, g.transform.position.z);
    }

    public virtual void pickUpBall()
    {



    }
}
