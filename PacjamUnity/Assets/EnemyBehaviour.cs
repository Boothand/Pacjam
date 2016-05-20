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
}
