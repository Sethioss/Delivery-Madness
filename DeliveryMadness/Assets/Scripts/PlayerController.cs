using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    private Camera cam;
    public float speed;
    public float rotationForce;
    private Rigidbody rb;

    [Header("Touch Zone")]
    private float top;
    private float bottom;

    [Header("Journal")]
    public GameObject journalPrefab;

    [Header("Aiming")]
    public bool inZone = false;
    public bool aiming = false;
    public GameObject letterThrowPoint;

    [Header("Vfx")]
    public GameObject vfx;


    /*[Header("Throw raycasts (Applied to both sides)")]
    [Range(0, 50)]
    public float throwDistance;
    [Range(1, 40)]
    public int accuracy = 4;
    [Range(1, 360)]
    public float accuracySpread;
    [Range(0, 360)]
    public float accuracyOffset;*/

    [Header("Animators")]
    public GameObject player;
    public GameObject bike;
    [HideInInspector]
    public Animator bikeAnim;
    [HideInInspector]
    public Animator mailmanAnim;

    [Header("Ragdoll")]
    public GameObject unragdolledPlayer;
    public GameObject ragdolledPlayer;

    #region screenCoordinates

    private Touch touch;
    private Vector3 touchPosition;
    private float screenHeight;

    #endregion

    #region Cached variables

    private GameManager gameManager;
    private UIManager uiManager;
    private CameraController cameraController;
    private LevelManager levelManager;
    private AudioManager audioManager;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bikeAnim = bike.GetComponent<Animator>();
        mailmanAnim = player.GetComponent<Animator>();
    }

    private void Start()
    {
        screenHeight = Screen.height;

        bottom = 0;
        top = screenHeight / 2;

        //DebugPlayableArea();

        gameManager = GameManager.instance;
        gameManager.FindObjects();

        uiManager = gameManager._UIManager;
        cameraController = gameManager.cameraController;
        cam = cameraController.gameObject.GetComponent<Camera>();
        levelManager = gameManager.levelManager;
        audioManager = gameManager.audioManager;
    }

    private void Update()
    {
        //CheckRays();

        if (Input.touchCount > 0 && inZone && !gameManager.gameWon)
        {
            Shoot();
        }

        if (Input.touchCount > 0 && !inZone && !aiming && !gameManager.gameOver)
        {
            Move();
        }

        else if (gameManager.gameStarted)
        {
            Stop();
        }
    }

    /*private void CheckRays()
    {
        //Raycast draw

        RaycastHit hit;
        float angle = 0;
        float r = throwDistance;
        int coll = 0;

        if (transform.position.x < 0)
        {

            for (int i = 0; i <= accuracy; i++)
            {
                angle = ((-accuracySpread / accuracy) * i) - accuracyOffset;

                float radAngle = angle * Mathf.Deg2Rad;

                Vector3 finalCoord = transform.position;
                finalCoord.x = -r * Mathf.Cos(radAngle);
                finalCoord.z = -r * Mathf.Sin(radAngle);
                finalCoord.y = transform.position.y;

                Debug.DrawRay(transform.position, finalCoord, Color.green);
                Ray ray = new Ray(transform.position, finalCoord);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("House"))
                    {
                        coll++;
                    }
                }
            }

            if (coll > 0)
            {
                inZone = true;
                GameManager.instance.cameraController.TriggerCamera(GameManager.instance.cameraController.rightDestination);
                GameManager.instance.levelManager.ChangeLevelSpeed(2);
            }
            else
            {
                inZone = false;
                GameManager.instance.cameraController.TriggerCamera(GameManager.instance.cameraController.basePoint);
                GameManager.instance.levelManager.ChangeLevelSpeed(GameManager.instance.levelManager.originalSpeed);
                aiming = false;
            }
        }

        else
        {
            for (int i = 0; i <= accuracy; i++)
            {

                angle = ((-accuracySpread / accuracy) * i) - accuracyOffset;

                float radAngle = angle * Mathf.Deg2Rad;

                Vector3 finalCoord = transform.position;
                finalCoord.x = r * Mathf.Cos(radAngle);
                finalCoord.z = -r * Mathf.Sin(radAngle);
                finalCoord.y = transform.position.y;

                Debug.DrawRay(transform.position, finalCoord, Color.green);
                Ray ray = new Ray(transform.position, finalCoord);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("House"))
                    {
                        coll++;
                    }
                }
            }

            if (coll > 0)
            {
                inZone = true;
                GameManager.instance.cameraController.TriggerCamera(GameManager.instance.cameraController.leftDestination);
                GameManager.instance.levelManager.ChangeLevelSpeed(2);
            }
            else
            {
                inZone = false;
                GameManager.instance.cameraController.TriggerCamera(GameManager.instance.cameraController.basePoint);
                GameManager.instance.levelManager.ChangeLevelSpeed(GameManager.instance.levelManager.originalSpeed);
                aiming = false;
            }
        }

        

    }*/

    private void Stop()
    {
        if (rb.velocity.x > 0.004f || rb.velocity.x < -0.004f)
        {
            Vector3 newVelocity = Vector3.Lerp(rb.velocity, Vector3.zero, 0.4f);
            rb.velocity = newVelocity;


            Vector3 newRotation = Vector3.Lerp(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z), Vector3.zero, 0.4f);
            OrientCharacter(newRotation.y);
        }

    }

    private void Move()
    {
        touch = Input.GetTouch(0);

        if (touch.position.y < top && touch.position.y > bottom)
        {
            gameManager.FindObjects();

            if (touch.phase == TouchPhase.Began && !gameManager.gameStarted)
            {
                gameManager.gameStarted = true;
                uiManager.DeActivateCanva(uiManager.prompt);
                bikeAnim.SetBool("moveBike", true);
                mailmanAnim.SetBool("cycling", true);
            }

            touchPosition = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 1));

            //Get the distance between touchPosition and player
            float xDifference = touchPosition.x - transform.position.x;
            xDifference = Mathf.Clamp(xDifference, -1f, 1f);
            //Vector3 transformPos = new Vector3(touchPosition.x, transform.position.y, transform.position.z);
            //transform.position = transformPos;
            rb.velocity = new Vector3(xDifference * speed * Time.fixedDeltaTime, rb.velocity.y, rb.velocity.z);

            OrientCharacter(xDifference);
        }

        else
        {
            Stop();
        }
    }

    private void OrientCharacter(float yRotation)
    {
        transform.rotation = new Quaternion(0, yRotation * rotationForce * Time.deltaTime, 0, 1);
    }

    private void DebugPlayableArea()
    {
        Vector2 topLeft;
        Vector2 botRight;

        topLeft.x = 0;
        topLeft.y = bottom;

        botRight.x = screenHeight;
        botRight.y = top;

        //Debug
        Debug.DrawLine(new Vector3(topLeft.x, topLeft.y), new Vector3(botRight.x, topLeft.y), Color.red);
        Debug.DrawLine(new Vector3(botRight.x, topLeft.y), new Vector3(botRight.x, botRight.y), Color.red);
        Debug.DrawLine(new Vector3(botRight.x, botRight.y), new Vector3(topLeft.x, botRight.y), Color.red);
        Debug.DrawLine(new Vector3(topLeft.x, botRight.y), new Vector3(topLeft.x, topLeft.y), Color.red);
    }

    /*public void ThrowEnvelope(string colorToThrow)
    {
        if (GameManager.instance.gameStarted)
        {

            /*switch (colorToThrow)
            {
                case "Yellow":
                    {
                        GameObject newObj = GameObject.Instantiate(yellowEnveloppe, transform.position, Quaternion.identity);
                        newObj.GetComponent<YellowEnveloppe>().destination = CheckHousePosition();
                        newObj.GetComponent<YellowEnveloppe>().EnveloppeThrow();
                        break;
                    }

                case "Red":
                    {
                        GameObject newObj = GameObject.Instantiate(redEnveloppe, transform.position, Quaternion.identity);
                        newObj.GetComponent<RedEnveloppe>().destination = CheckHousePosition();
                        newObj.GetComponent<RedEnveloppe>().EnveloppeThrow();
                        break;
                    }

                case "Blue":
                    {
                        GameObject newObj = GameObject.Instantiate(blueEnveloppe, transform.position, Quaternion.identity);
                        newObj.GetComponent<BlueEnveloppe>().destination = CheckHousePosition();
                        newObj.GetComponent<BlueEnveloppe>().EnveloppeThrow();
                        break;
                    }
            }
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerEntrance"))
        {
            mailmanAnim.SetBool("aiming", true);
            if (transform.position.x < 0)
            {
                mailmanAnim.SetBool("aimingLeft", true);
                cameraController.TriggerCamera(cameraController.rightDestination);
            }

            else
            {
                mailmanAnim.SetBool("aimingRight", true);
                cameraController.TriggerCamera(cameraController.leftDestination);
            }

            aiming = true;
            inZone = true;
            levelManager.ChangeLevelSpeed(2);
        }

        else if (other.CompareTag("TriggerExit"))
        {
            mailmanAnim.SetBool("aiming", false);
            mailmanAnim.SetBool("aimingLeft", false);
            mailmanAnim.SetBool("aimingRight", false);
            cameraController.TriggerCamera(cameraController.basePoint);
            aiming = false;
            inZone = false;
            levelManager.ChangeLevelSpeed(levelManager.originalSpeed);
        }

        else if (other.CompareTag("Goal"))
        {
            if (!gameManager.bonus)
            {
                cameraController.rightDestination.transform.eulerAngles = new Vector3(0, -90, 0);
                audioManager.PlaySound("winJingle");
                Vibrator.Vibrate();
                gameManager.bonus = true;
                inZone = true;
                mailmanAnim.SetBool("aiming", true);
                mailmanAnim.SetBool("aimingLeft", true);
                cameraController.TriggerCamera(cameraController.rightDestination);
                uiManager.overlayCam.SetActive(true);
            }

            else
            {
                cameraController.gameObject.transform.parent = levelManager.level.transform;
                gameManager.gameWon = true;
                uiManager.overlayCam.SetActive(false);
            }
        }
    }

    private void Shoot()
    {
        touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            float top = screenHeight;
            float bottom = screenHeight / 3f;

            if (touch.position.y < top && touch.position.y > bottom)
            {
                Vector3 touchPosition = new Vector3(touch.position.x, touch.position.y, 1);
                Ray worldRay = cam.ScreenPointToRay(touchPosition);

                GameObject journal = Instantiate(journalPrefab, letterThrowPoint.transform.position, Quaternion.identity);

                journal.GetComponent<Journal>().directionToTake = worldRay.direction;

                string[] soundsToPlayBetween = { "whoosh1", "whoosh2" };
                audioManager.PlayRandomBetween(soundsToPlayBetween);
                mailmanAnim.SetTrigger("shooting");
            }
        }

    }

    public void PlayCoinVfx()
    {
        vfx.SetActive(true);
        ParticleSystem[] particles = vfx.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            particle.Emit(4);
        }
    }

    public void RagdollPlayer()
    {
        unragdolledPlayer.SetActive(false);
        ragdolledPlayer.SetActive(true);
        Rigidbody[] rigidbodies = ragdolledPlayer.gameObject.GetComponentsInChildren<Rigidbody>();
        Vector3 flyingAngle = new Vector3(0, 1, 1);

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.AddForce(flyingAngle * 500);
        }

        bikeAnim = bike.GetComponent<Animator>();
        bikeAnim.SetBool("moveBike", false);

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("House"))
        {
            inZone = false;
            GameManager.instance.cameraController.TriggerCamera(GameManager.instance.cameraController.basePoint);
            GameManager.instance.levelManager.ChangeLevelSpeed(GameManager.instance.levelManager.originalSpeed);
            aiming = false;
        }
    }*/

    /*public Vector3 CheckHousePosition()
     {
         RaycastHit hit;
         float angle = 45f;
         float r = throwDistance;

         if (transform.position.x < 0)
         {
             for (int i = 0; i <= accuracy; i++)
             {
                 angle = (-accuracySpread / accuracy) * i - accuracyOffset;

                 float radAngle = angle * Mathf.Deg2Rad;

                 Vector3 finalCoord = transform.position;
                 finalCoord.x = -r * Mathf.Cos(radAngle);
                 finalCoord.z = -r * Mathf.Sin(radAngle);
                 finalCoord.y = transform.position.y;

                 Ray ray = new Ray(transform.position, finalCoord);

                 if (Physics.Raycast(ray, out hit))
                 {
                     if (hit.transform.CompareTag("House"))
                     {
                         Debug.Log("On a la maison! ");
                         if(!hit.transform.GetComponent<House>().receivedLetter)
                         {
                             return hit.transform.GetComponent<House>().mailBoxPos.transform.position;
                         }
                     }
                 }
             }
         }

         else if (transform.position.x > 0)
         {
             for (int i = 0; i <= accuracy; i++)
             {
                 angle = ((-accuracySpread / accuracy) * i) + accuracyOffset;

                 float radAngle = angle * Mathf.Deg2Rad;

                 Vector3 finalCoord = transform.position;
                 finalCoord.x = r * Mathf.Cos(radAngle);
                 finalCoord.z = -r * Mathf.Sin(radAngle);
                 finalCoord.y = transform.position.y;

                 Ray ray = new Ray(transform.position, finalCoord);

                 if (Physics.Raycast(ray, out hit))
                 {
                     if (hit.transform.CompareTag("House"))
                     {
                         Debug.Log("On a la maison! ");
                         if (!hit.transform.GetComponent<House>().receivedLetter)
                         {
                             return hit.transform.GetComponent<House>().mailBoxPos.transform.position;
                         }
                     }
                 }
             }
         }

         return Vector3.zero;
     }*/
}
