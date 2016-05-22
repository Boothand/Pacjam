using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour
{
	public Text candyText, timeText, killText, sumText, totalText;
	public bool show;
	public bool finished = false;

	int pos = 0;

	float candyDisplay, timeDisplay, killDisplay, sumDisplay, totalDisplay;
	int candyTemp = 100, timeTemp = 45, killTemp = 25, sumTemp = 170, totalTemp = 2500 + 170;

	void Start()
	{

		totalDisplay = GameManager.score;

		candyText.text = "";
		timeText.text = "";
		killText.text = "";
		sumText.text = "";
		totalText.text = "x" + Mathf.RoundToInt(totalDisplay);

		int totalScore;
	}

	void Update()
	{
		if (GameManager.instance.state != GameManager.States.SceneScore)
			return;

		if (show)
		{
			switch (pos)
			{
				case 0:
					if ((int)candyDisplay < GameManager.instance.player.candy)
					{
						candyDisplay += Time.deltaTime * 25;
						candyText.text = "x" + Mathf.RoundToInt(candyDisplay);
					}
					else
					{
						candyDisplay = GameManager.instance.player.candy;
						candyText.text = "x" + Mathf.RoundToInt(candyDisplay);
						pos++;
					}
					break;

				case 1:
					if ((int)timeDisplay < GameManager.instance.player.timeUsed)
					{
						timeDisplay += Time.deltaTime * 25;
						timeText.text = "x" + Mathf.RoundToInt(timeDisplay);
					}
					else
					{
						timeDisplay = GameManager.instance.player.timeUsed;
						timeText.text = "x" + Mathf.RoundToInt(timeDisplay);
						pos++;
					}
					break;

				case 2:
					if ((int)killDisplay < GameManager.instance.player.kills)
					{
						killDisplay += Time.deltaTime * 25;
						killText.text = "x" + Mathf.RoundToInt(killDisplay);
					}
					else
					{
						killDisplay = GameManager.instance.player.kills;
						killText.text = "x" + Mathf.RoundToInt(killDisplay);
						pos++;
					}
					break;

				case 3:
					if ((int)totalDisplay < totalTemp)
					{
						totalDisplay += Time.deltaTime * 50;
						totalText.text = "x" + Mathf.RoundToInt(totalDisplay);
					}
					else
					{
						totalDisplay = totalTemp;
						totalText.text = "x" + Mathf.RoundToInt(totalDisplay);
						pos++;
					}
					break;
				case 4:
					finished = true;
					break;
			}
			Sum();
		}
	}

	void Sum()
	{
		sumDisplay = candyDisplay + timeDisplay + killDisplay;
		sumText.text = "x" + Mathf.RoundToInt(sumDisplay).ToString();
	}

}
