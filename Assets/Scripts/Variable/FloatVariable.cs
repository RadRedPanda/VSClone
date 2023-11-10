using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{
	public float Value;

	public void SetValue(float value)
	{
		Value = value;
	}

	public void SetValue(FloatVariable value)
	{
		SetValue(value.Value);
	}

	public void ApplyChange(float amount)
	{
		Value += amount;
	}

	public void ApplyChange(FloatVariable amount)
	{
		ApplyChange(amount.Value);
	}
}
