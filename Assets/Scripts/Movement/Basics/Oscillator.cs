using UnityEngine;
using Assets.Scripts.Movement.Basics;
/// <summary>
/// Makes an object oscillate between two states over time.
/// </summary>

public class Oscillator
{
	private readonly float			timePhase;
	private readonly float			timeDelay;
	private readonly IState			stateMoving;
	private readonly IState			stateWaiting;
	private readonly StateMachine	pendulum = new ();
	private float					timeLastChange;

	public Oscillator(	float timePhase,
						float timeDelay,
						IState stateMove,
						IState stateWait)
	{
		this.timePhase = timePhase;
		this.timeDelay = timeDelay;
		stateMoving = stateMove;
		stateWaiting = stateWait;
		if (stateMoving == null || stateWaiting == null)
			throw new System.Exception("Failed oscillator initialization");
		pendulum.InitializeState(stateWaiting);
		timeLastChange = Time.realtimeSinceStartup;
	}
	public void Oscillate()
	{
		UpdateState();
		pendulum.Update();
	}

	private void UpdateState()
	{
		if (pendulum.currentState == stateMoving && Time.realtimeSinceStartup > timeLastChange + timePhase)
			pendulum.ChangeState(stateWaiting);
		else if (pendulum.currentState == stateWaiting && Time.realtimeSinceStartup > timeLastChange + timeDelay)
			pendulum.ChangeState(stateMoving);
		else
			return ;
		timeLastChange = Time.realtimeSinceStartup;
	}
}


