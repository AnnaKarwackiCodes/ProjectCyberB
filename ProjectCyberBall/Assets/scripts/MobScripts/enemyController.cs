using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{

    public GameController gameController;
    public Map mapLocal;
    public InfoBall theInfo;

    private static int GOAL_ATTACK_TARGET = 1;
    private static int GOAL_ATTACK_GENERAL = 2;
    private static int GOAL_DEFEND_TARGET = 3;
    private static int GOAL_RETURN_BALL = 4;

    private int goal; //what is the current goal of the enemies
    private Hex goalTarget; //location of the goal
    private int mana;
    private int manaMax;
    private bool enemyDiedLastTurn = false;
    private GameObject[] bigEnemies;
    private GameObject[] lilEnemies;
    private Hex[] spawnHexs;
    private Hex infoHex;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        mapLocal = gameController.theMap;
        spawnHexs = mapLocal.getHexsWithType(Hex.TYPE.SPAWN);
        infoHex = mapLocal.getHexsWithType(Hex.TYPE.INFO)[0];
    }

    // Update is called once per frame
    void Update()
    {

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
        enemyDiedLastTurn = false;
        int temptMana = manaMax;
        foreach (GameObject enemy in bigEnemies) //resets big bois
        {
            enemy.GetComponent<mobBase>().CanMove = true; //resets movement
            temptMana -= 3;
        }
        foreach (GameObject enemy in lilEnemies) //resets lil bois
        {
            enemy.GetComponent<mobBase>().CanMove = true; //resets movement
            temptMana -= 2;
        }
        if(mana < temptMana) { enemyDiedLastTurn = true; }
        mana = temptMana;
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
        //attack target
        ///player/minion has info --> attack player/minion
        //attack general
        ///player/minion are close to ball --> attack closest target
        //defend target
        ///player/minion are far away from ball --> defend target 
        //return ball
        ///enemy has info --> return to info hex
        ///rest of enemies defend info holder
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
        List<GameObject> allEnemies = new List<GameObject>();
        allEnemies.AddRange(bigEnemies);
        allEnemies.AddRange(lilEnemies);
        foreach (GameObject enemy in allEnemies)
        {
            majBoiScript boi = enemy.GetComponent<majBoiScript>();
            List<Hex> neigbors = new List<Hex>();
            for(int i = 0; i < 6; i++)
            {
                Hex neigbor = mapLocal.getNeigbor(boi.StandingHex, i);
                if(neigbor != null && neigbor.occupant != null && neigbor.occupant.Alligence != boi.Alligence)
                {
                    neigbors.Add(neigbor); //neigbor tile had player/minion on it
                }
            }
            if(neigbors.Count == 0) { continue; } //nothing to attack
            else if(neigbors.Count == 1) { boi.mobAttack(neigbors[0].occupant); continue; }//only one thing to attack
            else
            {
                if (goal == GOAL_ATTACK_TARGET && neigbors.Contains(goalTarget)) {
                    boi.mobAttack(goalTarget.occupant);
                    continue;
                }
                //sorts to attack target with lowest health
                neigbors.Sort(delegate (Hex a, Hex b)
                {
                    if(a.occupant.Health < b.occupant.Health) { return -1; }
                    else if(a.occupant.Health > b.occupant.Health) { return 1; }
                    else //health are the same, pick closest to the info
                    {
                        if(mapLocal.distanceBetween(a, theInfo.StandingHex) == mapLocal.distanceBetween(b, theInfo.StandingHex)) { return 0; } //same distance to info orb
                        return mapLocal.distanceBetween(a, theInfo.StandingHex) == mapLocal.distanceBetween(b, theInfo.StandingHex) ? -1 : 1; //closer player/minion to info orb gets attacked
                    }
                });
                boi.mobAttack(neigbors[0].occupant);
            }
        }
    }
}
