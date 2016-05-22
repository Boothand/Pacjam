using UnityEngine;

public class PauseScreen : MonoBehaviour
{
	bool menuIsUp;
	CanvasGroup group;

	void Start()
	{
		group = GetComponent<CanvasGroup>();
		group.interactable = false;
		group.alpha = 0f;
	}

	public void Resume()
	{
		group.interactable = false;
		group.alpha = 0f;
		GameManager.gamePaused = false;
		Time.timeScale = 1f;
		menuIsUp = false;
	}

	public void Exit()
	{
		Application.Quit();
	}

	void DisplayPauseMenu()
	{
		menuIsUp = true;
		group.interactable = true;
		group.alpha = 1f;
	}

	void Update()
	{
		if (GameManager.gamePaused && !menuIsUp)
		{
			DisplayPauseMenu();
		}
		else if (menuIsUp && !GameManager.gamePaused)
		{
			Resume();
		}
	}
}