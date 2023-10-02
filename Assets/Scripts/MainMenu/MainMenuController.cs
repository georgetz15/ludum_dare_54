using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

	public GameObject CreditsBox;
	public void StartGame()
	{
		SceneManager.LoadScene("SampleScene");
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void ShowCredits()
	{
		CreditsBox.SetActive(true);
	}
}
