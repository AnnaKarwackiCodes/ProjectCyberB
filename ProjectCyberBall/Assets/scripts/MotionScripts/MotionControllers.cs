using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class MotionControllers : MonoBehaviour
{

    public GameObject lefty;
    public GameObject righty;
    //public GameObject text;
    public GameObject LeftUIInteract;
    public GameObject LeftUICan;

    private GameObject lUI;

    private Vector3 leftPosition;
    private Quaternion leftRotation;
    private Vector3 rightPosition;
    private Quaternion rightRotation;

    private bool leftUICreate;
    private int curOption;
    private GameController gCon;

    public GameObject boiHL;
    private bool createBoiHL;
    private GameObject myBoiHL;

    public GameObject hexHL;
    private bool createHexHL;
    private GameObject myHexHL;

    public GameObject enemyHL;
    private bool createEnemyHL;
    private GameObject myEnemyHL;

    private GameObject preHex;

    private string selectX;
    private string selectY;
    private string selectButton;
    private float axisDegree;

    private float preDegree;

    // Use this for initialization
    void Start()
    {
        leftUICreate = false;
        createBoiHL = true;
        createHexHL = true;
        curOption = -1; //this will select none of the menu options
        gCon = this.gameObject.GetComponent<playerScript>().gameController;
        preHex = null;

        selectX = "Left_Thumbstick_X";
        selectY = "Left_Thumbstick_Y";
        selectButton = "Left_Trigger";
        axisDegree = .05f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.GetAxis(selectButton));
        Track();
        gCon = this.gameObject.GetComponent<playerScript>().gameController;
        if (gCon.PlayersTurn)
        {
            //LeftHandInteractions();
            if (leftUICreate)
            {
                lUI.transform.localPosition = leftPosition;
            }
        }
    }
    //everything that deals with tracking the controllers goes here
    private void Track()
    {
        //how to get the left controller's position
        leftPosition = InputTracking.GetLocalPosition(XRNode.LeftHand);
        leftPosition.z += .1f; // this gives the player a  little more room to move the controller around and not have it clip into your head
        leftRotation = InputTracking.GetLocalRotation(XRNode.LeftHand);

        //how to get the right controller's position
        rightPosition = InputTracking.GetLocalPosition(XRNode.RightHand);
        rightPosition.z += .1f; //same deal as the left hand
        rightRotation = InputTracking.GetLocalRotation(XRNode.RightHand);

        //moving the left "hand"
        lefty.transform.localPosition = leftPosition;
        lefty.transform.localRotation = leftRotation;// * new Quaternion(90,0,0,0);

        //moving the right "hand"
        righty.transform.localPosition = rightPosition;
        righty.transform.localRotation = rightRotation;// * new Quaternion(0,90,0,0);
    }
    public void RemoveUI()
    {
        if (leftUICreate)
        {
            Destroy(lUI);
            leftUICreate = false;
        }
    }
    public void HexInteraction(GameObject curSel)
    {
        
        curOption = -1;
        int distance = gameObject.GetComponent<playerScript>().mapLocal.distanceBetween(gameObject.GetComponent<playerScript>().StandingHex, curSel.GetComponent<Hex>());
        if (preHex!= null && curSel.transform.position != preHex.transform.position)
        {
            if (!createHexHL)
            {
                Destroy(myHexHL);
                createHexHL = true;
            }
        }
        if (leftUICreate == false)
        {
            lUI = Instantiate(LeftUIInteract, leftPosition, new Quaternion(0, 0, 0, 0), LeftUICan.transform);
            leftUICreate = true;
        }
        if (createHexHL && !curSel.GetComponent<Hex>().occupant && distance <= 2)
        {
            myHexHL = Instantiate(hexHL, (curSel.transform.position + new Vector3(0,1,0)), new Quaternion(0, 0, 0, 0));
            createHexHL = false;
            if (!createBoiHL)
            {
                Destroy(myBoiHL);
                createBoiHL = true;
            }
            if (!createEnemyHL)
            {
                Destroy(myEnemyHL);
                createEnemyHL = true;
            }
        }
        lUI.transform.GetChild(0).GetComponent<Text>().text = "Summon Minion:\n Browse\n Cost: " + gameObject.GetComponent<playerScript>().BigSumCost;
        lUI.transform.GetChild(1).GetComponent<Text>().text = "Summon Minion:\n Tab\n Cost: " + gameObject.GetComponent<playerScript>().SmolSumCost;
        if (gameObject.GetComponent<playerScript>().CanMove) lUI.transform.GetChild(3).GetComponent<Text>().text = "Move";
        else lUI.transform.GetChild(3).GetComponent<Text>().text = "";
        lUI.transform.GetChild(4).GetComponent<Text>().text = "";
        lUI.transform.GetChild(5).GetComponent<Text>().text = "";
        if (Input.GetAxis(selectX) > axisDegree)// && Input.GetAxis(selectY) < -axisDegree)
        {
            //summon browser
            curOption = 0;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.blue;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(2).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.white;
        }
        if (Input.GetAxis(selectX) < -axisDegree)// && Input.GetAxis(selectY) > axisDegree)
        {
            //summon tab
            curOption = 1;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.blue;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(2).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.white;
        }
        if (Input.GetAxis(selectY) > axisDegree)//(Input.GetAxis(selectX) > axisDegree && Input.GetAxis(selectY) < axisDegree)
        {
            //Move
            curOption = 2;
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.blue;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(2).GetComponent<Text>().color = Color.white;
        }
        if (curOption == -1)
        {
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(2).GetComponent<Text>().color = Color.white;
        }
        if (Input.GetAxis(selectButton) > axisDegree && preDegree != Input.GetAxis(selectButton))
        {
            preDegree = Input.GetAxis(selectButton);
            switch (curOption)
            {
                case 0:
                    if (gameObject.GetComponent<playerScript>().Mana - gameObject.GetComponent<playerScript>().BigSumCost >= 0)
                    {
                        lUI.transform.GetChild(0).GetComponent<Text>().color = Color.cyan;
                        gameObject.GetComponent<playerScript>().SelectedObj = curSel;
                        gameObject.GetComponent<playerScript>().Action = "Big";
                        if (!createHexHL)
                        {
                            Destroy(myHexHL);
                            createHexHL = true;
                        }
                    }
                    break;
                case 1:
                    if (gameObject.GetComponent<playerScript>().Mana - gameObject.GetComponent<playerScript>().SmolSumCost >= 0)
                    {
                        lUI.transform.GetChild(1).GetComponent<Text>().color = Color.cyan;
                        gameObject.GetComponent<playerScript>().SelectedObj = curSel;
                        gameObject.GetComponent<playerScript>().Action = "Small";
                        if (!createHexHL)
                        {
                            Destroy(myHexHL);
                            createHexHL = true;
                        }
                    }
                    break;
                case 2:
                    if (gameObject.GetComponent<playerScript>().CanMove)
                    {
                        lUI.transform.GetChild(3).GetComponent<Text>().color = Color.cyan;
                        gameObject.GetComponent<playerScript>().SelectedObj = curSel;
                        gameObject.GetComponent<playerScript>().Action = "Move";
                        if (!createHexHL)
                        {
                            Destroy(myHexHL);
                            createHexHL = true;
                        }
                    }
                    break;
                default:
                    break;
            }
            curOption = -1;
            if (leftUICreate)
            {
                Destroy(lUI);
                leftUICreate = false;
            }
            Input.ResetInputAxes();
        }
        preHex = curSel;
        preDegree = Input.GetAxis(selectButton);
    }
    public void BoiInteraction(GameObject curSel)
    {
        curOption = -1;
        int distance = gameObject.GetComponent<playerScript>().mapLocal.distanceBetween(gameObject.GetComponent<playerScript>().StandingHex, curSel.GetComponent<mobBase>().StandingHex);
        if (preHex != null && curSel.transform.position != preHex.transform.position)
        {
            if (!createBoiHL)
            {
                Destroy(myBoiHL);
                createBoiHL = true;
            }
        }
        if (leftUICreate == false)
        {
            lUI = Instantiate(LeftUIInteract, leftPosition, new Quaternion(0, 0, 0, 0), LeftUICan.transform);
            leftUICreate = true;
        }
        if (createBoiHL && distance <= 3)
        {
            myBoiHL = Instantiate(boiHL, curSel.transform.position, new Quaternion(0, 0, 0, 0));
            createBoiHL = false;
            if (!createHexHL)
            {
                Destroy(myHexHL);
                createHexHL = true;
            }
            if (!createEnemyHL)
            {
                Destroy(myEnemyHL);
                createEnemyHL = true;
            }
        }
        if (curSel.GetComponent<mobBase>().CanMove) lUI.transform.GetChild(0).GetComponent<Text>().text = "Move";
        else lUI.transform.GetChild(0).GetComponent<Text>().text = "";
        if (curSel.GetComponent<mobBase>().CanAttack) lUI.transform.GetChild(1).GetComponent<Text>().text = "Have Attack";
        else lUI.transform.GetChild(1).GetComponent<Text>().text = "";
        if (gameObject.GetComponent<playerScript>().HasBall || curSel.GetComponent<mobBase>().HasBall) lUI.transform.GetChild(3).GetComponent<Text>().text = "Ball Pass\nCost: " + gameObject.GetComponent<playerScript>().UseBallCost;
        else lUI.transform.GetChild(3).GetComponent<Text>().text = "";
        lUI.transform.GetChild(4).GetComponent<Text>().text = curSel.GetComponent<mobBase>().mobName;
        lUI.transform.GetChild(5).GetComponent<Text>().text = "Health: " + curSel.GetComponent<mobBase>().Health;

        if (Input.GetAxis(selectX) > axisDegree)
        {
            //move
            curOption = 0;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.blue;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(2).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.white;
        }
        if (Input.GetAxis(selectX) < -axisDegree)
        {
            //attack
            curOption = 1;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.blue;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(2).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.white;
        }
        if ((gameObject.GetComponent<playerScript>().HasBall || curSel.GetComponent<mobBase>().HasBall) && (Input.GetAxis(selectY) > axisDegree))
        {
            //pass ball
            curOption = 2;
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.blue;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(2).GetComponent<Text>().color = Color.white;
        }
        if(curOption == -1)
        {
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(2).GetComponent<Text>().color = Color.white;
        }
        if (Input.GetAxis(selectButton) > axisDegree && preDegree != Input.GetAxis(selectButton))//(Input.GetAxis(selectButton) > axisDegree)
        {
            switch (curOption)
            {
                case 0:
                    if (curSel.GetComponent<mobBase>().CanMove)
                    {
                        Debug.Log("moving");
                        lUI.transform.GetChild(0).GetComponent<Text>().color = Color.cyan;
                        gameObject.GetComponent<playerScript>().SelectedMinion = curSel.GetComponent<mobBase>();
                        gameObject.GetComponent<playerScript>().Action = "Move Boi";
                    }
                    break;
                case 1:
                    Debug.Log("can attack? " + curSel.GetComponent<mobBase>().CanAttack);
                    if (curSel.GetComponent<mobBase>().CanAttack)
                    {
                        Debug.Log("Boi attacki");
                        lUI.transform.GetChild(1).GetComponent<Text>().color = Color.cyan;
                        Debug.Log(curSel.GetComponent<mobBase>().name);
                        gameObject.GetComponent<playerScript>().SelectedMinion = curSel.GetComponent<mobBase>();
                        gameObject.GetComponent<playerScript>().Action = "Boi Attack";
                    }
                    break;
                case 2:
                    if ((gameObject.GetComponent<playerScript>().HasBall || curSel.GetComponent<mobBase>().HasBall) && gameObject.GetComponent<playerScript>().Mana - gameObject.GetComponent<playerScript>().UseBallCost > 0)
                    {
                        lUI.transform.GetChild(3).GetComponent<Text>().color = Color.cyan;
                        gameObject.GetComponent<playerScript>().Action = "Pass Ball to";
                    }
                    break;
                default:
                    break;
            }
            curOption = -1;
            if (leftUICreate)
            {
                Destroy(lUI);
                leftUICreate = false;
            }
            //Input.ResetInputAxes();
        }
        preHex = curSel;
        preDegree = Input.GetAxis(selectButton);
    }
    public void EnemyInteraction(GameObject curSel)
    {
        curOption = -1;
        int distance = gameObject.GetComponent<playerScript>().mapLocal.distanceBetween(gameObject.GetComponent<playerScript>().StandingHex, curSel.GetComponent<mobBase>().StandingHex);
        if (leftUICreate == false)
        {
            lUI = Instantiate(LeftUIInteract, leftPosition, new Quaternion(0, 0, 0, 0), LeftUICan.transform);
            leftUICreate = true;
        }
        if (createEnemyHL && distance <= 3)
        {
            myEnemyHL = Instantiate(enemyHL, curSel.transform.position, new Quaternion(0, 0, 0, 0));
            createEnemyHL = false;
            if (!createHexHL)
            {
                Destroy(myHexHL);
                createHexHL = true;
            }
        }
        if(gameObject.GetComponent<playerScript>().Action != "Boi Attack")
        {
            lUI.transform.GetChild(3).GetComponent<Text>().text = "Fireball\nCost: " + gameObject.GetComponent<playerScript>().FireBallCost;
            lUI.transform.GetChild(1).GetComponent<Text>().text = "";
            lUI.transform.GetChild(0).GetComponent<Text>().text = "";
            lUI.transform.GetChild(4).GetComponent<Text>().text = curSel.GetComponent<mobBase>().mobName;
            lUI.transform.GetChild(5).GetComponent<Text>().text = "Health: " + curSel.GetComponent<mobBase>().Health;
        }
        else
        {
            lUI.transform.GetChild(3).GetComponent<Text>().text = "";
        }
        
        if (Input.GetAxis(selectY) > axisDegree)
        {
            //fireball that minion
            curOption = 0;
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.blue;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        }
        else
        {
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.white;
            curOption = -1;
        }
        if (Input.GetAxis(selectButton) > axisDegree && curOption == 0 && preDegree != Input.GetAxis(selectButton) && gameObject.GetComponent<playerScript>().Action != "Boi Attack")
        {
            if (gameObject.GetComponent<playerScript>().Mana - gameObject.GetComponent<playerScript>().FireBallCost >= 0)
            {
                lUI.transform.GetChild(3).GetComponent<Text>().color = Color.cyan;
                gameObject.GetComponent<playerScript>().SelectedMinion = curSel.GetComponent<mobBase>();
                gameObject.GetComponent<playerScript>().Action = "Fireball";
            }
            curOption = -1;
            if (leftUICreate)
            {
                Destroy(lUI);
                leftUICreate = false;
            }
            Input.ResetInputAxes();
        }
        preHex = curSel;
        preDegree = Input.GetAxis(selectButton);
    }
    public void InfoInteraction(GameObject curSel)
    {
        int distance = gameObject.GetComponent<playerScript>().mapLocal.distanceBetween(gameObject.GetComponent<playerScript>().StandingHex, curSel.GetComponent<agentScript>().StandingHex);
        if (leftUICreate == false && distance <= 1)
        {
            lUI = Instantiate(LeftUIInteract, leftPosition, new Quaternion(0, 0, 0, 0), LeftUICan.transform);
            leftUICreate = true;
        }
        if (createHexHL && distance <= 1)
        {
            myHexHL = Instantiate(hexHL, curSel.transform.position, new Quaternion(0, 0, 0, 0));
            createHexHL = false;
        }
        if (leftUICreate == true)
        {
            lUI.transform.GetChild(3).GetComponent<Text>().text = "Pick Up";
            lUI.transform.GetChild(1).GetComponent<Text>().text = "";
            lUI.transform.GetChild(0).GetComponent<Text>().text = "";
            lUI.transform.GetChild(4).GetComponent<Text>().text = "";
            lUI.transform.GetChild(5).GetComponent<Text>().text = "";
            if (Input.GetAxis(selectY) > axisDegree)
            {
                lUI.transform.GetChild(3).GetComponent<Text>().color = Color.blue;
                lUI.transform.GetChild(1).GetComponent<Text>().color = Color.white;
                lUI.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            }
            else
            {
                lUI.transform.GetChild(3).GetComponent<Text>().color = Color.white;
            }
            if (Input.GetAxis(selectButton) > axisDegree && preDegree != Input.GetAxis(selectButton))
            {
                int range = gameObject.GetComponent<playerScript>().mapLocal.distanceBetween(gameObject.GetComponent<playerScript>().StandingHex, curSel.GetComponent<InfoBall>().StandingHex);
                if (range <= 1 && gameObject.GetComponent<playerScript>().Mana - gameObject.GetComponent<playerScript>().UseBallCost >= 0)
                {
                    gameObject.GetComponent<playerScript>().pickUpBall();
                    GameObject.FindGameObjectWithTag("Info").GetComponent<InfoBall>().Move(gameObject.GetComponent<playerScript>().StandingHex);
                }
                curOption = -1;
                if (leftUICreate)
                {
                    Destroy(lUI);
                    leftUICreate = false;
                }
                Input.ResetInputAxes();
            }
        }
        preHex = curSel;
        preDegree = Input.GetAxis(selectButton);
    }
    public void StartUI(bool before)
    {
        if (leftUICreate == false)
        {
            lUI = Instantiate(LeftUIInteract, leftPosition, new Quaternion(0, 0, 0, 0), LeftUICan.transform);
            leftUICreate = true;
        }
        lUI.transform.GetChild(0).GetComponent<Text>().text = "";
        lUI.transform.GetChild(1).GetComponent<Text>().text = "";
        if (before)
        {
            lUI.transform.GetChild(3).GetComponent<Text>().text = "Start";
        }
        else
        {
            lUI.transform.GetChild(3).GetComponent<Text>().text = "Play Again";
        }
        lUI.transform.GetChild(4).GetComponent<Text>().text = "";
        lUI.transform.GetChild(5).GetComponent<Text>().text = "";
        if (Input.GetAxis(selectY) > axisDegree)
        {
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.blue;
            curOption = 1;
        }
        else
        {
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.white;
            curOption = -1;
        }
        if (Input.GetAxis(selectButton) > axisDegree && curOption == 1)
        {
            if (before)
            {
                gameObject.GetComponent<playerScript>().Action = "Lets go";
            }
            else
            {
                Debug.Log("reset");
                gameObject.GetComponent<playerScript>().Action = "Reset";
            }
        }
        preDegree = Input.GetAxis(selectButton);
    }
    public void RemoveHighlight()
    {
        if (!createBoiHL)
        {
            Destroy(myBoiHL);
            createBoiHL = true;
        }

        if (!createHexHL)
        {
            Destroy(myHexHL);
            createHexHL = true;
        }

        if (!createEnemyHL)
        {
            Destroy(myEnemyHL);
            createEnemyHL = true;
        }
    }
}
