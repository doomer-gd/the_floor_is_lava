using UnityEngine;

namespace Assets.Scripts.Movement.Basics
{
	public interface IMovable
	{
		public Vector3		Direction { get; set; }
		public Vector3		StartPosition { get; set; }
		public Transform	CurrentTransform { get; set; }
		public float		Speed { get; set; }
		
		public void	UpdateTransform();
		public void	SetPosition(Vector3 position);
	}

	public class Bouncer
	{
		private readonly Vector3	startPosition;
		private readonly Vector3	endPosition;
		public readonly IMovable	mover;

		public Bouncer(IMovable mover, Vector3 startPosition, Vector3 endPosition)
		{
			this.mover = mover;
			this.startPosition = startPosition;
			this.endPosition = endPosition;
		}

		public void	CorrectEdgePosition(bool isAtStartPosition)
		{
			if (isAtStartPosition)
				mover.SetPosition(startPosition);
			else
				mover.SetPosition(endPosition);
		}
	}

	public interface	IState
	{
		public void EnterState();
		public void Update();
		public void ExitState();
	}

	public class	StateMachine
	{
		public IState currentState;

		public void InitializeState(IState startingState)
		{
			currentState = startingState;
			currentState.EnterState();
		}
		public void ChangeState(IState newState)
		{
			currentState.ExitState();
			currentState = newState;
			currentState.EnterState();
		}
		public void Update()
		{
			currentState.Update();
		}
	}
}