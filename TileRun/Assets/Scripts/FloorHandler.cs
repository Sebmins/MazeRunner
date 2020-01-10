using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorHandler : MonoBehaviour
{
    private LevelText levelText;
    public GameObject tileSpawner;
    public GameObject[] tiles;
    public GameObject startTile;
    public GameObject defaultTile;
    public GameObject endTile;

    public List<Vector3> createdTiles;
    private GameObject[] tileObject;
    private GameObject groupedTiles;

    // public int firstPath;
    public int paths;
    public int standardDeviation;
    public int tilesPerPath;
    public int time;
    public int maxLevel;
    public int tileDist;
    public float waitTime;

    public float chanceUp;
    public float chanceRight;
    public float chanceDown;

    public GameObject canvasComplete;
    public GameObject canvasUI;
    public static bool update = false;
    private Vector3 spawnPosition;
    public static bool playing;
    public static bool custom = false;

    // Use this for initialization
    void Start() {
        FloorHandler.update = false;
        LevelText.levelCount = 1;
        LevelText.maxLevel = maxLevel;
        Timer.gameOver = false;

        //StopAllCoroutines();

        spawnPosition = transform.position;

        tiles = new GameObject[10000];

        groupedTiles = new GameObject("groupedTiles");

        //UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));

        CreateTile(startTile);
        //yield return new WaitForSeconds(0.5f);

        StartCoroutine(GeneratePaths(Random.Range(tilesPerPath - standardDeviation, tilesPerPath + standardDeviation), endTile));
        //Random.seed = 10;
    }

    int count = 0;
    IEnumerator GeneratePaths(int amount, GameObject tile)
    {
        playing = false;
        for (int i = 1; i <= amount; i++)
        {
            float dir = Random.Range(0f, 1f);
            //int tile = Random.Range(2, tiles.Length);

            Vector3 oldPos = transform.position;
            //CreateTile(tile);

            if (isCellEmpty(dir) == true)
            {
                CreateTile(defaultTile);
                yield return new WaitForSeconds(waitTime);
                CallMoveGen(dir);
                yield return new WaitForSeconds(waitTime);
                CreateTile(defaultTile);
                i++;
            }
        }
        yield return 0;

        CreateTile(tile);

        count++;
        if (count<=paths-1)
        {
            yield return new WaitForSeconds(0);
            StartCoroutine(GenerateDeadEnds(endTile));
        } else if(count==paths)
        {
            yield return new WaitForSeconds(0);
            //StartCoroutine("GenerateDeadEnds", startTile);
            StartCoroutine(GenerateDeadEnds(startTile));
            count++;
        }

        if (tile == startTile)
        {
            yield return new WaitForSeconds(2);
            update = true;
            Timer.pause = false;
            playing = true;
        }
    }

    IEnumerator GenerateDeadEnds(GameObject tile)
    {
        transform.position = spawnPosition;

        float dir = Random.Range(0f, 1f);
        //int tile = Random.Range(2, tiles.Length);
        //int paths = Random.Range(40, 80);

        while (findEmptyCell(dir) == false)
        {
            dir = Random.Range(0f, 1f);

        }

        yield return new WaitForSeconds(1);

        StartCoroutine(GeneratePaths(Random.Range(tilesPerPath-standardDeviation, tilesPerPath+standardDeviation), tile));

        yield return new WaitForSeconds(waitTime);


        yield return 0;
    }

    bool findEmptyCell(float dir)
    {
        Vector3 oldPos = transform.position;
        CallMoveGen(dir);
        CallMoveGen(dir);
        Vector3 newPos = transform.position;
        if (!createdTiles.Contains(transform.position))
        {
            transform.position = oldPos;
            return true;
        }
        else
        {
            transform.position = newPos;
            return false;
        }
    }

    bool isCellEmpty(float dir)
    {
        Vector3 oldPos = transform.position;
        CallMoveGen(dir);
        Vector3 newPos = transform.position;
        CallMoveGen(dir);
        if (!createdTiles.Contains(transform.position))
        {
            transform.position = newPos;
            return true;
        }
        else
        {
            transform.position = oldPos;
            return false;
        }
    }

    void CallMoveGen(float ranDir)
    {
        if (ranDir < chanceUp)
        {
            MoveGen(0);
        }
        else if (ranDir < chanceRight)
        {
            MoveGen(1);
        }
        else if (ranDir < chanceDown)
        {
            MoveGen(2);
        }
        else
        {
            MoveGen(3);
        }
    }

    void MoveGen(int dir)
    {
        switch (dir)
        {
            case 0:
                transform.position = new Vector3(transform.position.x, 0, transform.position.z + tileDist);
                break;
            case 1:
                transform.position = new Vector3(transform.position.x + tileDist, 0, transform.position.z);
                break;
            case 2:
                transform.position = new Vector3(transform.position.x, 0, transform.position.z - tileDist);
                break;
            case 3:
                transform.position = new Vector3(transform.position.x - tileDist, 0, transform.position.z);
                break;
        }
    }

    bool PosEmpty()
    {
        if (!createdTiles.Contains(transform.position))
        {
            return true;
        }
        else { return false; }
    }

    int countTiles = 0;
    void CreateTile(GameObject tile)
    {
        tiles[countTiles] = Instantiate(tile, transform.position, transform.rotation) as GameObject;

        createdTiles.Add(tiles[countTiles].transform.position);
        string number = ""+ createdTiles.Count;
        
        tiles[countTiles].name = tile.name+number;
        
        tiles[countTiles].transform.parent = groupedTiles.gameObject.transform;
        countTiles++;
    }

    public static int CameraDistance = 4;
    
    void FixedUpdate()
    {
        if (Timer.gameOver == true)
            return;

        int distX = Mathf.Abs((int)transform.position.x - (int)spawnPosition.x);
        int distZ = Mathf.Abs((int)transform.position.z - (int)spawnPosition.z);
        
        if (distX > CameraDistance)
        {
            CameraDistance = distX;
        }
        if (distZ > 20 && distZ > CameraDistance)
        {
            CameraDistance = distZ;
        }
        Debug.Log(update);
        if (update==true)
            checkPosition();
    }

    public LayerMask mask;
    void checkPosition()
    {
        Vector3 vec = Quaternion.AngleAxis(0, -transform.forward) * transform.up;
        Ray ray = new Ray(transform.position - transform.up, vec);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 50, mask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
            PlayerController.isTile=false;
            Timer.pause = true;
            update = false;
            if (LevelText.levelCount == LevelText.maxLevel)
            {
                CompletedLevel();
                return;
            }
            DestoryTiles();
            restartMaze();
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10, Color.green);
        }
    }
    
    void restartMaze()
    {
        LevelText.levelCount++;
        update = false;
        CameraDistance = 4;
        paths += 2;

        count = 0;
        spawnPosition = transform.position;

        createdTiles.Clear();
        countTiles = 0;

        groupedTiles = new GameObject("groupedTiles");

        CreateTile(startTile);

        int rando = (Random.Range(tilesPerPath - standardDeviation, tilesPerPath + standardDeviation));

        StartCoroutine(GeneratePaths(rando, endTile));
        //Random.seed = 10;
    }

    void DestoryTiles()
    {
        Destroy(groupedTiles);
        
    }

    void CompletedLevel()
    {
        canvasComplete.SetActive(true);
    }
}
