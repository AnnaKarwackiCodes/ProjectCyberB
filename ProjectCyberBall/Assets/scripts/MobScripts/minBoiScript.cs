using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minBoiScript : mobBase {

    // Use this for initialization
    public virtual new void Start () {
        base.Start();
        MoveDistance = 3;
        Type = "Small";
        Health = 2;
        Attack = 1;
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
        //Debug.Log("Minion update this minions health: " + Health);
        if (Health <= 0)
        {
            Debug.Log("is dead");
            if (Alligence)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<playerScript>().RemoveBoi(ArrayPos);
            }
            else if (!Alligence)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<enemyController>().RemoveBoi(ArrayPos, 0);
            }
        }
    }
}
