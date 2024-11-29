using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject roleSelectionUI;
    public GameObject roomUI;
    private string selectedRole; 

    private GameObject spawnedPlayerPrefab;

    // --------------------------------------- 1. SERVIDOR ---------------------------------------------------
    public void ConnectToServer() {
        PhotonNetwork.ConnectUsingSettings();               // --> Me crea el onConnectedToPhoton() y el onConnectedToMaster()
        Debug.Log("PHOTOM: Try Connect to server ...");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();                          // --> Me crea el onJoinedLobby
        Debug.Log("PHOTOM: Conected to Server :)");
    }
    // --------------------------------------------------------------------------------------------------------
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("PHOTOM: Joined to Lobby");

        roleSelectionUI.SetActive(true);                    // --> Llama a la funcion SelectRole
    }

    public void SelectRole(string role)
    {
        selectedRole = role; // Guardamos el rol seleccionado (Profesor o Estudiante)
        Debug.Log("PHOTOM: Role Selected: " + selectedRole);
        roleSelectionUI.SetActive(false);
        roomUI.SetActive(true);                             // --> llama a la funcion InitialiazeRoom
    }

    public void InitiliazeRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("PHOTOM: No estás conectado al servidor.");
            return;
        }

        string roomName = "MainRoom";

        // CRETE ROOM
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 2,
            IsVisible = true,
            IsOpen = true
        };

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);              // --> Me crea el onCreatedRoom y el onJoinedRoom

        // Asignamos el rol al jugador
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties["Role"] = selectedRole;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        // Cargamos la escena
        PhotonNetwork.LoadLevel("MainGame");
    }
    // --------------------------------------------------------------------------------------------------------

    public override void OnCreatedRoom()
    {
        Debug.Log("Photon: Se creo un ROOM");
        base.OnCreatedRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PHOTOM: Joined a Room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("PHOTOM: A New player joined");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
