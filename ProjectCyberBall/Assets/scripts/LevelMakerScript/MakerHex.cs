using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakerHex : Hex {

    //NULL = -1, FLOOR = 0, SPAWN = 1, START = 2, INFO = 3, END = 4, WALL = 5

    [SerializeField] private Material Null_R;
    [SerializeField] private Material Floor_R;
    [SerializeField] private Material Spawn_R;
    [SerializeField] private Material Start_R;
    [SerializeField] private Material Info_R;
    [SerializeField] private Material End_R;
    [SerializeField] private Material Wall_R;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        int i = (int)type;
        i++;
        if(i > (int)TYPE.WALL) { i = (int)TYPE.NULL; }
        if(i < (int)TYPE.NULL) { i = (int)TYPE.WALL; }
        type = (TYPE)i;

        switch (type)
        {
            case TYPE.NULL:
                gameObject.GetComponent<MeshRenderer>().material = Null_R;
                break;
            case TYPE.FLOOR:
                gameObject.GetComponent<MeshRenderer>().material = Floor_R;
                break;
            case TYPE.SPAWN:
                gameObject.GetComponent<MeshRenderer>().material = Spawn_R;
                break;
            case TYPE.START:
                gameObject.GetComponent<MeshRenderer>().material = Start_R;
                break;
            case TYPE.INFO:
                gameObject.GetComponent<MeshRenderer>().material = Info_R;
                break;
            case TYPE.END:
                gameObject.GetComponent<MeshRenderer>().material = End_R;
                break;
            case TYPE.WALL:
                gameObject.GetComponent<MeshRenderer>().material = Wall_R;
                break;
            default:
                break;
        }

    }

}
