using Netologia.Behaviours;
using Netologia.TowerDefence;
using Netologia.TowerDefence.Behaviors;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Netologia.Systems
{
	public class ProjectileSystem : GameObjectPoolContainer<Projectile>, Director.IManualUpdate
	{
		private EffectSystem _effects;		//injected
		
		[SerializeField, Min(0.01f)]
		private float _hitDistance = 0.3f;
		
		public void ManualUpdate()
        {
			if (_pools == null || _pools.Count == 0)
				return;
            foreach (var pool in this.ToList())
                foreach (var projectile in pool.ToList())
				{
					projectile.transform.position = Vector3.MoveTowards(projectile.transform.position,
																		projectile.TargetPosition,
																		Time.deltaTime * projectile.MoveSpeed);
					if (Vector3.SqrMagnitude(projectile.transform.position - projectile.TargetPosition) <= _hitDistance * _hitDistance)
					{
						projectile.Hit();
						pool.ReturnElement(projectile);
					}
				}

        }

		public void OnDespawnUnit(int unitID)
		{
			foreach (var pool in this)
				foreach (var projectile in pool)
					if(projectile.TargetID == unitID)
						projectile.ResetTarget();
		}

		[Inject]
		private void Construct(EffectSystem effects)
		{
			(_effects) = (effects);
			//SqrtMagnitude optimization
			_hitDistance *= _hitDistance;
		}
	}
}