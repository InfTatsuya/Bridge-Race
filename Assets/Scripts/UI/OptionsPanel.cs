using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
    [SerializeField] Button exitButton;


    [SerializeField] Toggle musicToggle;
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] GameObject musicOffImage;

    private void Start()
    {
        exitButton.onClick.AddListener(ReturnToMenu);
        musicToggle.onValueChanged.AddListener(ToggleMusic);
    }

    private void ReturnToMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void ToggleMusic(bool isOn)
    {
        masterMixer.SetFloat(StringCollection.masterVolumeString, isOn ? 10 : -80); 
        musicOffImage.SetActive(!isOn);
    }
}
