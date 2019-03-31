using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class ThreshQ : IGameScript
    {
        public static IAttackableUnit targets;

        public void OnActivate(IChampion owner)
        {
            
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            spell.SpellAnimation("Spell1_IN", owner);
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var current = new Vector2(owner.X, owner.Y);
            var to = Vector2.Normalize(new Vector2(spell.X, spell.Y) - current);
            var range = to * 1100;
            var trueCoords = current + range;
            CreateTimer(0.535f, () =>
             {
                 spell.AddProjectile("ThreshQMissile", owner.X, owner.Y, trueCoords.X, trueCoords.Y);
             });
            
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            owner.SetSpell("ThreshQLeap", 0, true);
            var ap = owner.Stats.AbilityPower.Total;
            var damage = 40 + spell.Level * 40f + ap * 0.5f;
            var p1 = AddParticleTarget(owner, "Thresh_Q_whip_beam.troy", target);
            AddParticleTarget(owner, "Thresh_Q_Pull_Sound.troy", target);
            AddParticleTarget(owner, "Thresh_Q_stab_tar.troy", target);
            //AddParticleTarget(owner, "ZiggsR_Nova.troy", target,2);
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            ((ObjAiBase)target).AddBuffGameScript("Stun", "Stun", spell, 1.5f, true);
            projectile.SetToRemove();
            targets = target;
            for (float a = 0.0f; a <= 1.5f; a += 0.8f)
            {
                CreateTimer(a, () =>
                 {
                     Pull(owner, target);
                 });
            }            

            
            CreateTimer(1.5f, () =>
             {
                 if (owner.Spells[0].SpellName == "ThreshQLeap")
                 {
                     owner.SetSpell("ThreshQ", 0, true);
                 }
                 CancelDash((ObjAiBase)target);
                 RemoveParticle(p1);
             });

        }

        private void Pull(IChampion owner, IAttackableUnit target)
        {
            var current = new Vector2(target.X, target.Y);
            var to = Vector2.Normalize(new Vector2(owner.X, owner.Y) - current);
            var range = to * 150;
            var trueCoords = current + range;

            AddParticleTarget(owner, "Thresh_Q_Voice_Special_sound.troy", owner);
            DashToLocation((ObjAiBase)target, trueCoords.X, trueCoords.Y, 300, false);
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
