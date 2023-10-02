using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	[SerializeField] private GameObject Menu;
	[SerializeField] private Toggle MenuToggle;

	public void ToggleMenu()
	{
		if (Menu.activeSelf)
		{
			Menu.SetActive(false);
			MenuToggle.isOn = false;
		} else
		{
			Menu.SetActive(true);
			MenuToggle.isOn = true;
		}
	}

	public void RestartSecene()
	{
		SceneManager.LoadScene("SampleScene");
	}

	public void CloseMenu()
	{
		if (Menu.activeSelf)
		{
			ToggleMenu();
		}
	}
	
	public void Quit()
	{
		Application.Quit();
	}
}
