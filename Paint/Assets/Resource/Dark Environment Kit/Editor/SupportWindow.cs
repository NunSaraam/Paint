using UnityEditor;
using UnityEngine;

namespace LimboStyleEnvironment
{
    public class SupportWindow : EditorWindow
    {
        // Serialized textures for the buttons and logo
        [SerializeField] private Texture documentationButton;
        [SerializeField] private Texture discordButton;
        [SerializeField] private Texture emailButton;
        [SerializeField] private Texture additionalDocButton;
        [SerializeField] private Texture logo;


        // Add a new menu item "Indian Ocean Assets/Support"
        [MenuItem("Tools/Indian Ocean Assets/Support")]
        public static void ShowWindow()
        {
            GetWindow<SupportWindow>("Support");
        }

        private void OnGUI()
        {
            // Set window size
            minSize = new Vector2(600, 400);
            maxSize = new Vector2(600, 400);

            // Header styles
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 28,
                alignment = TextAnchor.MiddleCenter
            };

            GUIStyle subtitleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 15,
                alignment = TextAnchor.MiddleCenter
            };

            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Indian Ocean Assets", titleStyle);

            GUILayout.Space(5);
            GUILayout.Label("\"Creating for Creators\"", subtitleStyle);
            GUILayout.Space(20);

            // Center the buttons
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();
            if (GUILayout.Button(documentationButton, GUILayout.Width(100), GUILayout.Height(100)))
            {
                Application.OpenURL(CustomInspectorButtons.DocumentationPath);
            }
            GUILayout.Label("Documentation", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
            GUILayout.EndVertical();

            GUILayout.Space(20);

            GUILayout.BeginVertical();
            if (GUILayout.Button(discordButton, GUILayout.Width(100), GUILayout.Height(100)))
            {
                Application.OpenURL(CustomInspectorButtons.DiscordUrl);
            }
            GUILayout.Label("Discord", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
            GUILayout.EndVertical();

            GUILayout.Space(20);

            GUILayout.BeginVertical();
            if (GUILayout.Button(emailButton, GUILayout.Width(100), GUILayout.Height(100)))
            {
                Application.OpenURL(CustomInspectorButtons.Email);
            }
            GUILayout.Label("Email", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
            GUILayout.EndVertical();

            GUILayout.Space(20);

            GUILayout.BeginVertical();
            if (GUILayout.Button(additionalDocButton, GUILayout.Width(100), GUILayout.Height(100)))
            {
                Application.OpenURL(CustomInspectorButtons.TutorialUrl);
            }
            GUILayout.Label("Tutorials", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(30);

            // Review Section
            GUIStyle reviewStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter
            };

            GUILayout.Label("Your review helps us grow and continue making amazing assets.\nIf you love our work, please take a moment to share your thoughts!", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 12, wordWrap = true });
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Rate This Asset", reviewStyle, GUILayout.Width(150), GUILayout.Height(30)))
            {
                Application.OpenURL(CustomInspectorButtons.AssetReviewUrl);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
    }
}
