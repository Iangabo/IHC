using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class ClickAndDestroyXR : MonoBehaviour
{
    public Transform rightHandController; // Asigna el Right Hand Controller en el inspector
    public InputActionProperty triggerAction; // Asigna la acción del trigger desde el Input System (XR Trigger)

    void Update()
    {
        // Verifica si el botón del controlador ha sido presionado
        if (triggerAction.action.WasPressedThisFrame())
        {
            RaycastHit hit;
            Ray ray = new Ray(rightHandController.position, rightHandController.forward); // El rayo se genera desde el controlador

            if (Physics.Raycast(ray, out hit))
            {
                BoxCollider bc = hit.collider as BoxCollider;
                if (bc != null)
                {
                    Destroy(bc.gameObject); // Destruye el objeto colisionado
                }
            }
        }
    }
}