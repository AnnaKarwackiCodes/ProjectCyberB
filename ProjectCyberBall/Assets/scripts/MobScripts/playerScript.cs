using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : agentScript {


    /// <summary>
    /// the player
    /// </summary>
    private int mana;
    private bool canPunch;
    private bool canMove;
    private GameObject selectedObj;
    private string action;
    private List<GameObject> allMinions;
    private int maxMin; //max number of minions a player can have out
    private int curNumMins; //total number of player minions on the field;
    private int bigSumCost;
    private int smolSumCost;
    private int fireBallCost;
    private int useBallCost;

    public GameObject ray;
    public GameObject smolMinion;
    public GameObject bigMinion;
    public mobBase selectedMinion;
    public GameObject endTurnPop;

    private bool endPopCreate;
    private GameObject etPop;
    private bool selectorOn;

    private bool testBool;

    // Use this for initialization
    new void Start () {
        base.Start();
        mana = 10; //amount of mana the player will have may change
        maxMin = 10; //to make changing this easier
        canPunch = true;
        allMinions = new List<GameObject>();

        //StandingHex = mapReference.map[1, 1];

        bigSumCost = 3;
        smolSumCost = 2;
        fireBallCost = 3;
        useBallCost = 1;
        canMove = true;
        MoveDistance = 2;
        Alligence = true;

        endPopCreate = false;
        selectorOn = false;

        testBool = false;
    }

    void Awake() {

        //GameObject gController = GameObject.Find("Game Controller");
        //mapReference = gController.GetComponent<Map>();

    }

	// Update is called once per frame
	void Update () {
        //base.Update();
        if (gameController.PlayersTurn)
        {
            switch (action)
            {
                case "Big":
                    SummonBig();
                    break;
                case "Small":
                    SummonSmall();
                    break;
                case "Move":
                    MovePlayer();
                    break;
                case "Move Boi":
                    MinionInteract("Move");
                    break;
                case "Boi Attack":
                    MinionInteract("Attack");
                    break;
                case "Pass Ball to":
                    break;
            }

            if (Input.GetAxis("Right_Grip_Button") == 1)
            {
                //MinionInteract();
                ray.GetComponent<RayCasting>().SelectingObj(5);
            }
            else
            {
                ray.GetComponent<RayCasting>().Line = false;
                gameObject.GetComponent<MotionControllers>().RemoveUI();
                gameObject.GetComponent<MotionControllers>().RemoveHighlight();
                action = "";
                selectedMinion = null;
                selectedObj = null;
            }
            
            for(int i = 0; i < curNumMins; i++)
            {
                Debug.Log(allMinions[i].GetComponent<mobBase>().Type + " " + allMinions[i].GetComponent<mobBase>().ArrayPos + " " + allMinions[i].GetComponent<mobBase>().Health);
            }

            endTurn();
        }

        if(this.hasBall && standingHex.Type == Hex.TYPE.END && movementPath.Count <= 0) //Victory condition
        {
            gameController.gameWin = 1;
        }

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
                    //movementPath.AddRange(mapLocal.pathfinding(standingHex, newHex));//list hex that agent needs to visit while headin to new location
                    standingHex = newHex;
                    setLocation(newHex.X, newHex.Y, newHex.Z); //agent knows where it is
                    GameObject g = gameController.theMap.getHex(newHex.X, newHex.Y, newHex.Z).gameObject;
                    this.gameObject.transform.position = new Vector3(g.transform.position.x, g.transform.position.y + yOffset, g.transform.position.z);  //agent's gameObjects move to proper location

                    //info orb movement stuff
                    if (hasBall) //holding the ball
                    {
                        gameController.theInfo.GetComponent<InfoBall>().Move(newHex);
                    }
                    else if (standingHex.Equals(gameController.theInfo.GetComponent<InfoBall>().StandingHex)) //if not holding ball and on same hex as ball
                    {
                        pickUpBall();
                    }

                    return;
                }
                else
                {
                    Debug.LogError("Hex: " + newHex + " is not within range of movement");
                    return;
                }
            }
            Debug.LogError("game Controller not found");
        }
    }

    public void SummonBig()
    {
        //use up certain amount of mana
        
        Debug.Log("Summon Big");
        //ray.GetComponent<RayCasting>().SelectingObj(9, "Hex");
        if(selectedObj != null)
        {
            mana -= bigSumCost; //place holder value
            action = "";
            allMinions.Add(Instantiate(bigMinion, (selectedObj.transform.position + new Vector3(0,1.2f,0)), new Quaternion(0, 0, 0, 0)));
            //allMinions[curNumMins].GetComponent<agentScript>().Move(selectedObj.GetComponent<Hex>());
            allMinions[curNumMins].GetComponent<agentScript>().mapLocal = GameObject.Find("Game Controller").GetComponent<GameController>().theMap;
            allMinions[curNumMins].GetComponent<agentScript>().spawnIn(selectedObj.GetComponent<Hex>(), this.gameController);
            allMinions[curNumMins].GetComponent<mobBase>().Alligence = true;
            allMinions[curNumMins].GetComponent<mobBase>().ArrayPos = curNumMins;
            curNumMins++;
            selectedObj = null;
        }
    }

    public void SummonSmall()
    {
        //use up certain amount of mana
        
        Debug.Log("Summon Small");
        //ray.GetComponent<RayCasting>().SelectingObj(9, "Hex");
        if (selectedObj != null)
        {
            mana -= smolSumCost; //place holder value
            action = "";
            allMinions.Add(Instantiate(smolMinion, (selectedObj.transform.position + new Vector3(0, .5f, 0)), new Quaternion(0, 0, 0, 0)));
            allMinions[curNumMins].GetComponent<agentScript>().mapLocal = GameObject.Find("Game Controller").GetComponent<Map>();
            allMinions[curNumMins].GetComponent<agentScript>().spawnIn(selectedObj.GetComponent<Hex>(), this.gameController);
            allMinions[curNumMins].GetComponent<mobBase>().Alligence = true;
            allMinions[curNumMins].GetComponent<mobBase>().ArrayPos = curNumMins;
            curNumMins++;
            selectedObj = null;
        }
    }

    public void CastFireball()
    {
        //use up certain amount of mana
        mana -= fireBallCost; //place holder value
    }

    public void PunchMinion()
    {
        //this can only be used once per turn/ change total amount of times used
        canPunch = false;
    }

    //this is for both passing the ball and picking up the ball
    //if the player has the ball then you are passing the ball
    //if the player doesnt have the ball then you are picking up the ball
    public void UseBall()
    {

        int range = mapLocal.distanceBetween(StandingHex, selectedMinion.StandingHex);

        if (range <= 3 && selectedMinion.HasBall == false)//temperary value of range
        {
            //passing the ball to someone

            this.selectedMinion.HasBall = true;

            //you will need to have a minion in range to pass the ball
            //once you select the minion then the ball gets passed to them
            HasBall = false;
        }
        else if (!HasBall)
        {
            //aquire the ball
            HasBall = true;
        }
        mana += useBallCost; // this is a temp value
    }
     public void MovePlayer()
    {
        //use the raycast to select spot to move
        //ray.GetComponent<RayCasting>().SelectingObj(9, "Hex");
        //move to that spot
        if(selectedObj != null)
        {
            action = "";
            //this.gameObject.transform.position = selectedObj.transform.position; //this was for testing
            Move(selectedObj.GetComponent<Hex>()); //this is for when in use
            selectedObj = null;//clear it out
            canMove = false;
        }
    }

    public void newTurn()
    {
        canMove = true;
        mana = 10;
        canPunch = true;
        selectedObj = null;
        for(int i = 0; i < allMinions.Count && i < maxMin; i++)
        {
            if(allMinions[i] != null)
            {
                allMinions[i].GetComponent<mobBase>().CanMove = true;
            }
        }
    }

    public void endTurn() //doing this will end the player turn
    {
        if(Input.GetAxis("Left_Trigger") == 1 && Input.GetAxis("Right_Trigger") == 1)
        {
            if (!endPopCreate)
            {
                etPop = Instantiate(endTurnPop, transform.GetChild(2).GetChild(1).transform.position, new Quaternion(0, 0, 0, 0),transform.GetChild(2).GetChild(1).transform);
                endPopCreate = true;
            }
            //pop up an end turn? menu
            if(Input.GetAxis("Left_Grip_Button") == 1 && Input.GetAxis("Right_Grip_Button") == 1)
            {
                gameController.PlayersTurn = false;
                if (endPopCreate)
                {
                    Destroy(etPop);
                    endPopCreate = false;
                }
            }
        }
        else
        {

            if (endPopCreate)
            {
                Destroy(etPop);
                endPopCreate = false;
            }
        }
    }

    public void MinionInteract(string action)
    {

        ray.GetComponent<RayCasting>().BoiFind(10, action);
        Debug.Log("Selecting new hex");
        if (selectedMinion != null && selectedObj != null && selectedObj.tag == "Hex" && action == "Move" && Input.GetAxis("Right_Trigger") == 1)
            {
                Debug.Log("On a hex");
                selectedMinion.Move(selectedObj.GetComponent<Hex>());
                //Input.ResetInputAxes();
                selectedMinion.Selected = false;
                selectedMinion.CanMove = false;
                selectedObj = null;
                selectedMinion = null;
                action = "";
            gameObject.GetComponent<MotionControllers>().RemoveHighlight();
            }
        else if(selectedObj != null && selectedObj.tag == "Enemy" && action == "Boi Attack")
        {
            //do later when there are actually baddies to attack
        }
    }

    public void RemoveBoi(int pos)
    {
        Debug.Log("Removing boi");
        //allMinions[pos].GetComponent<mobBase>().StandingHex.occupant = null;
        Destroy(allMinions[pos]);
        allMinions.RemoveAt(pos);
        curNumMins--;
        if (curNumMins < 0) curNumMins = 0;
        for(int i = 0; i < curNumMins; i++)
        {
            allMinions[i].GetComponent<mobBase>().ArrayPos = i;
        }
    }

    //get setters
    /// <summary>
    /// Players mana pool
    /// used to cast spells
    /// </summary>
    public int Mana
    {
        get { return mana; }
        set { mana = value; }
    }
    /// <summary>
    /// Whether player can make an attack
    /// </summary>
    public bool CanPunch
    {
        get { return canPunch; }
        set { canPunch = value; }
    }
    /// <summary>
    /// Stores selected tile
    /// </summary>
    public GameObject SelectedObj
    {
        set { selectedObj = value; }
    }
    /// <summary>
    /// ?
    /// </summary>
    public string Action
    {
        get { return action; }
        set { action = value; }
    }

    public mobBase SelectedMinion
    {
        set { selectedMinion = value; }
    }

    public int BigSumCost
    {
        get { return bigSumCost; }
    }
    public int SmolSumCost
    {
        get { return smolSumCost; }
    }
    public int FireBallCost
    {
        get { return fireBallCost; }
    }
    public int UseBallCost
    {
        get { return useBallCost; }
    }
    public bool CanMove
    {
        get { return canMove; }
    }
    public List<GameObject> AllMinions
    {
        get { return allMinions; }
    }
}
