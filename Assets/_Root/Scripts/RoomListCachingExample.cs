using Photon.Realtime;
using System.Collections.Generic;

public class RoomListCachingExample : ILobbyCallbacks, IConnectionCallbacks

{
    private TypedLobby customLobby = new TypedLobby("customLobby", LobbyType.Default);
    private LoadBalancingClient loadBalancingClient;
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    public void JoinLobby()
    {
        loadBalancingClient.OpJoinLobby(customLobby);
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }
    }

    void ILobbyCallbacks.OnJoinedLobby()
    {
        cachedRoomList.Clear();
    }

    void ILobbyCallbacks.OnLeftLobby()
    {
        cachedRoomList.Clear();
    }
    void ILobbyCallbacks.OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // here you get the response, empty list if no rooms found
        UpdateCachedRoomList(roomList);
    }

    void IConnectionCallbacks.OnDisconnected(DisconnectCause cause)
    {
        cachedRoomList.Clear();
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnConnectedToMaster()
    {
        throw new System.NotImplementedException();
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        throw new System.NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        throw new System.NotImplementedException();
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        throw new System.NotImplementedException();
    }
}
