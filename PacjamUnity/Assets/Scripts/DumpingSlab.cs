using UnityEngine;

public class DumpingSlab : MonoBehaviour
{
	AudioSource audioSrc;
	ScoreText scoreText;

	void Start()
	{
		audioSrc = GetComponentInChildren<AudioSource>();
		scoreText = GetComponentInChildren<ScoreText>();
	}

	public void PlaySound()
	{
		audioSrc.Play();
	}

	public void PlayScoreText()
	{
		scoreText.Reset();

		scoreText.transform.rotation = Camera.main.transform.rotation;
		scoreText.transform.position = transform.position + Vector3.up;

		//Color textColor = Color.yellow;
		//scoreText.SetColor(textColor);
		scoreText.SetText("+10");
		scoreText.Fade();
	}

}
