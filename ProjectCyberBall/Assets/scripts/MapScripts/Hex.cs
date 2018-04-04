﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{

    //cube coord
    public int x;
    public int y;
    public int z;
    //offset coord
    private int row;
    private int col;

    /// <summary>
    /// who is currently on the hex
    /// </summary>
    public agentScript occupant;

    /// <summary>
    /// What Type of hex the hex is
    /// </summary>
    private TYPE type;

    /// <summary>
    /// The different types of hexs that a hex can be
    /// </summary>
    public enum TYPE { NULL = -1, FLOOR = 0, SPAWN = 1, START = 2, INFO = 3, END = 4, WALL = 5 };

    //hex type mesh filter, renderers, and colliders
    [SerializeField] private Mesh FLOOR_FC;
    [SerializeField] private Material FLOOR_R;

    [SerializeField] private Mesh GOAL_FC;
    [SerializeField] private Material GOAL_R;

    [SerializeField] private Mesh WALL_FC;
    [SerializeField] private Material WALL_R;

    [SerializeField] private Mesh INFO_FC;
    [SerializeField] public Material INFO_R;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        Debug.Log("(" + x + ", " + y + ", " + z + ")");
        GameObject.FindGameObjectWithTag("Player").GetComponent<playerScript>().Move(this);
        //Debug.Log(occupant);
        //GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z);
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

    public TYPE Type //need to figure out how to switch around shapes. Only really matters for walls and info orb ped
    {
        get { return type; }
        set
        {
            type = value;
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<MeshCollider>().enabled = true;
            switch (type)
            {
                case TYPE.FLOOR:
                //break;
                case TYPE.START:
                //break;
                case TYPE.SPAWN:
                    gameObject.GetComponent<MeshFilter>().mesh = FLOOR_FC;
                    gameObject.GetComponent<MeshRenderer>().material = FLOOR_R;
                    gameObject.GetComponent<MeshCollider>().sharedMesh = FLOOR_FC;
                    break;
                case TYPE.END:
                    gameObject.GetComponent<MeshFilter>().mesh = GOAL_FC;
                    gameObject.GetComponent<MeshRenderer>().material = GOAL_R;
                    gameObject.GetComponent<MeshCollider>().sharedMesh = GOAL_FC;
                    break;
                case TYPE.INFO:
                    gameObject.GetComponent<MeshFilter>().mesh = INFO_FC;
                    gameObject.GetComponent<MeshRenderer>().material = INFO_R;
                    gameObject.GetComponent<MeshCollider>().sharedMesh = INFO_FC;
                    break;
                case TYPE.WALL:
                    gameObject.GetComponent<MeshFilter>().mesh = WALL_FC;
                    gameObject.GetComponent<MeshRenderer>().material = WALL_R;
                    gameObject.GetComponent<MeshCollider>().sharedMesh = WALL_FC;
                    break;
                case TYPE.NULL:
                default:
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    gameObject.GetComponent<MeshCollider>().enabled = false;
                    break;
            }
        }
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

    /// <summary>
    ///Figures out if the hex is solid or not
    /// </summary>
    public bool isSolid()
    {
        if (type == TYPE.WALL || type == TYPE.NULL)
        {
            return true;
        }
        return false;
    }

    public bool Equals(Hex h) { return x == h.X && y == h.Y && z == h.Z ? true : false; }
    public override string ToString() { return "(" + x + ", " + y + ", " + z + ")"; }
}
