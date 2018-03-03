using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [SerializeField] private GameObject hexagon; //hexagon obj
    public float offset; //space between hexagons
    public int width; //width of the map
    public int height; //height of the map
    public Hex[,] map; //array that keeps track //stored in offset coordinates

    /// <summary>
    /// the 6 neigbors a hex can have
    /// </summary>
    static int[,] cubeDirs = new int[,]
        { { 1, -1, 0 }, { 1, 0, -1 }, { 0, 1, -1 },
          { -1, 1, 0 }, { -1, 0, 1 }, { 0, -1, 1 } };

    // Use this for initialization
    void Start () {
        createRectangleMap(width, height, offset);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //static functionsa and variables

    /// <summary>
    /// gets the offset coords and changes them into cubic coords
    /// (x,y) --> (x,y,z)
    /// x + y + z = 0
    /// </summary>
    static public int[] offsetToCube(int r, int q) //offset coords to cube coords
    {
        int[] arr = new int[3];
<<<<<<< HEAD
        //arr[0] = q - (r - (r & 1)) / 2;
        //arr[2] = r;
        arr[0] = q;
        arr[2] = r - (q - (q & 1)) / 2; 
=======
        arr[0] = q;
        arr[2] = r - (q - (q & 1)) / 2;
>>>>>>> 826f4c7f7650d6004e262a275ff7e6e99b18bb82
        arr[1] = -arr[0] - arr[2];
        return arr; // [x,y,z]
    }

    /// <summary>
    /// gets the cubic coords and changes them into offset coords
    /// (x,y,z) --> (x,y)
    /// </summary>
    static public int[] cubeToOffset(int xx, int zz) //offset 
    {
        
        int[] arr = new int[2];
<<<<<<< HEAD
        //arr[0] = xx + (zz - (zz & 1)) / 2;
        //arr[1] = zz;
=======
>>>>>>> 826f4c7f7650d6004e262a275ff7e6e99b18bb82
        arr[0] = xx;
        arr[1] = zz + (xx - (xx & 1)) / 2;
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
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject go = Instantiate(hexagon);
                int[] arr = offsetToCube(i, j); //gets the cube coords of tile
                go.GetComponent<Hex>().setLocation(i, j, arr[0], arr[2]); //sets location for the tile
                addToMap(go.GetComponent<Hex>());
                go.transform.position = new Vector3(j % 2 == 0 ? i * (1 + off) : i * (1 + off) + (.5f + off), 0, j * (Mathf.Sqrt(3) / 2 + off)); //moves tile of physical position in space
            }
        }
        Debug.Log(map);
    }

    public void addToMap(Hex tile)
    {
        Debug.Log(tile.Row + ", " + tile.Col);
        map[tile.Row, tile.Col] = tile;
    }

    /// <summary>
    /// gets one of the 6 neighbors in a direction
    /// </summary>
    public Hex getNeigbor(Hex tile, int dir)//get neighbor of tile in direction
    {

        int[] arr = cubeToOffset(tile.X + cubeDirs[dir,0], tile.Z + cubeDirs[dir,2]); //gets offset location of neighboring tiles
        return map[arr[0], arr[1]];
    }

    public int distanceBetween(Hex center, Hex target) {

        int distance = 0;

        distance = Mathf.Max(Mathf.Abs(center.X - target.X), Mathf.Abs(center.Y - target.Y), Mathf.Abs(center.Z - target.Z));

        return distance;

    }

    public List<Hex> getNeighborAOE(Hex center, int radius)
    {

        //this is going to have errors

        List<Hex> area = new List<Hex>();

        for (int i = (center.X - radius); i <= (center.X + radius); i++) {

            for (int j = Mathf.Max(-radius, -center.X-radius); j <= Mathf.Min(radius, -center.X+radius); j++) {

                int z = -center.X - center.Y;

                if (hexExists(i, j, z))
                {

                    area.Add(getHex(i, j, z));

                }

            }

        }

        return area;

    }

    /// <summary>
    /// gets the hex from the map
    /// </summary>
    public Hex getHex(int xx, int yy, int zz)
    {
<<<<<<< HEAD
        if (inBounds(xx, yy, zz))
=======
        if(inBounds(xx, yy, zz))
>>>>>>> 826f4c7f7650d6004e262a275ff7e6e99b18bb82
        {
            int[] loc = cubeToOffset(xx, zz);
            return map[loc[1], loc[0]];
        }
        return null;
    }

    /// <summary>
    /// checks to see if a hex is in bounds and not type 'null'
    /// </summary>
    public bool hexExists(int xx, int yy, int zz)
    {
        Hex h = getHex(xx, yy, zz);
        if (h != null) //hex exists
        {
            if(h.Type != Hex.TYPE.NULL) //not null
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// checks to see if hex is inbounds of the map
    /// </summary>
    private bool inBounds(int xx, int yy, int zz)
    {
        int[] loc = cubeToOffset(xx, zz);
        if (loc[0] >= 0 && loc[0] < width && loc[1] >= 0 && loc[1] < height) //hex is in bounds
        {
            return true;
        }
        return false;
    }

}
