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
		Vector3 halfScale = transform.localScale / 2;
		halfScale.y = 0;
		float offset = 0.98f;

		Vector3 top = Vector3.forward * halfScale.z * offset;
		Vector3 bottom = Vector3.back * halfScale.z * offset;
		Vector3 right = Vector3.right * halfScale.x * offset;
		Vector3 left = Vector3.left * halfScale.x * offset;

		Vector3 verticalRight = transform.position + (top * direction.z) + (right * direction.x);
		Vector3 verticalLeft = transform.position + (top * direction.z) + (left * direction.x);

		Vector3 horizontalTop = transform.position + (right * direction.x) + (top * direction.x);
		Vector3 horizontalBottom = transform.position + (right * direction.x) + (bottom * direction.x);

		float rayLength = 0.8f;

		if (Mathf.Abs(direction.x) > 0)
		{
			bool topHit = Physics.Raycast(horizontalTop, direction, rayLength, collisionLayer);
			bool bottomHit = Physics.Raycast(horizontalBottom, direction, rayLength, collisionLayer);
			
			if (topHit || bottomHit)
			{
				return false;
			}
		}

		if (Mathf.Abs(direction.z) > 0)
		{
			bool leftHit = Physics.Raycast(verticalLeft, direction, rayLength, collisionLayer);
			bool rightHit = Physics.Raycast(verticalRight, direction, rayLength, collisionLayer);

			if (leftHit || rightHit)
			{
				return false;
			}
		}
		
		return true;
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

	void Update ()
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

			if (HasReachedTargetPos())
			{
				isMoving = false;
			}
		}
	}
}