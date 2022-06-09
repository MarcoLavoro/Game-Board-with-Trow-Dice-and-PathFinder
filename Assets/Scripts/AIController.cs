using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : CharacterController
{
    public static AIController Instance;
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
    public void MadeMove()
    {
        StartCoroutine(WaitAndMakeMove());
    }
    IEnumerator WaitAndMakeMove()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("IA roll the dice");
        if (CountBeforeSpecialDice <= 0)
        {
            DiceSwipeControl.Instance.TrowDice(true);
            UsedSpecialDice();
        }
        else
        {
            DiceSwipeControl.Instance.TrowDice(false);

            RecargeSpecialDice();
        }
    }
    public override void CharacterWinner()
    {
        Debug.Log("IA WIN");
        UIManager.Instance.SetGenericMessage("IA WIN");
        GameManager.Instance.EndGame();
    }
}
