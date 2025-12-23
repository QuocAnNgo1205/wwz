using UnityEngine;

namespace ZombieCoopFPS.Utilities
{
    public class FPSDisplay : MonoBehaviour
    {
        [Header("Cấu hình")]
        [SerializeField] private bool showFPS = true;
        [SerializeField] private int targetFPS = 60;
        
        private float deltaTime = 0.0f;
        private GUIStyle style = new GUIStyle();
        private Rect rect;

        void Update()
        {
            // Tính toán thời gian giữa các khung hình (làm mượt số liệu)
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }

        void OnGUI()
        {
            if (!showFPS) return;

            int w = Screen.width, h = Screen.height;

            // Cấu hình khung hiển thị
            if (rect.width != w) 
            {
                rect = new Rect(20, 20, w, h * 2 / 100);
            }

            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 3 / 100; // Cỡ chữ to bằng 3% chiều cao màn hình

            // Tính toán FPS
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;

            // Đổi màu dựa trên hiệu năng
            if (fps >= targetFPS - 5)
                style.normal.textColor = Color.green; // Mượt (Xanh)
            else if (fps >= targetFPS / 2)
                style.normal.textColor = Color.yellow; // Tạm ổn (Vàng)
            else
                style.normal.textColor = Color.red;    // Lag (Đỏ)

            // Hiển thị text: "60 FPS (16.6 ms)"
            string text = string.Format("{0:0.} FPS ({1:0.0} ms)", fps, msec);
            
            // Vẽ bóng đổ màu đen cho dễ đọc
            GUIStyle shadowStyle = new GUIStyle(style);
            shadowStyle.normal.textColor = Color.black;
            GUI.Label(new Rect(rect.x + 2, rect.y + 2, rect.width, rect.height), text, shadowStyle);
            
            // Vẽ chữ chính
            GUI.Label(rect, text, style);
        }
    }
}