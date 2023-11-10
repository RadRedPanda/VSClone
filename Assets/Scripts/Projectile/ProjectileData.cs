using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "ScriptableObjects/Projectile")]
public class ProjectileData : ScriptableObject
{
	public Sprite ProjectileImage;
	[Min(0)]
	public float Damage = 3f;
	[Min(0)]
	public float ProjectileSpeed = 3f;
	public float RotationSpeed = 0f;
	public int Pierce = 0; // How many enemies a shot will pierce through, default to none
	public int Multiply = 0; // How many shots will fire out in random directions on hit (not including original bullet), default to none
}