using UnityEngine;

public class AnimatorResetBoolAtEnd : StateMachineBehaviour {
 
    [SerializeField]
    private string booleanVariableName;
 
    public event AnimationEnd OnAnimationEnd;
    public delegate void AnimationEnd();
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(booleanVariableName, false);
        OnAnimationEnd?.Invoke();
    }
        
}