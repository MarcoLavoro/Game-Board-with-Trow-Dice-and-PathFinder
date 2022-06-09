using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //grind size
    public int width = 10;
    public int height = 10;
    public bool GenerateObstacles = true;

    //prefabs
    public GameObject tilePrefab;
    public GameObject prefabPlayer;
    public GameObject prefabIA;

    //Tile container
    public Transform tilesParent;

    //logic variables
    public PathFind.Grid grid;
    public static GameManager Instance;
    private PlayerController Player;
    private AIController IA;
    public static bool PlayerTurn = true;
    public static Vector3 ArrivalPoint;
    public static bool gameEnded = false;

    #region Initialization;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if ((width < 2) || (height < 2))
            if (GenerateObstacles)
            {
                Debug.LogWarning("THE GRID HAVE A DIMENSION TOO SMALL TO CREATE OBSTACLES, CONSIDER TO ENLARGE THE GRID OR DISABLE THE GENERATE OBSTACLES");
                return;
            }


        CreateTileMap();
        CreatePlayer();
        CreateIA();

        //be sure there is no dead end for player or IA, otherwise reset the game
        ChechIfDeadEnd();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key was pressed");
            Application.Quit();
        }
    }

    private void CreateTileMap()
    {
        bool[,] tilesmap = new bool[width, height];

        for (int y = 0; y < tilesmap.GetLength(0); y++)
        {
            for (int x = 0; x < tilesmap.GetLength(1); x++)
            {
                if (tilesmap[y, x] == false)
                {

                    GameObject GO = GameObject.Instantiate(tilePrefab, new Vector3(y, 0, x), Quaternion.identity);
                    GO.transform.SetParent(tilesParent);

                    Color color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
                    //if is the last one
                    if ((y == width - 1) && (x == height - 1))
                    {
                        tilesmap[y, x] = true;
                        color = Color.blue;
                    }
                    else
                    {
                        int RandomValue = Random.Range(0, 100);
                        //if I do not want obstacles
                        if (!GenerateObstacles)
                            RandomValue = 0;
                        //more than 80 = not walkable
                        if (RandomValue > 80)
                        {
                            tilesmap[y, x] = false;
                            color = Color.red;
                            //only if is not the last one
                            GO.transform.localScale = new Vector3(GO.transform.localScale.x, GO.transform.localScale.y * 10, GO.transform.localScale.z);
                        }
                        else
                        {
                            tilesmap[y, x] = true;
                            color = Color.green;
                        }
                    }
                    GO.GetComponent<MeshRenderer>().material.color = color;
                    ArrivalPoint = GO.transform.position;
                }
            }
        }

        // create a grid
        grid = new PathFind.Grid(width, height, tilesmap);

    }

    private void CreatePlayer()
    {
        Vector2 startingPoints = returnWalkablePoint();
        int x = (int)startingPoints.x;
        int y = (int)startingPoints.y;

        GameObject GO = GameObject.Instantiate(prefabPlayer, new Vector3(x, 0, y), Quaternion.identity);
        Player = GO.AddComponent<PlayerController>();
        Player.actualPosition = new PathFind.Point(x, y);

        UIManager.Instance.AssignPlayerButtons(Player);
    }

    private void CreateIA()
    {
        Vector2 startingPoints = returnWalkablePoint();
        int x = (int)startingPoints.x;
        int y = (int)startingPoints.y;//  if

        GameObject GO = GameObject.Instantiate(prefabIA, new Vector3(x, 0, y), Quaternion.identity);
        IA = GO.AddComponent<AIController>();
        IA.actualPosition = new PathFind.Point(x, y);
    }


    private void ChechIfDeadEnd()
    {
        if ((Player.ChechIfDeadEnd() || (IA.ChechIfDeadEnd())))
            ResetGame();
    }

    Vector2 returnWalkablePoint()
    {
        int maxWidth = width / 3;
        if (maxWidth > 2)
            maxWidth--;

        int maxHeight = height / 3;
        if (maxHeight > 2)
            maxHeight--;

        int x = Random.Range(0, maxWidth);
        int y = Random.Range(0, maxHeight);
        while (!grid.nodes[x, y].walkable)
        {
            x = Random.Range(0, maxWidth);
            y = Random.Range(0, maxHeight);
        }

        return new Vector2(x, y);
    }
    #endregion

    #region GameManagments;
    public void ChangeTurn()
    {
        if (GameManager.gameEnded) return;

        PlayerTurn = !PlayerTurn;
        UIManager.Instance.ChangeTurn(PlayerTurn);

        if (!PlayerTurn)
            AIController.Instance.MadeMove();

    }

    public void EndGame()
    {

        UIManager.Instance.EndGaame();
    }
    public void ResetGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    #endregion

}
