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

	[Header("Projectile properties")]
	public float BulletSpeed = 7;


    private Rigidbody2D _rigidbody;
    public GameObject BulletPrefab;

	private List<GameObject> bulletObjectPool = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

	private void Update()
	{
		if(Input.GetButtonDown("Fire1"))
		{
			GameObject bullet;
			if (bulletObjectPool.Count > 0)
			{
				bullet = bulletObjectPool[0];
				bullet.SetActive(true);
				bulletObjectPool.RemoveAt(0);
			}
			else
			{
				bullet = Instantiate(BulletPrefab);
				Bullet bulletScript = bullet.GetComponent<Bullet>();
				bulletScript.bulletObjectPool = bulletObjectPool;
			}
			bullet.transform.position = transform.position;

			Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
			Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 velocityVector = targetPosition - (Vector2)transform.position;
			bulletRB.velocity = velocityVector.normalized * BulletSpeed;
		}
	}

	void FixedUpdate()
	{
		Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		targetVelocity = Vector2.ClampMagnitude(targetVelocity, 1f) * _maxSpeed;
		_rigidbody.velocity = Vector2.MoveTowards(_rigidbody.velocity, targetVelocity, _accleration * Time.deltaTime);
	}

	private void Awake()
	{
		
	}
}
