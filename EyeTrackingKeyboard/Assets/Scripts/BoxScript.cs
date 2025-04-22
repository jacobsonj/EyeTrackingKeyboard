using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
public class BoxScript : MonoBehaviour
{
    [Header("Opening Tween")]
    [Tooltip("The part of your box that actually swings open (e.g. the lid).")]
    public Transform lid;

    [Tooltip("Local Euler angles the lid will rotate *to* when opening.")]
    public Vector3 openEulerAngles = new Vector3(0, 90, 0);

    [Tooltip("How long (in seconds) the lid takes to tween open.")]
    public float openDuration = 2f;

    [Header("Post‑Tween Animation")]
    [Tooltip("(Optional) Animator with an 'Open' trigger to play after the lid has finished tweening.")]
    public Animator animator;

    // Remember the closed rotation so we can snap back on reset
    private Quaternion _initialLocalRotation;

    // Prevent double‑triggers
    private bool _isAnimating = false;

    void Awake()
    {
        if (lid != null)
            _initialLocalRotation = lid.localRotation;
    }

    /// <summary>
    /// Call this when your manager's raycast first starts hitting this box.
    /// </summary>
    public void AnimateOpen()
    {
        if (_isAnimating || lid == null) 
            return;

        _isAnimating = true;

        // Tween the local rotation of the lid to the target Euler angles
        lid.DOLocalRotate(openEulerAngles, openDuration)
           .SetEase(Ease.InOutSine)
           .OnComplete(() =>
           {
               _isAnimating = false;
               //attatch animator to show animal jump out or something
            //    if (animator != null)
            //        animator.SetTrigger("Open");
            Debug.Log("Box Open");
           });
    }

    /// <summary>
    /// Call this when your manager's raycast stops hitting this box (or you want to force‑reset it).
    /// </summary>
    public void ResetBox()
    {
        // Kill any running tween on the lid
        if (lid != null)
            lid.DOKill();

        // Snap lid back to its original (closed) rotation
        lid.localRotation = _initialLocalRotation;
        _isAnimating = false;

        // If you fired an Animator trigger, you may want to clear it
        if (animator != null)
        {
            animator.ResetTrigger("Open");
            // optionally, if you have a "Close" animation, you could:
            // animator.SetTrigger("Close");
        }
    }
}