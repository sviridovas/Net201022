using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class QuickMatchExample : IMatchmakingCallbacks
{
    [SerializeField] private byte maxPlayers = 4;
    private LoadBalancingClient loadBalancingClient;
    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomOptions = roomOptions;
        loadBalancingClient.OpCreateRoom(enterRoomParams);
    }

    private void QuickMatch()
    {
        // loadBalancingClient.OpJoinRandomOrCreateRoom(null, null); ;
        
        // var opJoinRandomRoomParams = new OpJoinRandomRoomParams();
        loadBalancingClient.OpJoinRandomRoom();
    }
    // do not forget to register callbacks via loadBalancingClient.AddCallbackTarget
    // also deregister via loadBalancingClient.RemoveCallbackTarget
    void IMatchmakingCallbacks.OnJoinedRoom()
    {
        // joined a room successfully
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

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    public void OnLeftRoom()
    {
        throw new System.NotImplementedException();
    }
}