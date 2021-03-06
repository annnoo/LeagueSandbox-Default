﻿using System.Numerics;
using System.Linq;

using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace Spells
{
    public class RaiseMorale : IGameScript
    {


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
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            //spell.SpellAnimation("Spell3", owner);
            var p1 = AddParticleTarget(owner, "pirate_raiseMorale_cas.troy", owner);
            var p101 = AddParticleTarget(owner, "PirateCutlass_trail.troy", owner);
            //spell.AddProjectileTarget("pirate_raiseMorale_mis.troy", target);
            var p2 = AddParticleTarget(owner, "pirate_raiseMorale_tar.troy", owner);
            var p3 = AddParticleTarget(owner, "pirate_raiseMorale_tar.troy", target);
            CreateTimer(7.0f, () =>
            {
                RemoveParticle(p1);
                RemoveParticle(p3);
            }
            );
            ((ObjAiBase)owner).RemoveBuffGameScriptsWithName("GangplankEBuff", "RaiseMoraleBuffPassive");
            owner.AddBuffGameScript("GangplankEBuff", "RaiseMoraleBuff", spell, 7, true);
            foreach  (var champ in GetChampionsInRange(owner,500,true).Where(c => c.Team == owner.Team && c != owner))
            {
                owner.AddBuffGameScript("GangplankEBuff", "RaiseMoraleBuffTeam", spell, 3.5f, true);
            }
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
          
        }
    }
}
