using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCasting : MonoBehaviour {

	LineRenderer line;
	RaycastHit hit;

	//public Text txt;
	// Use this for initialization
	void Start () {
		line = gameObject.GetComponent<LineRenderer> ();
		line.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

    //should have a function that changes the range that the raycast is checking
    public void SelectingObj(float distance, string wantedTag)
    {
        Debug.Log("selecting obj");
        //drawing the line to show what it's interacting with
        line.enabled = true;
        Ray ray = new Ray(transform.position, transform.forward);

        line.SetPosition(0, ray.origin);
        line.SetPosition(1, ray.GetPoint(distance));

        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.gameObject.tag == wantedTag && Input.GetAxis("Right_Trigger") == 1.0f)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<playerScript>().SelectedObj = hit.collider.gameObject;
                line.enabled = false;
                
            }
        }
        
    }

    public bool Line
    {
        set { line.enabled = value; }
    }
}
