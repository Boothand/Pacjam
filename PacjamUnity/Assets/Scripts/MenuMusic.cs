using UnityEngine;

public class MenuMusic : MonoBehaviour
{
	float timer;
	float musicTimer;

	public AudioSource drumsOnly;
	public AudioSource music;

	public float speed = 0.1f;
	public float secondsUntilFade = 5f;
	
	void Start ()
	{
		music.volume = 0;
	}
	
	void Update ()
	{

		if (timer > secondsUntilFade)
		{
			musicTimer += Time.deltaTime * speed;

			drumsOnly.volume -= musicTimer;
			music.volume += musicTimer;
		}
		else
		{
			timer += Time.deltaTime;
		}

		if (music.volume > 1)
		{
			music.volume = 1;
			drumsOnly.volume = 0;
		}

	}
}