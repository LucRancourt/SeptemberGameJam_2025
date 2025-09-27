using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>//, ISaveable
{
    #region
    [Header("Level Information")]
    public int CurrentLevelIndex { get; private set; }
    //public Level CurrentLevel { get; private set; }
    public float DifficultyValue { get; private set; }

    [Header("Level Rewards")]
    //public PlayerUnit RewardedUnit;
    [SerializeField] GameObject _combatRewardPrefab;
    GameObject _spawnedReward;


    private List<int> _playableLevelIndices = new List<int>();
    private const string PERSISTENT_SCENE_NAME = "PersistentScene";
    private const int PLAYABLE_LEVEL_START_INDEX = 2;
    private const int MAIN_MENU_SCENE_INDEX = 1;
    #endregion

    // --- Level Load/Unloading ---
    // ----------------------------
    #region
    public void Start()
    {
        SceneManager.sceneLoaded += SetActiveScene;
        LoadLevel(MAIN_MENU_SCENE_INDEX);
    }
    private void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);    //so the persistent scene is not the one being unloaded
    }

    public int GetRandomPlayableLevelIndex()
    {
        int index = Random.Range(PLAYABLE_LEVEL_START_INDEX, SceneManager.sceneCountInBuildSettings);
        return index;
    }

    public void LoadLevel(int buildIndex)
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName(PERSISTENT_SCENE_NAME))     //do not unload the persistent scene
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);
    }

    /*
    public void SetLevel(Level level)
    {
        this.CurrentLevel = level;
        this.CurrentLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }
    */

    public void LoadMainMenu()
    {
        LoadLevel(MAIN_MENU_SCENE_INDEX);
    }
    #endregion
}