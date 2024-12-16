using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using Unity.LiveCapture.ARKitFaceCapture;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public enum ScriptAssociated {None, Mail, Coffe, Printer, Shredder}

[System.Serializable]
public class NPCJob
{
    public string Name;
    public string AnimationName;
    public string AnimationOverBodyName;
    public GameObject PlaceToBe;
    public bool Importen;
    public Expression ExpressionToDo;
    public AudioClip Sound;
    public ScriptAssociated ScriptAssociated;
}


public class NPCBrain : MonoBehaviour
{
    [Header("Info")]
    public bool SmartBrain;
    public float Temperment;
    public float Speed;

    [Header("Jobs")]
    public NPCJob JobDoing;
    public List<NPCJob> NPCJobList;

    [Header("Dead Time")]
    public float DeadTimer;

    [Header("Repeat")]
    public NPCJob TheJobToRepeat;
    public float RepeatTimer;

    [Header("Has Activatet The Script")]
    public bool HasActivatetTheScript;

    [Header("Animation")]
    public Animator Ani;
    public bool SatTurnDegress; //Rotation

    [Header("Overbody Animation")]
    public bool SmoothOverbodyTransition;
    public float SmoothOverbodyTransitionDuration;

    [Header("Face Animation")]
    public TheBlendShaper TheBlendShaper;
    public float TalkBlend;

    [Header("Personal Space")]
    public GameObject Player;
    public float PersonalSpace;
    public bool BackedUp;
    public bool MoveOn;

    [Header("Ekster Jobs")]
    public NPCJob WalkBack;
    public NPCJob CrossArms;
    public NPCJob Focus;
    public NPCJob Throw;

    [Header("Warning")]
    public int WarningCounter;
    public NPCJob WarningOne;
    public NPCJob WarningTwo;
    public NPCJob WarningThree;

    [Header("Fire")]
    public GameObject Fire;

    [Header("Detect PlaceToBe")]
    public NPCJob Turn;
    public bool HasTurned;
    public float detectionRadius = 20f; // Range of the detection
    public float detectionAngle = 30f;  // Half of the cone angle

    [Header("Audio")]
    public AudioSource AudioSource;

    [Header("Player Looking at NPC")]
    public float LookingTowardsNPCThreshold; // Threshold for determining "looking at" (dot product)
    public float NotLookingTowardsNPCTimer;

    [Header("Load Timer")]
    public bool HasStarted;
    public float StartTimer;

    // Start is called before the first frame update
    void Start()
    {
        TheBlendShaper = FindAnyObjectByType<TheBlendShaper>(); 
    }

    // Load in time
    void LoadIn()
    {
        if (StartTimer > 2)
        {
            if (HasStarted == false)
            {
                GetNextJobOnTheList();

                HasStarted = true;
            }
        }
        else
        {
            StartTimer += Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        LoadIn();

        GiveWarning();

        OverbodyTransition();

        WhatToDoOnTheJob();
    }

    // Gives the player a warning
    void GiveWarning()
    {
        if (Temperment < 60)
        {
            if (Temperment < 40)
            {
                if (Temperment > 20)
                {
                    if (WarningCounter <= 0)
                    {
                        WarningCounter = 1;

                        if (SmartBrain == true)
                        {
                            TheNPCShouldDoAnotherThing(WarningOne);
                        }
                    }
                }
            }
            else
            {
                if (WarningCounter <= 1)
                {
                    WarningCounter = 2;

                    if (SmartBrain == true)
                    {
                        TheNPCShouldDoAnotherThing(WarningTwo);
                    }
                }
            }
        }
        else
        {
            if (WarningCounter <= 2)
            {
                WarningCounter = 3;
                TheNPCShouldDoAnotherThing(WarningThree);
            }
        }
    }

    // Makes a smooth transition between the animations of the overbody
    void OverbodyTransition()
    {
        if (SmoothOverbodyTransition == true)
        {
            if (SmoothOverbodyTransitionDuration < 1)
            {
                SmoothOverbodyTransitionDuration += Time.deltaTime;
                Ani.SetLayerWeight(1, SmoothOverbodyTransitionDuration);
            }
        }
        else
        {
            if (SmoothOverbodyTransitionDuration > 0)
            {
                SmoothOverbodyTransitionDuration -= Time.deltaTime;
                Ani.SetLayerWeight(1, SmoothOverbodyTransitionDuration);
            }
        }
    }

    // Manages what the npc does on the given job
    void WhatToDoOnTheJob()
    {
        if (JobDoing.Name != "Turn")
        {
            if (JobDoing.PlaceToBe != null)
            {
                LookAtThePlayer();
            }
        }

        if (JobDoing.Name == "Walk")
        {
            Vector3 location = new Vector3(JobDoing.PlaceToBe.transform.position.x, 0, JobDoing.PlaceToBe.transform.position.z);

            if (Vector3.Distance(transform.position, location) < .1)
            {
                GetNextJobOnTheList();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, location, Speed * Time.deltaTime);
            }
        }

        if (JobDoing.Name == "Walk Back")
        {
            if (BackedUp == false)
            {
                Vector3 location = new Vector3(JobDoing.PlaceToBe.transform.position.x, 0, JobDoing.PlaceToBe.transform.position.z);

                Vector3 Playervec = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
                Vector3 thisvec = new Vector3(transform.position.x, 0, transform.position.z);

                if (Vector3.Distance(thisvec, Playervec) > PersonalSpace + .5f)
                {
                    if (SmartBrain == true)
                    {
                        TheNPCShouldDoAnotherThing(CrossArms);
                    }

                    if (AudioSource.isPlaying == false)
                    {
                        GetNextJobOnTheList();
                    }
                }
                else
                {
                    Vector3 _vec = new Vector3(transform.position.x, 0, transform.position.z);

                    transform.position = Vector3.MoveTowards(_vec, location, -Speed / 2 * Time.deltaTime);
                }
            }
            else
            {
                if (SmartBrain == true)
                {
                    Debug.Log("Cross Arms 2");
                    TheNPCShouldDoAnotherThing(CrossArms);
                }
            }
        }

        if (JobDoing.Name == "Cross Arms")
        {
            if (BackedUp == true)
            {
                Vector3 Playervec = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
                Vector3 thisvec = new Vector3(transform.position.x, 0, transform.position.z);

                if (Vector3.Distance(thisvec, Playervec) > PersonalSpace + .5f)
                {
                    if (AudioSource.isPlaying == false)
                    {
                        Debug.Log("Do Cross Arms");

                        SmoothOverbodyTransition = false;
                        GetNextJobOnTheList();
                    }
                }
            }
            else
            {
                if (AudioSource.isPlaying == false)
                {
                    Debug.Log("Do Cross Arms");

                    SmoothOverbodyTransition = false;
                    GetNextJobOnTheList();
                }
            }
        }

        if (JobDoing.Name == "Turn")
        {
            if (SatTurnDegress == false)
            {
                // Calculate direction from the object to the target
                Vector3 targetvec = new Vector3(JobDoing.PlaceToBe.transform.position.x, 0, JobDoing.PlaceToBe.transform.position.z);
                Vector3 thisvec = new Vector3(transform.position.x, 0, transform.position.z);

                Vector3 directionToTarget = (targetvec - thisvec).normalized;

                // Calculate the angle between the object's forward direction and the target direction
                float angleDifference = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);

                Debug.Log("AngleDifference: " + angleDifference);

                float normalizedAngleDifference = angleDifference / 180f;
                Debug.Log("NormalizedAngleDifference: " + normalizedAngleDifference);


                Ani.SetFloat("Turn Degress", normalizedAngleDifference);


                float minSpeed = 1f;  // Minimum animation speed
                float maxSpeed = 5f;  // Maximum animation speed
                float smallestAngle = .1f; // Minimum angle to avoid division by zero or overly high speeds
                float adjustedAngle = Mathf.Max(Mathf.Abs(angleDifference), smallestAngle); // Ensure a minimum angle threshold
                float speedFactor = Mathf.Clamp(180f / adjustedAngle, minSpeed, maxSpeed); // Speed is inversely proportional to angle

                Ani.SetFloat("Turn Speed", speedFactor);

                Debug.Log("AnimationSpeed: " + speedFactor);

                SatTurnDegress = true;
            }

            if (Ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && Ani.GetCurrentAnimatorStateInfo(0).IsName("Turn"))
            {
                GetNextJobOnTheList();
                SatTurnDegress = false;
            }
        }

        if (JobDoing.Name != "Press Buttons" && JobDoing.Name != "Push")
        {
            Vector3 Playervec = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
            Vector3 thisvec = new Vector3(transform.position.x, 0, transform.position.z);

            if (Vector3.Distance(thisvec, Playervec) < PersonalSpace)
            {
                if (JobDoing.Name != "Walk Back")
                {
                    if (SmartBrain == true)
                    {
                        TheNPCShouldDoAnotherThing(WalkBack);
                    }
                }
            }
        }

        if (JobDoing.Name == "Explain")
        {
            PlayerNotLookingAtNPC();

            if (AudioSource.isPlaying == false)
            {
                SetOverBodyAnimation("null");

                GetNextJobOnTheList();
            }
        }

        if (JobDoing.Name == "Throw")
        {
            if (AudioSource.isPlaying == false)
            {
                SetOverBodyAnimation("null");

                GetNextJobOnTheList();
            }
        }

        if (JobDoing.Name == "Focus")
        {
            if (AudioSource.isPlaying == false)
            {
                SetOverBodyAnimation("null");

                GetNextJobOnTheList();
            }
        }

        if (JobDoing.Name == "Warning")
        {
            if (AudioSource.isPlaying == false)
            {
                if (WarningCounter >= 3)
                {
                    Time.timeScale = 0;
                    AudioSource.enabled = false;
                }

                SetOverBodyAnimation("null");

                GetNextJobOnTheList();
            }
        }

        if (JobDoing.Name == "Press Buttons")
        {
            if (AudioSource.isPlaying == false)
            {
                SetOverBodyAnimation("null");

                GetNextJobOnTheList();
            }
        }

        if (JobDoing.Name == "Press Buttons Long")
        {
            if (DeadTimer > 60)
            {
                SetOverBodyAnimation("null");

                GetNextJobOnTheList();

                DeadTimer = 0;
            }
            else
            {
                DeadTimer += Time.deltaTime;
            }
        }

        if (JobDoing.Name == "Fire Wait")
        {
            if (SmartBrain == true)
            {
                if (DeadTimer > 10)
                {
                    SetOverBodyAnimation("null");

                    GetNextJobOnTheList();

                    DeadTimer = 0;
                }
                else
                {
                    DeadTimer += Time.deltaTime;
                }
            }
        }

        if (JobDoing.Name == "Fire Press")
        {
            if (Ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && Ani.GetCurrentAnimatorStateInfo(0).IsName("Point"))
            {
                Time.timeScale = 0;
            }
        }

        if (JobDoing.Name == "Wait")
        {
            if (HasActivatetTheScript == false)
            {
                if (JobDoing.ScriptAssociated == ScriptAssociated.Mail)
                {
                    FindObjectOfType<PCMouseScript>().YourTurn = true;
                }

                if (JobDoing.ScriptAssociated == ScriptAssociated.Coffe)
                {
                    Debug.Log("kaf");
                    FindObjectOfType<CoffeMachineControl>().YourTurn = true;
                }

                if (JobDoing.ScriptAssociated == ScriptAssociated.Printer)
                {
                    FindObjectOfType<Printerscript>().YourTurn = true;
                }

                if (JobDoing.ScriptAssociated == ScriptAssociated.Shredder)
                {
                    FindObjectOfType<Shredder>().YourTurn = true;
                }

                HasActivatetTheScript = true;
            }

            if (MoveOn == true)
            {
                RepeatTimer = 0;

                MoveOn = false;

                SetOverBodyAnimation("null");

                GetNextJobOnTheList();

                HasActivatetTheScript = false;
            }
            else
            {
                if (RepeatTimer > 60)
                {
                    if (SmartBrain == true)
                    {
                        TheNPCShouldDoAnotherThing(TheJobToRepeat);
                    }


                    RepeatTimer = 0;
                }
                else
                {
                    RepeatTimer += Time.deltaTime;
                }
            }
        }
    }

    // Looks at the player
    public void LookAtThePlayer()
    {
        // Find all colliders in the detection radius
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider target in targetsInRange)
        {
            GameObject TargetGM = target.gameObject;

            if (JobDoing.PlaceToBe == Player)
            {
                TargetGM = Player;
            }

            if (TargetGM == JobDoing.PlaceToBe)
            {
                // Get direction to the target
                Vector3 targetvec = new Vector3(TargetGM.transform.position.x, 0, TargetGM.transform.position.z);
                Vector3 thisvec = new Vector3(transform.position.x, 0, transform.position.z);

                Vector3 directionToTarget = (targetvec - thisvec).normalized;

                // Check if the target is within the cone angle and outside a certain range
                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
                float distanceToTarget = Vector3.Distance(transform.position, TargetGM.transform.position);


                if (SmartBrain == true)
                {
                    // Optional: Lock rotation to only certain axes (e.g., y-axis)
                    directionToTarget.y = 0;

                    // Rotate the object to face the target
                    transform.rotation = Quaternion.LookRotation(directionToTarget);
                }

            }
        }
    }

    // Make the npc do antoher thing that what it is doing
    public void TheNPCShouldDoAnotherThing(NPCJob _Job)
    {
        if (JobDoing.Importen == true)
        {
            NPCJobList.Insert(0, JobDoing);
        }

        JobDoing = _Job;

        GetNextJobOnTheListInfoSat();
    }

    // Set the animation
    public void SetAnimation(string animationName)
    {
        if (animationName != "")
        {
            Ani.SetBool("Walk", false);
            Ani.SetBool("Idle", false);
            Ani.SetBool("Walk Back", false);
            Ani.SetBool("Turn", false);
            Ani.SetBool("Turn2", false);
            Ani.SetBool("Talk", false);
            Ani.SetBool("Push", false);
            Ani.SetBool("Press Buttons", false);
            Ani.SetBool("Cross Arms", false);

            Debug.Log("animationName: " + animationName);

            Ani.SetBool(animationName, true);
        }

        if (SmartBrain == true)
        {
            if (JobDoing.ExpressionToDo != Expression.Nothing)
            {
                if (JobDoing.ExpressionToDo != Expression.Neutral)
                {
                    TheBlendShaper.currentExpression = JobDoing.ExpressionToDo;
                }
                else
                {
                    SetFaceToDefault();
                }
            }
        }
    }

    // Set the overbody animation
    public void SetOverBodyAnimation(string animationName)
    {
        if (animationName == "")
        {
            return;
        }

        Ani.SetBool("OverBody Talk", false);
        Ani.SetBool("Cross Arms Overbody", false);
        Ani.SetBool("Point", false);

        if (animationName == "null")
        {
            SmoothOverbodyTransition = false;
        }
        else
        {
            Ani.SetBool(animationName, true);

            SmoothOverbodyTransition = true;
        }
    }

    // Check if the player are looking at the npc
    public void PlayerNotLookingAtNPC()
    {
        if (SmartBrain == true)
        {
            // Check if the object is looking at the target
            Vector3 targetvec = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
            Vector3 thisvec = new Vector3(transform.position.x, 0, transform.position.z);

            Vector3 directionToTarget = (thisvec - targetvec).normalized;
            float dotProduct = Vector3.Dot(Player.transform.forward, directionToTarget);

            if (dotProduct > LookingTowardsNPCThreshold)
            {
                // Object is looking at the target
                NotLookingTowardsNPCTimer = 0f; // Reset the timer
            }
            else
            {
                // Object is not looking at the target
                NotLookingTowardsNPCTimer += Time.deltaTime;

                if (NotLookingTowardsNPCTimer >= 15 && Fire.active == false)
                {
                    Temperment += 5;

                    NotLookingTowardsNPCTimer = 0f;

                    TheNPCShouldDoAnotherThing(Focus);
                }
            }
        }
    }

    // Go to the next job on the list
    public void GetNextJobOnTheList()
    {
        if (NPCJobList[0].Name == "Wait")
        {
            TheJobToRepeat = JobDoing;
        }

        JobDoing = NPCJobList[0];
        NPCJobList.Remove(NPCJobList[0]);

        if (HasTurned == false)
        {
            if (JobDoing.PlaceToBe != null)
            {
                Collider[] targetsInRange = Physics.OverlapSphere(transform.position, detectionRadius);

                foreach (Collider target in targetsInRange)
                {
                    GameObject TargetGM = target.gameObject;

                    if (JobDoing.PlaceToBe == Player)
                    {
                        TargetGM = Player;
                    }

                    if (TargetGM == JobDoing.PlaceToBe)
                    {
                        Vector3 targetvec = new Vector3(TargetGM.transform.position.x, 0, TargetGM.transform.position.z);
                        Vector3 thisvec = new Vector3(transform.position.x, 0, transform.position.z);

                        Vector3 directionToTarget = (targetvec - thisvec).normalized;

                        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

                        if (angleToTarget > detectionAngle/2)
                        {
                            Turn.PlaceToBe = JobDoing.PlaceToBe;
                            TheNPCShouldDoAnotherThing(Turn);

                            HasTurned = true;

                            return;
                        }
                    }
                }
            }
        }

        if (JobDoing.Name != "Turn")
        {
            HasTurned = false;
        }

        GetNextJobOnTheListInfoSat();
    }

    // Some ekster info about the job
    public void GetNextJobOnTheListInfoSat()
    {
        SetAnimation(JobDoing.AnimationName);
        SetOverBodyAnimation(JobDoing.AnimationOverBodyName);

        if(JobDoing.Sound != null)
        {
            AudioSource.clip = JobDoing.Sound;
            AudioSource.Play();
        }

        if (JobDoing.Name == "Cross Arms" || JobDoing.Name == "Walk Back")
        {
            Temperment += 5;
        }

        if (JobDoing.Name == "Push")
        {
            Temperment += 10;
        }
    }

    //Face
    public void SetFaceToDefault()
    {
        if (Temperment < 60)
        {
            if (Temperment < 40)
            {
                if (Temperment < 20)
                {
                    TheBlendShaper.currentExpression = Expression.Neutral;
                }
                else
                {
                    TheBlendShaper.currentExpression = Expression.Mad;
                }
            }
            else
            {
                TheBlendShaper.currentExpression = Expression.Angry;
            }
        }
        else
        {
            TheBlendShaper.currentExpression = Expression.Angry;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        BackedUp = true;
    }

    private void OnTriggerExit(Collider other)
    {
        BackedUp = false;
    }
}
