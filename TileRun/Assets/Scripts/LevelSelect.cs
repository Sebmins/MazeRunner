using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    //public int intLevel;

    public void LoadScene(int intLevel)
    {
        FloorHandler.custom = false;
        SceneManager.LoadScene(intLevel);
    }

    
    public void NextScene()
    {
        //GameObject.Destroy(GameObject.Find("Level1"));
        if (SceneManager.GetActiveScene().buildIndex >= 6)
        {
            SceneManager.LoadScene(0);
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Return))
        {
            FloorHandler.update = false;
            LoadScene(0);
            return;
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                FloorHandler.update = false;
                LoadScene(0);
                return;
            }
        }
    }
}