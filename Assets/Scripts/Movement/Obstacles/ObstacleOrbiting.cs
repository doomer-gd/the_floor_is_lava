using UnityEngine;
using Assets.Scripts.Movement.Basics;
using Assets.Scripts.Movement.Obstacles;

[RequireComponent(typeof(Rigidbody))]
public class ObstacleOrbiting: OscillatingObstacle
{
	[SerializeField] private Vector3	center;
	private OrbitingPusher				mover;

	protected override IState SetMovingState()
	{
		mover = new OrbitingPusher(transform, GetComponent<Rigidbody>(), center, ref endPosition, 1 / timePhase);
		return new PendulumMovingState(mover, mover.StartPosition, endPosition);
	}
}