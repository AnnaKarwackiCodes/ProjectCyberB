using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour {

    //cube coord
    private int x;
    private int y;
    private int z;
    //offsetcoord
    private int row;
    private int col;

    


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        Debug.Log("(" + x + ", " + y + ", " + z + ")");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Piece>().setLocation(x, y, z);
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z);
    }

    //get/set functions
    public int X
    {
        get { return x; }
        set { x = value; }
    }

    public int Y
    {
        get { return y; }
        set { y = value; }
    }

    public int Z
    {
        get { return z; }
        set { z = value; }
    }

    public int Row
    {
        get { return row; }
        set { row = value; }
    }

    public int Col
    {
        get { return col; }
        set { col = value; }
    }

    //set location of tile's Cube from Offset 
    public void setLocation(int r, int q, int xx, int zz)
    {
        row = r;
        col = q;
        x = xx;
        z = zz;
        y = -xx - zz;
    }
}
