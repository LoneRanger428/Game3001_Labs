using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum TileStatus
{
    UNVISITED,
    OPEN,
    CLOSED,
    IMPASSABLE,
    GOAL,
    START
};

public enum NeighbourTile
{
    TOP_TILE,
    RIGHT_TILE,
    BOTTOM_TILE,
    LEFT_TILE,
    NUM_OF_NEIGHBOUR_TILES
};

public class GridManager : MonoBehaviour
{
    // Fill in for Lab 4 Part 1.

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject tilePanelPrefab;
    [SerializeField] private GameObject panelParent;
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private Color[] colors;
    [SerializeField] private float baseTileCosts = 1.0f;

    [SerializeField] private bool useManhattanHeuristic = true;

    private GameObject[,] grid; //2d array grid
    private int rows = 12;
    private int cols = 16;

    private List<GameObject> mines = new List<GameObject>();

    public static GridManager Instance { get; private set; } // Static object of the class.

    void Awake()
    {
        if (Instance == null) // If the object/instance doesn't exist yet.
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Initialize()
    {
        // Fill in for Lab 4 Part 1.
        BuildGrid();
        ConnectGrid();
    }

    void Update()
    {
        // Fill in for Lab 4 Part 1.
        if(Input.GetKeyDown(KeyCode.G))
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(!child.gameObject.activeSelf);
            }
            panelParent.gameObject.SetActive(!panelParent.gameObject.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.M)) // I have used GetKey, so that I can insert mines while holding my M key :)
        {
            Vector2 gridPosition = GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            GameObject mineInst = GameObject.Instantiate(minePrefab, new Vector3(gridPosition.x, gridPosition.y, 0f), Quaternion.identity);
            Vector2 mineIndex = mineInst.GetComponent<NavigationObject>().GetGridIndex();
            grid[(int)mineIndex.y, (int)mineIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.IMPASSABLE);

            mines.Add(mineInst); // to clear it from the screen
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach(GameObject mine in mines)
            {
                Vector2 mineIndex = mine.GetComponent<NavigationObject>().GetGridIndex();
                grid[(int)mineIndex.y, (int)mineIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.UNVISITED);
                Destroy(mine);
            }
            mines.Clear();
        }
    }

    private void BuildGrid()
    {
        // Fill in for Lab 4 Part 1.
        grid = new GameObject[rows, cols];
        int count = 0;
        float rowPos = 5.5f;

        for (int i = 0; i < rows; i++, rowPos--)
        {
            float colPos = -7.5f;
            for(int j = 0; j < cols; j++, colPos++)
            {
                GameObject tileInst = GameObject.Instantiate(tilePrefab, new Vector3(colPos, rowPos, 0.0f), Quaternion.identity);
                TileScript tileScript = tileInst.GetComponent<TileScript>();
                tileScript.SetColor(colors[System.Convert.ToInt32((count++ % 2 == 0))]);
                tileInst.transform.parent = transform;
                grid[i,j] = tileInst;

                // Instantiate a new TilePanel and link it to the Tile instance
                GameObject panelInst = GameObject.Instantiate(tilePanelPrefab, tilePanelPrefab.transform.position, Quaternion.identity);
                panelInst.transform.parent = panelParent.transform;
                RectTransform panelTransform = panelInst.GetComponent<RectTransform>();
                panelTransform.position = new Vector3(0f, 95f, 0f); 
                panelTransform.localScale = Vector3.one; 
                panelTransform.anchoredPosition = new Vector3(64f * j, -64f * i);
                tileScript.tilePanel = panelInst.GetComponent<TilePanel>();
               
            }
            count--;
        }
        // Set the tile under the ship to start

        GameObject ship = GameObject.FindGameObjectWithTag("Ship");
        Vector2 shipIndices = ship.GetComponent<NavigationObject>().GetGridIndex();
        grid[(int)shipIndices.y, (int)shipIndices.x].GetComponent<TileScript>().SetStatus(TileStatus.START);

        // Set the tile under the player to goal and set tile costs.

        GameObject planet = GameObject.FindGameObjectWithTag("Planet");
        Vector2 planetIndices = planet.GetComponent<NavigationObject>().GetGridIndex();
        grid[(int)planetIndices.y, (int)planetIndices.x].GetComponent<TileScript>().SetStatus(TileStatus.GOAL);
    }

    private void ConnectGrid()
    {
        // Fill in for Lab 4 Part 1.
        for(int i = 0; i < rows; i++) // i = row
        {
            for(int j = 0; j < cols; j++) // j = column
            {
                TileScript tileScript = grid[i,j].GetComponent<TileScript>();

                if(i > 0) // set top neighbour if tile is not in top row
                {
                    tileScript.SetNeighbourTile((int)NeighbourTile.TOP_TILE, grid[i-1, j]);
                }
                if (j < cols - 1) // set top neighbour if tile is not in top row
                {
                    tileScript.SetNeighbourTile((int)NeighbourTile.RIGHT_TILE, grid[i, j+1]);
                }
                if (i < rows - 1) // set top neighbour if tile is not in top row
                {
                    tileScript.SetNeighbourTile((int)NeighbourTile.BOTTOM_TILE, grid[i+1, j]);
                }

                if (j > 0) // set top neighbour if tile is not in top row
                {
                    tileScript.SetNeighbourTile((int)NeighbourTile.LEFT_TILE, grid[i, j-1]);
                }

            }
        }

    }

    public GameObject[,] GetGrid()
    {
        return grid;
    }

    // The following utility function creates the snapping to the center of a tile.
    public Vector2 GetGridPosition(Vector2 worldPosition)
    {
        float xPos = Mathf.Floor(worldPosition.x) + 0.5f;
        float yPos = Mathf.Floor(worldPosition.y) + 0.5f;
        return new Vector2(xPos, yPos);
    }

    public void SetTileCosts(Vector2 targetIndices)
    {
        float distance = 0f;
        float dx = 0f;
        float dy = 0f;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                TileScript tileScript = grid[i, j].GetComponent<TileScript>();

                if (useManhattanHeuristic)
                {
                    dx = Mathf.Abs(j - targetIndices.x);
                    dy = Mathf.Abs(i - targetIndices.y);
                    distance = dx + dy;
                }
                else // Euclidean
                {
                    dx = targetIndices.x - j;
                    dy = targetIndices.y - i;
                    distance = Mathf.Sqrt(dx * dx + dy * dy);
                }
                float adjustedCost = distance * baseTileCosts;
                tileScript.cost = adjustedCost;
                tileScript.tilePanel.costText.text = tileScript.cost.ToString("F1");
                    
            }
        }
    }
}
