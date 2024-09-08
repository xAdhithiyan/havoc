using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Playables;

// timeline and dialoge stuff lol
[System.Serializable]
public class dialogeWithTime
{
	public float time;
	public Dialoge dialoge;
}

public class TimeLineManager : MonoBehaviour
{
	public dialogeWithTime[] dialoges;
	private Queue<dialogeWithTime> dialogesQueue;
	public PlayableDirector playableDirector;

	private bool coroutinePLaying = false;
	private bool checkResuming = false;

	private void Awake()
	{
		dialogesQueue = new Queue<dialogeWithTime>();
		for(int i = 0; i < dialoges.Length; i++)
		{
			dialogesQueue.Enqueue(dialoges[i]);
		}
	}

	private void Update()
	{
		if(!FindObjectOfType<DialogeManager>().dialogeActive && checkResuming) {
			checkRestarting();
		}	
		if (dialogesQueue.Count > 0 && !coroutinePLaying)
		{
			StopAllCoroutines();
			StartCoroutine(StartDialagoeTrack());
		}
	}

	private IEnumerator StartDialagoeTrack()
	{
		coroutinePLaying = true;
		dialogeWithTime currentDialogeTrack = dialogesQueue.Dequeue();
		yield return new WaitForSeconds(currentDialogeTrack.time);
		playableDirector.playableGraph.GetRootPlayable(0).Pause();
		FindAnyObjectByType<DialogeManager>().startDialoge(currentDialogeTrack.dialoge, false);
		checkResuming = true;
	}		

	private void checkRestarting()
	{
		if(!FindObjectOfType<DialogeManager>().dialogeActive)
		{
			Debug.Log("asjda");
			playableDirector.playableGraph.GetRootPlayable(0).Play();
			coroutinePLaying = false;
			checkResuming = false;
		}
	}
}