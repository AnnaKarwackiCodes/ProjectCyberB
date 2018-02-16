﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [SerializeField] private GameObject hexagon; //hexagon obj
    public float offset; //space between hexagons
    public int width; //width of the map
    public int height; //height of the map
    public Hex[,] map; //array that keeps track //stored in offset coordinates

    static int[,] cubeDirs = new int[,]
        { { 1, -1, 0 }, { 1, 0, -1 }, { 0, 1, -1 },
          { -1, 1, 0 }, { -1, 0, 1 }, { 0, -1, 1 } };

    // Use this for initialization
    void Start () {
        createRectangleMap(10, 10, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //static functionsa and variables
    static public int[] offsetToCube(int r, int q) //offset coords to cube coords
    {
        int[] arr = new int[3];
        arr[0] = q - (r - (r & 1)) / 2;
        arr[2] = r;
        arr[1] = -arr[0] - arr[2];
        return arr; // [x,y,z]
    }

    static public int[] cubeToOffset(int xx, int zz) //offset 
    {
        int[] arr = new int[2];
        arr[0] = xx + (zz - (zz & 1)) / 2;
        arr[1] = zz;
        return arr; // [row, col]
    }

    private void createRectangleMap(int w, int h, float off) //creates a 'rectangular' map
    {
        if (w % 2 == 1) { w--; }
        if (h % 2 == 1) { h--; }
        width = w + 1;
        height = h + 1;
        map = new Hex[width, height];
        offset = off;
        for (int i = -width / 2; i <= width / 2; i++)
        {
            for (int j = -height / 2; j <= height / 2; j++)
            {
                GameObject go = Instantiate(hexagon);
                int[] arr = offsetToCube(i, j); //gets the cube coords of tile
                go.GetComponent<Hex>().setLocation(i, j, arr[0], arr[2]); //sets location for the tile
                //addToMap(go.GetComponent<Hex>());
                go.transform.position = new Vector3(j % 2 == 0 ? i * (1 + off) : i * (1 + off) + (.5f + off), 0, j * (Mathf.Sqrt(3) / 2 + off)); //moves tile of physical position in space
            }
        }
        Debug.Log(map);
    }

    public void addToMap(Hex tile)
    {
        map[tile.Row, tile.Col] = tile;
    }


    public Hex getNeigbor(Hex tile, int dir)//get neighbor of tile in direction
    {

        int[] arr = cubeToOffset(tile.X + cubeDirs[dir,0], tile.Z + cubeDirs[dir,2]); //gets offset location of neighboring tiles
        return map[arr[0], arr[1]];
    }

}
