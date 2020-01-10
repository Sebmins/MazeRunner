using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject tilesPerPathObj;
    public static int tilesPerPathText;
    public GameObject pathsObj;
    public static int pathsText;
    public GameObject standardDeviationObj;
    public static int standardDeviationText;
    public GameObject maxLevelObj;
    public static int maxLevelText;
    public GameObject timeObj;
    public static int timeText;

    public void getInput()
    {
        tilesPerPathText = int.Parse(tilesPerPathObj.GetComponent<Text>().text);
        pathsText = int.Parse(pathsObj.GetComponent<Text>().text);
        standardDeviationText = int.Parse(standardDeviationObj.GetComponent<Text>().text);
        maxLevelText = int.Parse(maxLevelObj.GetComponent<Text>().text);
        Timer.time = int.Parse(timeObj.GetComponent<Text>().text);

        FloorHandler.custom = true;
        
        SceneManager.LoadScene(7);
        
    }
    
    public void PlayGame()
    {
        FloorHandler.update = false;
        FloorHandler.custom = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
