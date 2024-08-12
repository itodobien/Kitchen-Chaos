using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint; // Point where plates will be visually stacked
    [SerializeField] private Transform plateVisualPrefab; // Prefab for the plate visual
    [SerializeField] private PlatesCounter platesCounter; // Reference to the PlatesCounter

    private List<GameObject> plateVisualGameObjectList; // List to keep track of plate visuals

    private void Awake()
    {
        // Initialize the list of plate visuals
        plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        // Subscribe to the plate spawned and removed events
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    // Event handler for when a plate is removed
    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        // Get the last plate visual in the list
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        // Remove it from the list
        plateVisualGameObjectList.Remove(plateGameObject);
        // Destroy the plate visual
        Destroy(plateGameObject);
    }

    // Event handler for when a plate is spawned
    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        // Instantiate a new plate visual at the counter top point
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        // Offset for stacking plates visually
        float plateOffsetY = .1f;

        // Set the local position of the new plate visual
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);

        // Add the new plate visual to the list
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
