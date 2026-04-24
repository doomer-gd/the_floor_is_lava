using System;
using Assets.Scripts.Controls;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;


public struct PlayerStatus
{
	public bool	isDead;
	public bool	isOnGround;
	public bool	isBeingPushed;
	public bool isControlled;
}

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController: MonoBehaviour
{
	[SerializeField] private Rigidbody		playerRigidbody;
	[SerializeField] private GameObject		playerCamera;
	[SerializeField] public InputManager	inputManager;
	[SerializeField] private float			linearDamping;
	[SerializeField] private Vector3		respawnPosition;
	private PlayerStatus					status;
	private GroundChecker					groundChecker;

	void Start()
	{
		groundChecker = new GroundChecker(GetComponent<Collider>());
		inputManager.InitializeCommands(playerRigidbody, playerCamera);
		status.isDead = true;
	}

	public void FixedUpdate()
	{
		if (status.isDead)
			Respawn();
		status.isOnGround = groundChecker.IsOnGround();
		status.isControlled = inputManager.IsInputTriggered();
		UpdateLinearDamping();
		UpdateRotationalConstraints();
		if (status is { isControlled: true, isOnGround: true })
			inputManager.ExecuteInput();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Deadly"))
			status.isDead = true;
	}

	public void OnCollisionStay(Collision collision)
	{
		if (collision.collider.CompareTag("Pusher"))
		{
			status.isBeingPushed = true;
		}
	}

	public void OnCollisionExit(Collision collision)
	{
		if (collision.collider.CompareTag("Pusher"))
		{
			Debug.Log("Left a pusher");
			status.isBeingPushed = false;
		}
	}

	private void Respawn()
	{
		playerRigidbody.linearVelocity = Vector3.zero;
		playerRigidbody.angularVelocity = Vector3.zero;
		transform.SetPositionAndRotation(respawnPosition, new Quaternion(0,0,0,0));
		status.isDead = false;
	}

	private void UpdateLinearDamping()
	{
		if (status.isOnGround)
			playerRigidbody.linearDamping = linearDamping;
		else
			playerRigidbody.linearDamping = 0;
	}

	private void UpdateRotationalConstraints()
	{
		if (status.isBeingPushed || status.isControlled || !status.isOnGround)
			playerRigidbody.constraints = RigidbodyConstraints.None;
		else
			playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
	}
}

public class GroundChecker
{
	private readonly Collider	body;
	private readonly float		lenghtRay;
	private readonly float		radiusSphere;

	public GroundChecker(Collider collider)
	{
		body = collider;
		lenghtRay = body.bounds.extents.y;
		radiusSphere = lenghtRay * 0.5f;
		lenghtRay *= 0.6f;
	}

	public bool IsOnGround()
	{
		Ray		ray;
		bool	isHit;

		ray = new Ray(body.transform.position, Vector3.down);
		isHit = Physics.SphereCast(ray, radiusSphere, out RaycastHit hit, lenghtRay, 1);
		return isHit;
	}
}

[Serializable]
public class InputManager
{
	[SerializeField] private PlayerInput	playerInput;
	[SerializeField] private float			moveSpeed;
	[SerializeField] private float			jumpForce;
	private InputAction						moveAction;
	private IControl						moverCommand;
	private InputAction						jumpAction;
	private IControl						jumperCommand;

	public void InitializeCommands(Rigidbody playerRigidbody, GameObject playerCamera)
	{
		InputSystem.actions.Disable();
		playerInput.currentActionMap?.Enable();
		moveAction = playerInput.actions["Move"];
		jumpAction = playerInput.actions["Jump"];
		moverCommand = new MoveCommand(playerRigidbody, playerCamera.GetComponent<Transform>(), moveAction, moveSpeed);
		jumperCommand = new JumpCommand(playerRigidbody, jumpAction, jumpForce);
	}

	public void ExecuteInput()
	{
		if (moveAction.IsPressed())
			moverCommand.ExecuteCommand();
		if (jumpAction.IsPressed())
			jumperCommand.ExecuteCommand();
	}

	public bool IsInputTriggered()
	{
		if (moveAction.IsPressed())
			return true;
		if (jumpAction.IsPressed())
			return true;
		return false;
	}
}