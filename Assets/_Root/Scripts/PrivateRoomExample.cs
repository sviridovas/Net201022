using Photon.Realtime;
using System.Collections.Generic;

public class PrivateRoomExample : IMatchmakingCallbacks
{
    private LoadBalancingClient loadBalancingClient;
    public void JoinOrCreatePrivateRoom(string nameEveryFriendKnows)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.PublishUserId = true;
        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomName = nameEveryFriendKnows;
        enterRoomParams.RoomOptions = roomOptions;
        loadBalancingClient.OpJoinOrCreateRoom(enterRoomParams);
    }

    void ExpectedUsers()
    {
        // массив идентификаторов пользователей
        var expectedUsers = new[] { "friend 1", "friend 2" };

        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.ExpectedUsers = expectedUsers;
        // create room example
        loadBalancingClient.OpCreateRoom(enterRoomParams);
        // join room example
        loadBalancingClient.OpJoinRoom(enterRoomParams);
        // join or create room example
        loadBalancingClient.OpJoinOrCreateRoom(enterRoomParams);
        // join random room example
        OpJoinRandomRoomParams opJoinRandomRoomParams = new OpJoinRandomRoomParams();
        opJoinRandomRoomParams.ExpectedUsers = expectedUsers;
        loadBalancingClient.OpJoinRandomRoom(opJoinRandomRoomParams);
    }

    void PlayWithFriends()
    {
        var leaderUserId = "leaderUserId";

        // {
        //     OpJoinRandomRoomParams opJoinRandomRoomParams = new OpJoinRandomRoomParams();
        //     opJoinRandomRoomParams.ExpectedUsers = teamMembersUserIds;
        //     loadBalancingClient.OpJoinRandomRoom(opJoinRandomRoomParams);
        // }

        // {
        //     EnterRoomParams enterRoomParams = new EnterRoomParams();
        //     enterRoomParams.ExpectedUsers = teamMembersUserIds;
        //     loadBalancingClient.OpCreateRoom(enterRoomParams);
        // }

        // {
        //     loadBalancingClient.OpFindFriends(new string[1] { leaderUserId });
        //     roomNameWhereTheLeaderIs
        // }

        // {
        //     EnterRoomParams enterRoomParams = new EnterRoomParams();
        //     enterRoomParams.RoomName = roomNameWhereTheLeaderIs;
        //     loadBalancingClient.OpJoinRoom(enterRoomParams);
        // }
    }

    // do not forget to register callbacks via loadBalancingClient.AddCallbackTarget
    // also deregister via loadBalancingClient.RemoveCallbackTarget
    void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
    {
        // log error code and message
    }
    void IMatchmakingCallbacks.OnJoinedRoom()
    {
        // joined a room successfully, OpJoinOrCreateRoom leads here on success
        // Player.UserId
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

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftRoom()
    {
        throw new System.NotImplementedException();
    }
}
