using Assets.Scripts.Parameters;
using UnityEngine;

namespace Assets.Scripts.NPCs
{
    public class DefaultEnemyAnimationController : NPCAnimationController
    {
        readonly int _walkingParameter;
        readonly int _punchParameter;
        readonly int _getHitParameter;
        readonly int _dieParameter;

        public DefaultEnemyAnimationController(Animator animator, NPCSignalBus signalBus) : base(animator, signalBus)
        {
            _walkingParameter = Animator.StringToHash(NameConstants.AnimatorParametersNames.IsWalkingParameter);
            _punchParameter = Animator.StringToHash(NameConstants.AnimatorParametersNames.PunchParameter);
            _getHitParameter = Animator.StringToHash(NameConstants.AnimatorParametersNames.GetHitParameter);
            _dieParameter = Animator.StringToHash(NameConstants.AnimatorParametersNames.DieParameter);

            _signalBus.Walk += Walk;
            _signalBus.Stop += Stop;
            _signalBus.Attack += Punch;
            _signalBus.GetHit += GetHit;
            _signalBus.Die += Die;
        }

        public void Walk() => _animator.SetBool(_walkingParameter, true);
        public void Stop() => _animator.SetBool(_walkingParameter, false);
        public void Punch() => _animator.SetTrigger(_punchParameter);
        public void GetHit(float _) => _animator.SetTrigger(_getHitParameter);
        public void Die() => _animator.SetBool(_dieParameter, true );

        public override void Dispose()
        {
            _signalBus.Walk -= Walk;
            _signalBus.Stop -= Stop;
            _signalBus.Attack -= Punch;
            _signalBus.GetHit -= GetHit;
            _signalBus.Die -= Die;
        }
    }
}
