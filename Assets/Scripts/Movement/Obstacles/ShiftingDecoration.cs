using UnityEngine;
using Assets.Scripts.Movement.Basics;
using Assets.Scripts.Movement.Obstacles;

public class ShiftingDecoration: OscillatingObstacle
{
	enum Type {MOVING, ROTATING, STRETCHING}

	[SerializeField] private Type	motionType;

	protected override IState SetMovingState()
	{
		IState		movingState;
		IMovable	mover;

		mover = SetMover(1 / timePhase);
		movingState = new PendulumMovingState(mover, mover.StartPosition, endPosition);
		return movingState;
	}

	private IMovable SetMover(float speed)
	{
		return motionType switch
		{
			Type.ROTATING =>	new RotatingMover(transform, endPosition, speed),
			Type.STRETCHING =>	new StretchingMover(transform, endPosition, speed),
			_ =>				new LinearMover(transform, endPosition, speed),
		};
	}
}

