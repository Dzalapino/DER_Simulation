using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        
        // Init buttons to be used later
        Button buttonCreateHouse = root.Q<Button>("CreateHouse");
        Button buttonCreateCity = root.Q<Button>("CreateCity");
        Button buttonCreateEnergyResource = root.Q<Button>("CreateEnergyResource");
        
        // Add desired events for buttons
        buttonCreateHouse.clicked += () => { Debug.Log("XD"); };
    }
}
