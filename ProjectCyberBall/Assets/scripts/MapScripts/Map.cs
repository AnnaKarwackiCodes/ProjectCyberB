using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [SerializeField] private GameObject hexagon; //hexagon obj
    public float offset; //space between hexagons
    public int width; //width of the map
    public int height; //height of the map
    public Hex[,] map; //array that keeps track //stored in offset coordinates

    private GameObject player;

    /// <summary>
    /// the 6 neigbors a hex can have
    /// </summary>
    static int[,] cubeDirs = new int[,]
        { { 1, -1, 0 }, { 1, 0, -1 }, { 0, 1, -1 },
          { -1, 1, 0 }, { -1, 0, 1 }, { 0, -1, 1 } };

    static int[] testMap = new int[]

        { 0, 0, 0, 0,-1,-1, 0, 2, 0,-1,-1, 0, 0, 0, 0,
          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
          1, 1, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 1, 1,
          1, 0, 0, 0, 5, 0, 0, 0, 0, 5, 0, 0, 0, 0, 1,
          0, 0, 0, 0, 0, 5, 0, 0, 0, 5, 0, 0, 0, 0, 0,
          0, 0, 0, 0, 5, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0,
          0, 0, 0, 0, 0, 5, 0, 3, 0, 5, 0, 0, 0, 0, 0,
          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
          0, 0, 0, 0, 0, 0, 0,-1, 0, 0, 0, 0, 0, 0, 0,
          0, 0, 0, 0, 5, 5,-1,-1, 5, 5, 0, 0, 0, 0, 0,
          0, 0, 0, 5, 0, 0,-1,-1,-1, 0, 0, 5, 0, 0, 0,
          0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 1, 5, 0, 0, 0,
          0, 0, 0, 5, 5, 5, 0, 0, 0, 5, 5, 5, 0, 0, 0,
          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
          0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0 };

    static int[] map1 = new int[]
          { 5, 0, 0, 0, 5, 0, 0, 2, 0, 0, 5, 0, 0, 0, 5,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 5, 5, 0, 0,-1,-1,-1, 0, 0, 5, 5, 0, 0,
            0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0,
            0, 0, 0, 5, 0,-1, 0,-1, 0,-1, 0, 5, 0, 0, 0,
            0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0,
            5, 0, 0, 1, 1, 5, 0,-1, 0, 5, 1, 1, 0, 0, 5,
            5, 0, 0, 5, 5, 0, 0, 0, 0, 5, 5, 0, 0, 5, 5,
            0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0,
            0,-1,-1, 0, 1, 1, 0, 0, 1, 1, 0,-1,-1, 0, 0,
            5, 0, 0, 0, 5, 1, 1, 0, 1, 1, 5, 0, 0, 0, 5,
            5, 0, 0, 0, 5, 5, 0, 0, 5, 5, 0, 0, 0, 5, 5,
            0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0,
            1, 1, 5, 0, 0, 5, 5, 5, 5, 0, 0, 5, 1, 1, 0,
            5, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 5,
            5, 1, 5,-1,-1,-1, 0, 0,-1,-1,-1, 5, 1, 5, 5,
           -1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,-1 };

    static int[] map2 = new int[]
      { 1, 1, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 1, 1,
        1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1,
        1, 1, 0, 0, 0, 5,-1, 0, 0,-1, 5, 5, 5, 5, 5,
        5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
       -1,-1, 5, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
        1, 0, 0,-1, 0, 0,-1, 5, 5, 5, 5, 5, 5, 5, 5,
        1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
        1,-1, 5,-1, 0, 0, 0, 0,-1, 0, 0, 0, 0, 1, 1,
        5, 5, 5, 5, 5,-1, 0, 3, 0, 5, 5, 5, 5, 5, 5,
        1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,-1,
        1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,-1,
        5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,-1, 0, 0, 0,
        5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0,-1, 0, 0, 0,
        5, 1, 0, 0,-1, 5, 5, 5, 5, 0, 0, 0, 0, 0, 5,
        5, 1, 1, 0,-1, 0, 0, 0, 0,-1, 0, 0, 0, 1, 5,
        5, 1, 0, 0, 0, 0, 0, 0,-1, 5, 0, 0, 1, 1, 5,
        5, 5, 5, 5, 5, 5, 4, 4, 4, 5, 5, 5, 5, 5, 5 };

    // Use this for initialization
    void Start () {
        //createRectangleMap(width, height, offset);
        float ran = Random.Range(0, 3);
        if(ran < 1) { createMapFromArray(map1); }
        else if(ran < 2) { createMapFromArray(map2); }
        else
        {
            createMapFromArray(testMap);
        }
        //player = GameObject.Find("Player_obj");

        //player.GetComponent<playerScript>().mapReference = this.GetComponent<Map>();

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
        //arr[0] = q - (r - (r & 1)) / 2;
        //arr[2] = r;
        arr[0] = q;
        arr[2] = r - (q - (q & 1)) / 2;
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
        //arr[0] = xx + (zz - (zz & 1)) / 2;
        //arr[1] = zz;
        arr[0] = xx;
        arr[1] = zz + (xx - (xx & 1)) / 2;
        return arr; // [row, col]
    }

    /// <summary>
    /// creates a map from an array of numbers
    /// </summary>
    /// <param name="newMap">the map that is being used</param>
    private void createMapFromArray(int[] newMap)
    {
        //Debug.Log("Test");
        createRectangleMap(width, height, offset); //reset everything
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                map[i, j].Type = (Hex.TYPE)newMap[(j * width) + i];
            }
        }
    }

    protected void createRectangleMap(int w, int h, float off) //creates a 'rectangular' map
    {
        if (w % 2 == 1) { w--; }
        if (h % 2 == 1) { h--; }
        width = w + 1;
        height = h + 1;
        map = new Hex[width, height];
        GameObject[] TBD = GameObject.FindGameObjectsWithTag("Hex");
        while(TBD.Length < 0) { GameObject.Destroy(TBD[0]); } //clears all tiles so there is no overlap
        offset = off;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject go = Instantiate(hexagon);
                int[] arr = offsetToCube(i, j); //gets the cube coords of tile
                go.GetComponent<Hex>().setLocation(i, j, arr[0], arr[2]); //sets location for the tile
                addToMap(go.GetComponent<Hex>());
                go.transform.position = new Vector3(j % 2 == 0 ? i * (1 + off) : i * (1 + off) + (.5f + (off/2)), 0, j * (Mathf.Sqrt(3) / 2 + off)); //moves tile of physical position in space
            }
        }

       // Debug.Log(map);

    }

    public void addToMap(Hex tile)
    {
        //Debug.Log(tile.Row + ", " + tile.Col);
        map[tile.Row, tile.Col] = tile;
    }

    /// <summary>
    /// A* path finding in hex map
    /// finds quickest path from start to end
    /// </summary>
    /// <param name="start">starting hex</param>
    /// <param name="end">ending hex</param>
    /// <returns>list of hexs, in order, from start to end</returns>
    public Hex[] pathfinding(Hex start, Hex end)
    {
        List<Hex> visited = new List<Hex>(); //list of hexs that already been visited
        List<Hex> fringe = new List<Hex>(); //list of hexs that have been discovered, but not visited
        fringe.Add(start);
        Dictionary<Hex, Hex> cameFrom = new Dictionary<Hex, Hex>(); //Hex can most efficiently be reached from
        Dictionary<Hex, int> gScore = new Dictionary<Hex, int>(); //the cost of getting from the start hex to that hex
        gScore.Add(start, 0);

        while (fringe.Count > 0)
        {
            Hex current = fringe[0];
            if(current.Equals(end)) { return reconstructPath(cameFrom, current).ToArray(); }

            fringe.RemoveAt(0);
            visited.Add(current);

            if(current.isSolid()) { continue; } //if current is a wallm then skip it
            for (int j = 0; j < 6; j++) //checks each direction
            {
                Hex neighbor = getNeigbor(current, j); //returns neighbor that is in bounds and not NULL type
                if (neighbor != null) //got a hex
                {
                    if (visited.Contains(neighbor)) //if neighbor has already been visited
                    {
                        continue; //passes over rest of code in for loop and iterates
                    }
                    if (!fringe.Contains(neighbor))
                    {
                        fringe.Add(neighbor);
                    }

                    int tentGScore = gScore[current] + 1;
                    if(gScore.ContainsKey(neighbor) && tentGScore >= gScore[neighbor]) { continue; }
                    if (!gScore.ContainsKey(neighbor))//if gscore does not contain neighbor
                    {
                        cameFrom.Add(neighbor, current);
                        gScore.Add(neighbor, tentGScore);
                    }
                    else
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentGScore;
                    }
                }
            }

            //sort fringe
            fringe.Sort(delegate(Hex a, Hex b)
            {
                float i = (distanceBetween(a, end) + gScore[a]) - (distanceBetween(b, end) + gScore[b]);
                if(i == 0)
                {
                    if (a.Type == Hex.TYPE.INFO) { return 1; }
                    if (b.Type == Hex.TYPE.INFO) { return -1; }
                    return 0;
                }
                return i < 0 ? -1 : 1;
            });
        }

        return null; //no path can be found
    }

    /// <summary>
    /// reconstructs path from start to end
    /// recursion bois
    /// </summary>
    private List<Hex> reconstructPath(Dictionary<Hex, Hex> cameFrom, Hex current)
    {
        if (cameFrom.ContainsKey(current))
        {
            List<Hex> path = new List<Hex>();
            path.AddRange(reconstructPath(cameFrom, cameFrom[current]));
            path.Add(current);
            return path;
        }
        return new List<Hex>() { current };
    }

    /// <summary>
    /// Gets distance between two hexs, ignores type of hex
    /// </summary>
    /// <param name="center"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public int distanceBetween(Hex center, Hex target)
    {
        return Mathf.Max(Mathf.Abs(center.X - target.X), Mathf.Abs(center.Y - target.Y), Mathf.Abs(center.Z - target.Z));
    }

    /// <summary>
    /// gets one of the 6 neighbors in a direction
    /// </summary>
    public Hex getNeigbor(Hex tile, int dir)//get neighbor of tile in direction
    {
        if (hexExists(tile.X, tile.Y, tile.Z) && hexExists(tile.X + cubeDirs[dir, 0], tile.Y + cubeDirs[dir, 1], tile.Z + cubeDirs[dir, 2]))
        {
            int[] arr = cubeToOffset(tile.X + cubeDirs[dir, 0], tile.Z + cubeDirs[dir, 2]); //gets offset location of neighboring tiles
            //Debug.Log(arr[1] + ", " + arr[0]);
            return map[arr[1], arr[0]];
        }
        return null;
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
    /// gets all hexs with the type desired
    /// </summary>
    /// <param name="type">type of hex desired</param>
    /// <returns>all hexs in the map with the desired type</returns>
    public Hex[] getHexsWithType(Hex.TYPE type)
    {
        if (map == null) Debug.Log("why tho");
        List<Hex> hexs = new List<Hex>();
        foreach (Hex h in map)
        {

           // Debug.Log(h);

            if (h.Type == type) {

                //Debug.Log("Found: " + h);

                hexs.Add(h);

            }

        }
        return hexs.ToArray();
    }

    /// <summary>
    /// gets the hex from the map
    /// </summary>
    public Hex getHex(int xx, int yy, int zz)
    {

        if(inBounds(xx, yy, zz))
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
        //printMap();
        return false;
    }

    /// <summary>
    /// checks to see if hex is inbounds of the map
    /// </summary>
    private bool inBounds(int xx, int yy, int zz)
    {
        int[] loc = cubeToOffset(xx, zz);
        if (loc[1] >= 0 && loc[1] < width && loc[0] >= 0 && loc[0] < height) //hex is in bounds
        {
            return true;
        }
        return false;
    }

    protected void printMap()
    {
        string s = "{ ";
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (i == width - 1 && j == height - 1) { s += (int)map[i, j].Type; }
                else { s += (int)map[i, j].Type + ", "; }
            }
            s += "\n";
        }
        s += " }";
        Debug.Log(s);
    }

}
