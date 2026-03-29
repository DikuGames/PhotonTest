using Gameplay.Match;

namespace Gameplay.StateMachine.States
{
    public class GameState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;
        private readonly MatchFinishService _matchFinishService;

        public GameState(IStateSwitcher stateSwitcher, MatchFinishService matchFinishService)
        {
            _stateSwitcher = stateSwitcher;
            _matchFinishService = matchFinishService;
        }

        public void Enter()
        {
            _matchFinishService.Finished += OnFinished;
        }

        public void Exit()
        {
            _matchFinishService.Finished -= OnFinished;
        }

        private void OnFinished()
        {
            _stateSwitcher.SwitchState<GameOverState>();
        }
    }
}
