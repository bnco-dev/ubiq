using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace Ubiq.Samples.Demo.Editor
{
    internal static class RequireSamplesXRIUtil
    {
        public const string DEMO_SCENE_GUID = "c2a1c4e636b99c04fa572a8f01cf438b";
        public const string XRI_SAMPLE_PLAYER_GUID = "f6336ac4ac8b4d34bc5072418cdc62a0";

        public static bool AssetExistsFromGUID(string guid)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            // If an asset previously existed in this editor session, Unity will
            // cache the path and still return it, even if the asset is missing.
            // We use the OnlyExistingAssets option (only available on
            // AssetPathToGUID) to make sure it's really present.
            var resultGuid = AssetDatabase.AssetPathToGUID(path,AssetPathToGUIDOptions.OnlyExistingAssets);
            return !string.IsNullOrEmpty(resultGuid);
        }
    }

#if XRI_2_4_3_OR_NEWER
    [InitializeOnLoad]
    public class RequireSamplesXRI
    {
        static RequireSamplesXRI()
        {
            EditorSceneManager.activeSceneChangedInEditMode += ActiveSceneChangedInEditMode;
        }

        static void ActiveSceneChangedInEditMode(Scene prev, Scene next)
        {
            var openedSceneGuid = AssetDatabase.AssetPathToGUID(next.path);
            if (openedSceneGuid != RequireSamplesXRIUtil.DEMO_SCENE_GUID)
            {
                return;
            }

            // We have opened the demo scene. Check AssetDatabase to make sure
            // the XRI StarterAssets are loaded
            if (RequireSamplesXRIUtil.AssetExistsFromGUID(RequireSamplesXRIUtil.XRI_SAMPLE_PLAYER_GUID))
            {
                return;
            }

            // XRI StarterAssets are not loaded. Popup a window to let the user know
            var window = EditorWindow.GetWindow<RequireSamplesXRIWindow>();
            var size = new Vector2(520, 180);
            var center = EditorGUIUtility.GetMainWindowPosition().center;
            var position = center - size * 0.5f;

            window.titleContent = new GUIContent("XRI Samples Required");
            window.minSize = size;
            window.position = new Rect(position,size);
        }
    }
#endif

    public class RequireSamplesXRIWindow : EditorWindow
    {
        void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            var header = new Label(
                "<b>Ubiq Demo: Please import XRI Samples</b>"
            );
            header.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
            header.style.fontSize = 18;
            header.style.paddingTop = 18;
            header.style.paddingBottom = 18;
            root.Add(header);

            // Check AssetDatabase to see whether the XRI StarterAssets are loaded
            if (RequireSamplesXRIUtil.AssetExistsFromGUID(RequireSamplesXRIUtil.XRI_SAMPLE_PLAYER_GUID))
            {
                // Starter assets loaded. This means the user has added them
                // while this window was open.
                var thanks = new Label(
                    "XRI Samples detected!\n\nThank you. Please close this" +
                    " window. It will not be shown again.");
                thanks.style.whiteSpace = new StyleEnum<WhiteSpace>(WhiteSpace.Normal);
                thanks.style.paddingBottom = 18;
                thanks.style.paddingLeft = 10;
                thanks.style.paddingRight = 10;
                root.Add(thanks);
                return;
            }

            // XRI StarterAssets not found. Give user instructions on importing
            var instructions = new Label(
                "This scene requires the StarterAssets and XR Device" +
                " Simulator from Unity's XR" +
                " Interaction Toolkit, and it looks like these are not" +
                " yet imported in your project. To get this scene" +
                " working correctly, please import them manually at the" +
                " following:");
            instructions.style.whiteSpace = new StyleEnum<WhiteSpace>(WhiteSpace.Normal);
            instructions.style.paddingBottom = 18;
            instructions.style.paddingLeft = 10;
            instructions.style.paddingRight = 10;
            root.Add(instructions);

            var path = new Label(
                "Window > Package Manager > XR Interaction Toolkit > Starter Assets\n" +
                "Window > Package Manager > XR Interaction Toolkit > XR Device Simulator");
            path.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
            path.style.paddingLeft = 16;
            path.style.paddingRight = 16;
            root.Add(path);
        }
    }
}