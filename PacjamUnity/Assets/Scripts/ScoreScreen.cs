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
	[HideInInspector] public int newTotalScore;

	float pauseTime = 0.25f;
	float CurrentPauseTime = 0.5f;

	float countSpeed = 50f;
	float countTotalSpeed = 80f;

	void Start()
	{
		auds = GetComponent<AudioSource>();
		oldTotalScore = GameManager.score;
		newTotalScore = oldTotalScore;
		
		totalDisplay = oldTotalScore;
		GameManager.instance.scoreScreen = this;

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

		if (Input.anyKey) // turn up the speed
		{
			countSpeed = 150f;
			countTotalSpeed = 300f;
			CurrentPauseTime = 0f;
		}
		else
		{
			countSpeed = 50f;
			countTotalSpeed = 80f;
		}


		if (show)
		{
			if (CurrentPauseTime > 0)
				CurrentPauseTime -= Time.deltaTime;
			else
			{
				switch (pos)
				{
					case 0:
						if ((int)candyDisplay < GameManager.instance.player.candy)
						{
							candyDisplay += Time.deltaTime * countSpeed;
							candyText.text = "x" + Mathf.RoundToInt(candyDisplay);
							auds.Play();
						}
						else
						{
							candyDisplay = GameManager.instance.player.candy;
							candyText.text = "x" + Mathf.RoundToInt(candyDisplay);
							pos++;
							CurrentPauseTime = pauseTime;
						}
						break;

					case 1:
						if ((int)timeDisplay < (int)Mathf.Clamp(100 - (int)GameManager.instance.player.timeUsed, 0, 100))
						{
							timeDisplay += Time.deltaTime * countSpeed;
							timeText.text = "x" + Mathf.RoundToInt(timeDisplay);
							auds.Play();
						}
						else
						{
							timeDisplay = (int)Mathf.Clamp(100 - (int)GameManager.instance.player.timeUsed, 0, 100);
							timeText.text = "x" + Mathf.RoundToInt(timeDisplay);
							pos++;
							CurrentPauseTime = pauseTime;
						}
						break;

					case 2:
						if ((int)killDisplay < GameManager.instance.player.kills * killPoints)
						{
							killDisplay += Time.deltaTime * countSpeed;
							killText.text = "x" + Mathf.RoundToInt(killDisplay);
							auds.Play();
						}
						else
						{
							killDisplay = GameManager.instance.player.kills * killPoints;
							killText.text = "x" + Mathf.RoundToInt(killDisplay);
							pos++;
							CurrentPauseTime = pauseTime;
						}
						break;

					case 3:
						newTotalScore = GameManager.instance.player.kills * killPoints +
							GameManager.instance.player.candy +
							(int)Mathf.Clamp(100 - (int)GameManager.instance.player.timeUsed, 0, 100) +
							oldTotalScore;
						if ((int)totalDisplay < newTotalScore)
						{
							totalDisplay += Time.deltaTime * countTotalSpeed;
							totalText.text = "x" + Mathf.RoundToInt(totalDisplay);
							auds.Play();
						}
						else
						{
							totalDisplay = newTotalScore;
							totalText.text = "x" + Mathf.RoundToInt(totalDisplay);
							pos++;
							CurrentPauseTime = pauseTime;
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
