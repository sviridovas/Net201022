using Photon.Realtime;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine;

public class RandomSQLMatchmakingExample : IMatchmakingCallbacks
{
    public const string ELO_PROP_KEY = "C0";
    public const string MAP_PROP_KEY = "C1";
    private TypedLobby sqlLobby = new TypedLobby("customSqlLobby", LobbyType.SqlLobby);
    private LoadBalancingClient loadBalancingClient;
    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomProperties = new Hashtable { { ELO_PROP_KEY, 400 }, { MAP_PROP_KEY, "Map3" } };
        roomOptions.CustomRoomPropertiesForLobby = new[] { ELO_PROP_KEY, MAP_PROP_KEY }; //makes "C0" and "C3" available in the lobby
        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomOptions = roomOptions;
        enterRoomParams.Lobby = sqlLobby;
        loadBalancingClient.OpCreateRoom(enterRoomParams);
    }
    private void JoinRandomRoom()
    {
        string sqlLobbyFilter = "C0 BETWEEN 345 AND 475 AND C3 = 'Map2'";
        //string sqlLobbyFilter = "C0 > 345 AND C0 < 475 AND (C3 = 'Map2' OR C3 = \"Map3\")";
        //string sqlLobbyFilter = "C0 >= 345 AND C0 <= 475 AND C3 IN ('Map1', 'Map2', 'Map3')";
        OpJoinRandomRoomParams opJoinRandomRoomParams = new OpJoinRandomRoomParams();
        opJoinRandomRoomParams.SqlLobbyFilter = sqlLobbyFilter;
        loadBalancingClient.OpJoinRandomRoom(opJoinRandomRoomParams);
    }
    // do not forget to register callbacks via loadBalancingClient.AddCallbackTarget
    // also deregister via loadBalancingClient.RemoveCallbackTarget
    void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }
    void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("Room creation failed with error code {0} and error message { 1} ", returnCode, message);
    }
    void IMatchmakingCallbacks.OnJoinedRoom()
    {
        // joined a room successfully, both JoinRandomRoom or CreateRoom lead here on success
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        throw new System.NotImplementedException();
    }

    public void OnCreatedRoom()
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
