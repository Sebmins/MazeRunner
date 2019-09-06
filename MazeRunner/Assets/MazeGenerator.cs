using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

    public GameObject[] tiles;
    public GameObject[] vTiles;
    public GameObject wall;
    public GameObject startTile;

    public List<Vector3> createdTiles;
    public List<Vector3> visitedTiles;

    public int tileAmount;
    public int tileDist;
    public float waitTime;

    public float chanceUp;
    public float chanceRight;
    public float chanceDown;

    // Use this for initialization
    IEnumerator Start()
    {
        UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));

        CreateTile(0);

        yield return new WaitForSeconds(1);

        StartCoroutine(GeneratePaths(tileAmount));

        //Random.seed = 10;
    }

    IEnumerator GeneratePaths(int amount)
    {
        for (int i = 1; i <= amount; i++)
        {
            float dir = Random.Range(0f, 1f);
            int tile = Random.Range(2, tiles.Length);

            Vector3 oldPos = transform.position;
            //CreateTile(tile);

            if (isCellEmpty(dir) == true)
            {
                CreateTile(Random.Range(2, tiles.Length));
                yield return new WaitForSeconds(0.01f);
                CallMoveGen(dir);
                yield return new WaitForSeconds(0.01f);
                CreateTile(Random.Range(2, tiles.Length));
                i++;
            }
            else
            {
                //i--;
            }
            //yield return new WaitForSeconds(0.01f);
        }
        yield return 0;

        CreateTile(1);

        StartCoroutine(GenerateDeadEnds());
    }

    IEnumerator GenerateDeadEnds()
    {
        transform.position = new Vector3(0, 0, 0);

        float dir = Random.Range(0f, 1f);
        int tile = Random.Range(2, tiles.Length);
        int paths = Random.Range(40, 80);

        while (findEmptyCell(dir) == false)
        {
            dir = Random.Range(0f, 1f);

            //yield return new WaitForSeconds(1);
        }


        StartCoroutine(GeneratePaths(paths));

        //yield return new WaitForSeconds(1);
        

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
        else {
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
                transform.position = new Vector3(transform.position.x, transform.position.y + tileDist, 0);
                break;
            case 1:
                transform.position = new Vector3(transform.position.x + tileDist, transform.position.y, 0);
                break;
            case 2:
                transform.position = new Vector3(transform.position.x, transform.position.y - tileDist, 0);
                break;
            case 3:
                transform.position = new Vector3(transform.position.x - tileDist, transform.position.y, 0);
                break;
        }
    }

    bool PosEmpty()
    {
        if (!createdTiles.Contains(transform.position))
        {
            return true;
        }
        else { return false;  }
    }

    void CreateTile(int tileIndex)
    {
        GameObject tileObject;

        tileObject = Instantiate(tiles[tileIndex], transform.position, transform.rotation) as GameObject;

        createdTiles.Add(tileObject.transform.position);
    }
}
