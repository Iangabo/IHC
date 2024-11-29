using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerPrefab;
    public Transform profesorSpawnPoint;
    public Transform estudianteSpawnPoint;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        // Determinar el rol del jugador
        string playerRole = PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role") ? (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"] : null;

        if(playerRole == "Profesor")
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", profesorSpawnPoint.position, profesorSpawnPoint.rotation);
            ConfigurePlayerRole(playerRole);
        }
        else if (playerRole == "Estudiante")
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", estudianteSpawnPoint.position, estudianteSpawnPoint.rotation);
            ConfigurePlayerRole(playerRole);
        }
        else
        {
            Debug.LogError("PHOTOM: No se ha asignado un rol al jugador.");
        }
    }

    public void ConfigurePlayerRole(string role)
    {
        // Accedemos al componente NetworkPlayer en el prefab del jugador
        NetworkPlayer networkPlayer = spawnedPlayerPrefab.GetComponent<NetworkPlayer>();

        if (role == "Profesor")
        {
            networkPlayer.SetAsProfesor();
        }
        else if (role == "Estudiante")
        {
            networkPlayer.SetAsEstudiante();
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        if (spawnedPlayerPrefab != null)
        {
            PhotonNetwork.Destroy(spawnedPlayerPrefab);
        }
    }
}