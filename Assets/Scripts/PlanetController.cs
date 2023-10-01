using System;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public int planetID;
    private Animator animator;
    [SerializeField] private GameObject glow;
    [SerializeField] private Color hoverColor = Color.gray;
    [SerializeField] private Color fromColor = Color.blue;
    [SerializeField] private Color toColor = Color.green;

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
        var mat = glow.GetComponent<Renderer>()?.material;
        switch (glowType)
        {
            case GlowType.Hover:
                mat?.SetColor("_BaseColor", hoverColor);
                break;
            case GlowType.From:
                mat?.SetColor("_BaseColor", fromColor);
                break;
            case GlowType.To:
                mat?.SetColor("_BaseColor", toColor);
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