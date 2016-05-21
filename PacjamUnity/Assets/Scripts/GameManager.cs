using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	//
	[HideInInspector] public ScoreIndicator scoreScript;

	//public static
	public static GameManager instance;
	public static int score;
	
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
		state = States.SceneLoad;
		SceneManager.LoadScene(GameObject.FindObjectOfType<NextLevel>().sceneName);
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