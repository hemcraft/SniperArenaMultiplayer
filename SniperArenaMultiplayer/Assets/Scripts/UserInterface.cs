using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public TextMeshProUGUI healthValueText;

    public TextMeshProUGUI MainPlayerNameText;
    public TextMeshProUGUI MainPlayerScoreText;
    public TextMeshProUGUI EnemyPlayerNameText;
    public TextMeshProUGUI EnemyPlayerScoreText;
    public GameObject EnemyPlayerScorePanel;

    public TextMeshProUGUI WaitingForOtherPlayersText;

    public Image RedBackgroundImage;
    public Image GreenBackgroundImage;

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

    public void EnemyPlayerLeft(NetworkRunner runner)
    {
        mainPlayerRig.Score = 0;
        mainPlayerRig.Health = 100;

        NetworkObject enemyPlayerNetworkObject = enemyPlayerRig.GetComponent<NetworkObject>();
        StartCoroutine(DespawnEnemyInSeconds(runner, enemyPlayerNetworkObject));

        enemyPlayerRig = null;

        ShowWaitingForOtherPlayersText(true);
        ShowEnemyPlayerScore(false);
    }

    IEnumerator DespawnEnemyInSeconds(NetworkRunner runner, NetworkObject enemyPlayerNetworkObject)
    {
        enemyPlayerNetworkObject.RequestStateAuthority();

        yield return new WaitForSeconds(1f);

        runner.Despawn(enemyPlayerNetworkObject);
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

    public void ShowRedPanel()
    {
        StartCoroutine(ShowRedPanelInSeconds());
    }

    IEnumerator ShowRedPanelInSeconds()
    {
        RedBackgroundImage.enabled = true;

        yield return new WaitForSeconds(2f);

        RedBackgroundImage.enabled = false;
    }

    public void ShowGreenPanel()
    {
        StartCoroutine(ShowGreenPanelInSeconds());
    }

    IEnumerator ShowGreenPanelInSeconds()
    {
        GreenBackgroundImage.enabled = true;

        yield return new WaitForSeconds(2f);

        GreenBackgroundImage.enabled = false;
    }
}
