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
	NavMeshAgent nma;

	void Start ()
	{
        rigidbody = GetComponent<Rigidbody>();
		nma = GetComponent<NavMeshAgent>();
	}
	
	void Update ()
	{
        if (target)
        {
			nma.SetDestination(target.transform.position);
        }
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
			PlayerController pc = col.gameObject.GetComponent<PlayerController>();
			print(pc.MoveToAngle.eulerAngles);
			if (pc.IsMoving)
			{
				if (pc.MoveToAngle.x == 0 || pc.MoveToAngle.x == 180)
					if (pc.MoveToAngle.z == 0 || pc.MoveToAngle.z == 180)
					{
						print("Correct");
					}
				
			}
        }
    }
}
