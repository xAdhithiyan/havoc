using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimeLineGameManager : MonoBehaviour
{
	public PlayableDirector playableDirector;
	public TimelineAsset timelineAsset;
	public GameObject enemyObject;

	private void Start()
	{
		playableDirector.playableAsset = timelineAsset;

		foreach (var track in timelineAsset.GetOutputTracks())
		{
			if (track is AnimationTrack animationTrack)
			{
				Debug.Log(track);
				playableDirector.SetGenericBinding(animationTrack, enemyObject);
			}
		}
		playableDirector.Resume();
	}
}
