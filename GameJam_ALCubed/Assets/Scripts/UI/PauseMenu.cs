using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Menu<PauseMenu>
{
    // Variables
    [Header("Pause Settings")]
    [SerializeField] private GameObject pauseMenu;

    [Header("Buttons")]
    [SerializeField] private Button resumeGame;
    [SerializeField] private Button openSettingsMenu;
    [SerializeField] private Button returnToMainMenu;


    // Functions
    protected override void Awake()
    {
        base.Awake();

        pauseMenu.SetActive(false);

        resumeGame.onClick.AddListener(UnPauseGame);
        openSettingsMenu.onClick.AddListener(OpenSettingsMenu);
        returnToMainMenu.onClick.AddListener(ReturnToMainMenu);
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PauseGame();
    }




    public void PauseGame()
    {
        Time.timeScale = 0.0f;

        pauseMenu.SetActive(true);
    }

    public void UnPauseGame()
    {
        pauseMenu.SetActive(false);

        Time.timeScale = 1.0f;
    }

    private void OpenSettingsMenu()
    {
        SettingsMenu.Instance.OpenMenu();
    }

    private void ReturnToMainMenu()
    {
        UnPauseGame();
        LevelManager.Instance.LoadMainMenu();
    }
}