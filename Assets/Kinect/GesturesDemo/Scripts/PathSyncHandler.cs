using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PathSyncHandler : MonoBehaviourPun, IPunObservable
{
    private PathDefinition pathDefinition;
    private List<Vector3> syncedNodes = new List<Vector3>(); // Lista para sincronizar nodos
    private bool isProfesor;

    void Start()
    {
        // Encuentra el objeto en la jerarquía
        pathDefinition = FindObjectOfType<PathDefinition>();
        if (pathDefinition == null)
        {
            Debug.LogError("PathDefinition no encontrado en la escena.");
        }

        // Determina el rol asignado al jugador
        NetworkPlayer networkPlayer = FindObjectOfType<NetworkPlayer>();
        if (networkPlayer != null)
        {
            isProfesor = networkPlayer.isProfesor;
            Debug.Log("PathSyncHandler: Rol asignado - " + (isProfesor ? "Profesor" : "Estudiante"));
        }
        else
        {
            Debug.LogError("PathSyncHandler: No se encontró NetworkPlayer. No se pudo determinar el rol.");
        }
    }

    // Método RPC para sincronizar un nodo
    [PunRPC]
    public void SyncPathNode(Vector3 newPosition)
    {
        if (pathDefinition != null)
        {
            pathDefinition.AddNode(newPosition, false); // Agrega el nodo localmente
            Debug.Log("Nodo sincronizado: " + newPosition);
        }
    }

    // Método para agregar un nodo nuevo localmente y sincronizarlo
    public void RequestAddNode(Vector3 newPosition)
    {
        if (PhotonNetwork.IsMasterClient) // Reemplazar por prfesorrrrrrrr
        {
            // Agrega el nodo localmente
            pathDefinition.AddNode(newPosition, true);

            // Llama al RPC para sincronizar en otros clientes
            photonView.RPC("SyncPathNode", RpcTarget.AllBuffered, newPosition);
        }
        else
        {
            Debug.LogWarning("PathSyncHandler: Solo los profesores pueden agregar nodos al camino.");
        }
    }

    // Método para manejar la sincronización automática de nodos
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Enviar datos (solo desde el MasterClient)
            if (PhotonNetwork.IsMasterClient)
            {
                stream.SendNext(syncedNodes.Count); // Enviar la cantidad de nodos
                foreach (Vector3 node in syncedNodes)
                {
                    stream.SendNext(node); // Enviar cada nodo
                }
            }
        }
        else
        {
            // Recibir datos
            int count = (int)stream.ReceiveNext();
            syncedNodes.Clear();
            for (int i = 0; i < count; i++)
            {
                Vector3 node = (Vector3)stream.ReceiveNext();
                syncedNodes.Add(node);

                // Actualizar el camino localmente
                if (pathDefinition != null)
                {
                    pathDefinition.AddNode(node, false);
                }
                else
                {
                    Debug.LogWarning("PathDefinition no está asignado.");
                }
            }
        }
    }
}
