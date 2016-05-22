using UnityEngine;
using UnityEngine.UI;

public class ScoreIndicator : MonoBehaviour
{
	public Text textField;
	public Image image;
	Animator anim;
	
	void Start ()
	{
		anim = GetComponent<Animator>();
		GameManager.instance.scoreScript = this;
	}

	public void PlayWobbleAnimation()
	{
		anim.SetTrigger("Wobble");
	}
	
	void Update ()
	{
		textField.text = GameManager.instance.player.candy.ToString() + "x";
	}
}