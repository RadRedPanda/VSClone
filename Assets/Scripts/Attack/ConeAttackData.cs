using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "ScriptableObjects/Cone Attack")]
public sealed class ConeAttackData : BaseAttackData
{
	[SerializeField, Min(0)]
	public float Radius = 1;

	[SerializeField, Range(0f, 180f)]
	public float Angle = 90f;
	public override sealed Collider2D ApplyCollider(GameObject go)
	{
		// remove the old collider
		Collider2D oldCollider = go.GetComponent<Collider2D>();
		if (oldCollider != null)
			Destroy(oldCollider);

		// add the new collider
		PolygonCollider2D newCollider = go.AddComponent<PolygonCollider2D>();
		float angleRadians = Angle * Mathf.Deg2Rad;
		newCollider.points = new List<Vector2>{
		Vector2.zero,
		Radius * new Vector2(Mathf.Sin(angleRadians / 2), Mathf.Cos(angleRadians / 2)),
		new Vector2(0, Radius),
		Radius * new Vector2(-Mathf.Sin(angleRadians / 2), Mathf.Cos(angleRadians / 2))
		}.ToArray();
		newCollider.isTrigger = true;
		return newCollider;
	}

	public static List<Vector2> GetConePoints(float radius, float angle)
	{
		List<Vector2> points = new List<Vector2>();
		float angleRadians = angle * Mathf.Deg2Rad;
		points.Add(Vector2.zero);
		points.Add(radius * new Vector2(Mathf.Sin(angleRadians / 2), Mathf.Cos(angleRadians / 2)));
		points.Add(new Vector2(0, radius));
		points.Add(radius * new Vector2(-Mathf.Sin(angleRadians / 2), Mathf.Cos(angleRadians / 2)));
		return points;
	}
}