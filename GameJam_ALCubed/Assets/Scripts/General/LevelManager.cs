using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    // Variables
    [SerializeField] private string startScreen = "MainMenu";


    // Functions
    private void Start()
    {
        LoadLevel(startScreen);
        SceneManager.sceneLoaded += SetActiveScene;
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        LoadingScreen.Instance.LoadLevel(levelName);
    }

    private void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
    }
}