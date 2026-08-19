using Netologia.Behaviours;
using Netologia.TowerDefence;
using Netologia.TowerDefence.Behaviors;
using UnityEngine;
using Zenject;

namespace Netologia.Systems
{
	public class TowerSystem : GameObjectPoolContainer<Tower>, Director.IManualUpdate
	{
		private UnitSystem _units;				//injected
		private ProjectileSystem _projectiles;	//injected

		public void ManualUpdate()
		{
			foreach (var pool in this)
				foreach (var tower in pool)
				{
					if (!tower.DecrementAttackReload(Time.deltaTime)) //on reload
						return;
					if (!tower.HasTarget)
					{
						var unitFound = _units.FindTarget(tower.transform.position, tower.Range);
						if (unitFound is default(Unit)) //nothing found
							return;
						else 
							tower.Target = unitFound;
					}
					var newProjectile = _projectiles[tower.Projectile].Get;
                    newProjectile.PrepareData(transform.position, tower.Target, tower.Damage, tower.AttackElemental);
					tower.Attack();
				}
        }

		public void OnDespawnUnit(int unitID)
		{
			foreach (var pair in this)
				foreach (var tower in pair)
					if (tower.TargetID == unitID)
						tower.Target = null;
		}
		
		[Inject]
		private void Construct(UnitSystem units, ProjectileSystem projectiles)
		{
			_units = units;
			_projectiles = projectiles;
		}
	}
}