using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Tooltip("How fast we want them to move")]
    private float _maxSpeed = 3f;
    [SerializeField, Tooltip("How fast we want them to start moving")]
	private float _accleration = 3f;
	private Rigidbody2D _rigidbody;
    [SerializeField]
    private Transform _playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void FixedUpdate()
	{
        Vector2 movementVector = getMovementVector(_playerTransform.position);
        Vector2 newVelocity = _rigidbody.velocity;
        newVelocity += movementVector * _accleration * Time.deltaTime; // multiply it by the acceleration, accounting for time
        _rigidbody.velocity = Vector2.ClampMagnitude(newVelocity, _maxSpeed);
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
}
