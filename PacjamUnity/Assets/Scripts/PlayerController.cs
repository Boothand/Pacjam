using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Private
    bool isMoving;
    Vector3 direction;
    Vector3 targetPosition;
    Vector3 fromPosition;
    Quaternion fromRotation;
	Quaternion moveToAngle;
	AudioSource audioSrc;
	bool isDead;
	float lerpValue;

	Transform trappedTarget;

    //Public
	public HoleTrigger[] holeTriggers;

    public LayerMask collisionLayer;
    public LayerMask groundLayer;
	public LayerMask enemyLayer;
    public AnimationCurve moveCurve;
    public float moveDuration = 1f;
    [Range(0,1)]
    public float slopeHeight = 0.35f;

    public bool IsMoving
    {
        get { return isMoving; }
    }

	void Start()
	{
		audioSrc = GetComponentInChildren<AudioSource>();
		GameManager.instance.player = this;
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.transform.GetComponent<EnemyBehaviour>())
		{
			if (!isDead)
			{
				Die();
			}
		}
	}

	void Die()
	{
		isDead = true;
		GameManager.instance.state = GameManager.States.SceneDead;

		//Edit the wobbly joints.
		foreach (Transform child in transform.FindChild("Skeleton"))
		{
			HingeJoint joint = child.GetComponent<HingeJoint>();
			JointSpring spring = joint.spring;
			//spring.damper = 2f;
			spring.spring = 0f;

			joint.spring = spring;
		}
	
		//Animation.
		//Timers, states.
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

		if (Mathf.Abs(direction.x) > 0)
		{
			bool topHit = Physics.Raycast(horizontalTop, direction, rayLength, collisionLayer);
			bool bottomHit = Physics.Raycast(horizontalBottom, direction, rayLength, collisionLayer);
			
			if (topHit || bottomHit)
			{
				canGo = false;
			}
		}

		if (Mathf.Abs(direction.z) > 0)
		{
			bool leftHit = Physics.Raycast(verticalLeft, direction, rayLength, collisionLayer);
			bool rightHit = Physics.Raycast(verticalRight, direction, rayLength, collisionLayer);

			if (leftHit || rightHit)
			{
				canGo = false;
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
                //print("Hit something down");
                hitLength = Vector2.Distance(hitInfo.point, rayEndPos);
            }
            else
            {
                return false;
            }

            

			float remainder = 1 - hitLength;
            if (remainder > 0.35 || remainder < -0.35)
                return false;


            targetPosition.y += remainder;

            targetPosition = new Vector3(Mathf.RoundToInt(targetPosition.x), targetPosition.y, Mathf.RoundToInt(targetPosition.z));
		}
		
		return true;
	}

	void MoveTo(Vector3 from, Vector3 pos, Quaternion fromRot)
	{
		pos.x = Mathf.RoundToInt(pos.x);
		pos.z = Mathf.RoundToInt(pos.z);
		//pos.y = transform.position.y;

		lerpValue += Time.deltaTime;
        

		if (lerpValue > moveDuration)
		{
			lerpValue = moveDuration;
		}

		float moveJourney = lerpValue / moveDuration;

        //print(moveCurve.Evaluate(moveJourney));

		moveToAngle = Quaternion.Euler(90 * direction.z, 0, 90 * -direction.x) * fromRotation;
		transform.position = Vector3.MoveTowards(from, targetPosition, moveCurve.Evaluate(moveJourney));
		transform.rotation = Quaternion.Lerp(fromRotation, moveToAngle, moveCurve.Evaluate(moveJourney));
	}

	bool HasReachedTargetPos()
	{
		//print(transform.position.x + " " + targetPosition.x);
		//print(transform.position.z + " " + targetPosition.z);
		if ( Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetPosition.x, 0, targetPosition.z)) < 0.01f)
		{
			transform.position = targetPosition;
			lerpValue = 0;
			direction = Vector3.zero;
			audioSrc.Play();
			return true;
		}

		return false;
	}

	Vector3 GetMovedir()
	{
		float dirX = Input.GetAxisRaw("Horizontal");
		float dirZ = Input.GetAxisRaw("Vertical");

		float threshold = 0.25f;

		if (dirX > threshold)
		{
			return Vector3.right;
		}
		else if (dirX < -threshold)
		{
			return Vector3.left;
		}

		if (dirZ > threshold)
		{
			return Vector3.forward;
		}
		else if (dirZ < -threshold)
		{
			return Vector3.back;
		}

		return Vector3.zero;
	}

	void EnemyCollision()
	{
		if (!trappedTarget)
		{
			RaycastHit hit;
			Physics.Raycast(transform.position, direction, out hit, 1, enemyLayer);
			if (hit.transform)
			{
				foreach (HoleTrigger hole in holeTriggers)
				{
					if (hole.collisionObject == hit.transform.gameObject)
					{
						hit.transform.GetComponent<Collider>().enabled = false;
						trappedTarget = hole.collisionObject.transform;
						trappedTarget.SetParent(transform);
						trappedTarget.localPosition = Vector3.zero;
						trappedTarget.up = transform.up;
						trappedTarget.GetComponent<EnemyBehaviour>().GetCaught();
					}
				}
			}
		}
	}

	void EnemyDrop()
	{
		if (trappedTarget && HasReachedTargetPos())
		{
			trappedTarget.transform.SetParent(null);
			trappedTarget.GetComponent<EnemyBehaviour>().GetDropped();
			trappedTarget = null;
		}
	}

	void OnTriggerStay(Collider col)
	{
		if (col.tag == "DropCheck")
		{
			if (trappedTarget)
			{
				EnemyDrop();
				col.transform.root.GetComponent<DumpingSlab>().PlaySound();
			}
		}
	}

	void Update ()
	{
		if (!isDead)
		{
			if (!isMoving)
			{
				direction = GetMovedir();

				targetPosition = transform.position + direction;
				fromPosition = transform.position;
				fromRotation = transform.rotation;

				if (Mathf.Abs(direction.magnitude) > 0)
				{
					if (CanMoveTo(targetPosition))
					{
						isMoving = true;
					}
				}
			}

			if (isMoving)
			{
				MoveTo(fromPosition, targetPosition, fromRotation);
				EnemyCollision();
				if (HasReachedTargetPos())
				{
					isMoving = false;
				}
			}
		}
	}
}