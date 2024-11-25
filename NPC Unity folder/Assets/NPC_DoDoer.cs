using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using Unity.LiveCapture.ARKitFaceCapture;

[System.Serializable]
public class NPCDo
{
    public string Name;
    public string AnimationName;
    public string AnimationOverBodyName;
    public GameObject PlaceToBe;
    public bool Importen;
    public Expression ExpressionToDo;

    NPCDo(string _Name, string _AnimationName, string _AnimationOverBodyName, GameObject _PlaceToBe, bool importen, Expression expressionToDo)
    {
        Name = _Name;
        AnimationName = _AnimationName;
        AnimationOverBodyName = _AnimationOverBodyName;
        PlaceToBe = _PlaceToBe;
        Importen = importen;
        ExpressionToDo = expressionToDo;
    }
}


public class NPC_DoDoer : MonoBehaviour
{
    public float Temperment;

    public NPCDo DoNow;

    public List<NPCDo> NPCDoingList;

    public List<NPCDo> OLDNPCMenuScript;

    public List<NPCDo> NPCMenuScript;

    public bool KSpurgt;

    public float Speed;
    public Animator Ani;

    public bool SatTurnDegress;
    public bool TurnAnimationFix;

    public bool CheckPointMessing;
    public Vector3 CheckPointLastPossing;
    public GameObject Player;

    public float PersonalSpace;
    public NPCDo PushPlayerAway;
    public NPCDo WalkBack;
    public NPCDo CrossArms;

    public bool BackedUp;

    public float TalkBlend;

    public bool SmoothOverbodyTransition;
    public float SmoothOverbodyTransitionDuration;

    [Header("Detection")]
    public NPCDo Turn;

    public bool HasTurned;

    public float detectionRadius = 30f; // Range of the detection
    public float detectionAngle = 25f;  // Half of the cone angle

    [Header("Rotation Settings")]
    public GameObject Head;
    public Transform target; // The target to look at
    public float minAngle = -45f; // Minimum angle limit
    public float maxAngle = 45f;  // Maximum angle limit
    public float rotationSpeed = 5f; // Rotation speed

    public SkinnedMeshRenderer skinnedMeshRenderer;

    public TheBlendShaper TheBlendShaper;
 //   private poosay poosay.pink = true; 

    // Start is called before the first frame update
    void Start()
    {

        Mesh mesh = skinnedMeshRenderer.sharedMesh;

        string blendShapeNames = "Blendshapes: ";
        int blendShapeCount = mesh.blendShapeCount;
        for (int i = 0; i < blendShapeCount; i++)
        {
            string blendShapeName = mesh.GetBlendShapeName(i);
            blendShapeNames += blendShapeName;

            // Add a separator if this is not the last blendshape
            if (i < blendShapeCount - 1)
                blendShapeNames += ", ";
        }

        Debug.Log(blendShapeNames);



        TheBlendShaper = FindAnyObjectByType<TheBlendShaper>();

        GetNextDo();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (DoNow.Name != "Turn")
        {
            if (DoNow.PlaceToBe != null)
            {
                DetectPlayerInCone();
            }
        }

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



        if (DoNow.PlaceToBe != null)
        {
            if (CheckPointLastPossing != DoNow.PlaceToBe.transform.position)
            {
                CheckPointLastPossing = DoNow.PlaceToBe.transform.position;
                SetAnimation("Turn2");
                SatTurnDegress = false;
            }
            else
            {
                Ani.SetBool("Turn2", false);
            }
        }


        if (DoNow.Name == "Walk")
        {
            Vector3 location = new Vector3(DoNow.PlaceToBe.transform.position.x, 0, DoNow.PlaceToBe.transform.position.z);

            if (Vector3.Distance(transform.position, location) < .1)
            {
                GetNextDo();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, location, Speed * Time.deltaTime);
            }
        }

        if (DoNow.Name == "Walk Back")
        {
            if (BackedUp == false)
            {
                Vector3 location = new Vector3(DoNow.PlaceToBe.transform.position.x, 0, DoNow.PlaceToBe.transform.position.z);

                if (Vector3.Distance(transform.position, Player.transform.position) > PersonalSpace + .5f)
                {
                    GetNextDo();
                }
                else
                {
                    Vector3 _vec = new Vector3(transform.position.x, 0, transform.position.z);

                    transform.position = Vector3.MoveTowards(_vec, location, -Speed / 2 * Time.deltaTime);
                }
            }
            else
            {
                Debug.Log("Cross Arms 2");
                NewDoIsNeeded(CrossArms);
            }
        }


        if (DoNow.Name == "Cross Arms")
        {
            if (BackedUp == true)
            {
                if (Vector3.Distance(transform.position, Player.transform.position) > PersonalSpace + .5f)
                {
                    Debug.Log("Do Cross Arms");

                    SmoothOverbodyTransition = false;
                    GetNextDo();
                }
                else
                {
                    Debug.Log("Set Push");

                    NewDoIsNeeded(PushPlayerAway);
                }
            } 
        }

        if (DoNow.Name == "Turn")
        {
            if (SatTurnDegress == false)
            {
                // Calculate direction from the object to the target
                Vector3 directionToTarget = (DoNow.PlaceToBe.transform.position - transform.position).normalized;

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
                if (TurnAnimationFix == false)
                {
                    GetNextDo();
                    SatTurnDegress = false;
                    CheckPointMessing = false;

                    TurnAnimationFix = true;
                }
                else
                {
                    SetAnimation("Turn2");
                }
            }
        }

        if (DoNow.Name != "Press Buttons" && DoNow.Name != "Cross Arms" && DoNow.Name != "Push")
        {
            if (BackedUp == false)
            {
                if (Vector3.Distance(transform.position, Player.transform.position) < PersonalSpace)
                {
                    NewDoIsNeeded(WalkBack);
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, Player.transform.position) < PersonalSpace)
                {
                    Debug.Log("Cross Arms 1: " + DoNow.Name);
                    NewDoIsNeeded(CrossArms);
                }
            }
        }

        if (DoNow.Name == "Push")
        {
            if (Vector3.Distance(transform.position, Player.transform.position) > PersonalSpace)
            {
                GetNextDo();
            }
            else
            {
                NewDoIsNeeded(PushPlayerAway);
            }
        }

        if (DoNow.Name == "Explain" || DoNow.Name == "Press Buttons")
        {
            if (KSpurgt == true)
            {
                KSpurgt = false;

                SetOverBodyAnimation("null");

                GetNextDo();
            }
        }
    }

    void DetectPlayerInCone()
    {
        if (CheckPointMessing == false)
        {
            // Find all colliders in the detection radius
            Collider[] targetsInRange = Physics.OverlapSphere(transform.position, detectionRadius);

            foreach (Collider target in targetsInRange)
            {
                if (target.gameObject == DoNow.PlaceToBe)
                {
                    // Get direction to the target
                    Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

                    // Check if the target is within the cone angle and outside a certain range
                    float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
                    float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                    if (angleToTarget > detectionAngle && distanceToTarget > 2.2f) // Adjust distance as needed
                    {
                        CheckPointMessing = true;
                        Turn.PlaceToBe = DoNow.PlaceToBe;
                        NewDoIsNeeded(Turn);
                    }
                    else
                    {
                        // Optional: Lock rotation to only certain axes (e.g., y-axis)
                        directionToTarget.y = 0;

                        // Rotate the object to face the target
                        transform.rotation = Quaternion.LookRotation(directionToTarget);
                    }
                }
            }
        }
    }

    public void NewDoIsNeeded(NPCDo _Do)
    {
        if (DoNow.Importen == true)
        {
            NPCDoingList.Insert(0, DoNow);
        }

        DoNow = _Do;

        GetNextDoInfoSat();
    }

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


            TurnAnimationFix = false;
        }

        if (DoNow.ExpressionToDo != Expression.Nothing) 
        {
            if (DoNow.ExpressionToDo != Expression.Neutral)
            {
                TheBlendShaper.currentExpression = DoNow.ExpressionToDo;
            }
            else
            {
                SetFaceToDefault();
            }
        }
    }


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



    public void RemoveDo(int number)
    {
        NPCDoingList.Remove(NPCDoingList[number]);
    }

    public void GetNextDo()
    {
        DoNow = NPCDoingList[0];
        RemoveDo(0);

        if (HasTurned == false)
        {
            if (DoNow.PlaceToBe != null)
            {
                // Find all colliders in the detection radius
                Collider[] targetsInRange = Physics.OverlapSphere(transform.position, detectionRadius);

                foreach (Collider target in targetsInRange)
                {
                    if (target.gameObject == DoNow.PlaceToBe)
                    {
                        // Get direction to the target
                        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

                        // Check if the target is within the cone angle and outside a certain range
                        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

                        if (angleToTarget > detectionAngle/4) // Adjust distance as needed
                        {
                            Debug.Log("LOOK!!!");
                            Turn.PlaceToBe = DoNow.PlaceToBe;
                            NewDoIsNeeded(Turn);

                            HasTurned = true;

                            return;
                        }
                    }
                }
            }
        }

        if (DoNow.Name != "Turn")
        {
            HasTurned = false;
        }

        GetNextDoInfoSat();
    }

    public void GetNextDoInfoSat()
    {
        SetAnimation(DoNow.AnimationName);
        SetOverBodyAnimation(DoNow.AnimationOverBodyName);

        if (DoNow.PlaceToBe != null)
        {
            CheckPointLastPossing = DoNow.PlaceToBe.transform.position;
        }

        if (DoNow.Name == "Cross Arms" || DoNow.Name == "Walk Back")
        {
            Temperment += 5;
        }

        if (DoNow.Name == "Push")
        {
            Temperment += 10;
        }
    }

    private void LateUpdate()
    {
        //LookAtPlayer();    dont worky
    }


    public void LookAtPlayer()
    {
        // Get the direction to the target
        Vector3 directionToTarget = target.position - Head.transform.position;

        // Ignore vertical differences
        directionToTarget.y = 0;

        // If the direction is zero, return
        if (directionToTarget == Vector3.zero) return;

        // Calculate the target rotation based on the direction
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Convert target rotation to an angle relative to the object's current rotation
        float angleToTarget = Mathf.DeltaAngle(Head.transform.eulerAngles.y, targetRotation.eulerAngles.y);

        // Clamp the angle to the min and max limits
        float clampedAngle = Mathf.Clamp(angleToTarget, minAngle, maxAngle);

        // Calculate the final rotation
        Quaternion clampedRotation = Quaternion.Euler(0, transform.eulerAngles.y + clampedAngle, 0);

        // Smoothly rotate towards the clamped rotation
        Head.transform.rotation = Quaternion.Slerp(Head.transform.rotation, clampedRotation, Time.deltaTime * rotationSpeed);
    }


    //Face
    public void SetFaceToDefault()
    {
        if (Temperment < 100)
        {
            if (Temperment < 80)
            {
                if (Temperment < 40)
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

    // Optional: visualize the detection area in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw the cone in the editor for better visualization
        Vector3 rightLimit = Quaternion.Euler(0, detectionAngle, 0) * transform.forward * detectionRadius;
        Vector3 leftLimit = Quaternion.Euler(0, -detectionAngle, 0) * transform.forward * detectionRadius;
        Gizmos.DrawLine(transform.position, transform.position + rightLimit);
        Gizmos.DrawLine(transform.position, transform.position + leftLimit);
    }
}
