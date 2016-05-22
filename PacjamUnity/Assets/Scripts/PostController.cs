using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PostController : MonoBehaviour
{
	PHPWrite php;

	[SerializeField]
	InputField inputField;

	[SerializeField]
	Text errorMessage;

	[SerializeField]
	Text score;


	void Start ()
	{
		php = GetComponent<PHPWrite>();
		score.text = "Score: " + GameManager.score.ToString();
	}

	public void Post()
	{
		int score = GameManager.score;
		string name = inputField.text;
		if (score > 0)
		{
			if (php.sendState == "" || php.sendState == "You need to enter a name" || php.sendState == "Could not Complete!Try Again.")
			{
				php.Submit(name, score);
			}
		}
		else
			GoToMainMenu();
	}

	public void GoToMainMenu()
	{
		SceneManager.LoadScene(0);
	}

	void Update()
	{
		errorMessage.text = php.sendState;

		if (php.sendState == "Success!")
		{
			GoToMainMenu();
		}
	}
}
