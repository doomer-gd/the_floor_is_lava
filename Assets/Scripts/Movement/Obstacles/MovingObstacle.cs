using UnityEngine;
using Assets.Scripts.Movement.Basics;
using Assets.Scripts.Movement.Obstacles;

[RequireComponent(typeof(Rigidbody))]
public class MovingObstacle: OscillatingObstacle
{
	enum Type {MOVING, ACCELERATING, ROTATING}

	[SerializeField] private Type		motionType;
	private Rigidbody					body;

	protected override IState SetMovingState()
	{
		IState		movingState;
		IMovable	mover;

		body = GetComponent<Rigidbody>();
		mover = SetMover(1 / timePhase);
		movingState = new PendulumMovingState(mover, mover.StartPosition, endPosition);
		return movingState;
	}

	private IMovable SetMover(float speed)
	{
		return motionType switch
		{
			Type.ACCELERATING =>	new AcceleratingPusher(transform, endPosition, speed, body, GetComponent<ConstantForce>()),
			Type.ROTATING =>		new RotatingPusher(transform, endPosition, speed, body),
			_ =>					new LinearPusher(transform, endPosition, speed, body),
		};
	}
}
