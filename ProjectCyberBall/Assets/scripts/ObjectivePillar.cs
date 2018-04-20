using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivePillar : MonoBehaviour {

    [SerializeField] float turnSpeed = 1f; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, turnSpeed * Time.deltaTime, 0));
	}
}
