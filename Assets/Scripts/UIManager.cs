using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField]
    private Button buttonSpecialDice, buttonNormalDice, buttonEndGame;
    [SerializeField]
    private GameObject PanelPlayerTurn, PanelIATurn;

    [SerializeField]
    private Text UIGenericMessage;

    #region Inizialization
    private void Awake()
    {
        Instance = this;
        UIGenericMessage.text = "";
        buttonEndGame.gameObject.SetActive(false);
        buttonEndGame.onClick.AddListener(ResetGame);
    }
    public void AssignPlayerButtons(PlayerController playerController)
    {
        buttonNormalDice.onClick.AddListener(playerController.PlayerTrowNormalDice);
        buttonSpecialDice.onClick.AddListener(playerController.PlayerTrowSpecialDice);

        buttonNormalDice.onClick.AddListener(DisableButtonPlaer);
        buttonSpecialDice.onClick.AddListener(DisableButtonPlaer);

    }
    #endregion

    #region visual behavour
    public void SetDiceResult(int diceRes)
    {
        UIGenericMessage.text = "Dice Result = " + diceRes.ToString();
    }
    public void SetGenericMessage(string message)
    {
        UIGenericMessage.text = message;
    }
    public void EnableButtonPlaer()
    {
        SetPlayerButtonEnabled(true);
        if (PlayerController.Instance.CountBeforeSpecialDice <= 0)
            buttonSpecialDice.interactable = true;
        else
            buttonSpecialDice.interactable = false;
    }
    public void DisableButtonPlaer()
    {
        SetPlayerButtonEnabled(false);
    }
    private void SetPlayerButtonEnabled(bool enabled)
    {
        buttonSpecialDice.gameObject.SetActive(enabled);
        buttonNormalDice.gameObject.SetActive(enabled);
    }
    public void ChangeTurn(bool isPlayerTurn)
    {
        UIGenericMessage.text = "";
        PanelPlayerTurn.SetActive(isPlayerTurn);
        PanelIATurn.SetActive(!isPlayerTurn);

        if (isPlayerTurn)
            EnableButtonPlaer();
    }

    #endregion

    #region game behavour
    public void EndGaame()
    {
        buttonEndGame.gameObject.SetActive(true);
        PanelPlayerTurn.SetActive(false);
        PanelIATurn.SetActive(false);
    }
    public void ResetGame()
    {
        GameManager.Instance.ResetGame();
    }

    #endregion



}
