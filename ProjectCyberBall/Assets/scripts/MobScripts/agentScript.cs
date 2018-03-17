using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// agentScript is responsible for the inheritance of all major universal functions
/// position saving
/// holding a ball
/// the health
/// movement
/// setting current hex
/// </summary>

public class agentScript : MonoBehaviour {

    public GameController gameController;

    /// <summary>
    /// Stores a local copy of the map for mobs to be able to see and use
    /// Does this need to be a property?
    /// No
    /// Is it a property?
    /// Yes
    /// Why?
    /// Because it can be, ergo it is.
    /// </summary>
    public Map mapLocal;

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
    [SerializeField] protected float yOffset = 0f;

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
    /// what side agent is on
    /// true = player's side
    /// false = enemy's side
    /// </summary>
    private bool alligence;
    public bool Alligence
    {
        get { return alligence; }
        set { this.alligence = value; }
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

    

    // Use this for initialization
    public virtual void Start () {
       Debug.Log("AGENT START");
       gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
       mapLocal = gameController.theMap;
	}

    void Awake()
    {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Gets list of all possible moves the agent can make, taking into account other agents, walls, etc.
    /// </summary>
    /// <returns>All hexs that the agent can move to</returns>
    public Hex[] getPossibleMoves()
    {
        List<Hex> visited = new List<Hex>(); //list of hexs that already been visited
        visited.Add(standingHex);
        Dictionary<int, List<Hex>> fringe = new Dictionary<int, List<Hex>>(); //layered list of fringe hexs
        fringe.Add(0, new List<Hex>());
        fringe[0].Add(standingHex);
        List<Hex> possibleMoves = new List<Hex>(); //hexs the agent can actually move to

        for(int i = 1; i <= moveDistance; i++)
        {
            fringe.Add(i, new List<Hex>());
            foreach(Hex h in fringe[i - 1])
            {
                for(int j = 0; j < 6; j++) //checks each direction
                {
                    Hex neighbor = mapLocal.getNeigbor(h, j); //returns neighbor that is in bounds and not NULL type
                    if(neighbor != null) //got a hex
                    {
                        if (!visited.Contains(neighbor)) //hex has not be visited yet
                        {
                            visited.Add(neighbor);
                            if (!neighbor.isSolid() && !(neighbor.occupant != null && neighbor.occupant.Alligence != this.alligence))
                            {
                                fringe[i].Add(neighbor);
                                if(neighbor.occupant == null) //no one is on the hex
                                {
                                    possibleMoves.Add(neighbor);
                                }
                            }
                        }
                    }
                }
            }
        }
        return possibleMoves.ToArray();
    }

    /// <summary>
    /// Move agent to hex
    /// </summary>
    public virtual void Move(Hex newHex)
    {
        Debug.Log("(" + newHex.X + ", " + newHex.Y + ", " + newHex.Z + ") (" + newHex.Row + ", " + newHex.Col + ")");
        int dist = mapLocal.distanceBetween(standingHex, newHex);

        if (gameController != null) //agent is in a game
        {
            if (gameController.theMap.hexExists(newHex.X, newHex.Y, newHex.Z))//hex exist to move to
            {
                if (dist <= moveDistance && !newHex.isSolid()) //hex within distance and not solid
                {
                    gameController.theMap.getHex(x, y, z).occupant = null; //remove from start hex
                    gameController.theMap.getHex(newHex.X, newHex.Y, newHex.Z).occupant = this; //new hex know something is now on it
                    StandingHex = newHex;
                    setLocation(newHex.X, newHex.Y, newHex.Z); //agent knows where it is
                    GameObject g = gameController.theMap.getHex(newHex.X, newHex.Y, newHex.Z).gameObject;
                    this.gameObject.transform.position = new Vector3(g.transform.position.x, g.transform.position.y + yOffset, g.transform.position.z);  //agent's gameObjects move to proper location
                    return;
                }
                else
                {
                    Debug.LogError("hex not within range");
                    return;
                }
            }
            Debug.LogError("Hex: " + newHex + " does not exist");
            return;
        }
        Debug.LogError("game Controller not found");
    }

    public virtual void spawnIn(Hex newHex, GameController gCon)
    {
        if (newHex == null) Debug.LogError("newHex not found");
        if (gCon == null) Debug.LogError("game controller not found");
        //gCon.theMap.getHex(newHex.X, newHex.Y, newHex.Z).occupant = this; //NULL REFERENCE ERROR
        setLocation(newHex.X, newHex.Y, newHex.Z); //agent knows where it is
        GameObject g = gCon.theMap.getHex(newHex.X, newHex.Y, newHex.Z).gameObject;
        standingHex = newHex;
        this.gameObject.transform.position = new Vector3(g.transform.position.x, g.transform.position.y + yOffset, g.transform.position.z);
    }

    public virtual void pickUpBall()
    {



    }
}
