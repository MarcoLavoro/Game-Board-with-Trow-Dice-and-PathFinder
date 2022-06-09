using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DiceSwipeControl : MonoBehaviour
{
    // Static Instance of the Dice
    public static DiceSwipeControl Instance;
    #region Public Variables
    // Orignal Dice
    public GameObject orignalDice;
    // Special Dice
    public GameObject specialDice;
    //dice resultant number..
    public int diceCount;
    //dice play view camera...
    public Camera dicePlayCam;
    //Can Throw Dice
    public bool isDiceThrowable = true;
    public Transform diceCarrom;
    #endregion

    #region Private Varibles
    private GameObject diceClone;
    private Vector3 initPos;
    private float initXpose;
    private float timeRate;
    // To Save Camera Postion
    private Vector3 currentCampPos;
    Vector3 objectPos;
    internal float diceThrowInit;
    private Vector3 cameraStartPos;
    private Quaternion cameraStartRot;
    private bool trowDiceCalled = false;
    #endregion

    void Awake()
    {
        cameraStartPos = Camera.main.transform.position;
        cameraStartRot = Camera.main.transform.rotation;
        Instance = this;

    }

    void Start()
    {
       // generateDice();
    }

    void Update()
    {
        if (isDiceThrowable)
        {

            initPos = new Vector2(Screen.width / 2, Screen.height / 2);

            Vector3 currentPos = new Vector2(Screen.width / 2, Screen.height / 2);
            currentPos.z = 25f;
            Vector3 newPos = dicePlayCam.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
              newPos.y = Mathf.Clamp(newPos.y, -114.5f, 100);
            newPos = dicePlayCam.ScreenToWorldPoint(currentPos);


            if (trowDiceCalled)
            {
                trowDiceCalled = false;
                initPos = dicePlayCam.ScreenToWorldPoint(initPos);

                enableTheDice();
                addForce(newPos);
                isDiceThrowable = false;

                StartCoroutine(getDiceCount());
            }
        }
    }

    public void TrowDice(bool IsSPecial)
    {
        generateDice(IsSPecial);
        trowDiceCalled = true;

    }
    void addForce(Vector3 lastPos)
    {
        float randomForce = Random.Range(700, 1000);
        diceClone.transform.GetComponent<Rigidbody>().AddTorque(Vector3.Cross(lastPos, initPos) * randomForce, ForceMode.Impulse);
        lastPos.y += 12;
        diceClone.GetComponent<Rigidbody>().AddForce(((lastPos - initPos).normalized) * (Vector3.Distance(lastPos, initPos)) * 30 * diceClone.GetComponent<Rigidbody>().mass);
    }

    void enableTheDice()
    {
        diceClone.transform.rotation = Quaternion.Euler(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));
        diceThrowInit = 0;
    }

    void generateDice(bool IsSPecial)
    {
        if(IsSPecial)
            diceClone = Instantiate(specialDice, dicePlayCam.transform.position, Quaternion.Euler(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180))) as GameObject;

        else
            diceClone = Instantiate(orignalDice, dicePlayCam.transform.position, Quaternion.Euler(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180))) as GameObject;

        Debug.Log("dice rolled by player=" + GameManager.PlayerTurn);

    }
    #region Coroutines

    IEnumerator getDiceCount()
    {
        //Time.timeScale = 1.3f;	
        currentCampPos = dicePlayCam.transform.position;
        //wait fore dice to stop...
        yield return new WaitForSeconds(1.0f);
        while (diceClone.GetComponent<Rigidbody>().velocity.magnitude > 0.05f)
        {
            yield return 0;
        }

        Time.timeScale = 0.2f;
        timeRate = 0.001f;
        float startTime = Time.time;
        Vector3 risePos = dicePlayCam.transform.position;
        Vector3 setPos = new Vector3(diceCarrom.position.x, diceClone.transform.position.y + 25f, diceCarrom.position.z);
        float speed = 0.18f;
        float fracComplete = 0;

        while (Vector3.Distance(dicePlayCam.transform.position, setPos) > 0.5f)
        {
            Vector3 center = (risePos + setPos) * 0.5f;
            center -= new Vector3(0, 2, -1);
            Vector3 riseRelCenter = risePos - center;
            Vector3 setRelCenter = setPos - center;

            if (fracComplete > 0.85f && fracComplete < 1f)
            {
                speed += Time.deltaTime * 0.3f;
                Time.timeScale -= Time.deltaTime * 4f;
            }
            dicePlayCam.transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            dicePlayCam.transform.position += center;
            dicePlayCam.transform.LookAt(diceCarrom);
            fracComplete = (Time.time - startTime) / speed;
            yield return 0;
        }

        Time.timeScale = 1.0f;
        diceCount = Dice.Instance.GetDiceCount(); 

        if(GameManager.PlayerTurn)
             PlayerController.Instance.DiceRolledReturn(diceCount);
        else
            AIController.Instance.DiceRolledReturn(diceCount);

        UIManager.Instance.SetDiceResult(diceCount);

        yield return new WaitForSeconds(5f);

        Dice.Instance.diceCount = 0;
        diceClone.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        ResetPlay();
        GameManager.Instance.ChangeTurn();
    }

    public void ResetPlay()
    {
        Camera.main.transform.position = cameraStartPos;
        Camera.main.transform.rotation = cameraStartRot;

        Destroy(diceClone);
        isDiceThrowable = true;

    }

    #endregion
}
