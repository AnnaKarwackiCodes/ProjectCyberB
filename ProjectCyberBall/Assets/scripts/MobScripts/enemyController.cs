using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{

    public GameController gameController;
    public Map mapLocal;
    public InfoBall theInfo;

    private const int GOAL_ATTACK_TARGET = 1;
    private const int GOAL_ATTACK_GENERAL = 2;
    private const int GOAL_DEFEND_TARGET = 3;
    private const int GOAL_RETURN_BALL = 4;

    private static int MOD_DEFEND_INFO_RANGE = 3;
    private static int MOD_SMALL_BOI_COST = 2;
    private static int MOD_BIG_BOI_COST = 3;
    private static int MOD_SPAWN_BEFORE_INFO_TAKEN = 2;
    private static int MOD_SPAWN_AFTER_INFO_TAKEN = 3;

    private int goal; //what is the current goal of the enemies
    private Hex goalTarget; //location of the goal
    private int mana;
    private int manaMax = 10;
    private bool enemyDiedLastTurn = false;
    private bool infoBallTaken = false;
    private List<GameObject> bigEnemies;
    private List<GameObject> smallEnemies;
    private Hex[] spawnHexs;
    private Hex infoHex;
    public GameObject smolMinion;
    public GameObject bigMinion;

    //enemy turn arrays //TEST
    public int turnSubPhase = 0;

    public const int SUB_SPAWN_FUN = 1;
    public const int SUB_SPAWN_EXC = 2;
    public const int SUB_MOVE_FUN = 3;
    public const int SUB_MOVE_EXC = 4;
    public const int SUB_ATTACK_FUN = 5;
    public const int SUB_ATTACK_EXC = 6;
    public const int SUB_END_EXC = 7;

    List<char> leftToSpawnBoi;
    List<Hex> leftToSpawnHex;
    List<mobBase> leftToMoveBoi;
    //List<Hex> leftToMoveHex;
    List<mobBase> leftToAttackBoi;
    //List<Hex> leftToAtackHex;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        mapLocal = gameController.theMap;
        spawnHexs = mapLocal.getHexsWithType(Hex.TYPE.SPAWN);
        infoHex = mapLocal.getHexsWithType(Hex.TYPE.INFO)[0];
        bigEnemies = new List<GameObject>();
        smallEnemies = new List<GameObject>();
        mana = manaMax;

        leftToSpawnBoi = new List<char>();
        leftToSpawnHex = new List<Hex>();
        leftToMoveBoi = new List<mobBase>();
        //leftToMoveHex = new List<Hex>();
        leftToAttackBoi = new List<mobBase>();
        //leftToAtackHex = new List<Hex>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.PlayersTurn)
        {
            //spawn
            if (turnSubPhase == SUB_SPAWN_FUN) { spawnEnemies(); turnSubPhase = SUB_SPAWN_EXC; }
            else if (turnSubPhase == SUB_SPAWN_EXC)
            {
                if (leftToSpawnBoi.Count > 0 && leftToSpawnHex.Count > 0)
                {
                    switch (leftToSpawnBoi[0])
                    {
                        case 'b':
                            if (mana - MOD_BIG_BOI_COST >= 0)
                            {
                                GameObject boi = Instantiate(bigMinion, (leftToSpawnHex[0].gameObject.transform.position + new Vector3(0, .5f, 0)), new Quaternion(0, 0, 0, 0));
                                boi.GetComponent<agentScript>().mapLocal = mapLocal;
                                boi.GetComponent<agentScript>().spawnIn(leftToSpawnHex[0], this.gameController);
                                boi.GetComponent<agentScript>().Alligence = false;
                                bigEnemies.Add(boi);
                                mana -= MOD_BIG_BOI_COST;
                            }
                            break;
                        case 's':
                        default:
                            if (mana - MOD_SMALL_BOI_COST >= 0)
                            {
                                //Debug.Log("Spawn smol");
                                GameObject boi = Instantiate(smolMinion, (leftToSpawnHex[0].gameObject.transform.position + new Vector3(0, .5f, 0)), new Quaternion(0, 0, 0, 0));
                                boi.GetComponent<agentScript>().mapLocal = mapLocal;
                                boi.GetComponent<agentScript>().spawnIn(leftToSpawnHex[0], this.gameController);
                                boi.GetComponent<agentScript>().Alligence = false;
                                smallEnemies.Add(boi);
                                mana -= MOD_SMALL_BOI_COST;
                            }
                            break;
                    }
                    leftToSpawnBoi.RemoveAt(0);
                    leftToSpawnHex.RemoveAt(0);
                }
                else { turnSubPhase = SUB_MOVE_FUN; }
            }
            //move
            else if (turnSubPhase == SUB_MOVE_FUN)
            {
                foreach (GameObject go in bigEnemies) { leftToMoveBoi.Add(go.GetComponent<mobBase>()); }
                foreach (GameObject go in smallEnemies) { leftToMoveBoi.Add(go.GetComponent<mobBase>()); }
                turnSubPhase = SUB_MOVE_EXC;
            }
            else if (turnSubPhase == SUB_MOVE_EXC)
            {
                if (leftToMoveBoi[0].CanMove == true) { moveEnemy(leftToMoveBoi[0]); } //calculate movement
                if (leftToMoveBoi[0].MovementPath.Count <= 0) { leftToMoveBoi.RemoveAt(0); } //finished moving
                if (leftToMoveBoi.Count <= 0) { turnSubPhase = SUB_ATTACK_FUN; }
            }
            //attack
            else if (turnSubPhase == SUB_ATTACK_FUN)
            {
                foreach (GameObject go in bigEnemies) { leftToAttackBoi.Add(go.GetComponent<mobBase>()); }
                foreach (GameObject go in smallEnemies) { leftToAttackBoi.Add(go.GetComponent<mobBase>()); }
                turnSubPhase = SUB_ATTACK_EXC;
            }
            else if(turnSubPhase == SUB_ATTACK_EXC)
            {
                attackwithEnemy(leftToAttackBoi[0]);
                leftToAttackBoi.RemoveAt(0);
                if (leftToAttackBoi.Count <= 0) { turnSubPhase = SUB_END_EXC; }
            }
            //end
            else if (turnSubPhase == SUB_END_EXC) { endTurn(); }
            
        }
    }

    /// <summary>
    /// resets everything for the beginning of the turn
    /// </summary>
    public void newTurn()
    {
        enemyDiedLastTurn = false;
        int temptMana = manaMax;
        foreach (GameObject enemy in bigEnemies) //resets big bois
        {
            enemy.GetComponent<mobBase>().CanMove = true; //resets movement
            temptMana -= MOD_BIG_BOI_COST;
        }
        foreach (GameObject enemy in smallEnemies) //resets lil bois
        {
            enemy.GetComponent<mobBase>().CanMove = true; //resets movement
            temptMana -= MOD_SMALL_BOI_COST;
        }
        if(mana < temptMana) { enemyDiedLastTurn = true; }
        mana = temptMana;

        leftToSpawnBoi.Clear();//TEST
        leftToSpawnHex.Clear();//TEST
        determineGoal();//TEST
        turnSubPhase = SUB_SPAWN_FUN;
    }

    /// <summary>
    /// does anything at the end of the turn
    /// </summary>
    private void endTurn() //doing this will end the player turn
    {
        //turnStart = false;
        //turnSubPhase = 0;
    }

    /// <summary>
    /// Figures out what the overaching goal is the enemy need to complete
    /// </summary>
    private void determineGoal()
    {
        //Debug.Log("Determine Goal Start");
        if(theInfo.X != infoHex.X || theInfo.Y != infoHex.Y || theInfo.Z != infoHex.Z) //if the info is not on starting spot
        {
            if(!infoBallTaken) { infoBallTaken = true; }
            //attack target
            ///player/minion has info --> attack player/minion
            if (theInfo.Holder != null && theInfo.Holder.GetComponent<agentScript>().Alligence == true)
            {
                goal = GOAL_ATTACK_TARGET;
                goalTarget = mapLocal.getHex(theInfo.X, theInfo.Y, theInfo.Z);
                return;
            }

            //return ball
            ///enemy has info --> return to info hex
            ///rest of enemies defend info holder
            goal = GOAL_RETURN_BALL;
            goalTarget = mapLocal.getHex(theInfo.X, theInfo.Y, theInfo.Z);
            return;
        }

        Hex[] tilesPM = getTilesWithPM();
        foreach(Hex h in tilesPM)
        {
            if(mapLocal.pathfinding(infoHex, h).Length - 1 <= MOD_DEFEND_INFO_RANGE)//if player/min is within the defend info range of the info
            {
                //attack general
                ///player/minion are close to ball --> attack closest target
                goal = GOAL_ATTACK_GENERAL;
                goalTarget = infoHex;
                return;
            }

            //defend target
            ///player/minion are far away from ball --> defend target 
            goal = GOAL_DEFEND_TARGET;
            goalTarget = mapLocal.getHex(theInfo.X, theInfo.Y, theInfo.Z);
            return;
        }
    }

    /// <summary>
    /// Figures out how many, what type, and where enemies should be spawned
    /// </summary>
    private void spawnEnemies()
    {
        //Debug.Log("Spawn Enemies Start");
        if (mana < MOD_SMALL_BOI_COST) { Debug.Log("enemy not enough Mana: " + mana); return; } //nothing can be spawned because not enough points
        
        ///setup what enemies might be spawned in
        char[] es = { };
        if (infoBallTaken) { es = new char[MOD_SPAWN_AFTER_INFO_TAKEN]; }
        else { es = new char[MOD_SPAWN_BEFORE_INFO_TAKEN]; }

        for(int i = 0; i < es.Length; i++)
        {
            switch (i % 3)
            {
                case 0:
                    es[i] = 's';
                    leftToSpawnBoi.Add('s'); //TEST
                    break;
                case 1:
                    if (enemyDiedLastTurn) { es[i] = 'b'; leftToSpawnBoi.Add('b'); //TEST
                    }
                    else { es[i] = 's'; leftToSpawnBoi.Add('s'); //TEST
                    }
                    break;
                case 2:
                    es[i] = 'b';
                    leftToSpawnBoi.Add('b'); //TEST
                    break;
                default:
                    break;
            }
        }

        //Debug.Log("Enemy Spawn Array Comp: " + es[0]);

        ///gets list of hexs where enemies can be placed
        List<Hex> sh = new List<Hex>(); //list of spawn hexs
        sh.AddRange(spawnHexs);
        sh.Sort(delegate (Hex a, Hex b)
        {
            return mapLocal.pathfinding(a, goalTarget).Length <= mapLocal.pathfinding(b, goalTarget).Length ? -1 : 1;
        });
        for(int i = 0; i < sh.Count;) //remove any tiles that have things on them already
        {
            if (sh[i].occupant != null)
            {
                sh.RemoveAt(i);
                continue;
            }
            i++;
        }
        if(sh.Count <= 0) { Debug.Log("No Spawn"); return; } //no place to put enemies
        leftToSpawnHex.AddRange(sh);//TEST
        //Debug.Log("Enemy Spawn Array Comp: " + sh[0]);

        //spawn enemy time
        for (int i = 0; false && i < es.Length && i < sh.Count; i++)//TEST remove false later
        {
            //Debug.Log("Spawn Enemy " + i + " " + es[i] + " : " + mana);
            switch (es[i])
            {
                case 'b':
                    if(mana - MOD_BIG_BOI_COST >= 0)
                    {
                        GameObject boi = Instantiate(bigMinion, (sh[i].gameObject.transform.position + new Vector3(0, .5f, 0)), new Quaternion(0, 0, 0, 0));
                        boi.GetComponent<agentScript>().mapLocal = mapLocal;
                        boi.GetComponent<agentScript>().spawnIn(sh[i], this.gameController);
                        boi.GetComponent<agentScript>().Alligence = false;
                        bigEnemies.Add(boi);
                        mana -= MOD_BIG_BOI_COST;
                    }
                    break;
                case 's':
                default:
                    if (mana - MOD_SMALL_BOI_COST >= 0)
                    {
                        //Debug.Log("Spawn smol");
                        GameObject boi = Instantiate(smolMinion, (sh[i].gameObject.transform.position + new Vector3(0, .5f, 0)), new Quaternion(0, 0, 0, 0));
                        boi.GetComponent<agentScript>().mapLocal = mapLocal;
                        boi.GetComponent<agentScript>().spawnIn(sh[i], this.gameController);
                        boi.GetComponent<agentScript>().Alligence = false;
                        smallEnemies.Add(boi);
                        mana -= MOD_SMALL_BOI_COST;
                    }
                    break;
            }
        }
        //Debug.Log("Spawn Enemies End");
    }

    private void moveEnemy(mobBase boi)
    {
        switch (goal)
        {
            case GOAL_RETURN_BALL: //minion with info move towards info hex, other mobs move towards minion with info
                Hex holdersHex = null;

                List<Hex> moveToHexs = new List<Hex>();
                if (boi.HasBall)
                {
                        
                    orderToClosest(boi, goalTarget, out moveToHexs);
                    if (!boi.StandingHex.Equals(moveToHexs[0])) { boi.Move(moveToHexs[0]); } //if not already on the tile
                    holdersHex = moveToHexs[0];
                    break;
                }

                moveToHexs = new List<Hex>();
                orderToClosest(boi, holdersHex, out moveToHexs);
                if (!boi.StandingHex.Equals(moveToHexs[0])) { boi.Move(moveToHexs[0]); }//if not already on the tile
               
                break;
            case GOAL_ATTACK_GENERAL: //enemies move towards closest allied force, to hopefully attack

                    List<Hex> closestPM = new List<Hex>();
                    closestPM.AddRange(getTilesWithPM());
                    closestPM.Sort(delegate (Hex a, Hex b) //orders Allied in order of closest to boi 
                    {
                        return mapLocal.pathfinding(a, boi.StandingHex).Length < mapLocal.pathfinding(b, boi.StandingHex).Length ? -1 : 1;
                    });
                    moveToHexs = new List<Hex>();
                    orderToClosest(boi, closestPM[0], out moveToHexs); //gets closest movement to the closest Allied
                    if (!boi.StandingHex.Equals(moveToHexs[0])) { boi.Move(moveToHexs[0]); }//if not already on the tile
                
                break;
            case GOAL_DEFEND_TARGET:
            case GOAL_ATTACK_TARGET:
            default: //everyone move towards the target
                //Debug.Log("move towards a target");
                if (goalTarget == null) { Debug.LogError("No goal target found"); break; }

                    moveToHexs = new List<Hex>();
                    orderToClosest(boi, goalTarget, out moveToHexs);
                    //Debug.Log("Enemy move, " + boi.StandingHex.ToString() + " closest " + moveToHexs[0].ToString());
                    if (!boi.StandingHex.Equals(moveToHexs[0])) { boi.Move(moveToHexs[0]); }//if not already on the tile
                
                break;
        }
    }

    /// <summary>
    /// orders a list of hexs that are closest to the target, closes to far, includes the hex the boi is on
    /// </summary>
    /// <param name="boi">minion that would move</param>
    /// <param name="target">hext that the boi wants to get closer to</param>
    /// <param name="list"></param>
    private void orderToClosest(mobBase boi, Hex target, out List<Hex> list)
    {
        list = new List<Hex>();
        list.Add(boi.StandingHex); //adds current hex
        list.AddRange(boi.getPossibleMoves()); //add hexs to list around current hex
        list.Sort(delegate (Hex a, Hex b)
        {
            return mapLocal.pathfinding(a, target).Length < mapLocal.pathfinding(b, target).Length ? -1 : 1;
        });
    }

    /// <summary>
    /// attacks player or minion of next to the enemy
    /// </summary>
    private void attackwithEnemy(mobBase boi)
    {

            List<Hex> neigbors = new List<Hex>();
            for (int i = 0; i < 6; i++)
            {
                Hex neigbor = mapLocal.getNeigbor(boi.StandingHex, i);
                if (neigbor != null && neigbor.occupant != null && neigbor.occupant.Alligence != boi.Alligence)
                {
                    neigbors.Add(neigbor); //neigbor tile had player/minion on it
                }
            }
            if (neigbors.Count == 0) { return; } //nothing to attack
            else if (neigbors.Count == 1) { boi.mobAttack(neigbors[0].occupant); return; }//only one thing to attack
            else
            {
                if (goal == GOAL_ATTACK_TARGET && neigbors.Contains(goalTarget))
                {
                    boi.mobAttack(goalTarget.occupant);
                    return;
                }
                //sorts to attack target with lowest health
                neigbors.Sort(delegate (Hex a, Hex b)
                {
                    if (a.occupant.Health < b.occupant.Health) { return -1; }
                    else if (a.occupant.Health > b.occupant.Health) { return 1; }
                    else //health are the same, pick closest to the info
                    {
                        if (mapLocal.distanceBetween(a, theInfo.StandingHex) == mapLocal.distanceBetween(b, theInfo.StandingHex)) { return 0; } //same distance to info orb
                        return mapLocal.distanceBetween(a, theInfo.StandingHex) == mapLocal.distanceBetween(b, theInfo.StandingHex) ? -1 : 1; //closer player/minion to info orb gets attacked
                    }
                });
                boi.mobAttack(neigbors[0].occupant);
            }
        
    }

    private Hex[] getTilesWithPM()//gets all tiles with players/minions on it
    {
        List<Hex> hexs = new List<Hex>();
        foreach (Hex h in mapLocal.map)
        {
            if (!h.isSolid() && h.occupant!=null && h.occupant.Alligence == true) { hexs.Add(h); }
        }
        return hexs.ToArray();
    }

}
