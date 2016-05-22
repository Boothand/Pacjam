using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LivesIndicator : MonoBehaviour
{
	public Text textField;
	public Image image;
	Animator anim;

	void Start()
	{
		anim = GetComponent<Animator>();
		GameManager.instance.livesScript = this;
	}

	public void PlayWobbleAnimation()
	{
		anim.SetTrigger("Wobble");
	}

	void Update()
	{
		textField.text =  "x" + GameManager.lives.ToString();
	}
}
