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
		if (projectileData.RotationSpeed != 0) // spins the projectile
		{
			transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
			_rigidbody.angularVelocity = projectileData.RotationSpeed;
		}
		else // points the projectile in the direction
		{
			float angle = Mathf.Atan2(directionVector.y, directionVector.x);
			transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle - 90); // assuming sprite faces up
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<EnemyController>() != null)
		{
			collision.GetComponent<EnemyController>().TakeDamage(projectileData.Damage);
		}
		gameObject.SetActive(false);
		bulletObjectPool.Add(this);
	}
}