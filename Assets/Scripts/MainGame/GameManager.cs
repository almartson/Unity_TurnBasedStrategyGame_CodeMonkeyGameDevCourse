using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A simple State Pattern Class, that controls the Main Flow of the Game States.
/// Winning...
/// Losing...
/// All that is controlled here. An Orchestra Director.
/// </summary>
public class GameManager : MonoBehaviour
{

	#region Attributes

	/// <summary>
	/// Make game manager public static, so it can be accessed from other scripts
	/// </summary>
	public static GameManager Gm;

	#region Timer Management

	public float startTime = 5f;
	
	/// <summary>
	/// Current time
	/// </summary>
	private float _currentTime;

	/// <summary>
	/// Show timer on screen
	/// </summary>
	public TextMeshProUGUI mainTimerDisplay;

	#endregion Timer Management
	
	#region Players

	[Tooltip("It must be set here,  or the Game would show an Exception.")]
	[SerializeField]
	private GameObject _playerToTheLeft;
	
	[Tooltip("It must be set here,  or the Game would show an Exception.")]
	[SerializeField]
	private GameObject _playerToTheRight;

	#endregion Players

	#region Game States (State Pattern)

	public enum GameStates { Playing, Death, GameOver, BeatLevel };
	
	[SerializeField]
	private GameStates _gameState = GameStates.Playing;

	#endregion Game States (State Pattern)

	
	#region Exposed "public" fields

	[SerializeField]
	private int _score=0;
	[SerializeField]
	private bool _canBeatLevel = false;
	[SerializeField]
	private int _beatLevelScore=0;

	[SerializeField]
	private GameObject _mainCanvas;
	[SerializeField]
	private TextMeshProUGUI _mainScoreDisplay;
	[SerializeField]
	private GameObject _gameOverCanvas;
	[SerializeField]
	private TextMeshProUGUI _gameOverScoreDisplay;

	[Tooltip("Only need to set if canBeatLevel is set to true.")]
	[SerializeField]
	private GameObject _beatLevelCanvas;
	[Tooltip("This is a Score container in case you beat the Level. Only need to set if canBeatLevel is set to true.")]
	[SerializeField]
	private TextMeshProUGUI _beatLevelScoreDisplay;

	
	[SerializeField]
	private AudioSource _backgroundMusicAudioSource;
	
	// [SerializeField]
	// private AudioSource _backgroundSoundsSFX;

	[SerializeField]
	private AudioClip _gameOverSFX1;
	[SerializeField]
	private AudioClip _gameOverSFX2;

	[Tooltip("Only need to set if canBeatLevel is set to true.")]
	[SerializeField]
	private AudioClip _beatLevelSFX;
	
	#endregion Exposed "public" fields

	
	#region private fields

	private Health _healthOfPlayerToTheLeft;
	private Health _healthOfPlayerToTheRight;
	
	// private Health _playerHealth;
	
	#endregion private fields

	
	#region Loading Scenes + using GameObjects to Shoot at, or Touch

	// [SerializeField]
	// private GameObject _playAgainButtons;
	// [SerializeField]
	// private string _playAgainLevelToLoad;
	//
	// /// Buttons to restart the game to Level One
	// [Tooltip("Buttons to restart the game to Level One")]
	// [SerializeField]
	// private GameObject _restartGameButtons;
	//
	// [SerializeField]
	// private string _restartGameLevelToLoad;
	//
	// [SerializeField]
	// private GameObject _nextLevelButtons;
	//
	// [SerializeField]
	// private string _nextLevelToLoad;

	#endregion Loading Scenes + using GameObjects to Shoot at, or Touch
	
	#endregion Attributes


	#region Unity Methods

	/// setup the game
	void Awake ()
	{
		if (Gm == null) 
			Gm = gameObject.GetComponent<GameManager>();

		// if (_playerToTheLeft == null)
		// {
		// 	_playerToTheLeft = GameObject.FindWithTag("Player");
		// }

		// Access to:  Health  Components
		// Left
		_healthOfPlayerToTheLeft = _playerToTheLeft.GetComponent<Health>();
		// Right
		_healthOfPlayerToTheRight = _playerToTheRight.GetComponent<Health>();

		// setup score display
		Collect (0);

		// make other UI inactive
		_gameOverCanvas.SetActive (false);
		//
		if (_canBeatLevel)
		{
			if ( _beatLevelCanvas != null )
			{
				
				_beatLevelCanvas.SetActive (false);
			}
		}
		
		// Set the current time to the startTime specified
		//
		_currentTime = startTime;

		_backgroundMusicAudioSource.volume = .05f;

	}//End Awake


	// private void Start()
	// {
	// 	// Initialize Background Sounds SFX AudioSource
	// 	//
	// 	_backgroundSoundsSFX.Stop();
	// 	_backgroundSoundsSFX.loop = false;
	// 	_backgroundSoundsSFX.playOnAwake = false;
	// }


	void Update ()
	{
		switch (_gameState)
		{
			case GameStates.Playing:
				
				if (!_healthOfPlayerToTheLeft.isAlive  &&  !_healthOfPlayerToTheRight.isAlive)
				{
					// You  LOSE  the Game!
					//
					EndGame();

				}
				else if (_canBeatLevel && _score>=_beatLevelScore)
				{
					// You  WIN  the Game!
					//
					BeatLevel();

				}
				else if (_currentTime < 0) 
				{
					// check to see if timer has run out
					//
					EndGame ();
				} 
				else 
				{
					// game playing state, so update the timer
				
					_currentTime -= Time.deltaTime;
					mainTimerDisplay.text = _currentTime.ToString ("0");
					
				}//End GameStates.Playing
				
				break;
			
			case GameStates.Death:
				
				_backgroundMusicAudioSource.volume -= 0.01f;
				
				if (_backgroundMusicAudioSource.volume<=0.0f)
				{
					// Original Code: AudioSource.PlayClipAtPoint (_gameOverSFX,gameObject.transform.position);
					//AudioSource.PlayClipAtPoint (_gameOverSFX,_backgroundMusicAudioSource.transform.position);
					//
					//PlayTwoSoundsInAChainForGameOver(_gameOverSFX1, _gameOverSFX2, _backgroundMusicAudioSource.transform.position);
					//
					AudioSource.PlayClipAtPoint (_gameOverSFX1, _backgroundMusicAudioSource.transform.position);

					_gameState = GameStates.GameOver;
					
				}// GameStates.Death
				break;
			
			case GameStates.BeatLevel:
				
				_backgroundMusicAudioSource.volume -= 0.01f;
				if (_backgroundMusicAudioSource.volume<=0.0f)
				{
					AudioSource.PlayClipAtPoint (_beatLevelSFX,_backgroundMusicAudioSource.transform.position, .08f);
					
					_gameState = GameStates.GameOver;

				}// GameStates.BeatLevel:
				break;
			
			case GameStates.GameOver:

				// nothing
				break;
			
			default:
				Debug.LogError($"Error, Exception: Entered a weird STATE in the STATE PATTERN in Class -> {this.name}");
				break;
		}//End switch

	}// End Update

	#endregion Unity Methods


	#region My Custom Methods

		
	#region Field's PROPERTIES (Get, Set functions)

	public static GameManager Gm1
	{
		get => Gm;
		set => Gm = value;
	}

	public GameObject PlayerToTheLeft
	{
		get => _playerToTheLeft;
		set => _playerToTheLeft = value;
	}

	public GameObject PlayerToTheRight
	{
		get => _playerToTheRight;
		set => _playerToTheRight = value;
	}

	public GameStates GameState
	{
		get => _gameState;
		set => _gameState = value;
	}

	public int Score
	{
		get => _score;
		set => _score = value;
	}

	public bool CanBeatLevel
	{
		get => _canBeatLevel;
		set => _canBeatLevel = value;
	}

	public int BeatLevelScore
	{
		get => _beatLevelScore;
		set => _beatLevelScore = value;
	}

	public GameObject MainCanvas
	{
		get => _mainCanvas;
		set => _mainCanvas = value;
	}

	public TextMeshProUGUI MainScoreDisplay
	{
		get => _mainScoreDisplay;
		set => _mainScoreDisplay = value;
	}

	public GameObject GameOverCanvas
	{
		get => _gameOverCanvas;
		set => _gameOverCanvas = value;
	}

	public TextMeshProUGUI GameOverScoreDisplay
	{
		get => _gameOverScoreDisplay;
		set => _gameOverScoreDisplay = value;
	}

	public GameObject BeatLevelCanvas
	{
		get => _beatLevelCanvas;
		set => _beatLevelCanvas = value;
	}

	public TextMeshProUGUI BeatLevelScoreDisplay
	{
		get => _beatLevelScoreDisplay;
		set => _beatLevelScoreDisplay = value;
	}

	public AudioSource BackgroundMusic
	{
		get => _backgroundMusicAudioSource;
		set => _backgroundMusicAudioSource = value;
	}

	public AudioClip GameOverSfx
	{
		get => _gameOverSFX1;
		set => _gameOverSFX1 = value;
	}

	public AudioClip BeatLevelSfx
	{
		get => _beatLevelSFX;
		set => _beatLevelSFX = value;
	}

	public Health PlayerHealth
	{
		get => _healthOfPlayerToTheLeft;
		set => _healthOfPlayerToTheLeft = value;
	}

	// public GameObject PlayAgainButtons
	// {
	// 	get => _playAgainButtons;
	// 	set => _playAgainButtons = value;
	// }
	//
	// public string PlayAgainLevelToLoad
	// {
	// 	get => _playAgainLevelToLoad;
	// 	set => _playAgainLevelToLoad = value;
	// }
	//
	// public GameObject RestartGameButtons
	// {
	// 	get => _restartGameButtons;
	// 	set => _restartGameButtons = value;
	// }
	//
	// public string RestartGameLevelToLoad
	// {
	// 	get => _restartGameLevelToLoad;
	// 	set => _restartGameLevelToLoad = value;
	// }
	//
	// public GameObject NextLevelButtons
	// {
	// 	get => _nextLevelButtons;
	// 	set => _nextLevelButtons = value;
	// }
	//
	// public string NextLevelToLoad
	// {
	// 	get => _nextLevelToLoad;
	// 	set => _nextLevelToLoad = value;
	// }


	#endregion Field's PROPERTIES (Get, Set functions)


	#region WIN vs. LOSE
	
	/// <summary>
	/// When the Player Loses the Game...
	/// </summary>
	public void EndGame()
	{
		// game is over
		//_gameIsOver = true;

		// repurpose the timer to display a message to the player
		//_mainTimerDisplay.text = "GAME OVER";

		// activate the gameOverScoreOutline gameObject, if it is set 
		// if (_gameOverScoreOutline)
		// 	_gameOverScoreOutline.SetActive (true);
	
		// // activate the playAgainButtons gameObject, if it is set 
		// if (_playAgainButtons)
		// 	_playAgainButtons.SetActive (true);
		//
		// // activate the restartGameButtons gameObject, if it is set
		// if (this._restartGameButtons)
		// 	this._restartGameButtons.SetActive (true);

		// reduce the pitch of the background music, if it is set 
		// if (_musicAudioSource)
		// 	_musicAudioSource.pitch = 0.5f; // slow down the music

		
		// You  LOSE  the Game!
					
		// update gameState
		_gameState = GameStates.Death;

		// set the end game score
		// Not necessary, the prefab already has a Good Text to show: _gameOverScoreDisplay.text = _mainScoreDisplay.text;

		// switch which GUI is showing		
		_mainCanvas.SetActive (false);
		_gameOverCanvas.SetActive (true);
		
		// // reduce the pitch of the background music, if it is set 
		// if (_backgroundMusicAudioSource != null)
		// 	_backgroundMusicAudioSource.pitch = 0.5f; // slow down the music
		
	}// End EndGame
	
	
	/// <summary>
	/// When the Player Wins the Game...
	/// </summary>
	void BeatLevel()
	{
		// // game is over
		// _gameIsOver = true;
		//
		// // repurpose the timer to display a message to the player
		// _mainTimerDisplay.text = "LEVEL COMPLETE";
		//
		// // activate the gameOverScoreOutline gameObject, if it is set 
		// if (_gameOverScoreOutline)
		// 	_gameOverScoreOutline.SetActive (true);

		// // activate the nextLevelButtons gameObject, if it is set 
		// if (_nextLevelButtons)
		// 	_nextLevelButtons.SetActive (true);
		//
		// // activate the restartGameButtons gameObject, if it is set
		// if (this._restartGameButtons)
		// 	this._restartGameButtons.SetActive (true);

		// // reduce the pitch of the background music, if it is set 
		// if (_musicAudioSource)
		// 	_musicAudioSource.pitch = 0.5f; // slow down the music
		
		// You  WIN  the Game!
					
		// update gameState
		_gameState = GameStates.BeatLevel;

		// hide the player so game doesn't continue playing
		_playerToTheLeft.SetActive(false);
		_playerToTheRight.SetActive(false);

		// set the end game score
		// Not necessary, the prefab already has a Good Text to show: _beatLevelScoreDisplay.text = _mainScoreDisplay.text;

		// switch which GUI is showing			
		_mainCanvas.SetActive (false);
		_beatLevelCanvas.SetActive (true);
		
	}// End BeatLevel

	#endregion WIN vs. LOSE

	
	#region Load Scenes Logic

	/// <summary>
	/// Public function that can be called to restart the level
	/// </summary>
	public void PlayAgainLevel ()
	{
		// we are just loading a scene (or reloading this scene)
		// which is an easy way to restart the level
		///// Original (2017/10):   Application.LoadLevel (playAgainLevelToLoad);
		//
		// SceneManager.LoadScene( this._playAgainLevelToLoad );
	}


	/// <summary>
	/// public function that can be called to restart the whole game
	/// </summary>
	public void RestartWholeGame ()
	{
		// we are just loading a scene (or reloading this scene)
		// which is an easy way to restart the level
		///// Original (2017/10):   Application.LoadLevel (restartGameLevelToLoad);
		//
		// SceneManager.LoadScene( this._restartGameLevelToLoad );
	}


	/// <summary>
	/// Public function that can be called to go to the next level of the game
	/// </summary>
	public void NextLevel ()
	{
		// we are just loading the specified next level (scene)
		///// Original (2017/10):  Application.LoadLevel (nextLevelToLoad);
		//
		// SceneManager.LoadScene( _nextLevelToLoad );
	}
	
	#endregion Load Scenes Logic
	
	#region Sound Utils
    
	private void PlayTwoSoundsInAChainForGameOver()
	{
		// Example:
		// audioSource.PlayOneShot(firstSFX);
		// Invoke(nameof(PlaySecondSound), firstSFX.length);
		
		AudioSource.PlayClipAtPoint (_gameOverSFX1, _backgroundMusicAudioSource.transform.position);
		//
		// Second Sound:
		//
		Invoke(nameof(PlaySecondGameOverSound), _gameOverSFX1.length);
	}

	private void PlaySecondGameOverSound()
	{
		AudioSource.PlayClipAtPoint(_gameOverSFX2, _backgroundMusicAudioSource.transform.position);
	}

	#endregion Sound Utils

	#region Collect Point to win the game

	public void Collect(int amount)
	{
		_score += amount;

		// Original version
		// if (_canBeatLevel)
		// {
		// 	_mainScoreDisplay.text = _score.ToString () + " of "+_beatLevelScore.ToString ();
		// }
		// else
		// {
		// 	_mainScoreDisplay.text = _score.ToString ();
		// }

		// Custom Code Here:
		//
		if (_canBeatLevel)
		{
			// _mainScoreDisplay.text = _score.ToString () + " of "+_beatLevelScore.ToString ();
		}
		else
		{
			// _mainScoreDisplay.text = _score.ToString ();
		}
	}// End Collect

	#endregion Collect Point to win the game
	
	#endregion My Custom Methods
}
