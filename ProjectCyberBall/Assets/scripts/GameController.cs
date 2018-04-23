using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Map theMap;
    private enemyController enemyControl;
    public GameObject userPrefab;
    public GameObject infoPrefab;
    public GameObject pillarPrefab;
    private GameObject theUser;
    public GameObject theInfo;
    private bool userIsIn = false;
    [SerializeField]private bool playersTurn;
    private bool inGame;
    public int turnNum; //number of turns that have happened
    public GameObject menu;
    private GameObject myMenu;
    private bool beforeGame;
    /// <summary>
    /// shows if either side has won the game
    /// 0 = still playing
    /// 1 = player wins
    /// -1 = enemy wins
    /// </summary>
    public int gameWin = 0;

	// Use this for initializations
	void Awake () {
        theMap = gameObject.GetComponent<Map>();
        enemyControl = gameObject.GetComponent<enemyController>();
        theUser = Instantiate(userPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        theInfo = Instantiate(infoPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        Debug.Log("awake");
    }

    void Start()
    {
        Debug.Log("start");
        playersTurn = true;
        //inGame = true;
        inGame = false;
        beforeGame = true;
        turnNum = 1;
    }
	
	// Update is called once per frame
	void Update () {
		if(theMap.map != null && !userIsIn)
        {
            theUser.GetComponent<playerScript>().mapLocal = theMap;
            enemyControl.mapLocal = theMap;
            Hex[] startHex = theMap.getHexsWithType(Hex.TYPE.START);
            if (startHex.Length == 1) //moves player to start hex
            {
                theUser.GetComponent<playerScript>().spawnIn(startHex[0], this);
            }
            else { theUser.GetComponent<playerScript>().spawnIn(theMap.map[0, 0], this); }

            theInfo.GetComponent<InfoBall>().mapLocal = theMap;
            enemyControl.theInfo = theInfo.GetComponent<InfoBall>();
            startHex = theMap.getHexsWithType(Hex.TYPE.INFO);
            if (startHex.Length == 1) //moves info to info hex
            {
                theInfo.GetComponent<InfoBall>().spawnIn(startHex[0], this);
            }
            else { theInfo.GetComponent<InfoBall>().spawnIn(theMap.map[0, 0], this); }

            userIsIn = true;
            updateObjective();
        }

        //game loop
        if (inGame)
        {
            if(gameWin == 1)
            {
                Debug.Log("Player WINS!!!");
                inGame = false;
                theUser.GetComponent<MotionControllers>().RemoveUI();
                return;
            }
            else if(gameWin == -1)
            {
                Debug.Log("Baddies WINS!!!");
                inGame = false;
                theUser.GetComponent<MotionControllers>().RemoveUI();
                return;
            }

            if (playersTurn) //it is the player's turn
            {
                //Debug.Log("Player turn");
            }
            else //it is the enemies turn
            {
                if (enemyControl.turnSubPhase == 0)
                {
                    //call enemies to do their thing
                    Debug.Log("Baddie turn");
                    //enemyControl.turn();
                    enemyControl.newTurn();
                }
                //when finished set it up for the player to be able to do their thing
                if (enemyControl.turnSubPhase == enemyController.SUB_END_EXC)
                {
                    enemyControl.turnSubPhase = 0;
                    theUser.GetComponent<playerScript>().newTurn();
                    changeTurn(true);
                }
            }
        }
        if (!inGame)
        {
            //menustuff
            if (beforeGame)
            {
                if (myMenu == null)
                {
                    myMenu = Instantiate(menu, new Vector3(theUser.transform.position.x, 4, 5), new Quaternion(0, 0, 0, 0));
                    myMenu.transform.GetChild(0).GetComponent<Text>().text = "Project Cyber B";
                }
                if (theUser.GetComponent<playerScript>().Action == "Lets go")
                {
                    inGame = true;
                    beforeGame = false;
                    Destroy(myMenu);
                }
            }
            else
            {
                if (myMenu == null)
                {
                    Hex[] startHex = theMap.getHexsWithType(Hex.TYPE.START);
                    theUser.GetComponent<playerScript>().spawnIn(startHex[0], this);
                    myMenu = Instantiate(menu, new Vector3(theUser.transform.position.x, 4, 5), new Quaternion(0, 0, 0, 0));
                    if (gameWin == 1)
                    {
                        myMenu.transform.GetChild(0).GetComponent<Text>().text = "YOU WIN";
                    }
                    else
                    {
                        myMenu.transform.GetChild(0).GetComponent<Text>().text = "YOU LOSE";
                    }
                    
                }
                if(theUser.GetComponent<playerScript>().Action == "Reset")
                {
                    theMap = gameObject.GetComponent<Map>();
                    enemyControl = gameObject.GetComponent<enemyController>();
                    userIsIn = false;

                }
            }
        }
    }

    public bool PlayersTurn
    {
    //    set { playersTurn = value; }
        get { return playersTurn; }
    }
    public bool InGame
    {
        get { return inGame; }
    }
    public bool BeforeGame
    {
        get { return beforeGame; }
    }

    public void changeTurn(bool newTurn) //changes the turn and increases the turn number
    {
        if(playersTurn != newTurn)
        {
            turnNum++;
        }
        playersTurn = newTurn;
    }

    public void updateObjective()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Pillar")) { DestroyImmediate(go); }
        if (theUser.GetComponent<playerScript>().HasBall)
        {
            foreach(Hex h in theMap.getHexsWithType(Hex.TYPE.END))
            {
                GameObject go = Instantiate(pillarPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                go.transform.position = new Vector3(h.gameObject.transform.localPosition.x, -4, h.gameObject.transform.localPosition.z);
            }
        }
        else
        {
            GameObject go = Instantiate(pillarPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            Vector3 v = theInfo.GetComponent<InfoBall>().StandingHex.gameObject.transform.localPosition;
            go.transform.position = new Vector3(v.x, -4, v.z);
            //go.transform.position = theInfo.GetComponent<InfoBall>().StandingHex.gameObject.transform.localPosition;
        }
    }
}
