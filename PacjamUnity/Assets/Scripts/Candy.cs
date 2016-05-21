﻿using UnityEngine;

public class Candy : MonoBehaviour
{
	public int score = 10;
	ScoreText scoreText;
	Renderer candymesh;
	ParticleSystem particles;
	
	void Start ()
	{
		scoreText = transform.GetComponentInChildren<ScoreText>();
		scoreText.gameObject.SetActive(false);
		candymesh = GetComponentInChildren<Renderer>();
		particles = GetComponentInChildren<ParticleSystem>();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.GetComponent<PlayerController>())
		{
			Stats player = col.GetComponent<Stats>();

			player.Score += score;
			GameManager.score += score;

			scoreText.gameObject.SetActive(true);
			scoreText.transform.SetParent(null);
			scoreText.transform.rotation = Camera.main.transform.rotation;
			scoreText.transform.position = transform.position;

			Color textColor = candymesh.material.color;
			scoreText.SetColor(textColor);
			scoreText.SetText("+" + score.ToString());
			scoreText.Fade();

			particles.transform.SetParent(null);
			particles.Emit(10);

			GameManager.instance.candyAmount--;

			transform.root.gameObject.SetActive(false);
		}
	}
	
	void Update ()
	{

	}
}