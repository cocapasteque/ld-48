using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public Building BuildingPrefab;
    public Text ButtonText;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        ButtonText.text = $"{BuildingPrefab.Name} ({BuildingPrefab.Cost})";
        button.onClick.AddListener(SelectBuilding);
    }

    private void Update()
    {
        button.interactable = DiggingManager.Instance.Gems >= BuildingPrefab.Cost;
    }

    private void SelectBuilding()
    {
        Layout.GridSystem.Instance.SetBuilding(BuildingPrefab);
    }
}