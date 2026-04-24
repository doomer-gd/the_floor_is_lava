using System;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Movement.Basics
{
	public class LinearPusher: IMovable
	{
		public Vector3				Direction { get; set; }
		public Vector3				StartPosition { get; set; }
		public Transform			CurrentTransform { get; set; }
		public float				Speed { get; set; }
		private readonly Rigidbody	body;
		private bool				isMoving = true;

		public LinearPusher(Transform startTransform, Vector3 endPosition, float speed, Rigidbody body)
		{
			CurrentTransform = startTransform;
			StartPosition = startTransform.position;
			Direction = endPosition - startTransform.position;
			Speed = speed;
			this.body = body;
			body.linearVelocity = Direction * Speed;
		}

		public void UpdateTransform(){}

		public void SetPosition(Vector3 position)
		{
			if (isMoving)
				body.constraints = RigidbodyConstraints.None;
			else
				body.constraints = RigidbodyConstraints.FreezePosition;
			isMoving ^= true;
			CurrentTransform.position = position;
			body.linearVelocity = Direction * Speed;
		}
	}

	public class AcceleratingPusher: IMovable
	{
		public Vector3					Direction { get; set; }
		public Vector3					StartPosition { get; set; }
		public Transform				CurrentTransform { get; set; }
		public float					Speed { get; set; }
		private bool					isMoving = true;
		private readonly Rigidbody		body;
		private readonly ConstantForce	constantForce;

		public AcceleratingPusher(Transform startTransform, Vector3 endPosition, float speed, Rigidbody body, ConstantForce force)
		{
			if (force == null)
				throw new System.Exception("ConstantForce component not found");
			CurrentTransform = startTransform;
			StartPosition = startTransform.position;
			Direction = (endPosition - startTransform.position) * body.mass;
			Speed = speed;
			this.body = body;
			constantForce = force;
			constantForce.force = Direction * (2 * Speed * Speed * Math.Sign(Speed));
		}

		public void UpdateTransform(){}

		public void SetPosition(Vector3 position)
		{
			if (isMoving)
				body.constraints = RigidbodyConstraints.None;
			else
				body.constraints = RigidbodyConstraints.FreezePosition;
			isMoving ^= true;
			CurrentTransform.position = position;
			constantForce.force = Direction * (2 * Speed * Speed * Math.Sign(Speed));
		}
	}

	public class RotatingPusher: IMovable
	{
		public Vector3				Direction { get; set; }
		public Vector3				StartPosition { get; set; }
		public Transform			CurrentTransform { get; set; }
		public float				Speed { get; set; }
		private readonly Rigidbody	body;
		private bool				isMoving = true;
		private float				angularSpeed;

		public RotatingPusher(Transform startTransform, Vector3 endPosition, float speed, Rigidbody body)
		{
			Vector3	rotationAxis;

			CurrentTransform = startTransform;
			StartPosition = startTransform.eulerAngles;
			(Quaternion.Inverse(startTransform.rotation) * Quaternion.Euler(endPosition)).ToAngleAxis(out angularSpeed, out rotationAxis);
			Direction = rotationAxis * (angularSpeed * Mathf.Deg2Rad);
			Direction = math.mul(startTransform.rotation, Direction);
			Speed = speed;
			this.body = body;
			body.angularDamping = 0.0f;
			body.angularVelocity = Direction * Speed;
		}

		public void UpdateTransform(){}

		public void SetPosition(Vector3 position)
		{
			if (isMoving)
				body.constraints = RigidbodyConstraints.None;
			else
				body.constraints = RigidbodyConstraints.FreezeRotation;
			isMoving ^= true;
			CurrentTransform.rotation = Quaternion.Euler(position);
			body.angularVelocity = Direction * Speed;
		}
	}

	//this one keeps disobeying the laws of physics, probably has to do with Unity's realization of them
	public class OrbitingPusher: IMovable
	{
		public Vector3				Direction { get; set; }
		public Vector3				StartPosition { get; set; }
		public Transform			CurrentTransform { get; set; }
		public float				Speed { get; set; }
		private readonly Rigidbody	body;
		private readonly Vector3	center;
		private bool				isMoving = true;
		private Vector3				axis;

		public OrbitingPusher(Transform startTransform, Rigidbody body, Vector3 center, ref Vector3 endPosition, float speed)
		{
			Vector3	rayStart;
			Vector3	rayEnd;

			CurrentTransform = startTransform;
			StartPosition = startTransform.position;
			rayStart = center - StartPosition;
			rayEnd = (center - endPosition).normalized * rayStart.magnitude;
			endPosition = center - rayEnd;
			axis = Vector3.Cross(rayStart, rayEnd).normalized;
			this.center = center;
			Direction = Vector3.Cross(rayStart, axis);
			Speed = Vector3.SignedAngle(rayStart, rayEnd, axis) * speed * Mathf.Deg2Rad;
			this.body = body;
			body.linearVelocity = Direction * Speed;
		}

		public void UpdateTransform()
		{
			Vector3	centrifugalForce;

			centrifugalForce = (center - CurrentTransform.position) * (body.mass * Mathf.Abs(Speed));
			body.AddForce(centrifugalForce);
		}


		public void SetPosition(Vector3 position)
		{
			if (isMoving)
				body.constraints = RigidbodyConstraints.None;
			else
				body.constraints = RigidbodyConstraints.FreezePosition;
			isMoving ^= true;
			CurrentTransform.position = position;
			Direction = Vector3.Cross(center - CurrentTransform.position, axis);
			body.linearVelocity = Direction * Speed;
		}
	}

}