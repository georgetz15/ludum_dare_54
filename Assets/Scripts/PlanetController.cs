using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
	public int planetID;
	private Animator animator;


	// Start is called before the first frame update
	void Start()
    {
		animator = GetComponent<Animator>();
	}

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	public void Initialize(float animSpeed, float animOffset)
	{
		// Set animation parameters
		animator.speed = animSpeed;
		animator.SetFloat("AnimatorCycleOffset", animOffset);
	}
}
