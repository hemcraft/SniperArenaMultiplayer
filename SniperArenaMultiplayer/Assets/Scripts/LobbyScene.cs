using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyScene : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject NickNamePanel;

    public TMP_InputField NickNameInputField;

    private GameSession gameSession;

    private void Start()
    {
        gameSession = FindObjectOfType<GameSession>();

        ShowMainPanel();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void SetNickName()
    {
        ShowNickNamePanel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowMainPanel()
    {
        MainPanel.SetActive(true);
        NickNamePanel.SetActive(false);
    }

    public void ShowNickNamePanel()
    {
        MainPanel.SetActive(false);
        NickNamePanel.SetActive(true);
    }

    public void SaveNickName()
    {
        string savedNickName = NickNameInputField.text;

        gameSession.nickName = savedNickName;
    }
}
