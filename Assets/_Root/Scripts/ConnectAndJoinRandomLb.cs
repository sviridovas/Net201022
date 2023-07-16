using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class ConnectAndJoinRandomLb : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{
    [SerializeField] Button ceateRoom;

    [SerializeField] GameObject roomsList;
    [SerializeField] Button roomBtnPref;

    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

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

    void CreateRoom()
    {
        var roomOptions = new RoomOptions
        {
            MaxPlayers = 12,
            // PublishUserId = true,
            CustomRoomPropertiesForLobby = new[] { GAME_MODE_KEY }
        };
        var enterRoomParams = new EnterRoomParams
        {
            RoomName = "NewRoom",
            RoomOptions = roomOptions,
            // ExpectedUsers = new[] { "12345" }
        };

        _lbc.OpCreateRoom(enterRoomParams);
    }

    void EnterRoom(Button roomBtn)
    {
        var lable = roomBtn.GetComponentInChildren<TMP_Text>();

        var enterRoomParams = new EnterRoomParams { RoomName = lable.text };
        _lbc.OpJoinRoom(enterRoomParams);
    }

    public void Play()
    {
        if (_lbc != null && _lbc.InRoom)
        {
            _lbc.CurrentRoom.IsOpen = false;
        }
    }

    [SerializeField] ServerSettings _serverSettings;
    LoadBalancingClient _lbc;

    const string GAME_MODE_KEY = "gm";
    const string AI_MODE_KEY = "gmai";

    List<Button> cachedBtn = new List<Button>();

    void Awake()
    {
        ceateRoom.onClick.AddListener(() => CreateRoom());
    }

    void Start()
    {
        _lbc = new LoadBalancingClient();
        _lbc.AddCallbackTarget(this);

        _lbc.ConnectUsingSettings(_serverSettings.AppSettings);
    }

    void OnDestroy()
    {
        if (_lbc != null)
            _lbc.RemoveCallbackTarget(this);
    }

    void Update()
    {
        if (_lbc != null)
        {
            _lbc.Service();

            var state = _lbc.State.ToString();
            // Debug.Log(state);
        }

        ceateRoom.gameObject.SetActive(cachedRoomList.Count == 0);

        foreach (var roomsBtn in cachedBtn) roomsBtn.gameObject.SetActive(false);

        int i = 0;
        foreach (var roomKey in cachedRoomList.Keys)
        {
            var room = cachedRoomList[roomKey];

            Button roomBtn = null;
            if (i >= cachedBtn.Count)
            {
                roomBtn = Instantiate<Button>(roomBtnPref, roomsList.transform);
                cachedBtn.Add(roomBtn);
                roomBtn.onClick.AddListener(() => EnterRoom(roomBtn));
            }
            else
            {
                roomBtn = cachedBtn[i];
            }

            var lable = roomBtn.GetComponentInChildren<TMP_Text>();
            lable.text = room.Name;

            roomBtn.gameObject.SetActive(true);
        }
    }

    void IConnectionCallbacks.OnConnected()
    {
        Debug.Log(nameof(IConnectionCallbacks.OnConnected));
    }

    void IConnectionCallbacks.OnConnectedToMaster()
    {
        Debug.Log(nameof(IConnectionCallbacks.OnConnectedToMaster));
        _lbc.OpJoinLobby(TypedLobby.Default);
    }

    void IMatchmakingCallbacks.OnCreatedRoom()
    {
        Debug.Log(nameof(IMatchmakingCallbacks.OnCreatedRoom));
    }

    void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(nameof(IMatchmakingCallbacks.OnCreateRoomFailed));
    }

    void IConnectionCallbacks.OnCustomAuthenticationFailed(string debugMessage)
    {
    }

    void IConnectionCallbacks.OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
    }

    void IConnectionCallbacks.OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(nameof(IConnectionCallbacks.OnDisconnected));

        cachedRoomList.Clear();
    }

    void IMatchmakingCallbacks.OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }

    void ILobbyCallbacks.OnJoinedLobby()
    {
        Debug.Log(nameof(ILobbyCallbacks.OnJoinedLobby));

        cachedRoomList.Clear();
    }

    void ILobbyCallbacks.OnLeftLobby()
    {
        Debug.Log(nameof(ILobbyCallbacks.OnJoinedLobby));

        cachedRoomList.Clear();
    }

    void IMatchmakingCallbacks.OnJoinedRoom()
    {
        Debug.Log(nameof(IMatchmakingCallbacks.OnJoinedRoom));
        // _lbc.CurrentRoom.IsOpen = false;
    }

    void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(nameof(IMatchmakingCallbacks.OnJoinRoomFailed));
        _lbc.OpCreateRoom(new EnterRoomParams());
    }

    void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
    {
    }

    void IMatchmakingCallbacks.OnLeftRoom()
    {
        Debug.Log(nameof(IMatchmakingCallbacks.OnLeftRoom));
    }

    void ILobbyCallbacks.OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
    }

    void IConnectionCallbacks.OnRegionListReceived(RegionHandler regionHandler)
    {
    }

    void ILobbyCallbacks.OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log(nameof(ILobbyCallbacks.OnRoomListUpdate));
        UpdateCachedRoomList(roomList);
    }

}
