using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CargoItemController : MonoBehaviour
{
    public void SetImage(Sprite sprite)
    {
        this.transform.Find("Image").GetComponent<Image>().sprite = sprite;
    }
}
