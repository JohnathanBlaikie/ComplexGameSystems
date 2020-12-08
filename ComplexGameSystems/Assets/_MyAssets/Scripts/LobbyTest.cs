using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.ATL.MyGame
{
    public class LobbyTest : MonoBehaviour
    {
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
            
        }
        #endregion

        // Update is called once per frame
        void Update()
        {

        }
    }
}