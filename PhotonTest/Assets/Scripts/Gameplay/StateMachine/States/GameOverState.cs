using Networking.Room;

namespace Gameplay.StateMachine.States
{
    public class GameOverState : IState
    {
        private readonly IRoomExitService _roomExitService;

        public GameOverState(IRoomExitService roomExitService)
        {
            _roomExitService = roomExitService;
        }

        public void Enter()
        {
            _roomExitService.ExitToLobby();
        }

        public void Exit()
        {
        }
    }
}
