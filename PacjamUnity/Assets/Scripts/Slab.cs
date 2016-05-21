using UnityEngine;

public class Slab : MonoBehaviour
{
	public GameObject candy;
	public float groundOffset = 0.5f;
	
	void Start ()
	{
		if (candy)
		{
			Instantiate(candy, transform.position + Vector3.up * groundOffset, candy.transform.rotation);
			GameManager.instance.candyAmount++;
		}
	}
}