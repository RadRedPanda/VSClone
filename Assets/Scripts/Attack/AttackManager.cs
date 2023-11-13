using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
	[SerializeField]
	private Attack _attackPrefab;
    private static AttackManager _instance;
    private static Stack<Attack> _attackObjectPool;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Second Projectile Manager found!");
            Destroy(gameObject);
        }

        _instance = this;
        _attackObjectPool = new Stack<Attack>();
    }

    public static Attack GetFromPool(BaseAttackData data, Vector2 position)
	{
		Attack attack;

		if (_attackObjectPool.Count > 0)
			attack = _attackObjectPool.Pop();
		else
			attack = Instantiate(_instance._attackPrefab);

		attack._attackData = data;
		attack._attackData.ApplyCollider(attack.gameObject);
		attack.transform.position = position;
		attack.gameObject.SetActive(true);
		return attack;
	}
	
	/// <summary>
	/// Disables the gameObject and adds it to the object pool. Won't do anything if it's already disabled
	/// </summary>
	/// <param name="attack">The object we want to add to the pool</param>
	public static void ReturnToPool(Attack attack)
	{
		if (!attack.gameObject.activeSelf) // don't add to pool because it might be duplicate
			return;
		attack.gameObject.SetActive(false);
		_attackObjectPool.Push(attack);
	}
}
