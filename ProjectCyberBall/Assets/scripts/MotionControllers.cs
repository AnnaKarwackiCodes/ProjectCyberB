using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class MotionControllers : MonoBehaviour {

	public GameObject lefty;
	public GameObject righty;
	public GameObject textBox;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		//how to get the left controller's position
		Vector3 leftPosition = InputTracking.GetLocalPosition (XRNode.LeftHand);
		Quaternion leftRotation = InputTracking.GetLocalRotation (XRNode.LeftHand);

		//how to get the right controller's position
		Vector3 rightPosition = InputTracking.GetLocalPosition (XRNode.RightHand);
		Quaternion rightRotation = InputTracking.GetLocalRotation (XRNode.RightHand);

		//Debug.Log (leftPosition);
		//Debug.Log (leftRotation);

		textBox.GetComponent<Text>().text = leftPosition + " " + leftRotation;

		//moving the left "hand"
		lefty.transform.localPosition = leftPosition;
		lefty.transform.localRotation = leftRotation;

		//moving the right "hand"
		righty.transform.localPosition = rightPosition;
		righty.transform.localRotation = rightRotation;
	}
}
