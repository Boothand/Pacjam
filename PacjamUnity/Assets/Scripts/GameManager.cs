using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public Scene nextScene;
	public static int candyAmount;
	
	void Start ()
	{
		
	}
	
	void Update ()
	{
		if (candyAmount <= 0)
		{
			//Inititate happy animations, ballons, stuff...
			print("Took all candiez");
		}
	}
}