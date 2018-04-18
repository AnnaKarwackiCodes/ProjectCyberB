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

    // Use this for initialization
    void Start()
    {
        leftUICreate = false;
        createBoiHL = true;
        createHexHL = true;
        curOption = -1; //this will select none of the menu options
        gCon = this.gameObject.GetComponent<playerScript>().gameController;
        preHex = null;
    }

    // Update is called once per frame
    void Update()
    {
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
        lefty.transform.localRotation = leftRotation;

        //moving the right "hand"
        righty.transform.localPosition = rightPosition;
        righty.transform.localRotation = rightRotation;
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
        if (createHexHL && !curSel.GetComponent<Hex>().occupant)
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
        lUI.transform.GetChild(0).GetComponent<Text>().text = "Summon Browse";
        lUI.transform.GetChild(1).GetComponent<Text>().text = "Summon Tab";
        lUI.transform.GetChild(3).GetComponent<Text>().text = "Move";
        if (Input.GetAxis("Left_Touchpad_X") == 1 && Input.GetAxis("Left_Touchpad_Y") == -1)
        {
            //summon browser
            curOption = 0;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.blue;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(2).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.white;
        }
        if (Input.GetAxis("Left_Touchpad_X") == -1 && Input.GetAxis("Left_Touchpad_Y") == 1)
        {
            //summon tab
            curOption = 1;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.blue;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(2).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.white;
        }
        if (Input.GetAxis("Left_Touchpad_X") == 1 && Input.GetAxis("Left_Touchpad_Y") == 1)
        {
            //Move
            curOption = 2;
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.blue;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(2).GetComponent<Text>().color = Color.white;
        }
        if (Input.GetButtonDown("Left_Touchpad_Pressed"))
        {
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
        }
        preHex = curSel;
    }
    public void BoiInteraction(GameObject curSel)
    {
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
        if (createBoiHL)
        {
            Debug.Log("creating");
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
        lUI.transform.GetChild(0).GetComponent<Text>().text = "Move";
        lUI.transform.GetChild(1).GetComponent<Text>().text = "Have Attack";
        lUI.transform.GetChild(3).GetComponent<Text>().text = "Ball Pass";
        lUI.transform.GetChild(4).GetComponent<Text>().text = curSel.GetComponent<mobBase>().mobName;
        lUI.transform.GetChild(5).GetComponent<Text>().text = "Health: " + curSel.GetComponent<mobBase>().Health;
        if (Input.GetAxis("Left_Touchpad_X") == 1 && Input.GetAxis("Left_Touchpad_Y") == -1)
        {
            //move
            curOption = 0;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.blue;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(2).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.white;
        }
        if (Input.GetAxis("Left_Touchpad_X") == -1 && Input.GetAxis("Left_Touchpad_Y") == 1)
        {
            //attack
            curOption = 1;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.blue;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(2).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.white;
        }
        if (Input.GetAxis("Left_Touchpad_X") == 1 && Input.GetAxis("Left_Touchpad_Y") == 1)
        {
            //pass ball
            curOption = 2;
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.blue;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(2).GetComponent<Text>().color = Color.white;
        }
        if (Input.GetButtonDown("Left_Touchpad_Pressed"))
        {
            Debug.Log("cur " + curOption);
            switch (curOption)
            {
                case 0:
                    if (gameObject.GetComponent<playerScript>().SelectedMinion.CanMove)
                    {
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
                    lUI.transform.GetChild(3).GetComponent<Text>().color = Color.cyan;
                    gameObject.GetComponent<playerScript>().Action = "Pass Ball to";
                    gameObject.GetComponent<playerScript>().HasBall = !gameObject.GetComponent<playerScript>().HasBall;
                    curSel.GetComponent<mobBase>().HasBall = !curSel.GetComponent<mobBase>().HasBall;
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
        }
        preHex = curSel;
    }
    public void EnemyInteraction(GameObject curSel)
    {
        if (leftUICreate == false)
        {
            lUI = Instantiate(LeftUIInteract, leftPosition, new Quaternion(0, 0, 0, 0), LeftUICan.transform);
            leftUICreate = true;
        }
        if (createEnemyHL)
        {
            myEnemyHL = Instantiate(enemyHL, curSel.transform.position, new Quaternion(0, 0, 0, 0));
            createEnemyHL = false;
            if (!createHexHL)
            {
                Destroy(myHexHL);
                createHexHL = true;
            }
        }
        lUI.transform.GetChild(3).GetComponent<Text>().text = "Fireball";
        lUI.transform.GetChild(1).GetComponent<Text>().text = "";
        lUI.transform.GetChild(0).GetComponent<Text>().text = "";
        lUI.transform.GetChild(4).GetComponent<Text>().text = curSel.GetComponent<mobBase>().mobName;
        lUI.transform.GetChild(5).GetComponent<Text>().text = "Health: " + curSel.GetComponent<mobBase>().Health;
        if (Input.GetAxis("Left_Touchpad_X") == 1 && Input.GetAxis("Left_Touchpad_Y") == 1)
        {
            //fireball that minion
            curOption = 0;
            lUI.transform.GetChild(3).GetComponent<Text>().color = Color.blue;
            lUI.transform.GetChild(1).GetComponent<Text>().color = Color.white;
            lUI.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        }
        if (Input.GetButtonDown("Left_Touchpad_Pressed"))
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
        }
        preHex = curSel;
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
