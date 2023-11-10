using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField]
	private Slider _healthBarSlider;
	[SerializeField]
	private FloatReference _currentHealth;
	[SerializeField]
	private FloatReference _maxHealth;

	void OnValidate()
	{
		UpdateMaxHealth(_maxHealth.Value);
		UpdateHealth(_currentHealth.Value);
	}

	void Update()
	{
		UpdateMaxHealth(_maxHealth.Value);
		UpdateHealth(_currentHealth.Value);
	}

	public void UpdateHealth(float newHealth)
	{
		_healthBarSlider.value = newHealth;
	}

	public void UpdateMaxHealth(float newHealth)
	{
		_healthBarSlider.maxValue = newHealth;
	}
}
