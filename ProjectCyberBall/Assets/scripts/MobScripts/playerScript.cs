using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playerScript : agentScript {


    /// <summary>
    /// the player
    /// </summary>
    private int mana;
    private bool canPunch; 
    private GameObject selectedObj;
    private string action;
    private List<GameObject> allMinions;
    private int maxMin; //max number of minions a player can have out
    private int curNumMins; //total number of player minions on the field;
    private int bigSumCost;
    private int smolSumCost;
    private int fireBallCost;
    private int useBallCost;
    private int fireBallDam;

    public GameObject ray;
    public GameObject smolMinion;
    public GameObject bigMinion;
    public mobBase selectedMinion;
    public GameObject endTurnPop;

    private bool endPopCreate;
    private GameObject etPop;
    private bool selectorOn;

    private bool endOfTurnButtonHold;

    public GameObject FireBall;
    private GameObject myFB;
    private bool createFireball;

    // Use this for initialization
    new void Start () {
        base.Start();
        mana = 10; //amount of mana the player will have may change
        maxMin = 10; //to make changing this easier
        Health = 10;
        canPunch = true;
        allMinions = new List<GameObject>();
        createFireball = false;

        //StandingHex = mapReference.map[1, 1];

        bigSumCost = 3;
        smolSumCost = 2;
        fireBallCost = 3;
        useBallCost = 1;
        fireBallDam = 3;
        CanMove = true;
        MoveDistance = 2;
        Alligence = true;

        endPopCreate = false;
        selectorOn = false;

        endOfTurnButtonHold = false;
    }

    void Awake() {

        //GameObject gController = GameObject.Find("Game Controller");
        //mapReference = gController.GetComponent<Map>();

    }

	// Update is called once per frame
	void Update () {
        //base.Update();
        transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = "Health: " + Health;
        transform.GetChild(2).GetChild(1).GetChild(1).GetComponent<Text>().text = "Mana: " + mana;

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
                    if (selectedMinion != null && selectedMinion.CanMove) { MinionInteract("Move"); }
                    break;
                case "Boi Attack":
                    MinionInteract("Attack");
                    break;
                case "Pass Ball to":
                    break;
                case "Fireball":
                    UseFireball();
                    break;
                default:
                    ray.GetComponent<RayCasting>().RemoveHighlight();
                    break;
            }

            if (Input.GetAxis("Right_Grip_Button") == 1 && (Input.GetAxis("Left_Trigger") != 1 && Input.GetAxis("Right_Trigger") != 1))
            {
                //MinionInteract();
                ray.GetComponent<RayCasting>().SelectingObj(5);
            }
            else
            {
                ray.GetComponent<RayCasting>().Line = false;
                gameObject.GetComponent<MotionControllers>().RemoveUI();
                gameObject.GetComponent<MotionControllers>().RemoveHighlight();
                ray.GetComponent<RayCasting>().RemoveHighlight();
                action = "";
                selectedMinion = null;
                selectedObj = null;
            }

            if (createFireball && myFB.GetComponent<FireBall>().HitTarget)
            {
                Destroy(myFB);
                createFireball = false;
                action = "";
                selectedMinion = null;
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
    public override void Move(Hex newHex)
    {
        //Debug.Log("(" + newHex.X + ", " + newHex.Y + ", " + newHex.Z + ") (" + newHex.Row + ", " + newHex.Col + ")");
        int dist = mapLocal.distanceBetween(standingHex, newHex);

        if (gameController != null) //agent is in a game
        {
            if (CanMove && gameController.theMap.hexExists(newHex.X, newHex.Y, newHex.Z))//hex exist to move to
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
                    CanMove = false;
                    return;
                }
                else
                {
                    Debug.LogError("Hex: " + newHex + " is not within range of movement");
                    return;
                }
            }
            return;
        }
        Debug.LogError("game Controller not found");
    }

    public void SummonBig()
    {
        //use up certain amount of mana
        
        Debug.Log("Summon Big");
        if(selectedObj != null)
        {
            if (selectedObj.GetComponent<Hex>().occupant == null)
            {
                mana -= bigSumCost; //place holder value
                action = "";
                allMinions.Add(Instantiate(bigMinion, (selectedObj.transform.position + new Vector3(0, 1.2f, 0)), new Quaternion(0, 0, 0, 0)));
                allMinions[curNumMins].GetComponent<agentScript>().mapLocal = GameObject.Find("Game Controller").GetComponent<GameController>().theMap;
                allMinions[curNumMins].GetComponent<agentScript>().spawnIn(selectedObj.GetComponent<Hex>(), this.gameController);
                allMinions[curNumMins].GetComponent<mobBase>().Alligence = true;
                allMinions[curNumMins].GetComponent<mobBase>().ArrayPos = curNumMins;
                curNumMins++;
                selectedObj = null;
            }
        }
    }

    public void SummonSmall()
    {
        //use up certain amount of mana
        
        Debug.Log("Summon Small");
        if (selectedObj != null)
        {
            if (selectedObj.GetComponent<Hex>().occupant == null) {
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
        //move to that spot
        if(selectedObj != null)
        {
            action = "";
            //this.gameObject.transform.position = selectedObj.transform.position; //this was for testing
            Move(selectedObj.GetComponent<Hex>()); //this is for when in use
            selectedObj = null;//clear it out
        }
    }

    public void newTurn()
    {
        CanMove = true;
        mana = 10;
        canPunch = true;
        selectedObj = null;
        for(int i = 0; i < allMinions.Count && i < maxMin; i++)
        {
            if(allMinions[i] != null)
            {
                allMinions[i].GetComponent<mobBase>().CanMove = true;

                allMinions[i].GetComponent<mobBase>().CanAttack = true;
            }
        }
    }

    public void endTurn() //doing this will end the player turn
    {
        if(Input.GetAxis("Left_Trigger") == 1 && Input.GetAxis("Right_Trigger") == 1)
        {
            if (!endPopCreate) //creates end of turn text
            {
                etPop = Instantiate(endTurnPop, transform.GetChild(2).GetChild(1).transform.position, new Quaternion(0, 0, 0, 0),transform.GetChild(2).GetChild(1).transform);
                endPopCreate = true;
            }
            //pop up an end turn? menu
            if(Input.GetAxis("Left_Grip_Button") == 1 && Input.GetAxis("Right_Grip_Button") == 1)
            {
                if (!endOfTurnButtonHold)
                {
                    Debug.Log("ending turn");
                    endOfTurnButtonHold = true;
                    gameController.changeTurn(false);
                    if (endPopCreate)
                    {
                        Destroy(etPop);
                        endPopCreate = false;
                    }
                }
            }
            else { endOfTurnButtonHold = false; }
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
        if (action == "Move" && selectedMinion != null && selectedObj != null && selectedObj.tag == "Hex" && Input.GetAxis("Right_Trigger") == 1)
            {
                Debug.Log("On a hex");
                selectedMinion.Move(selectedObj.GetComponent<Hex>());
                //Input.ResetInputAxes();
                selectedMinion.Selected = false;
                selectedMinion.CanMove = false;
                selectedObj = null;
                selectedMinion = null;
                action = "NOTHING";
                gameObject.GetComponent<MotionControllers>().RemoveHighlight();
                ray.GetComponent<RayCasting>().RemoveHighlight();
            }
        else if(action == "Attack" && selectedMinion != null && selectedObj != null && selectedObj.tag == "Enemy" &&  Input.GetAxis("Right_Trigger") == 1)
        {
            //do later when there are actually baddies to attack
            Debug.Log("bad boi select");
            selectedMinion.mobAttack(selectedObj.GetComponent<agentScript>());
            selectedMinion.Selected = false;
            selectedMinion.CanMove = false;
            selectedObj = null;
            selectedMinion = null;
            action = "NOTHING";
        }
    }
    public void UseFireball()
    {
        Debug.Log("using fireball");
        if(!createFireball)
        {
            Debug.Log("creating fireball");
            mana -= fireBallCost;
            myFB = Instantiate(FireBall, gameObject.transform.GetChild(2).transform.position, new Quaternion(0, 0, 0, 0));
            myFB.GetComponent<FireBall>().Target = selectedMinion.gameObject; 
            createFireball = true;
            
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
        get { return selectedMinion; }
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
    public int FireBallDam
    {
        get { return fireBallDam; }
    }
    public int UseBallCost
    {
        get { return useBallCost; }
    }
    public List<GameObject> AllMinions
    {
        get { return allMinions; }
    }
}
