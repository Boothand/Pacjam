﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
	[SerializeField]
	CanvasGroup introScreen, runScreen, blackScreen, scoreScreen;
	float fadeInn = 1f;
	float fadeOut = 20f;

	float blackFadeSpeed = 3f;

	void Start()
	{
		introScreen.alpha = 0;
		runScreen.alpha = 0;
		blackScreen.alpha = 1;
	}

	void Update()
	{
		switch (GameManager.instance.state)
		{
			case GameManager.States.SceneInfo:
				{
					introScreen.alpha = Mathf.Lerp(introScreen.alpha, 1, Time.deltaTime * fadeInn);
					runScreen.alpha = Mathf.Lerp(runScreen.alpha, 0, Time.deltaTime * fadeOut);
					blackScreen.alpha = Mathf.Lerp(blackScreen.alpha, 0, Time.deltaTime * 5);
					scoreScreen.alpha = Mathf.Lerp(scoreScreen.alpha, 0, Time.deltaTime * 5);
				}
				break;

			case GameManager.States.SceneLoad:
			case GameManager.States.SceneRun:
				{
					introScreen.alpha = Mathf.Lerp(introScreen.alpha, 0, Time.deltaTime * fadeOut);
					runScreen.alpha = Mathf.Lerp(runScreen.alpha, 1, Time.deltaTime * fadeInn);
					blackScreen.alpha = Mathf.Lerp(blackScreen.alpha, 0, Time.deltaTime * 5);
					scoreScreen.alpha = Mathf.Lerp(scoreScreen.alpha, 0, Time.deltaTime * 5);
				}
				break;

			case GameManager.States.SceneSuccess:
				{
					introScreen.alpha = Mathf.Lerp(introScreen.alpha, 0, Time.deltaTime * fadeOut);
					runScreen.alpha = Mathf.Lerp(runScreen.alpha, 0, Time.deltaTime * fadeOut);


					if (GameManager.instance.player.timeSpentVictory >= 3f)
					{
						//blackScreenTimer += Time.deltaTime;
						//blackScreen.alpha = Mathf.Lerp(0, 1, blackScreenTimer);
						blackScreen.alpha = Mathf.Lerp(blackScreen.alpha, 1, Time.deltaTime * blackFadeSpeed);
						scoreScreen.alpha = Mathf.Lerp(scoreScreen.alpha, 1, Time.deltaTime * 0.33f);
					}
				}
				break;
			case GameManager.States.SceneScore:
				{
					blackScreen.alpha = Mathf.Lerp(blackScreen.alpha, 1, Time.deltaTime * blackFadeSpeed);
					scoreScreen.alpha = Mathf.Lerp(scoreScreen.alpha, 1, Time.deltaTime * 0.33f);
				}
				break;

			case GameManager.States.SceneDead:
				{
					if (GameManager.instance.player.timeSpentDead >= 2)
					{
						introScreen.alpha = Mathf.Lerp(introScreen.alpha, 0, Time.deltaTime * fadeOut);
						runScreen.alpha = Mathf.Lerp(runScreen.alpha, 0, Time.deltaTime * fadeOut);

						blackScreen.alpha = Mathf.Lerp(blackScreen.alpha, 1, Time.deltaTime * blackFadeSpeed);
					}
				}
				break;
		}
	}
}
