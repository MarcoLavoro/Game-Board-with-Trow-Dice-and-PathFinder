using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterController : MonoBehaviour
{
    public Vector2 actualPos;

    public PathFind.Point actualPosition;

    public List<Vector3> positionDestinations;
    public int positionActualPoint = 0;
    public float vel = 3;
    public bool inMovements;
    private int WaitBeforeSpecialDice = 3;
    public int CountBeforeSpecialDice = 0;

    public void UpdateMovements()
    {
        if (inMovements)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, positionDestinations[positionActualPoint], vel*Time.deltaTime);
            if (Vector3.Distance(this.transform.position, positionDestinations[positionActualPoint]) < 0.01f)
            {
                Debug.Log("moved by 1 tile");
                positionActualPoint++;
                if (positionActualPoint >= positionDestinations.Count)
                {
                    inMovements = false;
                    CheckIfWin();
                }
            }
        }
    }

    public void SetPathAndStart(List<PathFind.Point> path, int tilesNumber)
    {
        List<Vector3> positions = new List<Vector3>();
        int step = tilesNumber;
        if (tilesNumber > path.Count)
            tilesNumber = path.Count;

        for (int i = 0; i < tilesNumber; i++)
        {
            positions.Add(new Vector3(path[i].x, 0, path[i].y));
           
        }
        actualPosition = new PathFind.Point(path[positions.Count-1].x, path[positions.Count-1].y);

        actualPos = new Vector2(actualPosition.x, actualPosition.y);

        positionActualPoint = 0;

        positionDestinations = positions;
        inMovements = true;
    }

    public void DiceRolledReturn(int diceResult)
    {
        //     PathFind.Point _from = new PathFind.Point(actualPosition.x, actualPosition.y);
        //     PathFind.Point _to = new PathFind.Point(GameManager.Instance.width - 1, GameManager.Instance.height - 1);
        //
        //     // path will either be a list of Points (x, y), or an empty list if no path is found.
        //     List<PathFind.Point> path = new List<PathFind.Point>();
        //     path = PathFind.Pathfinding.FindPath(GameManager.Instance.grid, _from, _to);
        List<PathFind.Point> path = returnPath();

        SetPathAndStart(path, diceResult);
    }
    public void UsedSpecialDice()
    {
        CountBeforeSpecialDice = WaitBeforeSpecialDice;
    }
    public void RecargeSpecialDice()
    {
        if (CountBeforeSpecialDice > 0)
            CountBeforeSpecialDice--;

        Debug.Log("Turn Before You can Use Special Dice Again = " + CountBeforeSpecialDice);
    }
    public void CheckIfWin()
    {
        if (Vector3.Distance(this.transform.position, GameManager.ArrivalPoint) < 0.01f)
        {
            Debug.Log("THIS PLAYER WINS");

            CharacterWinner();
        }

    }

    public abstract void CharacterWinner();

    public bool ChechIfDeadEnd()
    {
        List<PathFind.Point> path = returnPath();
        if (path != null)
            if (path.Count > 0)
                return false;


        return true;
    }

    private List<PathFind.Point> returnPath()
    {
        PathFind.Point _from = new PathFind.Point(actualPosition.x, actualPosition.y);
        PathFind.Point _to = new PathFind.Point(GameManager.Instance.width - 1, GameManager.Instance.height - 1);

        List<PathFind.Point> path = new List<PathFind.Point>();
        path = PathFind.Pathfinding.FindPath(GameManager.Instance.grid, _from, _to);
        return path;
    }
}
