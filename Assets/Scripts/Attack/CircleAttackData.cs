using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "ScriptableObjects/Circle Attack")]
public sealed class CircleAttackData : BaseAttackData
{
	[SerializeField, Min(0)]
	public float Radius = 1;
	public override sealed Collider2D ApplyCollider(GameObject go)
	{
		// remove the old collider
		Collider2D oldCollider = go.GetComponent<Collider2D>();
		if (oldCollider != null)
			Destroy(oldCollider);

		// add the new collider
		CircleCollider2D newCollider = go.AddComponent<CircleCollider2D>();
		newCollider.radius = Radius;
		newCollider.isTrigger = true;
		return newCollider;
	}
}