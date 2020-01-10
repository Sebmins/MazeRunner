using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelText : MonoBehaviour
{
    public Text levelText;
    public Text mapText;
    public static int levelCount;
    public GameObject canvasComplete;
    public static int maxLevel = 10;
    void Start()
    {
        levelCount = 1;
        mapText.text = "Map " + levelCount;
        Scene scene = SceneManager.GetActiveScene();
        levelText.text = "Level " + scene.buildIndex;
    }

    private void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        mapText.text = "Map " + levelCount;
        if (FloorHandler.custom == true)
           levelText.text = "Custom Level";
        else
            levelText.text = "Level " + scene.buildIndex;
        //levelText.text = "Level " + SceneManager.GetActiveScene().buildIndex;

        if (levelCount > maxLevel)
        {
            FloorHandler.custom = false;
            canvasComplete.SetActive(true);
            Timer.pause = true;
        }
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void changeText()
    {
        Scene scene = SceneManager.GetActiveScene();
        mapText.text = "Map " + levelCount;
        levelText.text = "Level " + scene.buildIndex;
        //levelText.text = "Level " + SceneManager.GetActiveScene().buildIndex;
    }
}
