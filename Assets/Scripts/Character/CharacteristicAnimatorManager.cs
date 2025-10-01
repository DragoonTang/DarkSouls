using UnityEngine;

/// <summary>
/// 🐉
/// </summary>
public class CharacteristicAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void UpdateAnimatorMovement(float horizontalValue, float verticalValue)
    {
        character.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
        character.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetAnimation(string targetAnim, bool isPerformingAction, bool applyRootMotion = false, bool canRotate = false, bool canMove = false)
    {
        character.animator.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnim, 0.2f);

        // 可以用来停止角色产生新的动作
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;
    }
}
