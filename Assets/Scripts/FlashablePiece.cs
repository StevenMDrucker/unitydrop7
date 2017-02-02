using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FlashablePiece : MonoBehaviour
{

    public AnimationClip flashAnimation;


    private bool isBeingFlashed = false;
    public bool IsBeingFlashed
    {
        get { return isBeingFlashed; }
    }

    public AnimationClip highlightAnimation;
    public AnimationClip unHighlightAnimation;

    private bool isBeingHighlighted = false;
    public bool IsBeingHighlighted
    {
        get { return isBeingHighlighted; }
    }

    private bool isBeingUnHighlighted = false;
    public bool IsBeingUnHighlighted
    {
        get { return isBeingUnHighlighted; }
    }



    protected GamePiece piece;

    private void Awake()
    {
        piece = GetComponent<GamePiece>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Flash()
    {
        isBeingFlashed = true;
        StartCoroutine(FlashCoroutine());
    }
    private IEnumerator FlashCoroutine()
    {
        Animator animator = GetComponent<Animator>();
        if (animator)
        {
            animator.Play(flashAnimation.name);
            yield return new WaitForSeconds(flashAnimation.length);
            isBeingFlashed = false;
        }
    }


    public void Highlight()
    {
        isBeingHighlighted = true;
        StartCoroutine(HighlightCoroutine());
    }
    private IEnumerator HighlightCoroutine()
    {
        Animator animator = GetComponent<Animator>();
        if (animator)
        {
            animator.Play(highlightAnimation.name);
            yield return new WaitForSeconds(highlightAnimation.length);
            isBeingHighlighted = false;
        }
    }

    public void UnHighlight()
    {
        isBeingUnHighlighted = true;
        StartCoroutine(UnHighlightCoroutine());
    }
    private IEnumerator UnHighlightCoroutine()
    {
        Animator animator = GetComponent<Animator>();
        if (animator)
        {
            animator.Play(unHighlightAnimation.name);
            yield return new WaitForSeconds(unHighlightAnimation.length);
            isBeingHighlighted = false;
        }
    }

}
