using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpaceshipStatsUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text cargoText;

    [SerializeField] private SpaceShipController spaceShipController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateCargoText()
    {
        var cargo = spaceShipController.Cargo;
        var maxCargo = spaceShipController.MaxCargoCapacity;
        cargoText.text = $"Cargo: {cargo}/{maxCargo} Units";
    }
}
