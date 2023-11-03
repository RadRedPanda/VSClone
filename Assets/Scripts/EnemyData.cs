using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObjects/Enemy")]
public class EnemyData : ScriptableObject
{
	public Sprite EnemySprite;
	public float MaxHealth;
	public float MaxSpeed;
}