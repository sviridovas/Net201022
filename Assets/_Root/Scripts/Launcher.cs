using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] Button connectBtn = null;

    // This client's version number. Users are separated from each other by gameVersion(which allows you to make breaking changes).
    string gameVersion = "1";
    // MonoBehaviour method called on GameObject by Unity during early initialization phase.
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    // MonoBehaviour method called on GameObject by Unity during initialization phase.
    void Start()
    {
        if(connectBtn != null) {
            connectBtn.onClick = new Button.ButtonClickedEvent();
            connectBtn.onClick.AddListener(ConnectDisconnect);
        }
    }
    // Start the connection process.
    // - If already connected, we attempt joining a random room
    // - if not yet connected, Connect this application instance to Photon Cloud Network
    public void Connect()
    {
        // we check if we are connected or not, we join if we are , else we initiate
        // the connection to the server. 
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public void ConnectDisconnect()
    {
        if(!PhotonNetwork.IsConnected) {
            Connect();
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

