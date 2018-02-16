using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCasting : MonoBehaviour {

	LineRenderer line;
	// Use this for initialization
	void Start () {
		line = gameObject.GetComponent<LineRenderer> ();
		line.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Right_Trigger") == 1.0f) {
			line.enabled = true;
			Ray ray = new Ray (transform.position, transform.forward);

			line.SetPosition (0, ray.origin);
			line.SetPosition (1, ray.GetPoint (10));
		}
		else {
			line.enabled = false;
		}
	}
}
