using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class MotionControllers : MonoBehaviour {

	public GameObject lefty;
	public GameObject righty;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Track ();
	}
	//everything that deals with tracking the controllers goes here
	private void Track(){
		//how to get the left controller's position
		Vector3 leftPosition = InputTracking.GetLocalPosition (XRNode.LeftHand);
		leftPosition.z += .1f; // this gives the player a  little more room to move the controller around and not have it clip into your head
		Quaternion leftRotation = InputTracking.GetLocalRotation (XRNode.LeftHand);

		//how to get the right controller's position
		Vector3 rightPosition = InputTracking.GetLocalPosition (XRNode.RightHand);
		rightPosition.z += .1f; //same deal as the left hand
		Quaternion rightRotation = InputTracking.GetLocalRotation (XRNode.RightHand);

		//moving the left "hand"
		lefty.transform.localPosition = leftPosition;
		lefty.transform.localRotation = leftRotation;

		//moving the right "hand"
		righty.transform.localPosition = rightPosition;
		righty.transform.localRotation = rightRotation;
	}

	private void LeftHandInteractions(){
		
	}
}
