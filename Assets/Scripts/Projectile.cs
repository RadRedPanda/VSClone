using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public ProjectileData projectileData;

	public List<Projectile> bulletObjectPool;

	private Rigidbody2D _rigidbody;
	private SpriteRenderer _sprite;

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_sprite = GetComponent<SpriteRenderer>();
	}

	public void FireProjectile(Vector2 directionVector)
	{
		_sprite.sprite = projectileData.ProjectileImage;
		_rigidbody.velocity = directionVector * projectileData.ProjectileSpeed;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		gameObject.SetActive(false);
		bulletObjectPool.Add(this);
	}
}