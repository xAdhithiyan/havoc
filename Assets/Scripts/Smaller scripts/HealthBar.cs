using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField] private Slider slider;
	[SerializeField] private Gradient _gradient;
	[SerializeField] Image fill;

	[SerializeField] private Slider dashSlider;
	[SerializeField] private Slider shurikenSlider;

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

	public void setMaxDashValue(float maxHealth)
	{
		dashSlider.maxValue = maxHealth;
		dashSlider.value = maxHealth;
	}

	public void setDashValue(float health)
	{
		dashSlider.value = health;
	}

	public void setMaxShurikenValue(float maxHealth)
	{
		shurikenSlider.maxValue = maxHealth;
		shurikenSlider.value = maxHealth;
	}

	public void setShurikenValue(float health)
	{
		shurikenSlider.value = health;
	}
}
