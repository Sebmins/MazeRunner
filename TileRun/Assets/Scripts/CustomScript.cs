using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomScript : MonoBehaviour
{
    public GameObject myPrefab;
    public GameObject cComp;
    public GameObject cUI;

    // Start is called before the first frame update
    void Start()
    {
        GameObject prefabObject = Instantiate(myPrefab);
        prefabObject.GetComponent<FloorHandler>().tilesPerPath = MainMenu.tilesPerPathText;
        prefabObject.GetComponent<FloorHandler>().paths = MainMenu.pathsText;
        prefabObject.GetComponent<FloorHandler>().standardDeviation = MainMenu.standardDeviationText;
        prefabObject.GetComponent<FloorHandler>().maxLevel = MainMenu.maxLevelText;
        prefabObject.GetComponent<FloorHandler>().time = MainMenu.timeText;
        prefabObject.GetComponent<FloorHandler>().canvasComplete = cComp;
        prefabObject.GetComponent<FloorHandler>().canvasUI = cUI;


    }
    
}
