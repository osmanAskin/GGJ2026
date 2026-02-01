using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string sceneName = "LevelDesign";
    
    public void StartButton()
    {
        SceneManager.LoadScene(sceneName);
    }
}
