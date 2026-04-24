using UnityEngine;
using Assets.Scripts.Movement.Basics;
using System;

namespace Assets.Scripts.Movement.Obstacles
{
	public class PendulumMovingState: IState
	{
		public Bouncer	bouncer;
		private bool	isAtStartPosition = true;

		public PendulumMovingState(IMovable mover, Vector3 startPosition, Vector3 endPosition)
		{
			bouncer = new Bouncer(mover, startPosition, endPosition);
		}

		public void	EnterState()
		{
			bouncer.CorrectEdgePosition(isAtStartPosition);
		}

		public void	Update()
		{
			bouncer.mover.UpdateTransform();
		}
		public void	ExitState()
		{
			isAtStartPosition ^= true;
			bouncer.mover.Speed = -bouncer.mover.Speed;
			bouncer.CorrectEdgePosition(isAtStartPosition);
		}
	}

	public class dummyState: IState
	{
		public void	EnterState() {}
		public void	Update() {}
		public void	ExitState() {}
	}

	public abstract class OscillatingObstacle: MonoBehaviour
	{
		[SerializeField] protected Vector3	endPosition;
		[SerializeField] protected float	timePhase;
		[SerializeField] private float		timeDelay;
		private Oscillator					oscillator;

		protected abstract IState SetMovingState();

		public void Start()
		{
			IState		movingState;

			movingState = SetMovingState();
			oscillator = new Oscillator(timePhase, timeDelay, movingState, new dummyState());
		}

		public void FixedUpdate()
		{
			if (oscillator != null)
				oscillator.Oscillate();
		}

	}
}
