using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PathInputHandler : MonoBehaviour
{
    public PathDefinition pathDefinition;
    public Transform profesorTransform;
    public float stepDistance = 2.0f;

    private Vector3 lastNodePosition;
    private PathSyncHandler syncHandler;

    private bool isProfesor;

    void Start()
    {
        StartCoroutine(InitializeRole());
    }

    private IEnumerator InitializeRole()
    {
        // Espera hasta encontrar el PathSyncHandler
        syncHandler = FindObjectOfType<PathSyncHandler>();
        while (syncHandler == null)
        {
            Debug.Log("PathInputHandler: Esperando al PathSyncHandler...");
            yield return null; // Espera un frame
            syncHandler = FindObjectOfType<PathSyncHandler>();
        }

        // Espera hasta encontrar el NetworkPlayer
        NetworkPlayer localNetworkPlayer = null;
        while(localNetworkPlayer == null)
        {
            foreach(var player in FindObjectsOfType<NetworkPlayer>())
            {
                if (player.photonView.IsMine)
                {
                    localNetworkPlayer = player;
                    break;
                }
            }

            if(localNetworkPlayer == null)
            {
                Debug.Log("PathInputHandler: Esperando al NetworkPlayer...");
                yield return null;
            }
        }

        // Una vez encontrado, determina el rol
        Debug.Log("Booleanos - isProfesor: " + localNetworkPlayer.isProfesor + "  - isEstudiante: " + localNetworkPlayer.isEstudiante);
        isProfesor = localNetworkPlayer.isProfesor;
        Debug.Log("PathInputHandler: Rol asignado - " + (isProfesor ? "Profesor" : "Estudiante"));

        // Configura el nodo inicial si es profesor
        if (isProfesor && profesorTransform != null)
        {
            lastNodePosition = profesorTransform.position;
            syncHandler.RequestAddNode(lastNodePosition); // Agrega el nodo inicial
        }
    }

    void Update()
        {
        // Solo el profesor puede agregar nodos
        if (!isProfesor) return;

        Vector3 direction = Vector3.zero;

        // Detectar entrada de teclado
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector3.forward;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector3.back;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector3.right;
        }

        if (direction != Vector3.zero)
        {
            // Calcula la nueva posición del nodo
            Vector3 newNodePosition = lastNodePosition + direction * stepDistance;
            AddPathNode(newNodePosition);
        }
    }

    private void AddPathNode(Vector3 newPosition)
    {
        if (syncHandler != null)
        {
            lastNodePosition = newPosition;
            // Solicita al PathSyncHandler que sincronice el nodo
            syncHandler.RequestAddNode(newPosition);
        }
        else
        {
            Debug.LogError("No se puede agregar nodo porque PathSyncHandler no está inicializado.");
        }
    }
}