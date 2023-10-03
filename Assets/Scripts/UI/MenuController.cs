using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSourceInfo
{
    public AudioSource src {  get; set; }
    public float initialVol { get; set; }
}

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject Menu;
    [SerializeField] private Toggle MenuToggle;
	[SerializeField] private TMP_Text muteButtonText;

    private float FadeDuration = 0.1f;

    private List<AudioSourceInfo> audioSources = new List<AudioSourceInfo>();
	private void Awake()
	{
		var srcObj = GameController.GetObjectsInLayer(LayerMask.GetMask("Audio")).ToList();
        foreach (var src in srcObj)
        {
            var castSrc = src.gameObject.GetComponent<AudioSource>();

			AudioSourceInfo tmp = new AudioSourceInfo();
            tmp.src = castSrc;
            tmp.initialVol = castSrc.volume;
            audioSources.Add(tmp);
        }
	}

	public void ToggleMenu()
    {
        if (Menu.activeSelf)
        {
            Menu.SetActive(false);
            MenuToggle.isOn = false;
        }
        else
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

    public void Help()
    {
        MessageBox.Instance.DisplayMsgWithoutAutohide("- Click on a planet to travel\n\n" +
                                                      "- Hover over missions to see origin and destination\n\n" +
                                                      "- Click on a mission to load cargo\n\n" +
                                                      "- Travel to destination to auto-complete mission\n\n" +
                                                      "- Upgrade your ship's cargo capacity and range of travel");
    }

    public void Mute()
    {
        foreach (var audioSource in audioSources)
        {
            var src = audioSource.src;

            var isMuted = (src.volume == 0f); 

            if (isMuted)
            {
                StartCoroutine(FadeVolume(src, audioSource.initialVol));
            } else
            {
                StartCoroutine(FadeVolume(src, 0f));
            }
		}
	}

	private IEnumerator FadeVolume(AudioSource src, float targetVolume)
	{
		float startVolume = src.volume;
		float startTime = Time.time;

		while (Time.time - startTime < FadeDuration)
		{
			float elapsed = Time.time - startTime;
			float t = elapsed / FadeDuration;
			src.volume = Mathf.Lerp(startVolume, targetVolume, t);
			yield return null;
		}

		src.volume = targetVolume;
	}
}