using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem; // Para manejar las entradas del profesor (mouse/teclado)

public class NetworkPlayer : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public PhotonView photonView;

    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;

    private XROrigin XROrigin;

    public bool isProfesor = false;
    public bool isEstudiante = false;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if( photonView.IsMine)
        {
            // Desactivar el kinect para el profesor
            Camera mainCamera = Camera.main;
            if(mainCamera != null)
            {
                GestureListener gestureListener = mainCamera.GetComponent<GestureListener>();
                KinectManager kinectManager = mainCamera.GetComponent<KinectManager>();             

                if(gestureListener != null && kinectManager != null)
                {
                    if (isProfesor)
                    {
                        kinectManager.ShouldInitialized = false;
                        gestureListener.enabled = false;
                        kinectManager.enabled = false;
                        
                        Debug.Log("Kinect y GestureListener desactivados para el Profesor");
                    }
                    else if (isEstudiante)
                    {
                        kinectManager.ShouldInitialized = true;
                        gestureListener.enabled = true;
                        kinectManager.enabled = true;
                        Debug.Log("Kinect y GestureListener activados para el Estudiante");
                    }
                }
                else
                {
                    Debug.LogWarning("No se encontraron los componentes GestureListener o KinectManager en la cámara principal.");
                }
            }


            // Jugador Local --> Buscar el XR Origin
            XROrigin = FindObjectOfType<XROrigin>();
            if( XROrigin != null)
            {
                XROrigin.transform.position = transform.position;
                XROrigin.transform.rotation = transform.rotation;

                headRig = XROrigin.transform.Find("Camera Offset/Main Camera");
                leftHandRig = XROrigin.transform.Find("Camera Offset/LeftHand Controller");
                rightHandRig = XROrigin.transform.Find("Camera Offset/RightHand Controller");
            }
        }
        else
        {
            // No es el jugador local --> Desactivar componentes de XR
            head.gameObject.SetActive(true);
            leftHand.gameObject.SetActive(true);
            rightHand.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (isEstudiante)
            {
                MapPosition(head, headRig);
                MapPosition(leftHand, leftHandRig);
                MapPosition(rightHand, rightHandRig);

                HandleEstudianteBehavior();
            }

            // Configuracion de roles
            if (isProfesor)
            {
                HandleProfesorBehavior();
            }
        }
    }

    void MapPosition(Transform target, Transform rigTransform)
    {
        if(rigTransform != null)
        {
            target.position = rigTransform.position;
            target.rotation = rigTransform.rotation;
        }
    }

    public void SetAsProfesor()
    {
        isEstudiante = false;
        isProfesor = true;
        Debug.Log("PHOTOM: El jugador es un profesor");

        // Al configurar al jugador como Profesor, desactivamos el Kinect y GestureListener si es necesario
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            GestureListener gestureListener = mainCamera.GetComponent<GestureListener>();
            KinectManager kinectManager = mainCamera.GetComponent<KinectManager>();

            if (gestureListener != null && kinectManager != null)
            {
                gestureListener.enabled = false;
                kinectManager.enabled = false;
                Debug.Log("Kinect y GestureListener desactivados para el Profesor");
            }
        }
    }

    public void SetAsEstudiante()
    {
        isEstudiante = true;
        isProfesor = false;
        Debug.Log("PHOTOM: El jugador es un estudiante");

        // Al configurar al jugador como Estudiante, activamos Kinect y GestureListener
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            GestureListener gestureListener = mainCamera.GetComponent<GestureListener>();
            KinectManager kinectManager = mainCamera.GetComponent<KinectManager>();

            if (gestureListener != null && kinectManager != null)
            {
                gestureListener.enabled = true;
                kinectManager.enabled = true;
                Debug.Log("Kinect y GestureListener activados para el Estudiante");
            }
        }
    }

    void HandleProfesorBehavior()
    {
        /*
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Profesor presionó espacio");
        }
        */
    }

    void HandleEstudianteBehavior()
    {
        /*
        if (rightHand.gameObject.activeSelf)
        {
            Debug.Log("Estudiante está usando la mano derecha");
        }
        */
    }
}
