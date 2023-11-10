using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	[Header("Player properties")]
	[SerializeField, Tooltip("How fast we want them to move")]
	private float _maxSpeed = 3f;
	[SerializeField, Tooltip("How fast we want them to start moving")]
	private float _acceleration = 3f;

    private Rigidbody2D _rigidbody;


	[SerializeField]
	private ProjectileData _projectile;
    public Projectile ProjectilePrefab;
	private List<Projectile> projectileObjectPool = new List<Projectile>();

	private float _currentHealth;
	[SerializeField, Tooltip("How long in between shots"), Min(0)]
	private float _cooldown = 1f;
	private float _lastShotTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

	private void Update()
	{
		if (Input.GetButton("Fire1") && Time.time > _lastShotTime + _cooldown)
		{
			_lastShotTime = Time.time;
			fireProjectile();
		}
	}

	void FixedUpdate()
	{
		Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		targetVelocity = Vector2.ClampMagnitude(targetVelocity, 1f) * _maxSpeed;
		_rigidbody.velocity = Vector2.MoveTowards(_rigidbody.velocity, targetVelocity, _acceleration * Time.deltaTime);
	}

	#region Take Damage
	private UnityAction<float, Collider2D> _takeDamageAction;
	public void SubscribeTakeDamage(UnityAction<float, Collider2D> action) {_takeDamageAction += action;}
	public void UnsubscribeTakeDamage(UnityAction<float, Collider2D> action) {_takeDamageAction -= action;}
	public void TakeDamage(float amount, Collider2D source)
	{
		_takeDamageAction?.Invoke(amount, source);
		_currentHealth -= amount;
		if (_currentHealth <= 0)
			Die();
	}
	#endregion
	#region Die
	private UnityAction _dieAction;
	public void SubscribeDie(UnityAction action) { _dieAction += action; }
	public void UnsubscribeDie(UnityAction action) { _dieAction -= action; }
	public void Die()
	{
		_dieAction?.Invoke();
	}
	#endregion
	private void fireProjectile()
	{
		Projectile bullet;
		if (projectileObjectPool.Count > 0)
		{
			bullet = projectileObjectPool[0];
			projectileObjectPool.RemoveAt(0);
		}
		else
		{
			bullet = Instantiate(ProjectilePrefab);
			bullet.bulletObjectPool = projectileObjectPool;
		}
		bullet.projectileData = _projectile;
		bullet.transform.position = transform.position;
        bullet.gameObject.SetActive(true);
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 velocityVector = targetPosition - (Vector2)transform.position;
		bullet.FireProjectile(velocityVector.normalized, _projectile.Pierce, _projectile.Multiply);
	}
}
