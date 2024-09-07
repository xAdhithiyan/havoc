// script is not being used.

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GrappleHolder : MonoBehaviour
{
	[SerializeField] private GameObject[] _grappleHolders;
	[SerializeField] private Transform _playerPosition;

	[Header("Scripts")]
	[SerializeField] private Player _player;

	
	private void Update()
	{	 
		drawGrapple();
	}

	public void drawGrapple()
	{
		// gives the 6 planes of the camera 
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

		foreach (GameObject grappleHolder in _grappleHolders)
		{
			LineRenderer lineRenderer = grappleHolder.GetComponent<LineRenderer>();

			// creates a bound of type AABB (Axis-alligined Bounding Box). new Bounds() takes center, size arguments for creating the bound
			Bounds bound = new Bounds(grappleHolder.transform.position, Vector3.zero);
			// checks if the created bounds are intersecting the frustum plane 
			if (GeometryUtility.TestPlanesAABB(planes, bound) && Input.GetKey(KeyCode.Mouse1))
			{
				if (!lineRenderer.enabled)
				{
					lineRenderer.enabled = true;
				}

				lineRenderer.SetPosition(0, _playerPosition.transform.position);
				lineRenderer.SetPosition(1, grappleHolder.transform.position);

				_player.addForceForGrapple(grappleHolder.transform);
			}

			else
			{
				if (lineRenderer.enabled)
				{
					lineRenderer.enabled = false;
				}
			}
		}
	}
}
