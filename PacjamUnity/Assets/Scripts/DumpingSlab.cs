using UnityEngine;

public class DumpingSlab : MonoBehaviour
{
	AudioSource audioSrc;

	void Start()
	{
		audioSrc = GetComponentInChildren<AudioSource>();
	}

	public void PlaySound()
	{
		audioSrc.Play();
	}

}
