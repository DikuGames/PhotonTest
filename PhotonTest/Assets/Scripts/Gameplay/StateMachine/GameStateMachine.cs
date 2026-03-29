using System.Collections.Generic;
using System.Linq;
using Gameplay.StateMachine.Factory;
using Gameplay.StateMachine.States;
using Zenject;

namespace Gameplay.StateMachine
{
    public class GameStateMachine : IStateSwitcher, IInitializable
    {
        private readonly IStateFactory _stateFactory;

        private List<IState> _states;
        private IState _currentState;

        public GameStateMachine(IStateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }

        public void Initialize()
        {
            _states = new List<IState>
            {
                _stateFactory.CreateState<PrepareGameState>(),
                _stateFactory.CreateState<GameState>(),
                _stateFactory.CreateState<GameOverState>()
            };

            _currentState = _states[0];
            _currentState.Enter();
        }

        public void SwitchState<State>() where State : IState
        {
            var state = _states.FirstOrDefault(currentState => currentState is State);

            if (state == null || ReferenceEquals(_currentState, state))
            {
                return;
            }

            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }
    }
}
