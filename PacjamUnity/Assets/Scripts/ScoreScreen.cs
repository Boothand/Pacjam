using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour
{
	public bool show;
	public bool finished = false;

	[Header("Score Settings")]
	[SerializeField]
	int killPoints;
	[SerializeField]
	int timePoints;

	[Header("Text Components")]
	public Text candyText;
	public Text timeText;
	public Text killText;
	public Text sumText;
	public Text totalText;

	[Header("Sounds")]
	AudioSource auds;

	CanvasGroup scoreGroup;
	int pos = 0;

	float candyDisplay, timeDisplay, killDisplay, sumDisplay, totalDisplay;

	int oldTotalScore;
	int newTotalScore;

	float pauseTime = 0.5f;

	void Start()
	{
		auds = GetComponent<AudioSource>();
		oldTotalScore = GameManager.score;
		newTotalScore = oldTotalScore;
		
		totalDisplay = oldTotalScore;
		

		candyText.text = "";
		timeText.text = "";
		killText.text = "";
		sumText.text = "";
		totalText.text = "x" + Mathf.RoundToInt(totalDisplay);
		
	}

	void Update()
	{
		if (GameManager.instance.state != GameManager.States.SceneScore)
		{
			return;
		}
		else


		if (show)
		{
			if (pauseTime > 0)
				pauseTime -= Time.deltaTime;
			else
			{
				switch (pos)
				{
					case 0:
						if ((int)candyDisplay < GameManager.instance.player.candy)
						{
							candyDisplay += Time.deltaTime * 25;
							candyText.text = "x" + Mathf.RoundToInt(candyDisplay);
							auds.Play();
						}
						else
						{
							candyDisplay = GameManager.instance.player.candy;
							candyText.text = "x" + Mathf.RoundToInt(candyDisplay);
							pos++;
							pauseTime = 0.5f;
						}
						break;

					case 1:
						if ((int)timeDisplay < (int)Mathf.Clamp(100 - (int)GameManager.instance.player.timeUsed, 0, 100))
						{
							timeDisplay += Time.deltaTime * 25;
							timeText.text = "x" + Mathf.RoundToInt(timeDisplay);
							auds.Play();
						}
						else
						{
							timeDisplay = (int)Mathf.Clamp(100 - (int)GameManager.instance.player.timeUsed, 0, 100);
							timeText.text = "x" + Mathf.RoundToInt(timeDisplay);
							pos++;
							pauseTime = 0.5f;
						}
						break;

					case 2:
						if ((int)killDisplay < GameManager.instance.player.kills * killPoints)
						{
							killDisplay += Time.deltaTime * 25;
							killText.text = "x" + Mathf.RoundToInt(killDisplay);
							auds.Play();
						}
						else
						{
							killDisplay = GameManager.instance.player.kills * killPoints;
							killText.text = "x" + Mathf.RoundToInt(killDisplay);
							pos++;
							pauseTime = 0.5f;
						}
						break;

					case 3:
						newTotalScore = GameManager.instance.player.kills * killPoints +
							GameManager.instance.player.candy +
							(int)Mathf.Clamp(100 - (int)GameManager.instance.player.timeUsed, 0, 100) +
							oldTotalScore;
						if ((int)totalDisplay < newTotalScore)
						{
							totalDisplay += Time.deltaTime * 50;
							totalText.text = "x" + Mathf.RoundToInt(totalDisplay);
							auds.Play();
						}
						else
						{
							totalDisplay = newTotalScore;
							totalText.text = "x" + Mathf.RoundToInt(totalDisplay);
							pos++;
							pauseTime = 0.5f;
						}
						break;
					case 4:
						finished = true;
						break;
				}
				Sum();
			}
		}
	}

	void Sum()
	{
		sumDisplay = candyDisplay + timeDisplay + killDisplay;
		sumText.text = "x" + Mathf.RoundToInt(sumDisplay).ToString();
	}

}
