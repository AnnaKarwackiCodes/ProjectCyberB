using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Map theMap;

	// Use this for initialization
	void Start () {
        theMap = gameObject.GetComponent<Map>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
