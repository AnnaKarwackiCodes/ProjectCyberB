using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCasting : MonoBehaviour {

	LineRenderer line;
	RaycastHit hit;

	public Text txt;
	// Use this for initialization
	void Start () {
		line = gameObject.GetComponent<LineRenderer> ();
		line.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Right_Trigger") == 1.0f) {
			//drawing the line to show what its interacting with
			line.enabled = true;
			Ray ray = new Ray (transform.position, transform.forward);

			line.SetPosition (0, ray.origin);
			line.SetPosition (1, ray.GetPoint (10));

			//now we are going to see if we are colliding with something
			if (Physics.Raycast (ray, out hit, 10)) {
				if (hit.collider.gameObject.tag == "Hex") {
					txt.text = "hex";
				}
			} 
		}
		else {
			line.enabled = false;
		}
	}
}
