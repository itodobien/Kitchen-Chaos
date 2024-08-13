using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    // Reference to the BaseCounter that this visual represents
    [SerializeField] private BaseCounter baseCounter;

    // Array of GameObjects that represent the visual elements to be shown or hidden
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start()
    {
        // Subscribe to the OnSelectedCounterChanged event from the Player instance
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        // Check if the selected counter is the same as this base counter
        if (e.selectedCounter == baseCounter)
        {
            // Show the visual elements if the selected counter matches
            Show();
        }
        else
        {
            // Hide the visual elements if the selected counter does not match
            Hide();
        }
    }

    private void Show()
    {
        // Enable all visual GameObjects in the array
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        // Disable all visual GameObjects in the array
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
