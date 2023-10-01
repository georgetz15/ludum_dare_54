using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public int planetID;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Initialize(float animSpeed, float animOffset)
    {
        // Set animation parameters
        animator.speed = animSpeed;
        animator.SetFloat("AnimatorCycleOffset", animOffset);
    }

    public void OnHoverEnter()
    {
    }

    public void OnHoverExit()
    {
    }

    public void OnSelect()
    {
        var spaceshipController = GameController.GetSpaceshipController();
        spaceshipController.GetComponent<SpaceShipController>().TravelTo(gameObject);
    }
}