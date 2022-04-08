using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] AppleController appleController;
    [SerializeField] GamePlay gamePlayController;
    [SerializeField] GameObject EndPanel;
    public bool firstPersonMode = false;

    [SerializeField] float movementSpeed = 5;
    [SerializeField] float speedStep = 0.1f;
    [SerializeField] float rotationSpeed = 180;
    [SerializeField] GameObject bodyPrefab;

    [SerializeField] Vector2 boundsX;
    [SerializeField] Vector2 boundsZ;
    [SerializeField] float countdownTime = 1;

    char wantedDirection = ' ';
    public Vector3 targetPosition;
    public Vector3 previousTargetPosition;
    Vector3 lastBodyPreviousPosition;
    List<GameObject> bodyParts = new List<GameObject>();
    List<Vector3> positionHistory = new List<Vector3>();
    bool outOfBounds = false;
    bool runDeath = false;
    bool selfEaten = false;
    bool keyRigistered = false;
    string HUDButtonPressed = "";

    void Start()
    {
        targetPosition = getTargetPosition();
        previousTargetPosition = targetPosition;
    }

    public void PressHUDButton(string name)
    {
        HUDButtonPressed = name;
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        if (firstPersonMode == false && keyRigistered == false)
        {
            if ((Input.GetKeyDown("up") || HUDButtonPressed == "up") && wantedDirection != 'D' && wantedDirection != ' ')
            {
                keyRigistered = true;
                wantedDirection = 'U';
            }
            else if ((Input.GetKeyDown("right") || HUDButtonPressed == "right") && wantedDirection != 'L')
            {
                keyRigistered = true;
                wantedDirection = 'R';
            }
            else if ((Input.GetKeyDown("left") || HUDButtonPressed == "left") && wantedDirection != 'R' && wantedDirection != ' ')
            {
                keyRigistered = true;
                wantedDirection = 'L';
            }
            else if ((Input.GetKeyDown("down") || HUDButtonPressed == "down") && wantedDirection != 'U')
            {
                keyRigistered = true;
                wantedDirection = 'D';
            }
            HUDButtonPressed = "";
        }
        else if (keyRigistered == false)
        {
            if (Input.GetKeyDown("up") || HUDButtonPressed == "up")
            {
                keyRigistered = true;
                if (wantedDirection == ' ' || wantedDirection == 'D')
                {
                    wantedDirection = 'D';
                }
            }
            else if (Input.GetKeyDown("left") || HUDButtonPressed == "left")
            {
                keyRigistered = true;
                if (wantedDirection == 'D' || wantedDirection == ' ')
                {
                    wantedDirection = 'R';
                }
                else if (wantedDirection == 'L')
                {
                    wantedDirection = 'D';
                }
                else if (wantedDirection == 'U')
                {
                    wantedDirection = 'L';
                }
                else if (wantedDirection == 'R')
                {
                    wantedDirection = 'U';
                }
            }
            else if (Input.GetKeyDown("right") || HUDButtonPressed == "right")
            {
                keyRigistered = true;
                if (wantedDirection == 'D')
                {
                    wantedDirection = 'L';
                }
                else if (wantedDirection == 'L')
                {
                    wantedDirection = 'U';
                }
                else if (wantedDirection == 'U')
                {
                    wantedDirection = 'R';
                }
                else if (wantedDirection == 'R')
                {
                    wantedDirection = 'D';
                }
            }
            HUDButtonPressed = "";
        }

        if (wantedDirection == ' ')
        {
            return;
        }

        moveToTarget();
        checkCollision();

        bool eaten = appleController.checkIfInPosition(targetPosition);
        if (eaten)
        {
            growSnake();
        }

        if ((outOfBounds == true || selfEaten == true) && runDeath == false)
        {

            StartCoroutine(runDeathScene(0f));
            runDeath = true;
        }

        if (positionHistory.Count == 0)
        {
            return;
        }

        int index = 0;
        foreach (var body in bodyParts)
        {
            moveBodyToTarget(body, positionHistory[index]);
            index++;
        }
    }

    IEnumerator runDeathScene(float _time)
    {
        yield return new WaitForSeconds(_time);
        Time.timeScale = 0.50f;
        float _timeScale = 0.50f;
        while (_timeScale > 0)
        {
            _timeScale -= 0.02f;
            yield return new WaitForSeconds(countdownTime);
            if (_timeScale <= 0)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = _timeScale;
            }
        }
        gamePlayController.SetButtonAsPaused();
        EndPanel.SetActive(true);
        EndPanel.GetComponent<EndController>().setTitleTo();
    }

    void growSnake()
    {
        GameObject body = Instantiate(bodyPrefab);
        bodyParts.Add(body);
        if (positionHistory.Count == 0)
        {
            positionHistory.Add(previousTargetPosition);
        }
        else
        {
            positionHistory.Add(lastBodyPreviousPosition);
        }
        body.transform.position = positionHistory[positionHistory.Count - 1];

        if (bodyParts.Count > 1)
        {
            body.transform.rotation = bodyParts[bodyParts.Count - 2].transform.rotation;
        }
        else
        {
            body.transform.rotation = transform.rotation;
        }

        if (appleController.applesEaten % 5 == 0)
        {
            movementSpeed += speedStep;
        }

    }

    void checkCollision()
    {
        if (positionHistory.Count < 2)
        {
            return;
        }

        foreach (var position in positionHistory)
        {
            if ((position != positionHistory[1]) && (targetPosition == position))
            {
                selfEaten = true;
            }
        }
    }

    Vector3 getTargetPosition()
    {
        keyRigistered = false;
        Vector3 target = new Vector3(x: Mathf.Round(transform.position.x), y: transform.position.y, z: Mathf.Round(transform.position.z));

        if (wantedDirection == 'R')
        {
            target.x += 1;
            if (target.x % 2 == 0) target.x += 1;
        }
        else if (wantedDirection == 'L')
        {
            target.x -= 1;
            if (target.x % 2 == 0) target.x -= 1;
        }
        else if (wantedDirection == 'U')
        {
            target.z += 1;
            if (target.z % 2 == 0) target.z += 1;
        }
        else if (wantedDirection == 'D')
        {
            target.z -= 1;
            if (target.z % 2 == 0) target.z -= 1;
        }

        return target;
    }

    void moveToTarget()
    {
        bool inBoundsX = (transform.position.x - boundsX.x) * (boundsX.y - transform.position.x) >= 0;
        bool inBoundsZ = (transform.position.z - boundsZ.x) * (boundsZ.y - transform.position.z) >= 0;

        if ((inBoundsX && inBoundsZ) == false)
        {
            outOfBounds = true;
        }

        if (transform.position == targetPosition)
        {
            previousTargetPosition = targetPosition;
            targetPosition = getTargetPosition();
            arrangePositionHistory();
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        Vector3 targetDirection = targetPosition - transform.position;

        if (targetDirection == Vector3.zero)
        {
            return;
        }
        Quaternion nextRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(targetDirection), rotationSpeed * Time.deltaTime);
        transform.localRotation = nextRotation;
    }

    void arrangePositionHistory()
    {
        if (positionHistory.Count == 0)
        {
            return;
        }

        lastBodyPreviousPosition = positionHistory[positionHistory.Count - 1];
        for (int index = positionHistory.Count - 1; index >= 0; index--)
        {
            if (index == 0)
            {
                positionHistory[index] = previousTargetPosition;
            }
            else
            {
                positionHistory[index] = positionHistory[index - 1];
            }
        }
    }

    void moveBodyToTarget(GameObject body, Vector3 position)
    {
        body.transform.position = Vector3.MoveTowards(body.transform.position, position, movementSpeed * Time.deltaTime);

        Vector3 targetDirection = position - body.transform.position;

        if (targetDirection == Vector3.zero)
        {
            return;
        }
        Quaternion nextRotation = Quaternion.Lerp(body.transform.localRotation, Quaternion.LookRotation(targetDirection), rotationSpeed * Time.deltaTime);
        body.transform.localRotation = nextRotation;
    }
}
