using UnityEngine;
using UnityEditor;

public class SimpleMissingRefFinder : EditorWindow
{
    [MenuItem("Tools/Simple Missing Ref Finder")]
    public static void ShowWindow()
    {
        GetWindow<SimpleMissingRefFinder>("Simple Finder");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Find Missing"))
        {
            FindMissing();
        }
    }

    void FindMissing()
    {
        var allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (var go in allObjects)
        {
            var components = go.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component == null)
                {
                    Debug.Log($"Missing Component in GameObject: {go.name}", go);
                }
            }
        }
        Debug.Log("Search completed.");
    }
}