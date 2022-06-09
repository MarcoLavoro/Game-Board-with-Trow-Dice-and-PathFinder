using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour
{
    public int diceCount;
    public DiceType type;
    public static Dice Instance;

    internal Vector3 initPos;
    private void Awake()
    {
        GetComponent<Rigidbody>().solverIterations = 250;
        Instance = this;
        initPos = transform.position;
    }
    void OnEnable()
    {
        initPos = transform.position;
    }

    public int GetDiceCount()
    {
        diceCount = 0;
        if (type == DiceType.Standard)
            regularDiceCount();
        else
            specialDiceCount();

       
        return diceCount;
    }

    void regularDiceCount()
    {
        if (Vector3.Dot(transform.forward, Vector3.up) > 0.6f)
            diceCount = 5;
        if (Vector3.Dot(-transform.forward, Vector3.up) > 0.6f)
            diceCount = 2;
        if (Vector3.Dot(transform.up, Vector3.up) > 0.6f)
            diceCount = 3;
        if (Vector3.Dot(-transform.up, Vector3.up) > 0.6f)
            diceCount = 4;
        if (Vector3.Dot(transform.right, Vector3.up) > 0.6f)
            diceCount = 6;
        if (Vector3.Dot(-transform.right, Vector3.up) > 0.6f)
            diceCount = 1;

    }

    void specialDiceCount()
    {
        if (Vector3.Dot(transform.forward, Vector3.up) > 0.6f)
            diceCount = 10;
        if (Vector3.Dot(-transform.forward, Vector3.up) > 0.6f)
            diceCount = 7;
        if (Vector3.Dot(transform.up, Vector3.up) > 0.6f)
            diceCount = 9;
        if (Vector3.Dot(-transform.up, Vector3.up) > 0.6f)
            diceCount = 6;
        if (Vector3.Dot(transform.right, Vector3.up) > 0.6f)
            diceCount = 5;
        if (Vector3.Dot(-transform.right, Vector3.up) > 0.6f)
            diceCount = 8;

    }

}
public enum DiceType { Standard, Special };