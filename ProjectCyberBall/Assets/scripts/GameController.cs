﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Map theMap;
    private enemyController enemyControl;
    public GameObject userPrefab;
    public GameObject infoPrefab;
    private GameObject theUser;
    public GameObject theInfo;
    private bool userIsIn = false;
    [SerializeField]private bool playersTurn;
    private bool inGame;

	// Use this for initializations
	void Awake () {
        theMap = gameObject.GetComponent<Map>();
        enemyControl = gameObject.GetComponent<enemyController>();
        theUser = Instantiate(userPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        theInfo = Instantiate(infoPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));

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
            //Hex[] path = theMap.pathfinding(theMap.getHex(0, -7, 7), theMap.getHex(13, -14, 1));
            //string s = "";
            //for(int i = 0; i < path.Length; i++) //path finding testing
            //{
            //    s += path[i].ToString() + " -> ";
            //    path[i].gameObject.GetComponent<MeshRenderer>().material = path[i].INFO_R;
            //}
            //Debug.Log(s);
        }

        //game loop
        if (inGame)
        {
            if (playersTurn)
            {
                //Debug.Log("Player turn");
            }
            else
            {
                //call enemies to do their thing
                Debug.Log("Baddie turn");
                enemyControl.turn();
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
