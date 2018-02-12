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

	// Use this for initialization
	void Start () {
		leftUICreate = false;
	}
	
	// Update is called once per frame
	void Update () {
		Track ();
		LeftHandInteractions ();
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

	private void LeftHandInteractions(){
		if (Input.GetAxis("Left_Trigger") == 1.0f) {
			//text.GetComponent<Text> ().text = "click";
			if(leftUICreate == false){
				lUI = Instantiate(LeftUIInteract, leftPosition, new Quaternion(0,0,0,0),LeftUICan.transform);
				leftUICreate = true;
			}
		}
		else {
			//text.GetComponent<Text> ().text = "nah";
			if(leftUICreate){
				Destroy (lUI);
				leftUICreate = false;
			}
		}
	}
}
