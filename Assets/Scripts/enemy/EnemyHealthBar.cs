using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
	[SerializeField] private Slider slider;

	public void setEnemyMaxHealth(float maxHealth)
	{
		slider.maxValue = maxHealth;
		slider.value = maxHealth;
	}

	public void setEnemyHealth(float health)
	{
		slider.value = health;
	}
}
