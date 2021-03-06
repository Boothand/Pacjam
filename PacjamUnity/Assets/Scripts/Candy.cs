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
			//GameManager.score += score;
			col.GetComponent<PlayerController>().candy++;
			AudioSource auds = GetComponentInChildren<AudioSource>();
			auds.Play();
			auds.transform.SetParent(null);

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
			GameManager.instance.scoreScript.PlayWobbleAnimation();

			transform.root.gameObject.SetActive(false);
		}

		if (col.GetComponent<EnemyBehaviour>())
		{
			GameManager.instance.candyAmount--;
			Destroy(gameObject);
		}
	}
}