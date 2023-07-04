using Photon.Realtime;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateRoomWithLobbyPropertiesExample : IMatchmakingCallbacks
{
    public const string MAP_PROP_KEY = "map";
    public const string GAME_MODE_PROP_KEY = "gm";
    public const string AI_PROP_KEY = "ai";
    private LoadBalancingClient loadBalancingClient;
    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new []{MAP_PROP_KEY, GAME_MODE_PROP_KEY, AI_PROP_KEY };
        roomOptions.CustomRoomProperties = new Hashtable { { MAP_PROP_KEY, 1 }, {GAME_MODE_PROP_KEY, 0 } };
        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomOptions = roomOptions;
        loadBalancingClient.OpCreateRoom(enterRoomParams);
    }

    void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)
    {
        // log error message and code
    }
    void IMatchmakingCallbacks.OnCreatedRoom()
    {
    }
    void IMatchmakingCallbacks.OnJoinedRoom()
    {
        // joined a room successfully, OpCreateRoom leads here on success
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftRoom()
    {
        throw new System.NotImplementedException();
    }
}
