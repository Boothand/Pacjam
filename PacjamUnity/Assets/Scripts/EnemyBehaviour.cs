using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
	//Private
	Animator anim;
	AudioSource audioSrc;
	float lerpValue;
	Vector3 targetPosition;
	Vector3 direction;
	Vector3 fromPosition;
	bool isMoving;
	bool dropped;
	float dropStep = 0;
	Vector3 dropPoint;
	[SerializeField] bool celebrating;

    //Public
    public PlayerController target;
	public float moveDuration = 0.5f;
    public float detectRange = 8;
	public bool trapped;
	public AnimationCurve moveCurve;
	public AnimationCurve dropCurve;
	public LayerMask groundLayer;
	public LayerMask collisionLayer;
	public LayerMask friendLayer;
	public float dropSpeed = 1f;

	bool failedToMove = false;

	void Start ()
	{
		if (!target)
		{
			target = GameObject.Find("Wobbly").GetComponent<PlayerController>();
		}

		anim = GetComponent<Animator>();
		audioSrc = GetComponentInChildren<AudioSource>();
	}

	void MoveTo(Vector3 from, Vector3 pos)
	{
		pos.x = Mathf.RoundToInt(pos.x);
		pos.z = Mathf.RoundToInt(pos.z);

		lerpValue += Time.deltaTime;


		if (lerpValue > moveDuration)
		{
			lerpValue = moveDuration;
		}

		float moveJourney = lerpValue / moveDuration;
		
		transform.position = Vector3.MoveTowards(from, targetPosition, moveCurve.Evaluate(moveJourney));
	}

	bool CanMoveTo(Vector3 pos)
	{
		Vector3 halfScale = transform.localScale / 2;
		float offset = 0.98f;
		bool canGo = true;

		Vector3 top = Vector3.forward * halfScale.z * offset;
		Vector3 bottom = Vector3.back * halfScale.z * offset;
		Vector3 right = Vector3.right * halfScale.x * offset;
		Vector3 left = Vector3.left * halfScale.x * offset;

		Vector3 verticalRight = transform.position + (top * direction.z) + (right * direction.x);
		Vector3 verticalLeft = transform.position + (top * direction.z) + (left * direction.x);

		Vector3 horizontalTop = transform.position + (right * direction.x) + (top * direction.x);
		Vector3 horizontalBottom = transform.position + (right * direction.x) + (bottom * direction.x);


		Vector3 rayEndPos = transform.position + direction + (Vector3.up * halfScale.y);

		//Debug.DrawRay(rayEndPos, Vector3.down, Color.red);

		float rayLength = 0.8f;

		
		bool hit = Physics.Raycast(transform.position, direction, rayLength, collisionLayer);

		if (hit)
		{
			print("leftHit || rightHit");
			canGo = false;
		}

		if (Mathf.Abs(direction.magnitude) > 0)
		{
			bool friendHit = Physics.Raycast(transform.position, direction, 1.5f, friendLayer);

			if (friendHit)
			{
				print("Hit Friend");
				return false;
			}
		}

		if (!canGo)
		{
			return false;
		}
		else
		{
			RaycastHit hitInfo;
			Ray ray = new Ray(rayEndPos, Vector3.down);
			Physics.Raycast(ray, out hitInfo, 1.5f, groundLayer);

			float hitLength = 0;

			if (hitInfo.transform)
			{
				hitLength = Vector2.Distance(hitInfo.point, rayEndPos);
			}
			else
			{
				print("Too Deep");
				return false;
			}



			float remainder = 1 - hitLength;
			if (remainder > 0.35 || remainder < -0.35)
			{
				print("Not withing 0.35 || -0.35 margin");
				return false;
			}


			targetPosition.y += remainder;

			targetPosition = new Vector3(Mathf.RoundToInt(targetPosition.x), targetPosition.y, Mathf.RoundToInt(targetPosition.z));
		}

		return true;
	}

	bool HasReachedTargetPos()
	{
		float distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetPosition.x, 0, targetPosition.z));

		if (distance < 0.01f)
		{
			transform.position = targetPosition;
			lerpValue = 0;
			direction = Vector3.zero;
			audioSrc.Play();

			return true;
		}

		return false;
	}

	public void GetCaught()
	{
		trapped = true;
	}

	Vector3 DirectionTowardsTarget()
	{
		Vector3 distanceToTarget = target.transform.position - transform.position;
		distanceToTarget.x = Mathf.Round(distanceToTarget.x);
		distanceToTarget.z = Mathf.Round(distanceToTarget.z);

		//print("I must go " + distanceToTarget.x + " units in X, and " + distanceToTarget.z + " units in Z.");

		//float dirToGo = (distanceToTarget.x > distanceToTarget.z) ? distanceToTarget.x : distanceToTarget.z;
		float xDir, zDir = 0;

		if (Mathf.Abs(distanceToTarget.x) >= Mathf.Abs(distanceToTarget.z))
		{
			xDir = (distanceToTarget.x > 0) ? 1 : -1;
			zDir = 0;
		}
		else
		{
			zDir = (distanceToTarget.z > 0) ? 1 : -1;
			xDir = 0;
		}

		Vector3 dir = new Vector3(xDir, 0, zDir);
		
		return dir;
	}

	public void GetDropped()
	{
		Rigidbody rb = GetComponent<Rigidbody>();

		rb.isKinematic = false;
		rb.constraints = RigidbodyConstraints.None;
		transform.FindChild("Skeleton").gameObject.SetActive(false);
		dropped = true;
		dropPoint = transform.position;
	}



	void Update ()
	{
		if (GameManager.instance.state == GameManager.States.SceneDead)
		{
			celebrating = true;
		}

		if (celebrating)
		{
			anim.SetTrigger("Celebrate");
		}

		if (dropped)
		{
			dropStep += Time.deltaTime;
			transform.position = dropPoint + Vector3.up * dropCurve.Evaluate(dropStep);

			if (dropStep >= 0.3f)
			{
				GetComponent<Rigidbody>().velocity = new Vector3(0, -15f, 0);
				this.enabled = false;
			}
		}

		if (GameManager.instance.state != GameManager.States.SceneRun)
			return;


		if (target && !isMoving && !trapped)
		{
			if (!failedToMove)
			direction = DirectionTowardsTarget();

			targetPosition = transform.position + direction;
			fromPosition = transform.position;
			
			if (Mathf.Abs(direction.magnitude) > 0)
			{
				if (CanMoveTo(targetPosition))
				{
					isMoving = true;
					anim.SetTrigger("Jump");
					failedToMove = false;
				}
				else
				{
					int random = Random.Range(0, 2);
					if (random == 0)
						random = -1;
					direction = Quaternion.Euler(0, 90 * random, 0) * direction;
					failedToMove = true;
					print("Tried new Direction " + direction);
				}
			}
		}

		if (isMoving && !trapped)
		{
			MoveTo(fromPosition, targetPosition);
			//Debug.DrawLine(fromPosition, targetPosition, Color.blue);
			transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * 3);

			if (HasReachedTargetPos())
			{
				isMoving = false;
			}
		}
	}
}
