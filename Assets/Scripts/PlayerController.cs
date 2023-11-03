using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Player properties")]
	[SerializeField, Tooltip("How fast we want them to move")]
	private float _maxSpeed = 3f;
	[SerializeField, Tooltip("How fast we want them to start moving")]
	private float _accleration = 3f;

    private Rigidbody2D _rigidbody;
	[SerializeField]
	private ProjectileData _projectile;
    public Projectile ProjectilePrefab;

	private List<Projectile> projectileObjectPool = new List<Projectile>();

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

	private void Update()
	{
		if (Input.GetButtonDown("Fire1"))
			fireProjectile();
	}

	void FixedUpdate()
	{
		Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		targetVelocity = Vector2.ClampMagnitude(targetVelocity, 1f) * _maxSpeed;
		_rigidbody.velocity = Vector2.MoveTowards(_rigidbody.velocity, targetVelocity, _accleration * Time.deltaTime);
	}

	private void fireProjectile()
	{
		Projectile bullet;
		if (projectileObjectPool.Count > 0)
		{
			bullet = projectileObjectPool[0];
			bullet.gameObject.SetActive(true);
			projectileObjectPool.RemoveAt(0);
		}
		else
		{
			bullet = Instantiate(ProjectilePrefab);
			bullet.bulletObjectPool = projectileObjectPool;
		}
		bullet.projectileData = _projectile;
		bullet.transform.position = transform.position;
		Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 velocityVector = targetPosition - (Vector2)transform.position;
		bullet.FireProjectile(velocityVector.normalized);
	}
}
