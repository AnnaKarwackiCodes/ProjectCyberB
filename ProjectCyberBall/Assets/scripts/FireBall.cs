using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {
    private GameObject target;
    private playerScript player;
    private bool hitTarget;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<playerScript>();
        hitTarget = false;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("lets go");
        if (!hitTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position + new Vector3(0, .5f, 0), .5F);
        }
        if(transform.position == target.transform.position + new Vector3(0, .5f, 0))
        {
            target.GetComponent<mobBase>().Health -= player.FireBallDam;
            hitTarget = true;
        }
	}

    public GameObject Target
    {
        set { target = value; }
    }

    public bool HitTarget
    {
        get { return hitTarget; }
    }
}
