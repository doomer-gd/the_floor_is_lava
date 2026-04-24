using UnityEngine;
using Assets.Scripts.Maths;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Controls
{
	public interface IControl
	{
		public void ExecuteCommand();
	}

	public class MoveCommand: IControl
	{
		private Rigidbody		targetBody;
		private readonly Transform		targetPosition;
		private readonly InputAction	action;
		private readonly Transform		cameraPosition;
		private readonly float			force;

		public MoveCommand(Rigidbody newBody, Transform cameraPos, InputAction controller, float newForce)
		{
			targetBody = newBody;
			targetPosition = targetBody.transform;
			force = newForce;
			cameraPosition = cameraPos;
			action = controller;
		}

		public void ExecuteCommand()
		{
			Vector3	direction3D;
			Vector2	direction2D;
			Vector2	controllerDirection = action.ReadValue<Vector2>();

			direction3D = targetPosition.position - cameraPosition.position;
			direction2D = new Vector2(direction3D.x, direction3D.z);
			direction2D.Normalize();
			direction2D = Maths2.RotateVector2(direction2D, Mathf.Deg2Rad * Vector2.SignedAngle(Vector2.up, controllerDirection));
			direction2D *= force;
			direction3D = new Vector3(direction2D.x, 0, direction2D.y);
			targetBody.AddForce(direction3D, ForceMode.VelocityChange);
		}
	}
	
	public class JumpCommand: IControl
	{
		private readonly Rigidbody	targetBody;
		private readonly InputAction	action;
		private readonly float		force;

		public JumpCommand(Rigidbody newBody, InputAction controller, float newForce)
		{
			targetBody = newBody;
			force = newForce;
			action = controller;
		}

		public void ExecuteCommand()
		{
			targetBody.AddForce(new Vector3(0, force, 0), ForceMode.Impulse);
			targetBody.linearDamping = 0;
		}
	}
}