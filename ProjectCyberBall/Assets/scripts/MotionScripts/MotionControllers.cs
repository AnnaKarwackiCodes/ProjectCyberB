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
    private bool openSub;
    private int addMenu;
    private GameController gCon;

    // Use this for initialization
    void Start()
    {
        leftUICreate = false;
        curOption = -1; //this will select none of the menu options
        openSub = false;
        gCon = this.gameObject.GetComponent<playerScript>().gameController;
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
        if (leftUICreate == false)
        {
            lUI = Instantiate(LeftUIInteract, leftPosition, new Quaternion(0, 0, 0, 0), LeftUICan.transform);
            leftUICreate = true;
        }
        lUI.transform.GetChild(0).GetComponent<Text>().text = "Summon Big";
        lUI.transform.GetChild(1).GetComponent<Text>().text = "Summon Small";
        lUI.transform.GetChild(3).GetComponent<Text>().text = "Move";
        if (Input.GetAxis("Left_Touchpad_X") == 1 && Input.GetAxis("Left_Touchpad_Y") == -1)
        {
            //summon minion
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
            switch (curOption)
            {
                case 0:
                    if (gameObject.GetComponent<playerScript>().Mana - gameObject.GetComponent<playerScript>().BigSumCost >= 0)
                    {
                        gameObject.GetComponent<playerScript>().SelectedObj = curSel;
                        gameObject.GetComponent<playerScript>().Action = "Big";
                    }
                    break;
                case 1:
                    if (gameObject.GetComponent<playerScript>().Mana - gameObject.GetComponent<playerScript>().SmolSumCost >= 0)
                    {
                        gameObject.GetComponent<playerScript>().SelectedObj = curSel;
                        gameObject.GetComponent<playerScript>().Action = "Small";
                    }
                    break;
                case 2:
                    if (gameObject.GetComponent<playerScript>().CanMove)
                    {
                        gameObject.GetComponent<playerScript>().SelectedObj = curSel;
                        gameObject.GetComponent<playerScript>().Action = "Move";
                    }
                    break;
                default:
                    Debug.Log("yeah no this wont work.");
                    break;
            }
            curOption = -1;
            if (leftUICreate)
            {
                Destroy(lUI);
                leftUICreate = false;
            }
        }
    }
    public void BoiInteraction(GameObject curSel)
    {
        if (leftUICreate == false)
        {
            lUI = Instantiate(LeftUIInteract, leftPosition, new Quaternion(0, 0, 0, 0), LeftUICan.transform);
            leftUICreate = true;
        }
        lUI.transform.GetChild(0).GetComponent<Text>().text = "Move Boi";
        lUI.transform.GetChild(1).GetComponent<Text>().text = "Boi Attack";
        lUI.transform.GetChild(3).GetComponent<Text>().text = "Pass Ball";
        if (Input.GetAxis("Left_Touchpad_X") == 1 && Input.GetAxis("Left_Touchpad_Y") == -1)
        {
            //summon minion
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
            switch (curOption)
            {
                case 0:
                    if (gameObject.GetComponent<playerScript>().Mana - gameObject.GetComponent<playerScript>().BigSumCost >= 0)
                    {
                        gameObject.GetComponent<playerScript>().SelectedMinion = curSel.GetComponent<mobBase>();
                        gameObject.GetComponent<playerScript>().Action = "Move Boi";
                    }
                    break;
                case 1:
                    if (gameObject.GetComponent<playerScript>().Mana - gameObject.GetComponent<playerScript>().SmolSumCost >= 0)
                    {
                        gameObject.GetComponent<playerScript>().SelectedMinion = curSel.GetComponent<mobBase>();
                        gameObject.GetComponent<playerScript>().Action = "Boi Attack";
                    }
                    break;
                case 2:
                    gameObject.GetComponent<playerScript>().Action = "Pass Ball to";
                    break;
                default:
                    Debug.Log("yeah no this wont work.");
                    break;
            }
            curOption = -1;
            if (leftUICreate)
            {
                Destroy(lUI);
                leftUICreate = false;
            }
        }

    }
}
