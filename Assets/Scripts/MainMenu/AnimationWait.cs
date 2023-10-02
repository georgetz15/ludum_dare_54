using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationWait : MonoBehaviour
{
	public Animator animator;
	public string triggerName = "PlayAnimation"; // Replace with your trigger parameter name

	private bool isAnimating = false;

	void Start()
	{
		// Trigger the animation by default
		animator.SetTrigger(triggerName);
		isAnimating = true;
	}

	void Update()
	{
		if (!isAnimating)
		{
			// Generate a random delay time
			float randomDelay = Random.Range(1.0f, 3.0f); // Adjust the range as needed

			StartCoroutine(RestartAnimationAfterDelay(randomDelay));
		}
	}

	IEnumerator RestartAnimationAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);

		// Restart the animation by triggering the parameter
		animator.SetTrigger(triggerName);

		isAnimating = true;
	}

	void RestartAnimation()
	{
		// Restart the animation by triggering the parameter
		animator.SetTrigger(triggerName);
	}
}
