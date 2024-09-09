using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks
{
    [Header("Screens")]
    public GameObject MainScreen;
    public GameObject LobbyScreen;
    
    [Header("MainScreen")]
    public Button CreateRoomButton;
    public Button JoinRoomButton;

    [Header("LobbyScreen")]
    public TextMeshProUGUI PlayerListText;
    public Button StartGameButton;

    void Start ()
    {
        CreateRoomButton.interactable = false;
        JoinRoomButton.interactable = false;
    }

    public override void OnConnectedToMaster()
    {
        CreateRoomButton.interactable = true;
        JoinRoomButton.interactable = true;
    }

    void SetScreen (GameObject screen)
    {
        MainScreen.SetActive(false);
        LobbyScreen.SetActive(false);

        screen.SetActive(true);
    }

    public void OnCreateRoomButton (TMP_InputField RoomNameInput)
    {
        NetworkManager.instance.CreateRoom(RoomNameInput.text);
    }

    public void OnJoinRoomButton (TMP_InputField RoomNameInput)
    {
        NetworkManager.instance.JoinRoom(RoomNameInput.text);
    }

    public void OnPlayerNameUpdate (TMP_InputField PlayerNameInput)
    {
        PhotonNetwork.NickName = PlayerNameInput.text;
    }

    public override void OnJoinedRoom ()
    {
        SetScreen(LobbyScreen);

        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyUI();
    }

    [PunRPC]
    public void UpdateLobbyUI ()
    {
        PlayerListText.text = "";

        foreach(Player player in PhotonNetwork.PlayerList)
        {
            PlayerListText.text += player.NickName + "\n";
        }

        if (PhotonNetwork.IsMasterClient)
            StartGameButton.interactable = true;
         else
            StartGameButton.interactable = false;
    }

    public void OnLeaveLobbyButton ()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(MainScreen);
    }

    public void OnStartGameButton ()
    {
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }
}
