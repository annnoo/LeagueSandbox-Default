using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Collections.Generic;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.API;
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace Spells
{
    public class KatarinaQ : IGameScript
    {
        private Particle _mark;
        private IChampion _owningChampion;
        private ISpell _owningSpell;
        private List<IAttackableUnit> _markTarget;
        private bool _listenerAdded;

        public void OnActivate(IChampion owner)
        {
            _owningChampion = owner;
            _markTarget = new List<IAttackableUnit>();
            _owningSpell = null;
            _listenerAdded = false;
            _mark = null;
        }

        private void OnProc(IAttackableUnit target, bool isCrit)
        {
            if (_mark == null || _markTarget == null || !_markTarget.Contains(target))
            {
                return;
            }


            RemoveParticle(_mark);

            var damage = new[] { 15, 30, 45, 60, 75 }[_owningSpell.Level - 1] + _owningChampion.Stats.AbilityPower.Total * 0.15f;
            target.TakeDamage(_owningChampion, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PASSIVE, false);

            _mark = null;
            _markTarget.Remove(target);
        }

        public void OnDeactivate(IChampion owner)
        {

        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            _owningSpell = spell;

            spell.AddProjectileTarget("KatarinaQ", target);

            if (!_listenerAdded)
            {
                ApiEventManager.OnHitUnit.AddListener(this, _owningChampion, OnProc);
                _listenerAdded = true;
            }

            foreach (var enemyTarget in GetUnitsInRange(target, 625, true))
            {
                if (enemyTarget != null && enemyTarget.Team == CustomConvert.GetEnemyTeam(owner.Team) && enemyTarget != target && enemyTarget != owner && target.GetDistanceTo(enemyTarget) < 100 && !(enemyTarget is IBaseTurret))
                {
                    CreateTimer(3.0f, () => {
                        if (!(enemyTarget.IsDead))
                        {
                            AddParticle(owner, "katarina_bouncingBlades_mis.troy", enemyTarget.X, enemyTarget.Y);
                        }
                        spell.AddProjectileTarget("KatarinaQMark", enemyTarget);
                    });
                }
            }

            //WE NEED A TIMER ON THE Q TO BE FIXED
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var damage = new[] { 60, 85, 110, 135, 160 }[spell.Level - 1] + owner.Stats.AbilityPower.Total * 0.45f;

            foreach (var enemyTarget in GetUnitsInRange(target, 625, true))
            {
                if (enemyTarget != null && enemyTarget.Team == CustomConvert.GetEnemyTeam(owner.Team) && enemyTarget != owner && !(enemyTarget is IBaseTurret))
                {
                    enemyTarget.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

                    if (!enemyTarget.IsDead || !(enemyTarget is IChampion))
                    {
                        _markTarget.Add(enemyTarget);
                        _mark = AddParticleTarget(owner, "katarina_daggered.troy", enemyTarget);
                    }
                }
            }
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            projectile.SetToRemove();
            CreateTimer(6.0f, () =>
            {
                if (_mark == null)
                    return;
                RemoveParticle(_mark);
                _markTarget.Clear();
                _mark = null;
            });

        }

        public void OnUpdate(double diff)
        {
        }
    }
}
