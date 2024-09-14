using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class HoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public TextMeshProUGUI buttonText;
	public Color defaultColor = Color.white;
	public Color hoverColor;

	public void OnPointerEnter(PointerEventData eventData)
	{
		buttonText.color = hoverColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		buttonText.color = defaultColor;
	}

}
