using System.Threading.Tasks;

namespace Networking.Lobby
{
    public interface ILobbyNetwork
    {
        Task<bool> TryJoinRandomRoomAsync();
        Task CreateRoomAsync();
    }
}
