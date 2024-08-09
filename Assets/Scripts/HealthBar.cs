using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField] private Slider slider;
	[SerializeField] private Gradient _gradient;
	[SerializeField] Image fill;

	public void setMaxHealth(float maxHealth)
	{
		slider.maxValue = maxHealth;
		slider.value = maxHealth;

		fill.color = _gradient.Evaluate(1f);
	}

	public void setHealth(float health)
	{
		slider.value = health;

		fill.color = _gradient.Evaluate(slider.normalizedValue);
	}
}
