namespace Gameplay.StateMachine.Factory
{
    public interface IStateFactory
    {
        T CreateState<T>() where T : IState;
    }
}