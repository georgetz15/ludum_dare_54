using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpaceshipStatsUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text cargoText;
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text creditsText;

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

    public void OnDaysChanged(int day)
    {
        dateText.text = $"Day: {day}";
    }

    public void OnCreditsChanged(int credits)
    {
        creditsText.text = $"Credits: {credits}";
    }
}
