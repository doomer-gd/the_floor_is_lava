using UnityEngine;

public class WorldSettings: MonoBehaviour
{
	[SerializeField] private float	gravity;

	void Start()
	{
		Physics.gravity = new Vector3(0, -gravity, 0);
		Physics.IgnoreLayerCollision(0,0);
	}
}