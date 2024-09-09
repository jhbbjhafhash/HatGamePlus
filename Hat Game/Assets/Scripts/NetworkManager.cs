using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Xml.Serialization;

public class NetworkManager : MonoBehaviourPunCallbacks 
{
    public static NetworkManager instance;

    void Awake ()
    {
        if (instance != null && instance != this)
            gameObject.SetActive(false);  
       else
           {
           instance = this;
           }
    }

    void Start ()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom (string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom (string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
