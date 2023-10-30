using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed;
	public float BulletSpeed = 7;
    private Rigidbody2D rigidBody;
    public GameObject BulletPrefab;

	private List<GameObject> bulletObjectPool = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
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

	private void FixedUpdate()
	{
        rigidBody.velocity = new Vector2(Input.GetAxis("Horizontal") * Speed, rigidBody.velocity.y);
	}

	private void Awake()
	{
		
	}
}
