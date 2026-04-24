using UnityEngine;

namespace Assets.Scripts.Maths
{
	public class Maths2
	{
		public static Vector2 RotateVector2(Vector2 vector, float angleRadians)
		{
			float sin = Mathf.Sin(angleRadians);
			float cos = Mathf.Cos(angleRadians);
			Vector2 result;
			result.x = vector.x * cos - vector.y * sin;
			result.y = vector.x * sin + vector.y * cos;
			return result;
		}
	}
}