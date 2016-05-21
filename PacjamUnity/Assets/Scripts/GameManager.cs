using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	//public static
	public static GameManager instance;
	public static int score;
	
	public int candyAmount;

	public States state = States.MainMenu;
	public enum States
	{
		MainMenu,
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
		if (candyAmount <= 0 && state == States.SceneRun)
		{
			state = States.SceneSuccess;
		}
	}
}