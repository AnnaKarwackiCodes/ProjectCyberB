using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class majBoiScript : mobBase {

    // Use this for initialization
    public virtual new void Start () {
        base.Start();
        MoveDistance = 2;
        Type = "Big";
        Health = 3;
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
        //Debug.Log("Minion update this minions health: " + Health);
        if (Health <= 0)
        {
            Debug.Log("is dead");
            GameObject.FindGameObjectWithTag("Player").GetComponent<playerScript>().RemoveBoi(ArrayPos);
        }
    }
}
