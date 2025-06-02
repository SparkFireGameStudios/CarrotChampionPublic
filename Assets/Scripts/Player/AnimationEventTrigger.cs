using System;
using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{
    public PlayerControl playerControl;

    private void Awake()
    {
        if (playerControl == null)
        {
            playerControl = GetComponentInParent<PlayerControl>();
            if (playerControl == null)
            {
                Debug.LogError("PlayerControl component not found on " + gameObject.name);
            }
        }
    }

    #region Animation Events

    public void JumpAnimationEvent()
    {
        playerControl?.JumpAnimationEvent();
    }

    // FinishJumpAnimationEvent 
    private void FinishJumpAnimationEvent()
    {
        playerControl?.FinishJumpAnimationEvent();
    }

    #endregion
}
