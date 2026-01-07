using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Vector3 vel;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float GravityForce;
    [SerializeField] private int FlipDir;
    [SerializeField] private int PickaxeNum;
    [SerializeField] private Transform model;
    [SerializeField] private ParticleSystem PS;
    [SerializeField] private DialogueBox Dia;
    [SerializeField] private GameObject AIBuddy;
    [SerializeField] private string[] hintText = new string[] { "Default" };
    [SerializeField] private float teleportVFXmaxRotationSpeed = 360f;
    [SerializeField] private float teleportTimer = 0f;
    [SerializeField] private bool canFlip = true;
    [SerializeField] private bool canHint = true;

    [SerializeField] private TMP_Text pickaxeText;
    [SerializeField] private GameObject gameUI;
    private bool inDialogue = false;
    private AudioSource pickaxeHitAudioSource;
    private AudioSource gravitySwitchAudioSource;
    private AudioSource breakBlockAudioSource;
    private AudioSource deathAudioSource;
    private Rigidbody rb;
    private bool isDead = false;
    private Animator anim;
    private bool grounded;
    private GameObject currentNPC = null;
    public ExitDoorBehavior exitDoor;
    private bool isTeleporting = false;
    private int totalPickaxeNum;
    private bool dialogueOn = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameUI != null) gameUI.SetActive(true);
        if (!exitDoor) exitDoor = GameObject.FindGameObjectWithTag("ExitDoor").GetComponent<ExitDoorBehavior>();
        grounded = false;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        //set the sounds and spacial blends
        AudioSource[] audioSources = GetComponents<AudioSource>();
        pickaxeHitAudioSource = audioSources[0];
        gravitySwitchAudioSource = audioSources[1];
        breakBlockAudioSource = audioSources[2];
        deathAudioSource = audioSources[3];
        pickaxeHitAudioSource.spatialBlend = 0.0f;
        gravitySwitchAudioSource.spatialBlend = 0.0f;
        breakBlockAudioSource.spatialBlend = 0.0f;
        deathAudioSource.spatialBlend = 0.0f;
        PS.Stop();
        AIBuddy.SetActive(false);
        totalPickaxeNum = PickaxeNum;
        UpdatePickaxeText();
    }

    private void Update()
    {

        if (isDead)
        {
            return;
        }
        
        if (isTeleporting)
        {
            TeleportVFX();
        }

        //allows the player to restart the level at any time before reaching the exit
        RestartLevel();

        //if you haven't beat the level you can move
        if (!exitDoor.PlayerReachedDoor)
        {
            GetMoveInput();
            if (PickaxeNum > 0) CheckPick();
            CheckDia();
            if (!inDialogue && canFlip)
            {
                Flip();
            }
        }
        if (exitDoor.PlayerReachedDoor)
        {
            if (gameUI != null) gameUI.SetActive(false);
            rb.linearVelocity = Vector3.zero;
        }
        if (!Dia.isActiveAndEnabled)
        {
            inDialogue = false;
            if (AIBuddy.activeInHierarchy) AIBuddy.SetActive(false);
            if (currentNPC != null)
            {
                NPCDialogueBehavior npcScript = currentNPC.GetComponent<NPCDialogueBehavior>();
                if (npcScript)
                    npcScript.setInDialogue(false);
                currentNPC = null;
            }
        }

    }

    private void GetMoveInput()
    {
        if (grounded && !inDialogue)
        {
            PS.Stop();
            Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            movement.Normalize();
            movement *= MovementSpeed;


            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            Vector3 camUp = Camera.main.transform.up;

            //if the player is using the top down or bottom up views 
            // camera forward vector is not going to move the player
            if (camForward.y > 0.9f)
            {
                //use camera right because at this 
                // angle it is what camera forward is intended to be
                camForward = -camUp;
            }
            else if (camForward.y < -0.9f)
            {
                camForward = camUp;
            }
            //if the player is using head-on view
            else
            {
                camForward.y = 0;
                camRight.y = 0;
            }
            vel = movement.y * camForward + movement.x * camRight;

            //if walking, play animation
            if (Vector3.Magnitude(movement) != 0)
                anim.SetBool("IsWalkingBool", true);
            //if stopped, immediately stop animation
            else
                anim.SetBool("IsWalkingBool", false);
        }
        else
        {
            if (grounded) PS.Stop();
            vel = Vector3.zero;
        }

        if (vel != Vector3.zero) transform.rotation = Quaternion.LookRotation(vel);

    }

    void FixedUpdate()
    {
        //once the level is complete, the player should stop all movement, 
        // even midair to not collide with anything
        if (!exitDoor.PlayerReachedDoor)
        {
            Move();
        }
    }

    void Move()
    {
        rb.AddForce(0, GravityForce * Mathf.Sign(FlipDir), 0);
        vel.y = rb.linearVelocity.y;
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, vel, 0.25f);
    }

    void Flip()
    {
        if (Input.GetButtonDown("Jump"))
        {
            //Debug.Log("Attempt Made!");
            if (grounded)
            {
                //AudioSource.PlayClipAtPoint(GravitySwitchSound, transform.position);
                gravitySwitchAudioSource.Play();
                model.transform.Rotate(180, 180, 0);
                grounded = false;
                FlipDir = -FlipDir;
                PS.Play();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collided");
        switch (Mathf.Sign(FlipDir))
        {
            case 1f:
                // Debug.Log("Up");
                if (collision.collider.CompareTag("Ground") && collision.contacts[0].normal.y < 1f)
                {
                    //    Debug.Log("Collided Up");
                    grounded = true;
                }
                break;
            case -1f:
                //  Debug.Log("Down");
                if (collision.collider.CompareTag("Ground") && collision.contacts[0].normal.y > .5f)
                {
                    //      Debug.Log("Collided Down");
                    grounded = true;
                }
                break;
            default:
                //  Debug.Log("oh...");
                return;
        }
    }

    private void CheckPick()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("IsSwinging");
            pickaxeHitAudioSource.Play();
            RaycastHit hit;
            bool didHit = Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 3f, LayerMask.GetMask("Breakable"));
            if (didHit)
            {
                Debug.Log("Poof!");
                Debug.Log(LayerMask.LayerToName(hit.collider.gameObject.layer));
                breakBlockAudioSource.Play();
                Destroy(hit.collider.gameObject);
                Debug.Log("Block destroyed");
                PickaxeNum--;
                UpdatePickaxeText();
            }
        }
    }

    public void Kill()
    {   
        deathAudioSource.Play();
        Transform deathVFX = transform.Find("DeathVFX");
        if (deathVFX != null)
            deathVFX.gameObject.SetActive(true);
        isDead = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        if (model != null)
        {
            model.gameObject.SetActive(false);
        }
        Invoke("ReloadScene", 1f);

    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetIsTeleporting(bool tf)
    {
        isTeleporting = tf;
    }

    void TeleportVFX()
    {
        teleportTimer += Time.deltaTime;
        float rotationSpeed = Mathf.Lerp(0, teleportVFXmaxRotationSpeed, teleportTimer / 2f);
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        if (teleportTimer >= 2f)
        {
            gameObject.SetActive(false);
            isTeleporting = false;
        }
    }

    private void CheckDia()
    {
        if (Input.GetKeyDown(KeyCode.F) || dialogueOn == true)
        {
            dialogueOn = false;
            if (!inDialogue)
            {
                Debug.Log("Checked");
                inDialogue = true;
                RaycastHit hit;
                bool didHit = Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 3f, LayerMask.GetMask("Dialogue"));
                if (didHit)
                {
                    currentNPC = hit.collider.gameObject;
                    NPCDialogueBehavior npcScript = currentNPC.GetComponent<NPCDialogueBehavior>();
                    if (npcScript)
                        npcScript.setInDialogue(true);
                    DialogueHolder d = currentNPC.GetComponent<DialogueHolder>();

                    Dia.gameObject.SetActive(true);
                    Dia.SetText(d.GetText());
                }
                else if (canHint)
                {
                    Dia.gameObject.SetActive(true);
                    AIBuddy.SetActive(true);
                    Dia.SetText(hintText);
                }
            }
            else
            {
                Dia.TurnPage();
            }
        }
    }

    public void DialogueSet() {
        dialogueOn = true;
    }

    private void UpdatePickaxeText()
    {
        if (pickaxeText != null)
        {
            pickaxeText.text = $"{PickaxeNum} / {totalPickaxeNum}";
            if (PickaxeNum == 0)
            {
                pickaxeText.color = Color.red;
            }
            else
            {
                pickaxeText.color = Color.white;
            }
        }
    }

    public void PutDialogue(string[] dialogue)
    {
        inDialogue = true;
        Dia.gameObject.SetActive(true);
        Dia.SetText(dialogue);
    }
    public bool InDialogue()
    {
        return inDialogue;
    }

    public void GiveAIBuddy()
    {
        canFlip = true;
        canHint = true;
    }

    public void RestartLevel()
    {
        //Restart the level if you haven't completed it yet
        if (Input.GetKeyDown(KeyCode.L) && !exitDoor.PlayerReachedDoor)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
