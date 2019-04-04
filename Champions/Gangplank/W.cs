using System.Numerics;
using System.Linq;

using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System;

namespace Spells
{
    public class RemoveScurvy : IGameScript
    {

        int[] _healthValues = { 80, 150, 220, 290, 360 };
        

        public void OnActivate(IChampion owner)
        {

           
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnUpdate(double diff)
        {

        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            AddParticleTarget(owner, "pirate_removeScurvy_citrus.troy", owner);

        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
           
            var healthGain = _healthValues[owner.Stats.Level - 1] + owner.Stats.AbilityPower.Total;
            var newHealth = target.Stats.CurrentHealth + healthGain;
            ((ObjAiBase)owner).ClearAllCrowdControl();
            target.Stats.CurrentHealth = Math.Min(newHealth, target.Stats.HealthPoints.Total);

            AddParticleTarget(owner, "pirate_removeScurvy_heal.troy", owner);
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
          
        }
    }
}
