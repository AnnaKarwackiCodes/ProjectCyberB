using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class MotionControllers : MonoBehaviour {

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
	void Start () {
		leftUICreate = false;
        curOption = -1; //this will select none of the menu options
        openSub = false;
        gCon = this.gameObject.GetComponent<playerScript>().gameController;
	}
	
	// Update is called once per frame
	void Update () {
		Track ();
        gCon = this.gameObject.GetComponent<playerScript>().gameController;
        if (gCon.PlayersTurn)
        {
            LeftHandInteractions();
            if (leftUICreate)
            {
                lUI.transform.localPosition = leftPosition;
            }
        }
	}
	//everything that deals with tracking the controllers goes here
	private void Track(){
		//how to get the left controller's position
		leftPosition = InputTracking.GetLocalPosition (XRNode.LeftHand);
		leftPosition.z += .1f; // this gives the player a  little more room to move the controller around and not have it clip into your head
		leftRotation = InputTracking.GetLocalRotation (XRNode.LeftHand);

		//how to get the right controller's position
		rightPosition = InputTracking.GetLocalPosition (XRNode.RightHand);
		rightPosition.z += .1f; //same deal as the left hand
		rightRotation = InputTracking.GetLocalRotation (XRNode.RightHand);

        //moving the left "hand"
        lefty.transform.localPosition = leftPosition;
		lefty.transform.localRotation = leftRotation;

        //moving the right "hand"
        righty.transform.localPosition = rightPosition;
		righty.transform.localRotation = rightRotation;
	}

    //this is menu control with the left controller
	private void LeftHandInteractions(){
		if (Input.GetAxis("Left_Grip_Button") == 1.0f && !openSub) {
			if(leftUICreate == false){
				lUI = Instantiate(LeftUIInteract, leftPosition, new Quaternion(0,0,0,0),LeftUICan.transform);
				leftUICreate = true;
			}
            //check to see if that player has selected what to do:
            //summon big or smol boi, fireball, punch, pass ball, move
            //when clicked thats whats going to happen or it takes you to the next selection
            if(Input.GetAxis("Left_Touchpad_X") == 1 && Input.GetAxis("Left_Touchpad_Y") == -1)
            {
                //summon minion
                curOption = 0;
                lUI.transform.GetChild(0).GetComponent<Text>().color = Color.blue;
                lUI.transform.GetChild(1).GetComponent<Text>().color = Color.black;
                lUI.transform.GetChild(2).GetComponent<Text>().color = Color.black;
                lUI.transform.GetChild(3).GetComponent<Text>().color = Color.black;
            }
            if (Input.GetAxis("Left_Touchpad_X") == -1 && Input.GetAxis("Left_Touchpad_Y") == 1)
            {
                //attack
                curOption = 1;
                lUI.transform.GetChild(1).GetComponent<Text>().color = Color.blue;
                lUI.transform.GetChild(0).GetComponent<Text>().color = Color.black;
                lUI.transform.GetChild(2).GetComponent<Text>().color = Color.black;
                lUI.transform.GetChild(3).GetComponent<Text>().color = Color.black;
            }
            if (Input.GetAxis("Left_Touchpad_X") == 1 && Input.GetAxis("Left_Touchpad_Y") == 1)
            {
                //pass ball
                curOption = 3;
                lUI.transform.GetChild(3).GetComponent<Text>().color = Color.blue;
                lUI.transform.GetChild(1).GetComponent<Text>().color = Color.black;
                lUI.transform.GetChild(0).GetComponent<Text>().color = Color.black;
                lUI.transform.GetChild(2).GetComponent<Text>().color = Color.black;
            }
            if (Input.GetAxis("Left_Touchpad_X") == -1 && Input.GetAxis("Left_Touchpad_Y") == -1)
            {
                //move
                curOption = 2;
                lUI.transform.GetChild(2).GetComponent<Text>().color = Color.blue;
                lUI.transform.GetChild(1).GetComponent<Text>().color = Color.black;
                lUI.transform.GetChild(3).GetComponent<Text>().color = Color.black;
                lUI.transform.GetChild(0).GetComponent<Text>().color = Color.black;
            }
            if (Input.GetButtonDown("Left_Touchpad_Pressed"))
            {
                switch (curOption)
                {
                    case 0:
                        Debug.Log("selected summon");
                        AdditionalMenus(0);
                        break;
                    case 3:
                        if (gameObject.GetComponent<playerScript>().CanMove)
                        {
                            gameObject.GetComponent<playerScript>().Action = "Move";
                            //Input.ResetInputAxes();
                        }
                        break;
                    default:
                        Debug.Log("yeah no this wont work.");
                        break;
                }
                curOption = -1;
            }
        }
        else if (openSub){
            Debug.Log("within the openSub");
            switch (addMenu)
            {
                case 0:
                    lUI.transform.GetChild(0).GetComponent<Text>().text = "";
                    lUI.transform.GetChild(1).GetComponent<Text>().text = "";
                    lUI.transform.GetChild(2).GetComponent<Text>().text = "Smol Boi";
                    lUI.transform.GetChild(3).GetComponent<Text>().text = "Big Boi";

                    if (Input.GetAxis("Left_Touchpad_Y") == 1)
                    {
                        curOption = 0;
                        lUI.transform.GetChild(3).GetComponent<Text>().color = Color.blue;
                        lUI.transform.GetChild(2).GetComponent<Text>().color = Color.black;
                    }
                    else if (Input.GetAxis("Left_Touchpad_Y") == -1)
                    {
                        curOption = 1;
                        lUI.transform.GetChild(2).GetComponent<Text>().color = Color.blue;
                        lUI.transform.GetChild(3).GetComponent<Text>().color = Color.black;
                    }
                    break;
                default:
                    Debug.Log("Add menu out of range");
                    break;
            }
            if (Input.GetButtonDown("Left_Touchpad_Pressed"))
            {
                Debug.Log("add menu press");
                if (curOption == 0 && (gameObject.GetComponent<playerScript>().Mana - gameObject.GetComponent<playerScript>().BigSumCost >= 0))
                {
                    gameObject.GetComponent<playerScript>().Action = "Big";
                }
                else if (curOption == 1 && (gameObject.GetComponent<playerScript>().Mana - gameObject.GetComponent<playerScript>().SmolSumCost >= 0))
                {
                    gameObject.GetComponent<playerScript>().Action = "Small";
                }
                openSub = false;
                lUI.transform.GetChild(0).GetComponent<Text>().text = "Summon";
                lUI.transform.GetChild(1).GetComponent<Text>().text = "Attack";
                lUI.transform.GetChild(2).GetComponent<Text>().text = "Pass Ball";
                lUI.transform.GetChild(3).GetComponent<Text>().text = "Move";
            }
        }
		else {
			if(leftUICreate && !openSub){
				Destroy (lUI);
				leftUICreate = false;
			}
        }
	}

    private void AdditionalMenus(int selection)
    {
        //this will show the choices of either what you can summon or the attacks you have
 
        switch (selection)
        {
            case 0:
                //summon
                Debug.Log("summon");
                addMenu = 0;
                openSub = true;
                break;
            default:
                Debug.Log("additional menu issue, out of range");
                break;
        }
        
    }
}
