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
    public static event EventHandler onReturnToMenu;

    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject menuUI;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject optionsPanel;

    [Space, Header("InGame Pannel Components")]
    [SerializeField] Button returnMainMenuButton;

    [Space, Header("MainMenu Pannel Components")]
    [SerializeField] TMP_InputField playerNameInput;
    [SerializeField] Button playButton;
    [SerializeField] Button optionButton;

    [Space, Header("Win Pannel Components")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Button nextLevel;

    private void Start()
    {
        WinTrigger.onCharacterWin += WinTrigger_onCharacterWin;
        nextLevel.onClick.AddListener(NextLevel);
        playButton.onClick.AddListener(PlayGame);
        playerNameInput.onEndEdit.AddListener(EditPlayerName);
        optionButton.onClick.AddListener(SwitchToOptionPanel);
        returnMainMenuButton.onClick.AddListener(ReturnToMainMenu);

        SwitchTo(menuUI);
    }

    private void EditPlayerName(string playerName)
    {
        PlayerRef.SetPlayerName(playerName);
    }

    private void WinTrigger_onCharacterWin(object sender, WinTrigger.OnCharacterWinArgs e)
    {
        SwitchTo(winPanel);
        if((e.character as Player) != null)
        {
            nameText.text = PlayerRef.PlayerName.ToUpper();
        }
        else
        {
            nameText.text = e.character.gameObject.name.ToUpper();
        }
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

    private void SwitchToOptionPanel()
    {
        optionsPanel.SetActive(true);
    }

    private void ReturnToMainMenu()
    {
        SwitchTo(menuUI);
        onNextLevel?.Invoke(this, EventArgs.Empty);
        LevelManager.Instance.ResetLevel();
        onReturnToMenu?.Invoke(this, EventArgs.Empty);
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
