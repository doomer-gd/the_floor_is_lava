using UnityEngine;
using Assets.Scripts.Camera;

[RequireComponent(typeof(Camera))]
public class CameraMovement: MonoBehaviour
{
	[SerializeField] private GameObject	target;
	[SerializeField] private Transform	center;
	[SerializeField] private float		distance;
	[SerializeField] private float		angle;
	private Transform		followerTransform;
	private FollowerRadial	followerRadial;
	private FollowerBehind	followerBehind;
	private enum CameraMode { RADIAL, THIRDPERSON }
	private CameraMode		cameraMode = CameraMode.RADIAL;
	void Start()
	{
		followerTransform = GetComponent<Transform>();
		followerRadial = new FollowerRadial(target, followerTransform, center, angle, distance);
		followerBehind = new FollowerBehind(target, followerTransform, angle, distance);
		followerRadial.Initialize();
		followerBehind.Initialize();
	}
	void LateUpdate()
	{
		if (cameraMode == CameraMode.RADIAL)
			followerRadial.Follow();
		if (cameraMode == CameraMode.THIRDPERSON)
			followerBehind.Follow();
	}
}