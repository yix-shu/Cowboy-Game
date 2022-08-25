using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public static class AnimationController 
    {
        public static void trigger(GameObject go, string triggerName)
        {
            Animator animator = go.GetComponent<Animator>();
            animator.SetTrigger(triggerName);
        }
    }
}