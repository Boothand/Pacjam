using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
	[SerializeField]
	CanvasGroup introScreen, runScreen;
	float fadeInn = 2f;
	float fadeOut = 4f;

	void Start ()
	{
		introScreen.alpha = 0;
		runScreen.alpha = 0;
	}
	
	void Update ()
	{
		switch (GameManager.instance.state)
		{
			case GameManager.States.SceneInfo:
				{
					introScreen.alpha = Mathf.Lerp(introScreen.alpha, 1, Time.deltaTime * fadeInn);
					runScreen.alpha = Mathf.Lerp(runScreen.alpha, 0, Time.deltaTime * fadeOut);
				}
				break;

			case GameManager.States.SceneLoad:
			case GameManager.States.SceneRun:
				{
					introScreen.alpha = Mathf.Lerp(introScreen.alpha, 0, Time.deltaTime * fadeOut);
					runScreen.alpha = Mathf.Lerp(runScreen.alpha, 1, Time.deltaTime * fadeInn);
				}
				break;

			case GameManager.States.SceneSuccess:
				{
					introScreen.alpha = Mathf.Lerp(introScreen.alpha, 0, Time.deltaTime * fadeOut);
					runScreen.alpha = Mathf.Lerp(runScreen.alpha, 0, Time.deltaTime * fadeOut);
				}
				break;
		}
	}
}
