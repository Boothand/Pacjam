using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour
{

	public void Play()
	{
		GameManager.lives = 3;
		GameManager.instance.SceneGoToNext();
	}
}
