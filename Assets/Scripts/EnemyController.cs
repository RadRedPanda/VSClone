using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Tooltip("The SO which holds the enemy data")]
    private EnemyData _enemyData;
    [SerializeField, Tooltip("How fast we want them to start moving")]
	private float _acceleration = 3f;
	private Rigidbody2D _rigidbody;

    private float _currentHealth;

    public EnemyManager EnemyManager;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _currentHealth = _enemyData.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void FixedUpdate()
	{
        Vector2 movementVector = getMovementVector(Camera.main.transform.position);
        Vector2 newVelocity = _rigidbody.velocity;
        newVelocity += movementVector * _acceleration * Time.deltaTime; // multiply it by the acceleration, accounting for time
        _rigidbody.velocity = Vector2.ClampMagnitude(newVelocity, _enemyData.MaxSpeed);
	}

    /// <summary>
    /// Finds out which way we need to go
    /// </summary>
    /// <param name="targetPosition">Where we want to go</param>
    /// <returns>A vector pointing in the direction we want to go</returns>
    private Vector2 getMovementVector(Vector2 targetPosition)
	{
		Vector2 movementVector = targetPosition - (Vector2)transform.position;
		movementVector = movementVector.normalized;
        return movementVector;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{

	}

    public void takeDamage(float amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0)
        {
            die();
        }
    }

    public void die()
    {
        gameObject.SetActive(false);
        EnemyManager.enemyObjectPool.Add(this);
    }

    public void setEnemyData(EnemyData data)
    {
        this._enemyData = data;
    }

}
