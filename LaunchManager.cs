using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LaunchManager : MonoBehaviourPunCallbacks
{
    public GameObject EnterGamePanel;
    public GameObject ConnectionStatusPanel;
    public GameObject LobbyPanel;





    #region UNITY Methods

    private void Awake()
    {

        //all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        EnterGamePanel.SetActive(true);
        ConnectionStatusPanel.SetActive(false);
        LobbyPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion



    #region Public Methods

    public void ConnectToPhotonServer()
    {

        if (!PhotonNetwork.IsConnected)
        {

            EnterGamePanel.SetActive(false);
            ConnectionStatusPanel.SetActive(true);

            PhotonNetwork.ConnectUsingSettings();
        }

    }


    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    #endregion




    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.NickName+ " Connected to Photon Server!");

        LobbyPanel.SetActive(true);
        ConnectionStatusPanel.SetActive(false);

    }

    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);


        PhotonNetwork.LoadLevel("GameScene");
    }
    

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName+ "joined to " + PhotonNetwork.CurrentRoom.Name + " " +PhotonNetwork.CurrentRoom.PlayerCount);
    }

    #endregion



    #region Private Methods
    private void CreateAndJoinRoom()
    {

        string randomRoomNAME = "Room" + Random.Range(0,10000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = (byte)20;

        PhotonNetwork.CreateRoom(randomRoomNAME, roomOptions);
               
    }




    #endregion
}
