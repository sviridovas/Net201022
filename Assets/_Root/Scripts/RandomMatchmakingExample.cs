using Photon.Realtime;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RandomMatchmakingExample : IMatchmakingCallbacks
{
    public const string MAP_PROP_KEY = "map";
    private LoadBalancingClient loadBalancingClient;
    public void JoinRandomRoom(byte mapCode, byte expectedMaxPlayers)
    {
        Hashtable expectedCustomRoomProperties = new Hashtable { { MAP_PROP_KEY, mapCode } };
        OpJoinRandomRoomParams opJoinRandomRoomParams = new OpJoinRandomRoomParams();
        opJoinRandomRoomParams.ExpectedMaxPlayers = expectedMaxPlayers;
        opJoinRandomRoomParams.ExpectedCustomRoomProperties = expectedCustomRoomProperties;
        loadBalancingClient.OpJoinRandomRoom();
    }
    // do not forget to register callbacks via loadBalancingClient.AddCallbackTarget
    // also deregister via loadBalancingClient.RemoveCallbackTarget
    void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message)
    {
        // log error code and message
        // here usually you create a new room
    }
    void IMatchmakingCallbacks.OnJoinedRoom()
    {
        // joined a room successfully, OpJoinRandomRoom leads here on success
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        throw new System.NotImplementedException();
    }

    public void OnCreatedRoom()
    {
        throw new System.NotImplementedException();
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftRoom()
    {
        throw new System.NotImplementedException();
    }
}
