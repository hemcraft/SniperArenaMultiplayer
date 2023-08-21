using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    public TextMeshProUGUI healthValueText;

    public TextMeshProUGUI MainPlayerNameText;
    public TextMeshProUGUI MainPlayerScoreText;
    public TextMeshProUGUI EnemyPlayerNameText;
    public TextMeshProUGUI EnemyPlayerScoreText;
    public GameObject EnemyPlayerScorePanel;

    public TextMeshProUGUI WaitingForOtherPlayersText;

    public NetworkPlayerRig mainPlayerRig;
    public NetworkPlayerRig enemyPlayerRig;

    private void Start()
    {
        ShowWaitingForOtherPlayersText(true);
        ShowEnemyPlayerScore(false);

        StartCoroutine(RefreshScoreBoardEverySecond());
    }

    public void SetMainPlayerRig(NetworkPlayerRig playerRig)
    {
        mainPlayerRig = playerRig;
        MainPlayerNameText.text = playerRig.PlayerNickName.ToString();
    }

    public void SetEnemyPlayerRig(NetworkPlayerRig playerRig)
    {
        enemyPlayerRig = playerRig;
        EnemyPlayerNameText.text = playerRig.PlayerNickName.ToString();

        ShowWaitingForOtherPlayersText(false);
        ShowEnemyPlayerScore(true);
    }

    private void ShowWaitingForOtherPlayersText(bool value)
    {
        WaitingForOtherPlayersText.gameObject.SetActive(value);
    }

    private void ShowEnemyPlayerScore(bool value)
    {
        EnemyPlayerScorePanel.SetActive(value);
    }

    IEnumerator RefreshScoreBoardEverySecond()
    {
        yield return new WaitForSeconds(1f);

        if (mainPlayerRig != null)
        {
            MainPlayerScoreText.text = mainPlayerRig.Score.ToString();
        }

        if (enemyPlayerRig != null)
        {
            EnemyPlayerScoreText.text = enemyPlayerRig.Score.ToString();
        }

        StartCoroutine(RefreshScoreBoardEverySecond());
    }
}
