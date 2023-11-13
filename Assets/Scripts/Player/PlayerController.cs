using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	[Header("Player properties")]
	[SerializeField, Tooltip("Player's Current Health")]
	private FloatVariable _currentHealth;
	[SerializeField, Tooltip("Player's Maximum Health")]
	private FloatVariable _maxHealth;

	[SerializeField, Tooltip("How fast we want them to move")]
	private float _maxSpeed = 3f;
	[SerializeField, Tooltip("How fast we want them to start moving")]
	private float _acceleration = 3f;

    private Rigidbody2D _rigidbody;

	[SerializeReference]
	private BaseAttackData _attackData;

	[SerializeField, Tooltip("How long in between attacks"), Min(0)]
	private float _cooldown = 1f;
	private float _lastAttackTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
		_currentHealth.Value = _maxHealth.Value;
    }

	private void Update()
	{
		if (Input.GetButton("Fire1") && Time.time > _lastAttackTime + _cooldown)
		{
			_lastAttackTime = Time.time;
			launchAttack();
		}
	}

	void FixedUpdate()
	{
		Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		targetVelocity = Vector2.ClampMagnitude(targetVelocity, 1f) * _maxSpeed;
		_rigidbody.velocity = Vector2.MoveTowards(_rigidbody.velocity, targetVelocity, _acceleration * Time.deltaTime);
	}

	#region Take Damage
	private UnityAction<float, EnemyController> _takeDamageAction;
	public void SubscribeTakeDamage(UnityAction<float, EnemyController> action) {_takeDamageAction += action;}
	public void UnsubscribeTakeDamage(UnityAction<float, EnemyController> action) {_takeDamageAction -= action;}
	public void TakeDamage(float amount, EnemyController source)
	{
		_takeDamageAction?.Invoke(amount, source);
		_currentHealth.Value -= amount;
		if (_currentHealth.Value <= 0)
			Die();
	}
	#endregion
	#region Die
	private UnityAction _dieAction;
	public void SubscribeDie(UnityAction action) { _dieAction += action; }
	public void UnsubscribeDie(UnityAction action) { _dieAction -= action; }
	public void Die()
	{
		Debug.Log("DIED");
		Time.timeScale = 0f;
		_dieAction?.Invoke();
	}
	#endregion

	private void launchAttack()
	{
		Attack attack = AttackManager.GetFromPool(_attackData, transform.position);

        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 velocityVector = targetPosition - (Vector2)transform.position;
		attack.LaunchAttack(velocityVector.normalized, _attackData.Pierce, _attackData.Multiply);
	}
}