using System;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public int planetID;
    private Animator animator;
    [SerializeField] private GameObject glow;
    [SerializeField] private Color hoverColor = Color.white;
    [SerializeField] private Color fromColor = new (217, 153, 93);
    [SerializeField] private Color toColor = new (93, 217, 124);

    public enum GlowType
    {
        Hover,
        From,
        To
    }

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
        ShowGlow(GlowType.Hover);
    }

    public void OnHoverExit()
    {
        HideGlow();
    }

    public void OnSelect()
    {
    }

    public void ShowGlow(GlowType glowType)
    {
        glow.SetActive(true);
        var spriteRenderer = glow.GetComponent<SpriteRenderer>();
        switch (glowType)
        {
            case GlowType.Hover:
                spriteRenderer.color = hoverColor;
                break;
            case GlowType.From:
                spriteRenderer.color = fromColor;
                break;
            case GlowType.To:
                spriteRenderer.color = toColor;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(glowType), glowType, null);
        }
    }

    public void HideGlow()
    {
        glow.SetActive(false);
    }
}