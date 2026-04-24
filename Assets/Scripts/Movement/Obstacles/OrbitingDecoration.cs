using UnityEngine;
using Assets.Scripts.Movement.Basics;
using Assets.Scripts.Movement.Obstacles;

public class OrbitingDecoration: MonoBehaviour
{
	[SerializeField] private Vector3	center;
	[SerializeField] private Vector3	axis;
	[SerializeField] protected float	timeRotation;
	private OrbitingMover				mover;

	public void Start()
	{
		mover = new OrbitingMover(transform, center, axis, 1 / timeRotation);
	}

	public void FixedUpdate()
	{
		mover.UpdateTransform();
	}
}