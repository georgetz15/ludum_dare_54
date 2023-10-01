using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class UpgradesController : MonoBehaviour
{
    [SerializeField] private List<Upgrade> availableUpgrades;
    [SerializeField] private GameObject upgradeUIItem;
    
    // Start is called before the first frame update
    void Awake()
    {
        foreach (var upgrade in availableUpgrades)
        {
            var item = Instantiate(upgradeUIItem, transform);
            var upgradeUIController = item.GetComponent<UpgradeUIController>();
            upgradeUIController.SetUpgrade(upgrade);
        }
    }
}
