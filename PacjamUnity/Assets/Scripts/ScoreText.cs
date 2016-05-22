using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreText : MonoBehaviour
{
	bool fading;
	CanvasGroup canvasGroup;
	Text txt;

	public float speed = 1f;
	public float fadeSpeed = 1f;
	
	void Awake ()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		txt = transform.GetComponentInChildren<Text>();
	}

	public void Fade()
	{
		if (!fading)
		{
			fading = true;
			StartCoroutine(FadeAndMove());
		}
	}

	public void SetText(string text)
	{
		txt.text = text;
	}

	public void SetColor(Color color)
	{
		txt.color = color;
	}

	IEnumerator FadeAndMove()
	{
		while (canvasGroup.alpha > 0.01f)
		{
			canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
			transform.position += Vector3.up * Time.deltaTime * speed;
			yield return new WaitForEndOfFrame();
		}

		gameObject.SetActive(false);
	}
	
}