#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using ZombieCoopFPS.Enemy; // Dùng ZombieAI mới
using ZombieCoopFPS.Data;
using ZombieCoopFPS.Setup; // Dùng GameTester và GameManagerSetup

namespace ZombieCoopFPS.Editor
{
    /// <summary>
    /// Công cụ Setup Game - Phiên bản Physics/Swarm (World War Z)
    /// </summary>
    public static class CompleteGameSetup
    {
        private const string PROJECT_ROOT = "Assets/_Project/";
        private const string PREFABS_PATH = PROJECT_ROOT + "Prefabs/";
        private const string SCRIPTABLE_PATH = PROJECT_ROOT + "ScriptableObjects/";
        private const string MATERIALS_PATH = PROJECT_ROOT + "Materials/";
        private const string RESOURCES_PATH = PROJECT_ROOT + "Resources/";

        // --- 1. HÀM FIX LỖI MÀU HỒNG ---
        private static Shader GetDefaultShader()
        {
            var pipeline = GraphicsSettings.currentRenderPipeline;
            if (pipeline != null)
            {
                if (pipeline.GetType().Name.Contains("Universal")) 
                    return Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("URP/Lit");
                if (pipeline.GetType().Name.Contains("HighDefinition")) 
                    return Shader.Find("HDRP/Lit");
            }
            return Shader.Find("Standard");
        }

        [MenuItem("Game Setup/1. Create Complete Project Structure")]
        public static void CreateProjectStructure()
        {
            CreateFolder("Assets", "_Project");
            CreateFolder(PROJECT_ROOT, "Scripts");
            CreateFolder(PROJECT_ROOT, "Prefabs");
            CreateFolder(PROJECT_ROOT, "ScriptableObjects");
            CreateFolder(PROJECT_ROOT, "Materials");
            CreateFolder(PROJECT_ROOT, "Resources");
            CreateFolder(PROJECT_ROOT, "Scenes");
            CreateFolder(PREFABS_PATH, "Zombies");
            CreateFolder(RESOURCES_PATH, "Zombies");
            AssetDatabase.Refresh();
        }

        [MenuItem("Game Setup/2. Create All Zombie Prefabs")]
        public static void CreateAllZombiePrefabs()
        {
            // Tạo Zombie chuẩn Physics (CharacterController) thay vì NavMesh
            CreateZombiePrefab("Zombie_Standard", ZombieType.Standard, Color.green, new Vector3(1, 2, 1));
            CreateZombiePrefab("Zombie_Tank", ZombieType.Tank, Color.red, new Vector3(1.5f, 2.5f, 1.5f));
            CreateZombiePrefab("Zombie_Exploder", ZombieType.Exploder, new Color(1f, 0.5f, 0f), new Vector3(1.2f, 1.8f, 1.2f));
            CreateZombiePrefab("Zombie_Grabber", ZombieType.Grabber, Color.yellow, new Vector3(0.8f, 2.2f, 0.8f));
            
            AssetDatabase.Refresh();
            Debug.Log("✓ Created Physics-based Zombies (WWZ Style)");
        }

        [MenuItem("Game Setup/4. Create Test Scene")]
        public static void CreateCompleteTestScene()
        {
            // 1. Tạo Ground (Fix màu hồng)
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(50, 1, 50);
            Renderer gr = ground.GetComponent<Renderer>();
            if (gr)
            {
                Material mat = new Material(GetDefaultShader()) { color = Color.gray };
                string matPath = MATERIALS_PATH + "Ground_Material.mat";
                if (!System.IO.Directory.Exists(MATERIALS_PATH)) System.IO.Directory.CreateDirectory(MATERIALS_PATH);
                if (!AssetDatabase.LoadAssetAtPath<Material>(matPath)) AssetDatabase.CreateAsset(mat, matPath);
                gr.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(matPath);
            }

            // 2. Tạo Player (Tự động chọn script controller phù hợp)
            GameObject player = CreatePlayer();

            // 3. Môi trường
            CreateSpawnPoints();
            CreateLighting();
            CreateBasicUI();

            // 4. Hệ thống (Managers)
            CreateManagers();

            Debug.Log("✓ Scene Created! (Ready for Play)");
        }

        // --- CÁC HÀM XỬ LÝ CHI TIẾT ---

        private static void CreateFolder(string parent, string folderName)
        {
            if (!AssetDatabase.IsValidFolder(parent + folderName))
                AssetDatabase.CreateFolder(parent.TrimEnd('/'), folderName);
        }

        private static void CreateZombiePrefab(string name, ZombieType type, Color color, Vector3 scale)
        {
            GameObject zombie = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            zombie.name = name;
            zombie.transform.localScale = scale;

            // Fix Material
            Renderer renderer = zombie.GetComponent<Renderer>();
            Material mat = new Material(GetDefaultShader()) { color = color };
            string matPath = MATERIALS_PATH + name + "_Material.mat";
            if (!System.IO.Directory.Exists(MATERIALS_PATH)) System.IO.Directory.CreateDirectory(MATERIALS_PATH);
            if (!AssetDatabase.LoadAssetAtPath<Material>(matPath)) AssetDatabase.CreateAsset(mat, matPath);
            renderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(matPath);

            // Xóa Collider cũ
            Object.DestroyImmediate(zombie.GetComponent<Collider>());

            // --- QUAN TRỌNG: CẤU HÌNH PHYSICS ---
            // 1. Thêm CharacterController (Đây là thứ ZombieAI mới cần)
            CharacterController cc = zombie.AddComponent<CharacterController>();
            cc.height = scale.y;
            cc.radius = scale.x * 0.4f;
            cc.center = new Vector3(0, scale.y / 2, 0);

            // 2. Thêm Script AI
            // Ưu tiên tìm SwarmZombie (code Claude), nếu không thì dùng ZombieAI
            if (System.Type.GetType("ZombieCoopFPS.Enemy.SwarmZombie") != null)
                zombie.AddComponent(System.Type.GetType("ZombieCoopFPS.Enemy.SwarmZombie"));
            else
                zombie.AddComponent<ZombieAI>();

            // Lưu Prefab
            string prefabPath = PREFABS_PATH + "Zombies/" + name + ".prefab";
            if (!System.IO.Directory.Exists(PREFABS_PATH + "Zombies/")) System.IO.Directory.CreateDirectory(PREFABS_PATH + "Zombies/");
            
            PrefabUtility.SaveAsPrefabAsset(zombie, prefabPath);
            AssetDatabase.CopyAsset(prefabPath, RESOURCES_PATH + "Zombies/" + name + ".prefab");

            Object.DestroyImmediate(zombie);
        }

        private static void CreateManagers()
        {
            GameObject managersRoot = new GameObject("=== GAME MANAGERS ===");
            
            // Gắn script GameManagerSetup (Script này sẽ tự đẻ ra các Manager khác khi Play)
            managersRoot.AddComponent<GameManagerSetup>();
            
            // Gắn script Test phím tắt (G, Z, X...)
            // Kiểm tra xem đang dùng bản Universal hay bản thường
            if (System.Type.GetType("ZombieCoopFPS.Setup.GameTesterUniversal") != null)
                managersRoot.AddComponent(System.Type.GetType("ZombieCoopFPS.Setup.GameTesterUniversal"));
            else
                managersRoot.AddComponent<GameTester>();
        }

        private static GameObject CreatePlayer()
        {
            GameObject player = new GameObject("Player");
            player.tag = "Player";
            player.layer = LayerMask.NameToLayer("Default");
            player.transform.position = new Vector3(0, 1, 0);

            // Thêm CharacterController (Quan trọng cho Player mới)
            CharacterController cc = player.AddComponent<CharacterController>();
            cc.height = 2f; cc.radius = 0.5f; cc.center = new Vector3(0, 1, 0);

            // Gắn Script điều khiển (Ưu tiên Universal, fallback về Simple)
            if (System.Type.GetType("ZombieCoopFPS.Player.UniversalPlayerController") != null)
            {
                player.AddComponent(System.Type.GetType("ZombieCoopFPS.Player.UniversalPlayerController"));
                if(System.Type.GetType("ZombieCoopFPS.Player.UniversalPlayerInteraction") != null)
                    player.AddComponent(System.Type.GetType("ZombieCoopFPS.Player.UniversalPlayerInteraction"));
            }
            else if (System.Type.GetType("ZombieCoopFPS.Setup.SimplePlayerController") != null)
            {
                player.AddComponent(System.Type.GetType("ZombieCoopFPS.Setup.SimplePlayerController"));
            }

            // Camera
            GameObject cam = new GameObject("Camera");
            cam.transform.parent = player.transform;
            cam.transform.localPosition = new Vector3(0, 1.6f, 0);
            cam.AddComponent<Camera>();
            cam.AddComponent<AudioListener>();

            // Link Camera vào Script (Dùng Reflection để không bị lỗi nếu khác tên biến)
            MonoBehaviour controller = player.GetComponent<MonoBehaviour>(); // Lấy script đầu tiên
            if (controller)
            {
                SerializedObject so = new SerializedObject(controller);
                var p = so.FindProperty("cameraTransform");
                if (p != null) { p.objectReferenceValue = cam.transform; so.ApplyModifiedProperties(); }
            }

            return player;
        }

        private static void CreateSpawnPoints()
        {
            GameObject root = new GameObject("SpawnPoints");
            for (int i = 0; i < 8; i++)
            {
                GameObject sp = new GameObject($"SpawnPoint_{i}");
                sp.transform.parent = root.transform;
                float angle = i * 45f * Mathf.Deg2Rad;
                sp.transform.position = new Vector3(Mathf.Cos(angle) * 30f, 0.5f, Mathf.Sin(angle) * 30f);
            }
        }

        private static void CreateBasicUI()
        {
            GameObject canvas = new GameObject("UI Canvas");
            canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }

        private static void CreateLighting()
        {
            GameObject sun = new GameObject("Directional Light");
            Light l = sun.AddComponent<Light>();
            l.type = LightType.Directional;
            sun.transform.rotation = Quaternion.Euler(50, -30, 0);
        }
        
        // Dummy methods để giữ menu không bị lỗi compile nếu thiếu script Data
        [MenuItem("Game Setup/3. Create ScriptableObject Data")] public static void CreateData() { Debug.Log("Skipped (Optional)"); }
        [MenuItem("Game Setup/5. Link References")] public static void LinkRefs() { Debug.Log("Skipped (Auto-linked)"); }
    }

    // --- GIAO DIỆN SETUP WINDOW ---
    public class GameSetupWindow : EditorWindow
    {
        [MenuItem("Game Setup/📋 Setup Window")]
        public static void ShowWindow() => GetWindow<GameSetupWindow>("Game Setup").minSize = new Vector2(300, 400);

        private void OnGUI()
        {
            GUILayout.Label("🧟 ZOMBIE WWZ SETUP (FIXED)", EditorStyles.boldLabel);
            GUILayout.Space(20);
            EditorGUILayout.HelpBox("Tools Updated: Physics Based & Universal Input Ready", MessageType.Info);
            
            if (GUILayout.Button("🚀 REBUILD SCENE (One Click)", GUILayout.Height(60)))
            {
                CompleteGameSetup.CreateProjectStructure();
                CompleteGameSetup.CreateAllZombiePrefabs();
                CompleteGameSetup.CreateCompleteTestScene();
                EditorUtility.DisplayDialog("Done!", "Scene rebuilt correctly.\nTry Playing now!", "OK");
            }
        }
    }
}
#endif