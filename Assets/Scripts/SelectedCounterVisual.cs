using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{ 
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject visualGameObject;


    private void Start()
    {
        // Subscribe to the OnSelectedCounterChanged event
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }
    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        // Set the position of the visual to the selected counter's position
        if (e.selectedCounter == clearCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Show()
    {
        visualGameObject.SetActive(true);
    }
    private void Hide()
    {
        visualGameObject.SetActive(false);
    }
}
