using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	//
	[HideInInspector] public ScoreIndicator scoreScript;
	[HideInInspector] public LivesIndicator livesScript;
	[HideInInspector] public ScoreScreen scoreScreen;

	//public static
	public static GameManager instance;
	public static int score;
	public static int lives = 3;

	public static bool gamePaused;
	
	public int candyAmount;
	public PlayerController player;

	public States state = States.MainMenu;
	public enum States
	{
		MainMenu,
		SceneInfo,
		SceneLoad,
		SceneRun,
		SceneSuccess,
		SceneScore,
		SceneDead
	}

	void Reset()
	{
		score = 0;
		lives = 3;
		state = GameManager.States.SceneInfo;
		candyAmount = 0;
	}

	public void NewGame()
	{
		Reset();
		SceneGoToNext();
	}

	void Awake()
	{
		if (!instance)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void SceneStart()
	{
		state = States.SceneRun;
	}

	public void SceneGoToNext()
	{
		GameManager.instance.candyAmount = 0;
		state = States.SceneInfo;
		SceneManager.LoadScene(GameObject.FindObjectOfType<NextLevel>().sceneName);
	}

	public void RestartLevel()
	{
		GameManager.instance.candyAmount = 0;
		if (GameManager.lives > 0)
		{
			state = States.SceneInfo;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		else
		{
			state = States.MainMenu;
			SceneManager.LoadScene(0);
		}
	}
	
	
	void Update ()
	{
		if (state == States.SceneInfo)
		{
			if (Input.anyKeyDown)
			{
				state = States.SceneLoad;
			}
		}

		if (state == States.SceneScore)
		{
			if (scoreScreen.finished)
			{
				if(Input.anyKeyDown)
				{
					GameManager.score = scoreScreen.newTotalScore;
					state = States.SceneInfo;
					SceneGoToNext();
				}
			}
		}

		if (candyAmount <= 0 && state == States.SceneRun)
		{
			state = States.SceneSuccess;
		}

		if (state == States.SceneRun)
		{
			if (Input.GetButtonDown("Pause"))
			{
				if (!gamePaused)
				{
					gamePaused = true;
					Time.timeScale = 0f;
				}
				else
				{
					gamePaused = false;
					Time.timeScale = 1f;
				}
			}
		}
	}
}