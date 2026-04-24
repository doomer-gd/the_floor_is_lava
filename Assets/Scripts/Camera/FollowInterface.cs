using UnityEngine;

namespace Assets.Scripts.Camera
{
	public abstract class AFollower
	{
		protected Transform					followerTransform;
		protected GameObject				target;
		protected Transform					targetTransform;
		public float						FollowAngle { get; set; }
		public float						FollowDistance { get; set; }

		protected abstract Vector3	CalculateNewPosition();
		public abstract void		Initialize();

		public void Follow()
		{
			if (targetTransform && followerTransform)
			{
				followerTransform.position = CalculateNewPosition();
				followerTransform.LookAt(targetTransform);
			}
		}
		public AFollower()
		{
			FollowAngle = 45.0f;
			FollowDistance = 10.0f;
		}
		public AFollower(GameObject newTarget, Transform newFollower, float angle, float distance)
		{
			target = newTarget;
			followerTransform = newFollower;
			FollowAngle = angle;
			FollowDistance = distance;
		}
	}

	public class FollowerRadial: AFollower
	{
		public Transform	center;

		protected override Vector3	CalculateNewPosition()
		{
			Vector3		radius;
			float		extensionFactor;
			radius = targetTransform.position - center.position;
			extensionFactor = 1.0f + FollowDistance / radius.magnitude;
			radius.x *= extensionFactor;
			radius.z *= extensionFactor;
			radius.y += FollowDistance / Mathf.Tan(Mathf.Deg2Rad * FollowAngle);
			return radius;
		}

		public override void Initialize()
		{
			if (target)
			{
				targetTransform = target.GetComponent<Transform>();
			}
		}

		public FollowerRadial(GameObject newTarget, Transform newFollower, Transform newCenter, float angle, float distance):
		base(newTarget, newFollower, angle, distance)
		{
			center = newCenter;
		}
	}

	public class FollowerBehind: AFollower
	{
		private Rigidbody	targetRigitBody;

		public override void Initialize()
		{
			if (target)
			{
				targetTransform = target.GetComponent<Transform>();
				targetRigitBody = target.GetComponent<Rigidbody>();
			}
		}

		protected override Vector3	CalculateNewPosition()
		{
			Vector3		radius;
			radius = targetTransform.position - targetRigitBody.linearVelocity.normalized * FollowDistance;
			radius.y = targetTransform.position.y + FollowDistance / Mathf.Tan(Mathf.Deg2Rad * FollowAngle);
			return radius;
		}
		public FollowerBehind(GameObject newTarget, Transform newFollower, float angle, float distance):base(newTarget, newFollower, angle, distance){}
	}
}
