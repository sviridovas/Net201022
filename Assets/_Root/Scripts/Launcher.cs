using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] Button connectBtn = null;
    [SerializeField] Button enterToLobbyBtn = null;
    [SerializeField] Button createRoomBtn = null;
    [SerializeField] Button leaveRoomBtn = null;

    [SerializeField] GameObject roomsList;
    [SerializeField] Button roomBtnPref;

    List<TMP_Text> cachedRoomList = new List<TMP_Text>();

    // This client's version number. Users are separated from each other by gameVersion(which allows you to make breaking changes).
    string gameVersion = "1";
    // MonoBehaviour method called on GameObject by Unity during early initialization phase.
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;

        for(int i = 0; i != 500; ++i) {
            var btn = Instantiate<Button>(roomBtnPref, roomsList.transform);
            btn.gameObject.SetActive(false);

            var lable = btn.GetComponentInChildren<TMP_Text>();
            cachedRoomList.Add(lable);
            // lable.text = "test";

            btn.onClick.AddListener(() => EnterRoom(lable));
        }
    }

    public override void OnJoinedLobby()
    {
        var roomsBtns = roomsList.GetComponentsInChildren<Button>();
        foreach(var roomsBtn in roomsBtns) roomsBtn.gameObject.SetActive(false);

        // PhotonNetwork.GetCustomRoomList(TypedLobby.Default, "");
    }

    public override void OnLeftLobby()
    {
        var roomsBtns = roomsList.GetComponentsInChildren<Button>();
        foreach(var roomsBtn in roomsBtns) roomsBtn.gameObject.SetActive(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        Debug.Log(roomList);

        var roomsBtns = roomsList.GetComponentsInChildren<Button>();
        foreach(var roomsBtn in roomsBtns) roomsBtn.gameObject.SetActive(false);

        // here you get the response, empty list if no rooms found
        for(int i = 0; i != roomList.Count; ++i) 
        {
            var roomBtn = roomsBtns[i];
            var room = roomList[i];

            var lable = roomBtn.GetComponentInChildren<TMP_Text>();
            lable.text = room.Name;

            roomBtn.gameObject.SetActive(true);
        }
    }


    // MonoBehaviour method called on GameObject by Unity during initialization phase.
    void Start()
    {
        if(connectBtn != null) {
            connectBtn.onClick.AddListener(ConnectDisconnect);
        }
        
        if(enterToLobbyBtn != null) {
            enterToLobbyBtn.onClick.AddListener(EnterToLobby);
        }
        if(createRoomBtn != null) {
            createRoomBtn.onClick.AddListener(CreateRoom);
        }

        if(leaveRoomBtn != null) {
            leaveRoomBtn.onClick.AddListener(LeaveRoom);
        }
    }
    void EnterToLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        // EnterRoomParams enterRoomParams = new EnterRoomParams();
        // enterRoomParams.RoomOptions = roomOptions;
        PhotonNetwork.CreateRoom("First room", roomOptions);
    }

    void EnterRoom(TMP_Text lable)
    {
        Debug.Log(lable.text);
        PhotonNetwork.JoinRoom(lable.text);
    }

    void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(false);
    }

    public void ConnectDisconnect()
    {
        if(!PhotonNetwork.IsConnected) {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
        else {
            PhotonNetwork.Disconnect();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN");

        connectBtn.image.color = Color.green;
        connectBtn.GetComponentInChildren<TMP_Text>().text = "Disconnect";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected() was called by PUN");
        base.OnDisconnected(cause);

        connectBtn.image.color = Color.blue;
        connectBtn.GetComponentInChildren<TMP_Text>().text = "Connect";
    }
}

