using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    //Public
    public GameObject target;
    public float speed;
    public float detectRange = 8;
    public bool trapped = false;

    //Private
    Rigidbody rigidbody;

	void Start ()
	{
        rigidbody = GetComponent<Rigidbody>();
	}
	
	void Update ()
	{
        if (target)
            if (Vector3.Distance(transform.position, target.transform.position) < detectRange)
        {
            transform.LookAt(target.transform.position);
            rigidbody.velocity = transform.forward * speed;
        }
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
			if (col.gameObject.GetComponent<PlayerController>().IsMoving)
			{
				Vector3 posUp = (col.transform.position + col.transform.up);
				Vector3 posDown = (col.transform.position + col.transform.up);

				if (posUp.y - 0.5f > col.transform.position.y)
				{
					print("Correct!");
				}
				else
				{
					print("Not Correct rotation");
				}
			}
			else
			{
				print("Player not moving");
			}
        }
    }
}
