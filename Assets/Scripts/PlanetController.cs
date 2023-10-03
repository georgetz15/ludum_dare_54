using System;
using TMPro;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public int planetID;
    private Animator animator;
    [SerializeField] private GameObject glow;
    private readonly Color _hoverColor = UIPalette.PlanetHoverColor;
    private readonly Color _fromColor = UIPalette.PlanetOriginColor;
    private readonly Color _toColor = UIPalette.PlanetDestinationColor;

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
                spriteRenderer.color = _hoverColor;
                break;
            case GlowType.From:
                spriteRenderer.color = _fromColor;
                break;
            case GlowType.To:
                spriteRenderer.color = _toColor;
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