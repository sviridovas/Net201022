using Photon.Realtime;
using System.Collections.Generic;

public class GetCustomRoomListExample : ILobbyCallbacks
{
    private TypedLobby sqlLobby = new TypedLobby("customSqlLobby", LobbyType.SqlLobby);

    private LoadBalancingClient loadBalancingClient;

    public void GetCustomRoomList(string sqlLobbyFilter)
    {
        loadBalancingClient.OpGetGameList(sqlLobby, sqlLobbyFilter);
    }

    void ILobbyCallbacks.OnJoinedLobby()
    {
        throw new System.NotImplementedException();
    }

    void ILobbyCallbacks.OnLeftLobby()
    {
        throw new System.NotImplementedException();
    }

    void ILobbyCallbacks.OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        throw new System.NotImplementedException();
    }

    // do not forget to register callbacks via loadBalancingClient.AddCallbackTarget
    // also deregister via loadBalancingClient.RemoveCallbackTarget
    void ILobbyCallbacks.OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // here you get the response, empty list if no rooms found
    }
}

