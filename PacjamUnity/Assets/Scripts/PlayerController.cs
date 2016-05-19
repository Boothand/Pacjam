using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//Private
	bool isMoving;
	Vector3 direction;
	Vector3 targetPosition;
	Vector3 fromPosition;
	Quaternion fromRotation;
	float lerpValue;

	//Public
	public LayerMask collisionLayer;
	public AnimationCurve moveCurve;
	public float speed = 2f;
	public float moveDuration = 1f;
	
	void Start ()
	{
		
	}

	bool CanMoveTo(Vector3 pos)
	{
		bool hit = Physics.Raycast(transform.position, pos - transform.position, 1f, collisionLayer);

		return !hit;
	}

	void MoveTo(Vector3 pos)
	{
		pos.x = Mathf.RoundToInt(pos.x);
		pos.z = Mathf.RoundToInt(pos.z);
		pos.y = transform.position.y;

		transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveCurve.Evaluate(Time.time) * speed * Time.deltaTime);
	}

	bool HasReachedTargetPos()
	{
		if ( Vector3.Distance(transform.position, targetPosition) < 0.001f)
		{
			transform.position = targetPosition;
			lerpValue = 0;
			direction = Vector3.zero;
			return true;
		}

		return false;
	}

	void MoveTo(Vector3 from, Vector3 pos, Quaternion fromRot)
	{
		pos.x = Mathf.RoundToInt(pos.x);
		pos.z = Mathf.RoundToInt(pos.z);
		pos.y = transform.position.y;

		lerpValue += Time.deltaTime * speed;

		if (lerpValue > moveDuration)
		{
			lerpValue = moveDuration;
		}

		float moveJourney = lerpValue / moveDuration;

		Quaternion angle = Quaternion.Euler(90 * direction.z, 0, 90 * -direction.x);
		transform.position = Vector3.MoveTowards(from, targetPosition, moveCurve.Evaluate(moveJourney));
		transform.rotation = Quaternion.Lerp(fromRotation, angle * fromRotation, moveCurve.Evaluate(moveJourney));
	}

	void Update ()
	{
		if (!isMoving)
		{
			float dirX = Input.GetAxisRaw("Horizontal");
			float dirZ = Input.GetAxisRaw("Vertical");

			if (Mathf.Abs(dirX) > 0f)
			{
				direction.x = dirX;
				direction.z = 0;
			}
			else if (Mathf.Abs(dirZ) > 0f)
			{
				direction.x = 0;
				direction.z = dirZ;
			}

			print(direction);

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
		else
		{
			MoveTo(fromPosition, targetPosition, fromRotation);

			if (HasReachedTargetPos())
			{
				isMoving = false;
			}
		}
	}
}