using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionFollower : MonoBehaviour
{
   void Update()
    {
        // Mantener la rotaci�n hacia la c�mara
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // Asegura que no se incline
    }
}
