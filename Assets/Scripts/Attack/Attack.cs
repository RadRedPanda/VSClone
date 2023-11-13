using UnityEngine;

public class Attack : MonoBehaviour
{
	public BaseAttackData _attackData;

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

    public void LaunchAttack(Vector2 directionVector, int pierce, int multiply, bool firedFromPlayer = true)
	{
        _invulnframes = 3;
        _pierce = pierce;
		_multiply = multiply;
        _sprite.sprite = _attackData.Image;
		_rigidbody.velocity = directionVector * _attackData.ProjectileSpeed;
        if (firedFromPlayer)
        {
            _invulnframes = 0;
        }
		if (_attackData.RotationSpeed != 0) // spins the projectile
		{
			transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0f, 360f));
			_rigidbody.angularVelocity = _attackData.RotationSpeed;
		}
		else // points the attack in the direction
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
			AttackManager.ReturnToPool(this);
			return;
		}
		if (!gameObject.activeSelf || !enemyHit.gameObject.activeSelf)
			return;
		if (_invulnframes > 0)
			return;

        enemyHit.TakeDamage(_attackData.Damage);

        Multiply(enemyHit);

        if (--_pierce == 0)
			AttackManager.ReturnToPool(this);
	}

	private void Multiply(EnemyController enemyHit)
	{
		for (int i = 0; i < _multiply; i++)
		{
            Attack bullet = AttackManager.GetFromPool(_attackData, transform.position);

            Vector2 velocityVector = UnityEngine.Random.insideUnitCircle;
            bullet.LaunchAttack(velocityVector.normalized, _pierce - 1, 0, false);
        }
	}
}