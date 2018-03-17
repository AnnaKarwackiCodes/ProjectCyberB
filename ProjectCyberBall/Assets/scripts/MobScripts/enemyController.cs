using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour {

    public GameController gameController;
    public Map mapLocal;

    private static int GOAL_ATTACK_TARGET = 1;
    private static int GOAL_ATTACK_GENERAL = 2;
    private static int GOAL_DEFEND_TARGET = 3;
    private static int GOAL_RETURN_BALL = 4;

    private int goal; //what is the current goal of the enemies
    private Hex goalTarget; //location of the goal
    private int mana;
    private int manaMax;
    private GameObject[] allEnemies;
    private Hex[] spawnHexs;
    private Hex infoHex;

    // Use this for initialization
    void Start () {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        mapLocal = gameController.theMap;
        spawnHexs = mapLocal.getHexsWithType(Hex.TYPE.SPAWN);
        infoHex = mapLocal.getHexsWithType(Hex.TYPE.INFO)[0];
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
        foreach (GameObject enemy in allEnemies)
        {
                enemy.GetComponent<mobBase>().CanMove = true; //resets movement
        }
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
