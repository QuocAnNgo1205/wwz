using UnityEngine;
using ZombieCoopFPS.Core;
using ZombieCoopFPS.Enemy;
using ZombieCoopFPS.Economy;
using ZombieCoopFPS.Building;
using ZombieCoopFPS.Missions;
using ZombieCoopFPS.Combat;
using System.Collections.Generic;

namespace ZombieCoopFPS.Setup
{
    /// <summary>
    /// Critical setup script: Creates Managers, Player, and Test Controls
    /// </summary>
    public class GameManagerSetup : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private bool autoSetup = true;
        
        private void Awake()
        {
            if (autoSetup) SetupManagers();
        }
        
        public void SetupManagers()
        {
            // 1. GameManager
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager == null) gameManager = new GameObject("GameManager").AddComponent<GameManager>();

            // 2. ZombieManager (Physics/Swarm Version)
            ZombieManager zombieManager = FindFirstObjectByType<ZombieManager>();
            if (zombieManager == null)
            {
                GameObject zmObj = new GameObject("ZombieManager");
                zombieManager = zmObj.AddComponent<ZombieManager>();
                // We removed HordeController requirement in the new swarm logic, so this is optional now
            }

            // 3. EconomyManager
            EconomyManager economyManager = FindFirstObjectByType<EconomyManager>();
            if (economyManager == null) economyManager = new GameObject("EconomyManager").AddComponent<EconomyManager>();

            // 4. BuildingManager
            BuildingManager buildingManager = FindFirstObjectByType<BuildingManager>();
            if (buildingManager == null) buildingManager = new GameObject("BuildingManager").AddComponent<BuildingManager>();

            // 5. MissionManager
            MissionManager missionManager = FindFirstObjectByType<MissionManager>();
            if (missionManager == null) missionManager = new GameObject("MissionManager").AddComponent<MissionManager>();

            // 6. Link Everything
            gameManager.ZombieManager = zombieManager;
            gameManager.EconomyManager = economyManager;
            gameManager.BuildingManager = buildingManager;
            gameManager.MissionManager = missionManager;
            
            // 7. Ensure GameTester exists for keyboard controls
            if (FindFirstObjectByType<GameTester>() == null)
            {
                gameObject.AddComponent<GameTester>();
            }

            Debug.Log("✓ Runtime Managers & Tester Setup Complete");
        }
    }

    // --- SIMPLE PLAYER CONTROLLER (Physics-based) ---
    [RequireComponent(typeof(CharacterController))]
    public class SimplePlayerController : MonoBehaviour
    {
        public float speed = 6f;
        public float mouseSensitivity = 2f;
        public Transform cameraTransform;

        private CharacterController characterController;
        private float verticalRotation = 0f;
        private float verticalVelocity = 0f;

        void Start()
        {
            characterController = GetComponent<CharacterController>();
            if(GameManager.Instance) GameManager.Instance.EconomyManager.RegisterPlayer(0);
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            // Rotation
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            transform.Rotate(0, mouseX, 0);
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
            if(cameraTransform) cameraTransform.localEulerAngles = new Vector3(verticalRotation, 0, 0);

            // Movement
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            
            // Gravity
            if(characterController.isGrounded) verticalVelocity = -2f;
            else verticalVelocity += Physics.gravity.y * Time.deltaTime;

            move.y = verticalVelocity;
            
            characterController.Move(move * speed * Time.deltaTime);

            // Unlock Mouse
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            
            // Click to lock again
            if (Input.GetMouseButtonDown(0) && Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            // Shooting
            if (Input.GetMouseButtonDown(0) && !Cursor.visible)
            {
                Shoot();
            }
        }

        void Shoot()
        {
            if(!cameraTransform) return;
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, 100f))
            {
                IDamageable target = hit.collider.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(25f, transform.position);
                    // Debug visualization
                    Debug.DrawLine(cameraTransform.position, hit.point, Color.green, 0.5f);
                }
            }
        }
    }
    
    // --- ZOMBIE PREFAB SETUP (Physics/Swarm Version) ---
    public class ZombiePrefabSetup : MonoBehaviour
    {
        public void SetupComponents()
        {
            // 1. AI Script
            if(!GetComponent<ZombieAI>()) gameObject.AddComponent<ZombieAI>();
            
            // 2. Physics - CharacterController is lighter for hordes than Rigidbody
            var cc = GetComponent<CharacterController>();
            if(!cc) cc = gameObject.AddComponent<CharacterController>();
            cc.center = new Vector3(0, 1, 0);
            cc.radius = 0.4f;
            cc.height = 2f;

            // 3. Cleanup old components
            var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if(agent) DestroyImmediate(agent);
            
            var rb = GetComponent<Rigidbody>();
            if(rb) DestroyImmediate(rb);
            
            var capsule = GetComponent<CapsuleCollider>();
            if(capsule) DestroyImmediate(capsule);

            gameObject.tag = "Enemy";
        }
    }
}