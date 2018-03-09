using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Map theMap;
    public GameObject userPrefab;
    private GameObject theUser;
    private bool userIsIn = false;

	// Use this for initializations
	void Awake () {
        theMap = gameObject.GetComponent<Map>();
        theUser = Instantiate(userPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));

        
    }

    void Start()
    {
        Debug.Log("start");
        //theUser.GetComponent<playerScript>().spawnIn(theMap.map[1, 1]);
        //theUser.GetComponent<playerScript>().MapLocal = theMap;
        //if (theMap.map == null) { Debug.Log("NO HEX"); }
        //theUser.GetComponent<playerScript>().GetComponent<playerScript>().spawnIn(theMap.map[1, 1]);

    }
	
	// Update is called once per frame
	void Update () {
		if(theMap.map != null && !userIsIn)
        {
            theUser.GetComponent<playerScript>().MapLocal = theMap;
            theUser.GetComponent<playerScript>().GetComponent<playerScript>().spawnIn(theMap.map[0, 0], this);
            userIsIn = true;
        }
    }
}
