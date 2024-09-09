using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class TargetIndicator : MonoBehaviour
{
	public Transform[] Target;
	private int count = 0;

	private void Update()
	{
		ShowArrow();
	}

	private void ShowArrow()
	{

		Vector3 dir = Target[count].position - transform.position;

		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
	}

	public void StopArrow()
	{
		gameObject.SetActive(false);
		count++;
	}
}
