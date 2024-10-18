using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionFollower : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float distance = 5.0f;

    private bool isCentered = false;

    private void OnBecameInvisible()
    {
        isCentered = false;    
    }

    // Update is called once per frame
    void Update()
    {
        if( !isCentered)
        {
            // Encontrar la posicion que necesitamos
            Vector3 targetPosition = FindTargetPosition();

            // Mover objeto
            MoveTowards(targetPosition);

            // Si termino el trayecto, no hacer nada
            if (ReachedPosition(targetPosition))
            {
                isCentered = true;
            }
        }        
    }

    private Vector3 FindTargetPosition()
    {
        // Posicion enfrente de la camara del jugador
        return cameraTransform.position + (cameraTransform.forward * distance);
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        transform.position += (targetPosition - transform.position) * 0.05f;
    }

    private bool ReachedPosition(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, transform.position) < 0.1f;
    }
}
