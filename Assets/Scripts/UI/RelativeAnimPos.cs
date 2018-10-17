using UnityEngine;

public class RelativeAnimPos : StateMachineBehaviour
{
   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.parent.gameObject.transform.position = animator.transform.position;
    }
}