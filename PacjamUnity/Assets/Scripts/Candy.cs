using UnityEngine;

public class Candy : MonoBehaviour
{
	public int score = 10;
	ScoreText scoreText;
	Renderer candymesh;
	
	void Start ()
	{
		scoreText = transform.GetComponentInChildren<ScoreText>();
		scoreText.gameObject.SetActive(false);
		candymesh = GetComponentInChildren<Renderer>();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.GetComponent<PlayerController>())
		{
			Stats player = col.GetComponent<Stats>();

			player.Score += score;

			scoreText.gameObject.SetActive(true);
			scoreText.transform.SetParent(null);
			scoreText.transform.rotation = Camera.main.transform.rotation;
			scoreText.transform.position = transform.position;

			Color textColor = candymesh.material.color;
			scoreText.SetColor(textColor);
			scoreText.SetText("+" + score.ToString());
			scoreText.Fade();

			transform.root.gameObject.SetActive(false);
		}
	}
	
	void Update ()
	{

	}
}