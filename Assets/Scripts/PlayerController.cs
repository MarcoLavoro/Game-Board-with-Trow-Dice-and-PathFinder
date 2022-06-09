using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    public static PlayerController Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovements();
    }

    public void PlayerTrowNormalDice()
    {
        DiceSwipeControl.Instance.TrowDice(false);
        RecargeSpecialDice();
    }

    public void PlayerTrowSpecialDice()
    {
        DiceSwipeControl.Instance.TrowDice(true);
        UsedSpecialDice();

    }
    public override void CharacterWinner()
    {
        Debug.Log("PLAYER WIN");
        UIManager.Instance.SetGenericMessage("PLAYER WIN");
        GameManager.Instance.EndGame();


    }

}
