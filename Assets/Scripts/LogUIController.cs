using System.Collections;
using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine;

public class LogUIController : MonoBehaviour
{
    [SerializeField] private GameObject logTextPrefab;
    [SerializeField] private float deleteTime = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCompletionMessage(PlayerTask task)
    {
        CreateLog($"+{task.Reward} credits");
        CreateLog($"-{task.CargoUnits} units");
    }

    public void ShowTaskActivation(PlayerTask task)
    {
        CreateLog($"+{task.CargoUnits} units");
    }

    void CreateLog(string message)
    {
        var obj = Instantiate(logTextPrefab, transform);
        obj.GetComponent<TMP_Text>().text = message;
        Destroy(obj, deleteTime);
    }
}
