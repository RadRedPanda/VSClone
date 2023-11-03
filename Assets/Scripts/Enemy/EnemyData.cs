using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObjects/Enemy")]
public class EnemyData : ScriptableObject
{
	public Sprite EnemySprite;
	[Min(0)]
	public float MaxHealth = 5f;
	[Min(0)]
	public float MaxSpeed;
}