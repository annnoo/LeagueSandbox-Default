using GameServerCore.Enums;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;


namespace GangplankEBuff
{
    internal class RaiseMoraleBuffTeam : IBuffGameScript
    {
       
        private IStatsModifier _statsMod;

        private IBuff _buff;

        public void OnActivate(IObjAiBase unit, ISpell ownerSpell)
        {
          
            _buff = AddBuffHudVisual("RaiseMoraleTeamBuff", 7f, 1, BuffType.COMBAT_ENCHANCER, unit);
            _statsMod = new StatsModifier();
            _statsMod.AttackDamage.FlatBonus = 2.5f + 3.5f * ownerSpell.Level;
            _statsMod.MoveSpeed.PercentBonus = 0.025f + 0.015f * ownerSpell.Level;
            unit.AddStatModifier(_statsMod);
           
        }

        public void OnDeactivate(IObjAiBase unit)
        {
            RemoveBuffHudVisual(_buff);
            unit.RemoveStatModifier(_statsMod);
        }

        public void OnUpdate(double diff)
        {

        }
    }
}
