using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	//
	[HideInInspector] public ScoreIndicator scoreScript;
	[HideInInspector] public LivesIndicator livesScript;

	//public static
	public static GameManager instance;
	public static int score;
	public static int lives = 3;
	
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
		SceneDead
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
			if (Input.GetButtonDown("Submit"))
			{
				state = States.SceneLoad;
			}
		}

		if (candyAmount <= 0 && state == States.SceneRun)
		{
			state = States.SceneSuccess;
		}
	}
}