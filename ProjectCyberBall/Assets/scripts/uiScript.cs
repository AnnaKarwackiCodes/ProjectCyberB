using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiScript : MonoBehaviour {

    Map mapLocal;

    // Use this for initialization
    void Start () {

        mapLocal = gameObject.GetComponent<Map>();

    }
	
	// Update is called once per frame
	void Update () {


	}

    void StartGame()
    {

        //code needed - All the loading effects.

        mapLocal.curMap = mapLocal.maps[1];
        mapLocal.sceneChange();

    }

}
