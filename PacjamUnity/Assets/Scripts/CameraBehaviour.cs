using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
	[SerializeField] Transform loadPositionGoal;
	[SerializeField] Transform gamePositionGoal;
	//[SerializeField] Transform victoryPosition;
	[SerializeField] float speed = 1f;
	[SerializeField] float playerViewOffset = 0.75f;

	Vector3 targetPosition;
	Quaternion targetRotation;


	void Start ()
	{
		if (!loadPositionGoal || !gamePositionGoal)
		{
			loadPositionGoal = GameManager.instance.transform.FindChild("LoadPositionGoal");
			gamePositionGoal = GameManager.instance.transform.FindChild("GamePositionGoal");
		}

		switch (GameManager.instance.state)
		{
			case GameManager.States.SceneLoad:
				transform.position = loadPositionGoal.position;
				transform.rotation = loadPositionGoal.rotation;
				break;
			case GameManager.States.SceneRun:
				transform.position = gamePositionGoal.position;
				transform.rotation = gamePositionGoal.rotation;
				break;
			case GameManager.States.SceneSuccess:
			case GameManager.States.SceneDead:
				Vector3 playerPos = GameManager.instance.player.transform.position;
				transform.position = Vector3.Lerp(transform.position, playerPos, playerViewOffset);
				transform.rotation = Quaternion.LookRotation(playerPos - transform.position, Vector3.up);
				break;
		}
	}
	
	void Update ()
	{
		switch (GameManager.instance.state)
		{
			case GameManager.States.SceneLoad:
			case GameManager.States.SceneRun:
				targetPosition = gamePositionGoal.position;
				targetRotation = gamePositionGoal.rotation;
				break;
			case GameManager.States.SceneSuccess:
			case GameManager.States.SceneDead:
				Vector3 playerPos = GameManager.instance.player.transform.position;
				targetPosition = Vector3.Lerp(gamePositionGoal.position, playerPos, playerViewOffset);
				targetRotation = Quaternion.LookRotation(playerPos - transform.position, Vector3.up);
				break;
		}

		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speed);
	}
}