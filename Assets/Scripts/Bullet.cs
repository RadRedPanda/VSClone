using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
	public List<GameObject> bulletObjectPool;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		gameObject.SetActive(false);
		bulletObjectPool.Add(gameObject);
	}
}