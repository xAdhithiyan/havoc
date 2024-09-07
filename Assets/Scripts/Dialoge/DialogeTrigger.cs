using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogeTrigger : MonoBehaviour
{
	public Dialoge dialoge;

	// changing for this script
	public Dialoge dialoge2;

	[SerializeField] private float triggerRadius;
	[SerializeField] private LayerMask playerLayerMask;
	[SerializeField] private Animator eButtonAnimtor;

	private bool checkForPlayer = true;
	private bool runSecondScript = true;

	public void Update()
	{
		trigger(dialoge);
		if (!FindObjectOfType<DialogeManager>().dialogeActive && runSecondScript)
		{
			//StopAllCoroutines();
			StartCoroutine(WaitAndTriiggerNextDialoge());	
		}
	}

	private void trigger(Dialoge dialoge)
	{
		if (checkForPlayer)
		{
			Collider2D player = Physics2D.OverlapCircle(transform.position, triggerRadius, playerLayerMask);
			if (player != null)
			{
				eButtonAnimtor.SetBool("isOpen", true);
			}
			else
			{
				eButtonAnimtor.SetBool("isOpen", false);
			}

			if (player != null && Input.GetKeyDown(KeyCode.E))
			{
				FindObjectOfType<DialogeManager>().startDialoge(dialoge, false);
				checkForPlayer = false;
			}
		}
	}

	private IEnumerator WaitAndTriiggerNextDialoge()
	{
		runSecondScript = false;
		yield return new WaitForSeconds(4f);
		FindObjectOfType<DialogeManager>().startDialoge(dialoge2, true);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, triggerRadius);
	}
}