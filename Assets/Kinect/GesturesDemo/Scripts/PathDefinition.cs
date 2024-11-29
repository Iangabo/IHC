using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class PathDefinition : MonoBehaviourPunCallbacks
{
    public float nodeRadius = 1.5f;
    public float pathWidth = 2.0f;
    public Color pathColor = Color.yellow;
    public float pathHeight = 0.1f; // Altura del camino sobre el suelo
    public bool visualizePath = true; // Para mostrar/ocultar la visualización
    private LineRenderer pathLine;

    private List<Vector3> pathNodes = new List<Vector3>();

    void Start()
    {
        if (visualizePath)
        {
            Debug.Log("Inicializando visualización del camino.");
            SetupPathVisualization();
        }
        else
        {
            Debug.Log("Visualización deshabilitada.");
        }
    }

    private void SetupPathVisualization()
    {
        if(pathLine == null)
        {
            // Crear un LineRenderer para visualizar el camino
            pathLine = gameObject.AddComponent<LineRenderer>();
            pathLine.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            pathLine.material.color = pathColor;
            pathLine.widthMultiplier = 0.2f;
        }

        updatePathVisualization();
    }

    private void updatePathVisualization()
    {
        if(pathLine == null)
        {
            return;
        }

        pathLine.positionCount = pathNodes.Count;

        // Establecer las posiciones
        for (int i = 0; i < pathNodes.Count; i++)
        {
            Vector3 nodePos = pathNodes[i];
            nodePos.y += pathHeight; // Elevar ligeramente el camino
            pathLine.SetPosition(i, nodePos);
        }
    }

    [PunRPC]
    private void SyncPathNode(Vector3 newPosition)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            pathNodes.Add(newPosition);
            updatePathVisualization();
        }
    }

    public void AddNode(Vector3 newPosition, bool sendRPC = true)
    {
        pathNodes.Add(newPosition);
        updatePathVisualization();

        // Sincronización opcional
        if (sendRPC && PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            // Obtén la instancia de PathSyncHandler para enviar el RPC
            var syncHandler = FindObjectOfType<PathSyncHandler>();
            if (syncHandler != null && syncHandler.photonView != null)
            {
                syncHandler.photonView.RPC("SyncPathNode", RpcTarget.All, newPosition);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (pathNodes == null || pathNodes.Count == 0)
            return;

        Gizmos.color = pathColor;

        // Dibuja nodos y conexiones
        for (int i = 0; i < pathNodes.Count; i++)
        {
            Vector3 nodePos = pathNodes[i];
            nodePos.y += pathHeight; // Elevar los gizmos

            // Dibuja esfera en cada nodo
            Gizmos.DrawWireSphere(nodePos, nodeRadius);

            // Dibuja líneas entre nodos
            if (i < pathNodes.Count - 1)
            {
                Vector3 nextPos = pathNodes[i + 1];
                nextPos.y += pathHeight;
                Gizmos.DrawLine(nodePos, nextPos);

                // Dibuja el ancho del camino
                Vector3 pathDirection = (nextPos - nodePos).normalized;
                Vector3 perpendicular = Vector3.Cross(pathDirection, Vector3.up).normalized * pathWidth;

                // Dibuja líneas para mostrar el ancho
                Gizmos.DrawLine(nodePos + perpendicular, nodePos - perpendicular);
                Gizmos.DrawLine(nextPos + perpendicular, nextPos - perpendicular);
            }
        }
    }

    // Método para verificar si una posición está dentro del camino
    public bool IsPositionValid(Vector3 position)
    {
        if (pathNodes == null || pathNodes.Count < 2)
            return false;

        // Ignorar la altura (y) para la verificación
        Vector3 flatPosition = new Vector3(position.x, 0, position.z);

        for (int i = 0; i < pathNodes.Count - 1; i++)
        {
            Vector3 pathStart = new Vector3(pathNodes[i].x, 0, pathNodes[i].z);
            Vector3 pathEnd = new Vector3(pathNodes[i + 1].x, 0, pathNodes[i + 1].z);

            Vector3 projection = ProjectPointOnLine(flatPosition, pathStart, pathEnd);
            float distance = Vector3.Distance(flatPosition, projection);

            if (distance <= pathWidth &&
                Vector3.Distance(projection, pathStart) + Vector3.Distance(projection, pathEnd) <=
                Vector3.Distance(pathStart, pathEnd) + 0.01f)
            {
                return true;
            }
        }

        return false;
    }

    private Vector3 ProjectPointOnLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 line = lineEnd - lineStart;
        float len = line.magnitude;
        line.Normalize();

        Vector3 v = point - lineStart;
        float d = Vector3.Dot(v, line);
        d = Mathf.Clamp(d, 0f, len);

        return lineStart + line * d;
    }
}