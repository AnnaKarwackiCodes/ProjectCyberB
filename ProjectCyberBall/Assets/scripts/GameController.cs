using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Map theMap;
    public GameObject userPrefab;
    private GameObject theUser;
    private bool userIsIn = false;
    private bool playersTurn;
    private bool inGame;

	// Use this for initializations
	void Awake () {
        theMap = gameObject.GetComponent<Map>();
        theUser = Instantiate(userPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
    }

    void Start()
    {
        Debug.Log("start");
        playersTurn = true;
        inGame = true;

    }
	
	// Update is called once per frame
	void Update () {
		if(theMap.map != null && !userIsIn)
        {
            theUser.GetComponent<playerScript>().MapLocal = theMap;
            theUser.GetComponent<playerScript>().GetComponent<playerScript>().spawnIn(theMap.map[0, 0], this);
            userIsIn = true;
        }

        //game loop
        if (inGame)
        {
            if (playersTurn)
            {
                Debug.Log("Player turn");
            }
            else
            {
                //call enemies to do their thing
                Debug.Log("Baddie turn");

                //when finished set it up for the player to be able to do their thing
                theUser.GetComponent<playerScript>().newTurn();
                playersTurn = true;
            }
        }
    }

    public bool PlayersTurn
    {
        set { playersTurn = value; }
        get { return playersTurn; }
    }
}
