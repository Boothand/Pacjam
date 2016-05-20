using UnityEngine;

public class StarSlab : MonoBehaviour
{
	public bool started = false;
	public float rotationSpeed = 360;
	float rotation = 0;
	public float rotationEnd = 1080;

	public GameObject player;
	GameObject ppl;
	public bool fastSpawn;
	bool spawnEnd = false;
	public AnimationCurve startAnimation;
	public float animationSpeed = 0.2f;
	float animationStep = 0f;

	void Start ()
	{
		if (fastSpawn) //used while debugging
		{
			Instantiate(player, transform.position + transform.up * 0.5f, player.transform.rotation);
			started = true;
			rotation = rotationEnd;
			Destroy(this);
		}
		else
		{
			ppl = Instantiate(player, transform.position - transform.up * 10f, player.transform.rotation) as GameObject;
		}
	}
	
	void Update ()
	{
		if (!spawnEnd && ppl)
		{
			animationStep += Time.deltaTime;
			ppl.transform.position = new Vector3(transform.position.x, startAnimation.Evaluate(animationStep * animationSpeed), transform.position.z);
			if (animationStep * animationSpeed > 1)
				Destroy(this);
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
		}
	}
}
