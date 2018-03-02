﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : agentScript {


    /// <summary>
    /// the player
    /// </summary>
    private int mana;
    private bool canPunch;
    private GameObject selectedObj;
    private string action;
    private GameObject[] allMinions;
    private int maxEachMin;
    private int curNumMins; //total number of player minions on the field;

    public GameObject ray;
    public GameObject smolMinion;
    public GameObject bigMinion;
    public mobBase selectedMinion;
    public Map mapReference;

    // Use this for initialization
    void Start () {
        mana = 10; //amount of mana the player will have may change
        maxEachMin = 10; //to make changing this easier
        canPunch = true;
        allMinions = new GameObject[maxEachMin];
    }
	
	// Update is called once per frame
	void Update () {
        if (action == "Move")
        {
            Debug.Log("is moving");
            MovePlayer();
        }
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
        }
	}

    public void SummonBig()
    {
        //use up certain amount of mana
        mana -= 3; //place holder value
        Debug.Log("Summon Big");
        ray.GetComponent<RayCasting>().SelectingObj(10, "Hex");
        if(selectedObj != null)
        {
            action = "";
            allMinions[curNumMins] = Instantiate(bigMinion, (selectedObj.transform.position + new Vector3(0,1.2f,0)), new Quaternion(0, 0, 0, 0));
            //allMinions.Add(Instantiate(bigMinion, new Vector3(300, 300, 300), new Quaternion(0, 0, 0, 0)));
            //allMinions[curNumMins].GetComponent<agentScript>().Move(selectedObj.GetComponent<Hex>());
            curNumMins++;
            selectedObj = null;
        }
    }

    public void SummonSmall()
    {
        //use up certain amount of mana
        mana -= 3; //place holder value
        Debug.Log("Summon Small");
        ray.GetComponent<RayCasting>().SelectingObj(10, "Hex");
        if (selectedObj != null)
        {
            action = "";
            allMinions[curNumMins] = Instantiate(smolMinion, (selectedObj.transform.position + new Vector3(0, .5f, 0)), new Quaternion(0, 0, 0, 0));
            //allMinions.Add(Instantiate(bigMinion, new Vector3(300, 300, 300), new Quaternion(0, 0, 0, 0)));
            //allMinions[curNumMins].GetComponent<agentScript>().Move(selectedObj.GetComponent<Hex>());
            curNumMins++;
            selectedObj = null;
        }
    }

    public void CastFireball()
    {
        //use up certain amount of mana
        mana -= 3; //place holder value
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

        int range = MapLocal.distanceBetween(StandingHex, selectedMinion.StandingHex);

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
        mana += 3; // this is a temp value
    }
     public void MovePlayer()
    {
        //use the raycast to select spot to move
        ray.GetComponent<RayCasting>().SelectingObj(10, "Hex");
        //move to that spot
        if(selectedObj != null)
        {
            action = "";
            this.gameObject.transform.position = selectedObj.transform.position; //this was for testing
            //Move(selectedObj.GetComponent<Hex>()); //this is for when in use
            selectedObj = null;//clear it out
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
}
