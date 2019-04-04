using GameServerCore.Enums;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace GangplankEBuff
{
    internal class RaiseMoraleBuff : IBuffGameScript
    {
       
        private IStatsModifier _statsMod;

        private IBuff _buff;

        private ISpell _spell;

        public void OnActivate(IObjAiBase unit, ISpell ownerSpell)
        {
            _spell = ownerSpell;
            _buff = AddBuffHudVisual("RaiseMoraleTeamBuff", 7f, 1, BuffType.COMBAT_ENCHANCER, unit);
            _statsMod = new StatsModifier();
            _statsMod.AttackDamage.FlatBonus = 5 + 7 * ownerSpell.Level;
            _statsMod.MoveSpeed.PercentBonus = 0.05f + 0.03f * ownerSpell.Level;
            unit.AddStatModifier(_statsMod);
           
        }

        public void OnDeactivate(IObjAiBase unit)
        {
            RemoveBuffHudVisual(_buff);
            unit.RemoveStatModifier(_statsMod);
            unit.AddBuffGameScript("GangplankEBuff", "RaiseMoraleBuffPassive", _spell);
        }

        public void OnUpdate(double diff)
        {

        }
    }
}
