using GameServerCore.Enums;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;


namespace GangplankEBuff
{
    internal class RaiseMoraleBuffPassive : IBuffGameScript
    {
       
        private IStatsModifier _statsMod;

        private IBuff _buff;

        public void OnActivate(IObjAiBase unit, ISpell ownerSpell)
        {
          
            _statsMod = new StatsModifier();
            _statsMod.AttackDamage.FlatBonus = 6 + 2 * ownerSpell.Level;
            _statsMod.MoveSpeed.PercentBonus = 0.03f + 0.01f * ownerSpell.Level;
            unit.AddStatModifier(_statsMod);
            
           
        }

        public void OnDeactivate(IObjAiBase unit)
        {
        
            unit.RemoveStatModifier(_statsMod);
            
        }

        public void OnUpdate(double diff)
        {

        }
    }
}
