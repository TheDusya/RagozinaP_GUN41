using Assets.Scripts.Parameters;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class DefaultEnemyAnimationController : NPCAnimationController
    {
        AnimatorControllerParameter _walkingParameter;
        AnimatorControllerParameter _punchParameter;
        AnimatorControllerParameter _getHitParameter;
        AnimatorControllerParameter _dieParameter;
        AnimationClip _deathAnimation;

        public DefaultEnemyAnimationController(Animator animator, NPCSignalBus signalBus) : base(animator, signalBus)
        {
            _walkingParameter = animator.GetParameter(Animator.StringToHash(NameConstants.AnimatorParametersNames.IsWalkingParameter));
            _punchParameter = animator.GetParameter(Animator.StringToHash(NameConstants.AnimatorParametersNames.PunchParameter));
            _getHitParameter = animator.GetParameter(Animator.StringToHash(NameConstants.AnimatorParametersNames.GetHitParameter));
            _dieParameter = animator.GetParameter(Animator.StringToHash(NameConstants.AnimatorParametersNames.DieParameter));

            //
        }

        public override void Dispose()
        {
        }
    }
}
