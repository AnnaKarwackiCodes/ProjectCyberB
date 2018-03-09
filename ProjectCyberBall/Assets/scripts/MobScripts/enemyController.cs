using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour {

    private int goal; //what is the current goal of the enemies
    private int mana;
    private int manaMax;
    private GameObject[] allEnemies;
    private Hex[] spawnHexs;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void turn()
    {
        newTurn();
        determineGoal();
        spawnEnemies();
        moveEnemies();
        attackWithEnemies();
        endTurn();
    }

    private void newTurn()
    {

    }

    public void endTurn() //doing this will end the player turn
    {

    }

    private void determineGoal()
    {

    }

    private void spawnEnemies()
    {

    }

    private void moveEnemies()
    {

    }

    private void attackWithEnemies()
    {

    }
}
