using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

namespace CoopZombieShooter.Player
{
    /// <summary>
    /// Networked third-person player controller - FIXED VERSION
    /// This version works with PlayerInput component and Unity Events
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NetworkObject))]
    public class PlayerController : NetworkBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 8f;
        [SerializeField] private float crouchSpeed = 2f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float gravity = -20f;
        [SerializeField] private float jumpHeight = 2f;

        [Header("Crouch Settings")]
        [SerializeField] private float standingHeight = 2f;
        [SerializeField] private float crouchHeight = 1f;
        [SerializeField] private float crouchTransitionSpeed = 10f;

        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.4f;
        [SerializeField] private LayerMask groundMask;

        [Header("References")]
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private GameObject playerModel;

        // Components
        private CharacterController characterController;
        private PlayerInput playerInput;
        private Camera mainCamera;
        
        // Input values
        private Vector2 moveInput;
        private bool isRunning;
        private bool isCrouching;
        private bool jumpPressed;
        private bool shootPressed;

        // Movement state
        private Vector3 velocity;
        private bool isGrounded;
        private float currentHeight;

        // Network variables
        private NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>();
        private NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>();

        #region Unity Lifecycle

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            currentHeight = standingHeight;
            
            Debug.Log($"PlayerController Awake - CharacterController: {characterController != null}, PlayerInput: {playerInput != null}");
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            Debug.Log($"Player spawned - IsOwner: {IsOwner}, ClientId: {OwnerClientId}");

            // Only enable input for the local player
            if (!IsOwner)
            {
                if (playerInput != null)
                    playerInput.enabled = false;

                if (cameraTarget != null)
                    cameraTarget.gameObject.SetActive(false);
            }
            else
            {
                // Find main camera
                mainCamera = Camera.main;
                if (mainCamera == null)
                {
                    Debug.LogError("Main Camera not found! Make sure camera has tag 'MainCamera'");
                }
                else
                {
                    Debug.Log($"Main Camera found: {mainCamera.name}");
                }

                // Setup camera
                SetupCamera();
                
                // Enable input
                if (playerInput != null)
                {
                    playerInput.enabled = true;
                    Debug.Log("PlayerInput enabled for local player");
                }
            }

            // Initialize network variables
            if (IsServer)
            {
                networkPosition.Value = transform.position;
                networkRotation.Value = transform.rotation;
            }
        }

        private void Update()
        {
            if (!IsOwner) 
            {
                InterpolateNetworkTransform();
                return;
            }

            // Debug: Log input every second
            if (Time.frameCount % 60 == 0 && moveInput.magnitude > 0.01f)
            {
                Debug.Log($"Move Input: {moveInput} | Position: {transform.position}");
            }

            HandleGroundCheck();
            HandleMovement();
            HandleRotation();
            HandleCrouch();
            HandleGravity();
            HandleShooting();
        }

        #endregion

        #region Input Handlers (Called by PlayerInput component via Unity Events)

        public void OnMove(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            
            moveInput = context.ReadValue<Vector2>();
            
            // Debug first few inputs
            if (Time.time < 5f && moveInput.magnitude > 0)
            {
                Debug.Log($"OnMove called: {moveInput}");
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            // Look input (for future camera rotation)
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            isRunning = context.ReadValueAsButton();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            
            if (context.performed)
            {
                isCrouching = !isCrouching;
                Debug.Log($"Crouch toggled: {isCrouching}");
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            
            if (context.performed && isGrounded)
            {
                jumpPressed = true;
                Debug.Log("Jump!");
            }
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            shootPressed = context.ReadValueAsButton();
        }

        #endregion

        #region Movement Logic

        private void HandleGroundCheck()
        {
            if (groundCheck != null)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            }
            else
            {
                isGrounded = characterController.isGrounded;
            }

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }

        private void HandleMovement()
        {
            // Determine speed based on state
            float currentSpeed = walkSpeed;
            
            if (isCrouching)
            {
                currentSpeed = crouchSpeed;
            }
            else if (isRunning && moveInput.magnitude > 0.1f)
            {
                currentSpeed = runSpeed;
            }

            // Calculate move direction relative to camera
            Vector3 moveDirection = GetMoveDirection();
            Vector3 move = moveDirection * currentSpeed;

            // Handle jump
            if (jumpPressed && !isCrouching)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpPressed = false;
            }

            // Apply movement
            if (move.magnitude > 0.01f)
            {
                characterController.Move(move * Time.deltaTime);
            }

            // Update network state
            if (IsServer)
            {
                networkPosition.Value = transform.position;
                networkRotation.Value = transform.rotation;
            }
            else if (IsOwner)
            {
                UpdatePositionServerRpc(transform.position, transform.rotation);
            }
        }

        private Vector3 GetMoveDirection()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                if (mainCamera == null)
                {
                    // Fallback: move in world space
                    return new Vector3(moveInput.x, 0, moveInput.y);
                }
            }

            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            return cameraForward * moveInput.y + cameraRight * moveInput.x;
        }

        private void HandleRotation()
        {
            if (moveInput.magnitude > 0.1f)
            {
                Vector3 moveDirection = GetMoveDirection();
                if (moveDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }

        private void HandleCrouch()
        {
            float targetHeight = isCrouching ? crouchHeight : standingHeight;
            currentHeight = Mathf.Lerp(currentHeight, targetHeight, crouchTransitionSpeed * Time.deltaTime);
            characterController.height = currentHeight;
            characterController.center = new Vector3(0, currentHeight / 2f, 0);
        }

        private void HandleGravity()
        {
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }

        #endregion

        #region Shooting Logic

        private void HandleShooting()
        {
            if (shootPressed)
            {
                ShootServerRpc();
            }
        }

        [ServerRpc]
        private void ShootServerRpc()
        {
            Debug.Log($"Player {OwnerClientId} shot!");
            ShootClientRpc();
        }

        [ClientRpc]
        private void ShootClientRpc()
        {
            Debug.Log("Playing shoot effects");
        }

        #endregion

        #region Network Synchronization

        [ServerRpc(RequireOwnership = false)]
        private void UpdatePositionServerRpc(Vector3 position, Quaternion rotation)
        {
            networkPosition.Value = position;
            networkRotation.Value = rotation;
        }

        private void InterpolateNetworkTransform()
        {
            transform.position = Vector3.Lerp(transform.position, networkPosition.Value, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Slerp(transform.rotation, networkRotation.Value, Time.deltaTime * 15f);
        }

        #endregion

        #region Camera Setup

        private void SetupCamera()
        {
            if (cameraTarget != null)
            {
                cameraTarget.gameObject.SetActive(true);
            }

            // Try to find SimpleCameraFollow
            var simpleCam = FindObjectOfType<SimpleCameraFollow>();
            if (simpleCam != null)
            {
                simpleCam.SetTarget(cameraTarget);
                Debug.Log("SimpleCameraFollow setup complete");
                return;
            }

            Debug.LogWarning("No SimpleCameraFollow found on Main Camera!");
        }

        #endregion

        #region Debug

        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = isGrounded ? Color.green : Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
            }
        }

        // Debug GUI
        private void OnGUI()
        {
            if (!IsOwner) return;

            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            GUILayout.Box("=== PLAYER DEBUG ===");
            GUILayout.Label($"IsOwner: {IsOwner}");
            GUILayout.Label($"Move Input: {moveInput}");
            GUILayout.Label($"Running: {isRunning}");
            GUILayout.Label($"Crouching: {isCrouching}");
            GUILayout.Label($"Grounded: {isGrounded}");
            GUILayout.Label($"Position: {transform.position}");
            GUILayout.Label($"Camera: {(mainCamera != null ? "OK" : "NULL")}");
            
            if (UnityEngine.InputSystem.Keyboard.current != null)
            {
                var kb = UnityEngine.InputSystem.Keyboard.current;
                GUILayout.Label($"W:{kb.wKey.isPressed} A:{kb.aKey.isPressed} S:{kb.sKey.isPressed} D:{kb.dKey.isPressed}");
            }
            
            GUILayout.EndArea();
        }

        #endregion
    }
}