using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
	public GameObject collisionObject;

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Enemy")
		{
			collisionObject = col.gameObject;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Enemy")
		{
			collisionObject = null;
		}
	}

}
