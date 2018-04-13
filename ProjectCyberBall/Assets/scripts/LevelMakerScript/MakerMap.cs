using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakerMap : Map {

	// Use this for initialization
	void Start () {
        createRectangleMap(width, height, offset);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("enter"))
        {
            printMap();
        }
	}
}
