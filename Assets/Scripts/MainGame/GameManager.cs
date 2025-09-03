using UnityEngine;
using System.Collections;
// using UnityEngine.UI; // include UI namespace so can reference UI elements
using UnityEngine.SceneManagement; // include so we can manipulate SceneManager
using TMPro;

public class GameManager : MonoBehaviour
{

    /// <summary>
    /// Static reference to game manager so can be called from other scripts directly (not just through gameobject component)
    /// </summary>
    public static GameManager myGameManager_GameManager;

    // levels to move to on victory and lose
    public string levelAfterVictory;
    public string levelAfterGameOver;

    // game performance
    public int score = 0;
    public int highscore = 0;
    public int startLives = 3;
    public int lives = 3;

    // UI elements to control. NOTE: Updated to TEXT MESH PRO.
    public /* Text */ TMP_Text scoreUI_TMP_Text;
    public /* Text */ TMP_Text highScoreUI_TMP_Text;
    public /* Text */ TMP_Text levelUI_TMP_Text;
    public GameObject[] extraLivesUI_ArrayOfGameObject;
    public GameObject gamePausedUI_GameObject;

    // private variables
    GameObject _player_GameObject;
    Vector3 _spawnLocation_Vector3;
    Scene _scene_Scene;

    // set things up here
    void Awake()
    {
        // setup reference to game manager
        if (myGameManager_GameManager == null)
            myGameManager_GameManager = this.GetComponent<GameManager>();

        // setup all the variables, the UI, and provide errors if things not setup properly.
        SetupDefaults();
    }

    // game loop
    void Update()
    {
        // if ESC pressed then pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale > 0f)
            {
                gamePausedUI_GameObject.SetActive(true); // this brings up the pause UI
                Time.timeScale = 0f; // this pauses the game action
            }
            else
            {
                Time.timeScale = 1f; // this unpauses the game action (ie. back to normal)
                gamePausedUI_GameObject.SetActive(false); // remove the pause UI
            }
        }
    }


    /// <summary>
    /// Setup all the variables, the UI, and provide errors if things not setup properly.
    /// </summary>
    void SetupDefaults()
    {
        // setup reference to player
        if (_player_GameObject == null)
            _player_GameObject = GameObject.FindGameObjectWithTag("Player");

        if (_player_GameObject == null)
            Debug.LogError("Player not found in Game Manager");

        // get current scene
        _scene_Scene = SceneManager.GetActiveScene();

        // get initial _spawnLocation based on initial position of player
        _spawnLocation_Vector3 = _player_GameObject.transform.position;

        // if levels not specified, default to current level
        if (levelAfterVictory == "")
        {
            Debug.LogWarning("levelAfterVictory not specified, defaulted to current level");
            levelAfterVictory = _scene_Scene.name;
        }

        if (levelAfterGameOver == "")
        {
            Debug.LogWarning("levelAfterGameOver not specified, defaulted to current level");
            levelAfterGameOver = _scene_Scene.name;
        }

        // friendly error messages
        if (scoreUI_TMP_Text == null)
            Debug.LogError("Need to set UIScore on Game Manager.");

        if (highScoreUI_TMP_Text == null)
            Debug.LogError("Need to set UIHighScore on Game Manager.");

        if (levelUI_TMP_Text == null)
            Debug.LogError("Need to set UILevel on Game Manager.");

        if (gamePausedUI_GameObject == null)
            Debug.LogError("Need to set UIGamePaused on Game Manager.");

        // get stored player prefs
        RefreshPlayerState();

        // get the UI ready for the game
        RefreshGUI();
    }


    /// <summary>
    /// Get stored Player Prefs if they exist, otherwise go with defaults set on gameObject
    /// </summary>
    void RefreshPlayerState()
    {
        lives = PlayerPrefManager.GetLives();

        // special case if lives <= 0 then must be testing in editor, so reset the player prefs
        if (lives <= 0)
        {
            PlayerPrefManager.ResetPlayerState(startLives, false);
            lives = PlayerPrefManager.GetLives();
        }
        score = PlayerPrefManager.GetScore();
        highscore = PlayerPrefManager.GetHighscore();

        // save that this level has been accessed so the MainMenu can enable it
        PlayerPrefManager.UnlockLevel();
    }


    /// <summary>
    /// Refresh all the GUI elements <br />
    /// It is called: at the beginning (in the Awake function, in "SetupDefaults"), AND each time the Player Dies (in "ResetGame").
    /// </summary>
    void RefreshGUI()
    {
        // set the text elements of the UI
        /* ORIGINAL: UIScore.text = "Score: " + score.ToString(); */
        scoreUI_TMP_Text.text = "Score: <size=83%><color=#FFF000>" + score.ToString() + "</color>";
        /* ORIGINAL: UIHighScore.text = "Highscore: " + highscore.ToString(); */
        highScoreUI_TMP_Text.text = "Highscore: <size=83%><color=#FFF000>" + highscore.ToString() + "</color>";
        /* ORIGINAL: UILevel.text = _scene.name; */
        levelUI_TMP_Text.text = "<size=83%><color=#FF0000>" + _scene_Scene.name + "</color>";

        // turn on the appropriate number of life indicators in the UI based on the number of lives left
        for (int i = 0; i < extraLivesUI_ArrayOfGameObject.Length; i++)
        {
            if (i < (lives - 1))
            {
                // show one less than the number of lives since you only typically show lifes after the current life in UI
                extraLivesUI_ArrayOfGameObject[i].SetActive(true);
            }
            else
            {
                extraLivesUI_ArrayOfGameObject[i].SetActive(false);
            }
        }
    }


    /// <summary>
    /// Public function to add Points and update the GUI and highscore player Prefs accordingly (each time the Player gets a a Pickup... a COIN or anything that increases his Score).
    /// </summary>
    /// <param name="amount"></param>
    public void AddPoints(int amount)
    {
        // increase score
        score += amount;

        // update UI
        /* ORIGINAL: UIScore.text = "Score: " + score.ToString(); */
        scoreUI_TMP_Text.text = "Score: <size=83%><color=#FFF000>" + score.ToString() + "</color>";

        // if score>highscore then update the highscore UI too
        if (score > highscore)
        {
            highscore = score;
            /* ORIGINAL: UIHighScore.text = "Highscore: " + score.ToString(); */
            highScoreUI_TMP_Text.text = "Highscore: <size=83%><color=#FFF000>" + highscore.ToString() + "</color>";
        }
    }


    /// <summary>
    /// Public function to Remove player life and Reset game accordingly.<br />
    /// It is called each time the Player Dies.
    /// </summary>
    public void ResetGame()
    {
        // remove life and update GUI
        lives--;
        RefreshGUI();

        if (lives <= 0)
        { 
            // no more lives
            // save the current player prefs before going to GameOver
            PlayerPrefManager.SavePlayerState(score, highscore, lives);

            // load the gameOver screen
            SceneManager.LoadScene(levelAfterGameOver);
        }
        else
        {
            // tell the player to respawn
            _player_GameObject.GetComponent<CharacterController2D>().Respawn(_spawnLocation_Vector3);
        }
    }


    /// <summary>
    /// Public function for level complete
    /// </summary>
    public void LevelCompete()
    {
        // save the current player prefs before moving to the next level
        PlayerPrefManager.SavePlayerState(score, highscore, lives);

        // use a coroutine to allow the player to get fanfare before moving to next level
        StartCoroutine(LoadNextLevel());
    }


    /// <summary>
    /// Load the nextLevel after delay
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(levelAfterVictory);
    }
}
