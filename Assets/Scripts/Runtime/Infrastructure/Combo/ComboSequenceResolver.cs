using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.StateMachine;

namespace Runtime.Infrastructure.Combo
{
    public sealed class ComboSequenceResolver
    {
        public ComboSequenceResolver(IGameStateMachine gameStateMachine, ComboView.Pool pool, ScoreEffect.Pool scoreEffectPool)
        {
            foreach (ComboView comboView in pool.InactiveItems)
            {
                comboView.Initialize(gameStateMachine);
            }
            
            foreach (ScoreEffect scoreEffect in scoreEffectPool.InactiveItems)
            {
                scoreEffect.Initialize(gameStateMachine);
            }
        }
    }
}