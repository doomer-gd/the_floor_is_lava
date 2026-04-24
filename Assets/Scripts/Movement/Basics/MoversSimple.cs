using UnityEngine;

namespace Assets.Scripts.Movement.Basics
{
	public class LinearMover: IMovable
	{
		public Vector3		Direction { get; set; }
		public Vector3		StartPosition { get; set; }
		public Transform	CurrentTransform { get; set; }
		public float		Speed { get; set; }

		public LinearMover(Transform startTransform, Vector3 endPosition, float speed)
		{
			CurrentTransform = startTransform;
			StartPosition = startTransform.position;
			Direction = endPosition - startTransform.position;
			Speed = speed;
		}

		public void UpdateTransform()
		{
			CurrentTransform.position += Direction * (Speed * Time.fixedDeltaTime);
		}

		public void SetPosition(Vector3 position)
		{
			CurrentTransform.position = position;
		}
	}

	public class RotatingMover: IMovable
	{
		public Vector3		Direction { get; set; }
		public Vector3		StartPosition { get; set; }
		public Transform	CurrentTransform { get; set; }
		public float		Speed { get; set; }
		private Quaternion	startRotation;
		private Quaternion	endRotation;
		private float		angle;

		public RotatingMover(Transform startTransform, Vector3 endPosition, float speed)
		{
			CurrentTransform = startTransform;
			StartPosition = startTransform.eulerAngles;
			Direction = endPosition - StartPosition;
			Speed = speed;
			startRotation = startTransform.rotation;
			endRotation = Quaternion.Euler(endPosition);
		}

		public void UpdateTransform()
		{
			Quaternion	newAngle;

			angle += Speed * Time.fixedDeltaTime;
			newAngle = Quaternion.Lerp(startRotation, endRotation, angle);
			CurrentTransform.rotation = newAngle;
		}

		public void SetPosition(Vector3 position)
		{
			CurrentTransform.rotation = Quaternion.Euler(position);
			if (Speed > 0)
				angle = 0;
			else
				angle = 1;
		}
	}

	public class StretchingMover: IMovable
	{
		public Vector3		Direction { get; set; }
		public Vector3		StartPosition { get; set; }
		public Transform	CurrentTransform { get; set; }
		public float		Speed { get; set; }

		public StretchingMover(Transform startTransform, Vector3 endPosition, float speed)
		{
			CurrentTransform = startTransform;
			StartPosition = startTransform.localScale;
			Direction = endPosition - StartPosition;
			Speed = speed;
		}

		public void UpdateTransform()
		{
			CurrentTransform.localScale += Direction * (Speed * Time.fixedDeltaTime);
		}

		public void SetPosition(Vector3 position)
		{
			CurrentTransform.localScale  = position;
		}
	}

	public class OrbitingMover: IMovable
	{
		public Vector3		Direction { get; set; }
		public Vector3		StartPosition { get; set; }
		public Transform	CurrentTransform { get; set; }
		public float		Speed { get; set; }
		private Vector3		center;
		private float		radius;

		public OrbitingMover(Transform startTransform, Vector3 center, Vector3 axis, float speed)
		{
			CurrentTransform = startTransform;
			StartPosition = startTransform.localPosition;
			Direction = axis.normalized;
			Speed = Mathf.PI * 0.5f * speed;
			this.center = center;
			radius = (startTransform.localPosition - center).magnitude;
		}

		public void UpdateTransform()
		{
			Vector3	rayCenter;
			Vector3	rayMidway;

			rayCenter = (CurrentTransform.localPosition - center).normalized * radius;
			rayMidway = Vector3.Cross(rayCenter, Direction);
			CurrentTransform.localPosition = Vector3.Slerp(rayCenter, rayMidway, Speed * Time.fixedDeltaTime) + center;
		}

		public void SetPosition(Vector3 position)
		{
			CurrentTransform.localPosition = position;
		}
	}
}