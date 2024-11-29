using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;

public class GestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    // GUI Text to display the gesture messages.
    public TextMeshProUGUI GestureInfo;

    // Panel de bienvenida
    public GameObject WelcomePanel;
    public TextMeshProUGUI WelcomeText; // Texto para el contador
    private float welcomeDisplayTime = 7f; // Duración en segundos
    public GameObject WelcomePanel2; // Segundo panel de bienvenida
    public TextMeshProUGUI WelcomeText2; // Texto para el contador del segundo panel


    // ************* PATH DEFINITION ***************************
    public PathDefinition pathSystem;
    public TextMeshProUGUI warningText;
    private float warningDisplayTime = 5f;
    private float warningTime = 0f;

    // *********************************************************

    // ************* MOVMENT ***************************
    public Transform characterTransform;
    public float stepDistance = 2.5f;     // Distancia en cada paso
    public float movmentDuration = 1f;  // Duraci�n del movimiento (segundos)
    public float rotationAngle = 90f;   // �ngulo de rotaci�n
    // *************************************************

    // ************* CONTROLADORES DE MOVIMIENTO **************************
    private bool isMoving = false;          // Bandera para controlar el movimiento
    private Vector3 targetPosition;         // Posici�n objetivo
    private Quaternion targetRotation;      // Rotaci�n objetivo
    private float currentMovementTime = 0f; // Tiempo actual de movimiento
    // ********************************************************************
    void Start()
    {
        if (WelcomePanel != null)
        {
            WelcomePanel.SetActive(false); // Asegúrate de que esté oculto al inicio.
            WelcomePanel2.SetActive(false);
            Debug.Log("Panel oculto hasta que se detecte a un usuario");
        }
    }
    void Update()
    {
        if (isMoving)
        {
            // Incrementar el tiempo de movimiento
            currentMovementTime += Time.deltaTime;
            float progress = currentMovementTime / movmentDuration;

            if (progress < 1f)
            {
                characterTransform.position = Vector3.Lerp(
                                                characterTransform.position,
                                                targetPosition,
                                                progress);

                characterTransform.rotation = Quaternion.Lerp(
                                                characterTransform.rotation,
                                                targetRotation,
                                                progress);
            }
            else
            {
                characterTransform.position = targetPosition;
                characterTransform.rotation = targetRotation;
                isMoving = false;
            }
        }

        // Actualizar el tiempo de advertencia
        if (warningTime > 0f)
        {
            warningTime -= Time.deltaTime;
            if (warningTime <= 0)
            {
                warningText.text = string.Empty;
            }
        }
    }

    // ############################################ FUNCIONES PROPIAS ################################################################
    private void StartMovement(Vector3 direction, float angle = 0f)
    {
        if (!isMoving)
        {
            // Calculamos la posici�n objetivo
            Vector3 potentialTarget = characterTransform.position +
                                        (characterTransform.forward * direction.z +
                                        characterTransform.right * direction.x) * stepDistance;

            if(pathSystem != null && pathSystem.IsPositionValid(potentialTarget))
            {
                isMoving = true;
                currentMovementTime = 0f;
                targetPosition = potentialTarget;
                targetRotation = characterTransform.rotation * Quaternion.Euler(0, angle, 0);
            }
            else
            {
                // Mostrar mensaje de advertencia
                ShowWarning("No puedes moverte en esa direccion");
            }
        }
    }

    private void ShowWarning(string message)
    {
        if (warningText != null)
        {
            warningText.text = message;
            warningTime = warningDisplayTime;
        }
    }
    // ###############################################################################################################################
    public void ShowWelcomePanels()
{
    if (WelcomePanel != null && WelcomeText != null && WelcomePanel2 != null && WelcomeText2 != null)
    {
        StartCoroutine(DisplayWelcomePanelsWithCountdown());
        Debug.Log("Mostrando paneles");
    }
}

    private IEnumerator DisplayWelcomePanelsWithCountdown()
    {
        // Mostrar el primer panel con su contador
    WelcomePanel.SetActive(true);
    float timeRemaining = welcomeDisplayTime;
    while (timeRemaining > 0)
    {
        WelcomeText.text = $"Desaparece en: {timeRemaining:F1} s";
        Debug.Log("Mostrando primer panel");
        yield return new WaitForSeconds(0.1f);
        timeRemaining -= 0.1f;
    }
    WelcomePanel.SetActive(false);

    // Mostrar el segundo panel con su contador
    if (WelcomePanel2 != null && WelcomeText2 != null)
    {
        WelcomePanel2.SetActive(true);
        timeRemaining = welcomeDisplayTime;
        while (timeRemaining > 0)
        {
            WelcomeText2.text = $"Desaparece en: {timeRemaining:F1} s";
            Debug.Log("Mostrando segundo panel");
            yield return new WaitForSeconds(0.1f);
            timeRemaining -= 0.1f;
        }
        WelcomePanel2.SetActive(false);
    }
    }
    public void UserDetected(uint userId, int userIndex)
    {
        KinectManager manager = KinectManager.Instance;

        // Detectar los gestos
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);       // Rotar Izquierda
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeRight);      // Rotar Derecha
        manager.DetectGesture(userId, KinectGestures.Gestures.RaiseRightHand);  // Avanzar
        manager.DetectGesture(userId, KinectGestures.Gestures.RaiseLeftHand);   // Retroceder

        if (GestureInfo != null)
        {
            GestureInfo.text = "Prueba mover tus manos...";
        }
        // Muestra el panel de bienvenida al detectar un usuario.
        ShowWelcomePanels();
        Debug.Log("Mostrando Panel...");
    }

    public void UserLost(uint userId, int userIndex)
    {
        if (GestureInfo != null)
        {
            GestureInfo.text = string.Empty;
        }
    }

    public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture,
                                  float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {

    }


    public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture,
                              KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
        string sGestureText = gesture + " detectado";
        if (GestureInfo != null)
        {
            GestureInfo.text = sGestureText;
        }

        switch (gesture)
        {
            case KinectGestures.Gestures.RaiseRightHand:
                StartMovement(Vector3.forward);
                Debug.Log("Avanzando");
                break;

            case KinectGestures.Gestures.RaiseLeftHand:
                StartMovement(Vector3.back);
                Debug.Log("Retrocediendo");
                break;

            case KinectGestures.Gestures.SwipeLeft:
                
                StartMovement(Vector3.zero, -rotationAngle); // Rotaci�n normal
                Debug.Log("Rotando izquierda");
                break;

            case KinectGestures.Gestures.SwipeRight:
                StartMovement(Vector3.zero, rotationAngle);
                Debug.Log("Rotando derecha");
                break;
        }

        return true;
    }

    public bool GestureCancelled(uint userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectWrapper.NuiSkeletonPositionIndex joint)
    {
        return true;

    }
}