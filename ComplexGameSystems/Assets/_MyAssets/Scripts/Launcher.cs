using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.ATL.MyGame
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serialized Fields
        [SerializeField]
        private byte maxPlayersPerRoom = 4;


        #endregion

        #region Private Fields
        public string versionNumber;
        string gameVersion = "1";
        #endregion

        #region MonoBehaviour CallBacks
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            //Connect() is redundant with the addition of the button.
            //Connect();
        }
        #endregion

        // Update is called once per frame
        void Update()
        {

        }

        #region Public Methods

        public void Connect()
        {
            if(PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
        #endregion

        #region MonoBehaviorPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() was called by PUN");
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("OnDisconnected() was called by PUN");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed() was called by PUN, no room was found.");
            PhotonNetwork.CreateRoom(null, new RoomOptions());

        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom() was called by PUN.");
        }

        public override void OnConnected()
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }
        #endregion

    }
}