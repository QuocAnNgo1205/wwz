
using UnityEngine;       
using UnityEditor;       
using ZombieCoopFPS.Setup;

namespace ZombieCoopFPS.Editor
{
    #if UNITY_EDITOR
    using UnityEditor;
    using ZombieCoopFPS.Setup;
    
    /// <summary>
    /// Editor menu items for quick setup
    /// </summary>
    public static class GameSetupMenu
    {
        [MenuItem("Game Setup/Create Game Managers")]
        public static void CreateGameManagers()
        {
            GameObject setupObj = new GameObject("GameSetup");
            GameManagerSetup setup = setupObj.AddComponent<GameManagerSetup>();
            setup.SetupManagers();
            
            Debug.Log("Game managers created! Check hierarchy.");
        }
        
        [MenuItem("Game Setup/Create Test Scene")]
        public static void CreateTestScene()
        {
            // Create ground
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(10, 1, 10);
            ground.layer = LayerMask.NameToLayer("Default");
            
            // Create player
            GameObject player = new GameObject("Player");
            player.tag = "Player";
            player.AddComponent<SimplePlayerController>();
            player.AddComponent<CapsuleCollider>();
            player.transform.position = new Vector3(0, 1, 0);
            
            // Create camera
            GameObject cam = new GameObject("Camera");
            cam.transform.parent = player.transform;
            cam.transform.localPosition = new Vector3(0, 0.5f, 0);
            Camera camera = cam.AddComponent<Camera>();
            cam.AddComponent<AudioListener>();
            
            // Create light
            GameObject light = new GameObject("Directional Light");
            Light lightComp = light.AddComponent<Light>();
            lightComp.type = LightType.Directional;
            light.transform.rotation = Quaternion.Euler(50, -30, 0);
            
            // Create game tester
            GameObject tester = new GameObject("GameTester");
            tester.AddComponent<GameTester>();
            
            // Create managers
            CreateGameManagers();
            
            Debug.Log("✓✓✓ Test scene created! Press G to start game, Z to spawn zombies.");
        }
        
        [MenuItem("Game Setup/Create Zombie Prefab")]
        public static void CreateZombiePrefab()
        {
            GameObject zombie = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            zombie.name = "Zombie_Standard";
            
            // Use sharedMaterial in edit mode to avoid memory leak
            Renderer renderer = zombie.GetComponent<Renderer>();
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = Color.green;
            renderer.sharedMaterial = mat;
            
            ZombiePrefabSetup setup = zombie.AddComponent<ZombiePrefabSetup>();
            setup.SetupComponents();
            
            // Create folder structure
            string path = "Assets/_Project/Prefabs/Zombies/";
            if (!AssetDatabase.IsValidFolder("Assets/_Project"))
                AssetDatabase.CreateFolder("Assets", "_Project");
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Prefabs"))
                AssetDatabase.CreateFolder("Assets/_Project", "Prefabs");
            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder("Assets/_Project/Prefabs", "Zombies");
            
            // Save as prefab
            string prefabPath = path + zombie.name + ".prefab";
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(zombie, prefabPath);
            
            // Save material as asset
            string matPath = "Assets/_Project/Materials/";
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Materials"))
                AssetDatabase.CreateFolder("Assets/_Project", "Materials");
            AssetDatabase.CreateAsset(mat, matPath + "ZombieMaterial.mat");
            
            // Cleanup
            UnityEngine.Object.DestroyImmediate(zombie);
            
            // Select the created prefab
            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);
            
            Debug.Log($"✓ Zombie prefab created at {prefabPath}");
        }
    }
    #endif
}