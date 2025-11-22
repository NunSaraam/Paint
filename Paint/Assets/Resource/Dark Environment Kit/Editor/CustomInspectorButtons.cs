using UnityEditor;
using UnityEngine;

namespace LimboStyleEnvironment
{
    [CustomEditor(typeof(PlayerController))]
    public class CustomInspectorButtons : Editor
    {
        // Define constants for URLs
        public static string DiscordUrl = "https://discord.gg/utgDzuksJ8";
        public static string TutorialUrl = "https://www.youtube.com/@indianoceanassets/videos";
        public static string DocumentationPath = "https://indianoceanassets.notion.site/Archery-Engine-19ca69905b8b80c09b29d35cccefbe7c";
        public static string Email = "mailto:support@indianoceanassets.com";
        public static string AssetReviewUrl = "https://assetstore.unity.com/packages/slug/308052";

        public override void OnInspectorGUI()
        {
            // Add a space at the top
            GUILayout.Space(5);

            // ======= Header: "ARCHERY ENGINE" in Yellow =======
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = 28;
            headerStyle.normal.textColor = Color.yellow;
            headerStyle.alignment = TextAnchor.MiddleCenter;

            GUILayout.Label("LIMBO-STYLE ENV", headerStyle);
            GUILayout.Space(5);

            // ======= Buttons at the Top =======
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Documentation", GUILayout.Height(25)))
            {
                Application.OpenURL(DocumentationPath);
            }

            if (GUILayout.Button("Tutorial", GUILayout.Height(25)))
            {
                Application.OpenURL(TutorialUrl);
            }

            if (GUILayout.Button("Join Discord", GUILayout.Height(25)))
            {
                Application.OpenURL(DiscordUrl);
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            // ======= Rate This Asset =======
            if (GUILayout.Button("Rate This Asset", GUILayout.Height(30)))
            {
                Application.OpenURL(AssetReviewUrl);
            }

            GUILayout.Space(10);

            // Draw the default Inspector (after buttons)
            DrawDefaultInspector();
        }
    }
}