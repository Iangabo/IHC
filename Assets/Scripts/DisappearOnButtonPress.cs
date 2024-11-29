using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisappearOnButtonPress : MonoBehaviour
{
    public XRGrabInteractable interactable; // Referencia al XRGrabInteractable
    public string buttonName = "Grab"; // Nombre del botón que deseas usar

    private void Start()
    {
        if (interactable == null)
        {
            interactable = GetComponent<XRGrabInteractable>();
        }
    }

    private void Update()
    {
        // Verifica si se presiona el botón
        if (interactable.isSelected && Input.GetButtonDown(buttonName))
        {
            // Desactiva el objeto
            gameObject.SetActive(false);
            // O si prefieres destruirlo
            // Destroy(gameObject);
        }
    }
}
