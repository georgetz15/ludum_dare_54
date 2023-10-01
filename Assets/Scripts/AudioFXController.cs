using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFXController : MonoBehaviour
{
	[SerializeField] private AudioSource audioPlayer;

	[SerializeField] private AudioClip taskCancelSFX;
	[SerializeField] private AudioClip taskCompleteSFX;

	public void PlayTaskComplete()
	{
		audioPlayer.clip = taskCompleteSFX;
		audioPlayer.Play();
	}

	public void PlayTaskCancel()
	{
		audioPlayer.clip = taskCancelSFX;
		audioPlayer.Play();
	}
}
