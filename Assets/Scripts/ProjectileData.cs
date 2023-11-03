using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "ScriptableObjects/Projectile")]
public class ProjectileData : ScriptableObject
{
	public Sprite ProjectileImage;
	public float Damage;
	public float ProjectileSpeed;
}