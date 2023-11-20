using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttackData : ScriptableObject
{
	public Sprite Image;
	[Min(0)]
	public float Damage = 3f;
	[Min(0)]
	public float ProjectileSpeed = 3f;
	public float RotationSpeed = 0f;
	[Tooltip("How many enemies a shot will pierce through, default to none, -1 is infinite")]
	public int Pierce = 0;
	[Tooltip("How many shots will fire out in random directions on hit (not including original bullet), default to none")]
	public int Multiply = 0;
	public bool Boomerang = false;
	[Tooltip("Real time seconds of how long the projectile lasts, doubles if Boomerang is true")]
	public float Lifespan = 3f;
	public List<Vector3Int> Nodes;

	public abstract Collider2D ApplyCollider(GameObject go);
}