using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text upgradeText;
    [SerializeField] private TMP_Text costText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetUpgrade(Upgrade upgrade)
    {
        upgradeText.text = upgrade.upgradeType switch
        {
            Upgrade.Type.Cargo => $"+{upgrade.amount} Cargo",
            Upgrade.Type.Range => $"+{upgrade.amount} Range",
            _ => upgradeText.text
        };
        costText.text = $"Cost: {upgrade.cost} Credits";
        var btn = GetComponent<Button>();
        switch (upgrade.upgradeType)
        {
            case Upgrade.Type.Cargo:
                btn.onClick.AddListener(delegate 
                {
                    Debug.Log("Used Cargo upgrade");
                    var sc = GameObject.FindWithTag("SpaceshipController")?.GetComponent<SpaceShipController>();
                    if (sc is null) return;
                    if (sc.Credits < upgrade.cost) return;
                    
                    sc.Credits -= upgrade.cost;
                    sc.UpgradeCargo(upgrade.amount);
                    Destroy(gameObject);
                });
                break;
            case Upgrade.Type.Range:
                btn.onClick.AddListener(delegate 
                {
                    Debug.Log("Used Range upgrade");
                    var sc = GameObject.FindWithTag("SpaceshipController")?.GetComponent<SpaceShipController>();
                    if (sc is null) return;
                    if (sc.Credits < upgrade.cost) return;
                    
                    sc.Credits -= upgrade.cost;
                    sc.UpgradeRange(upgrade.amount);
                    Destroy(gameObject);
                });
                break;
        }
    }
}