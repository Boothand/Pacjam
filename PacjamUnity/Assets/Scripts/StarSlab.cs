using UnityEngine;

public class StarSlab : MonoBehaviour
{
	bool started = false;
	public float rotationSpeed = 360;
	float rotation = 0;
	public float rotationEnd = 1080;

	public GameObject player;
	public bool fastSpawn;
	bool spawnEnd = false;
	public AnimationCurve startAnimation;
	public float animationSpeed = 0.3f;
	float animationStep = 0f;

	void Start ()
	{
		if (fastSpawn) //used while debugging
		{
			player.transform.position = transform.position + Vector3.up * 0.5f;
			started = true;
			rotation = rotationEnd;
			Destroy(this);
		}
	}
	
	void Update ()
	{
		if (!spawnEnd && player && !fastSpawn)
		{
			animationStep += Time.deltaTime;
			player.transform.position = new Vector3(transform.position.x, startAnimation.Evaluate(animationStep * animationSpeed), transform.position.z);
			if (animationStep * animationSpeed > 1)
			{
				GameManager.instance.SceneStart();
				Destroy(this);
			}
		}

		if (started)
		{
			if (rotation <= rotationEnd)
			{
				rotation += Time.deltaTime * rotationSpeed;
				rotationSpeed -= rotationSpeed * 0.5f * Time.deltaTime;
				transform.rotation = Quaternion.AngleAxis(rotation, Vector3.right);
			}
			else
			{
				transform.rotation = Quaternion.identity;
			}

			
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (!started)
		{
			started = true;
			GetComponentInChildren<ParticleSystem>().Emit(75);
		}
	}
}
