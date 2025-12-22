using UnityEngine;
using UnityEngine.UI; // Để dùng Text UI
using ZombieCoopFPS.Economy; // QUAN TRỌNG: Để dùng IInteractable

namespace ZombieCoopFPS.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float interactionRange = 3f;
        [SerializeField] private LayerMask interactableLayers;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        
        [Header("UI")]
        [SerializeField] private GameObject interactionPrompt; // Kéo cái Panel UI vào đây
        [SerializeField] private Text promptText;              // Kéo Text UI vào đây

        private IInteractable currentInteractable;

        private void Update()
        {
            CheckForInteractable();
            HandleInput();
        }

        private void CheckForInteractable()
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayers))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null && interactable.CanInteract(gameObject))
                {
                    currentInteractable = interactable;
                    ShowPrompt(interactable.GetInteractionPrompt());
                    return;
                }
            }
            // Nếu không nhìn thấy gì thì xóa prompt
            currentInteractable = null;
            if (interactionPrompt) interactionPrompt.SetActive(false);
        }

        private void HandleInput()
        {
            if (currentInteractable != null && Input.GetKeyDown(interactKey))
            {
                currentInteractable.Interact(gameObject);
            }
        }

        private void ShowPrompt(string message)
        {
            if (interactionPrompt) interactionPrompt.SetActive(true);
            if (promptText) promptText.text = message + " [E]";
        }
    }
}