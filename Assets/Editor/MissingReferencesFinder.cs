using UnityEngine;
using UnityEditor;
using System.Linq;

public class MissingReferencesFinder : EditorWindow
{
    [MenuItem("Tools/Find Missing References")]
    public static void ShowWindow()
    {
        GetWindow<MissingReferencesFinder>("Missing References Finder");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Find Missing References in Scene"))
        {
            FindMissingReferences();
        }
    }

    private void FindMissingReferences()
    {
        Debug.Log("Starting search for missing references...");
        int objectsChecked = 0;
        int missingRefsFound = 0;

        var objects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var go in objects)
        {
            objectsChecked++;
            var components = go.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component == null)
                {
                    Debug.LogError($"Missing Component in GameObject: {go.name}", go);
                    missingRefsFound++;
                    continue;
                }

                SerializedObject so = new SerializedObject(component);
                var sp = so.GetIterator();

                while (sp.NextVisible(true))
                {
                    if (sp.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (sp.objectReferenceValue == null && sp.objectReferenceInstanceIDValue != 0)
                        {
                            ShowError(go, component.GetType().Name, ObjectNames.NicifyVariableName(sp.name));
                            missingRefsFound++;
                        }
                    }
                }
            }
        }

        Debug.Log($"Search completed. Checked {objectsChecked} objects. Found {missingRefsFound} missing references.");
    }

    private void ShowError(GameObject go, string componentName, string propertyName)
    {
        Debug.LogError($"Missing reference in GameObject: {go.name}, Component: {componentName}, Property: {propertyName}", go);
    }
}