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

    /// <summary>
    /// The full turn that the enemy takes
    /// </summary>
    private void turn()
    {
        newTurn();
        determineGoal();
        spawnEnemies();
        moveEnemies();
        attackWithEnemies();
        endTurn();
    }

    /// <summary>
    /// resets everything for the beginning of the turn
    /// </summary>
    private void newTurn()
    {

    }

    /// <summary>
    /// does anything at the end of the turn
    /// </summary>
    public void endTurn() //doing this will end the player turn
    {

    }

    /// <summary>
    /// Figures out what the overaching goal is the enemy need to complete
    /// </summary>
    private void determineGoal()
    {

    }

    /// <summary>
    /// Figures out how many, what type, and where enemies should be spawned
    /// </summary>
    private void spawnEnemies()
    {

    }

    /// <summary>
    /// moves each enemy towards their goal
    /// </summary>
    private void moveEnemies()
    {

    }

    /// <summary>
    /// attacks player or minion of next to the enemy
    /// </summary>
    private void attackWithEnemies()
    {

    }
}
