using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TestMove: MonoBehaviour
{
	public float	radius;
	public float	currentSpeed;

	void Start()
	{
		GetComponent<Rigidbody>().linearVelocity = Vector3.forward * radius;
		GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 1, 0);
		GetComponent<ConstantForce>().relativeForce = new Vector3(radius, 0, 0);
	}

	void Update()
	{
		currentSpeed = GetComponent<Rigidbody>().linearVelocity.magnitude;
	}

}