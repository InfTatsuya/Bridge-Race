using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static event EventHandler onPlayGame;
    public static event EventHandler onNextLevel;

    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject menuUI;
    [SerializeField] GameObject winPanel;

    [SerializeField] Button playButton;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Button nextLevel;

    private void Start()
    {
        WinTrigger.onCharacterWin += WinTrigger_onCharacterWin;
        nextLevel.onClick.AddListener(NextLevel);
        playButton.onClick.AddListener(PlayGame);

        SwitchTo(menuUI);
    }

    private void WinTrigger_onCharacterWin(object sender, WinTrigger.OnCharacterWinArgs e)
    {
        SwitchTo(winPanel);
        nameText.text = e.character.gameObject.name.ToUpper();
    }

    private void PlayGame()
    {
        SwitchTo(inGameUI);
        onPlayGame?.Invoke(this, EventArgs.Empty);
    }

    private void NextLevel()
    {
        LevelManager.Instance.NextLevel();
        SwitchTo(inGameUI);
        onNextLevel?.Invoke(this, EventArgs.Empty);
    }

    private void SwitchTo(GameObject ui)
    {
        DeactiveAll();
        ui.gameObject.SetActive(true);
    }

    private void DeactiveAll()
    {
        inGameUI.SetActive(false);
        menuUI.SetActive(false);
        winPanel.SetActive(false);
    }
}
