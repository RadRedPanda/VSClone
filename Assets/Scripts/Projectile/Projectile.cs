using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public ProjectileData projectileData;
    public Projectile ProjectilePrefab;

    public List<Projectile> bulletObjectPool;

	private Rigidbody2D _rigidbody;
	private SpriteRenderer _sprite;
	private int _pierce;
	private int _multiply;
    private int _invulnframes = 3;

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_sprite = GetComponent<SpriteRenderer>();
	}

    private void FixedUpdate()
    {
        if (_invulnframes > 0)
        {
            _invulnframes--;
        }
    }

    public void FireProjectile(Vector2 directionVector, int pierce, int multiply, bool firedFromPlayer = true)
	{
        _invulnframes = 3;
        _pierce = pierce;
		_multiply = multiply;
        _sprite.sprite = projectileData.ProjectileImage;
		_rigidbody.velocity = directionVector * projectileData.ProjectileSpeed;
        if (firedFromPlayer)
        {
            _invulnframes = 0;
        }
		if (projectileData.RotationSpeed != 0) // spins the projectile
		{
			transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0f, 360f));
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
        EnemyController enemyHit = collision.GetComponent<EnemyController>();
		if (enemyHit == null)
		{
            gameObject.SetActive(false);
            bulletObjectPool.Add(this);
        } else if(_invulnframes <= 0)
		{
            //Debug.Log("Hit enemy", collision.gameObject);
            //Debug.Log("Bullet", this.gameObject);
            Debug.Log("Enabled = " + this.gameObject.activeSelf);
            enemyHit.TakeDamage(projectileData.Damage);
            Multiply(enemyHit);
            if (_pierce > 0)
            {
                _pierce--;
            }
            else // No more piercing
            {
                gameObject.SetActive(false);
                bulletObjectPool.Add(this);
            }
        }
	}

	private void Multiply(EnemyController enemyHit)
	{
		for (int i = 0; i < _multiply; i++)
		{
            Projectile bullet;
            if (bulletObjectPool.Count > 0)
            {
                bullet = bulletObjectPool[0];
                bulletObjectPool.RemoveAt(0);
            }
            else
            {
                bullet = Instantiate(ProjectilePrefab);
                bullet.bulletObjectPool = bulletObjectPool;
            }
            bullet.projectileData = projectileData;
            bullet.transform.position = transform.position;
            bullet.gameObject.SetActive(true);
            Vector2 velocityVector = UnityEngine.Random.insideUnitCircle;
            bullet.FireProjectile(velocityVector.normalized, _pierce - 1, 0, false);
        }
	}

}