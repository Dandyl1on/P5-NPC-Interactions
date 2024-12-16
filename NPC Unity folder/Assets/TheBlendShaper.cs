using UnityEngine;

public enum Expression
{
    Nothing, Neutral, Smile, Mad, Angry, Sad, Serious, Fear, Disgust, Confused, Uncomfortable, Explaining
}

public class TheBlendShaper : MonoBehaviour
{
    public Expression currentExpression = Expression.Neutral; // Current expression
    public float transitionSpeed = 5f;                        // Speed of the transition
    [Range(0, 100)] public float expressionIntensity = 100f;  // Intensity of the expression

    public SkinnedMeshRenderer skinnedMeshRenderer;
    private Mesh mesh;

    private float[] targetWeights;    // Target blendshape weights
    private float[] currentWeights;   // Current blendshape weights

    void Start()
    {
        mesh = skinnedMeshRenderer.sharedMesh;

        // Initialize weights
        int blendShapeCount = mesh.blendShapeCount;
        targetWeights = new float[blendShapeCount];
        currentWeights = new float[blendShapeCount];
    }

    void Update()
    {
        // Smoothly transition current weights to target weights
        for (int i = 0; i < targetWeights.Length; i++)
        {
            currentWeights[i] = Mathf.Lerp(currentWeights[i], targetWeights[i], Time.deltaTime * transitionSpeed);
            skinnedMeshRenderer.SetBlendShapeWeight(i, currentWeights[i]);
        }

        SetExpression(currentExpression);
    }

    public void SetExpression(Expression expression)
    {
        // Reset all blendshape target weights to zero
        for (int i = 0; i < targetWeights.Length; i++)
        {
            targetWeights[i] = 0f;
        }

        string[] primaryBlendshapes = null;
        string[] secondaryBlendshapes = null;
        string[] tertiaryBlendshapes = null;

        switch (expression)
        {
            case Expression.Smile:
                primaryBlendshapes = new string[] {
                    "Mouth_Smile_L", "Mouth_Smile_R",
                    "Cheek_Raise_L", "Cheek_Raise_R",
                    "Eye_Squint_L", "Eye_Squint_R"
                };
                secondaryBlendshapes = new string[] {
                    "Brow_Raise_Inner_L", "Brow_Raise_Inner_R",
                    "Mouth_Dimple_L", "Mouth_Dimple_R",
                    "Nose_Sneer_L", "Nose_Sneer_R"
                };
                tertiaryBlendshapes = new string[] {
                    "Head_Tilt_L",
                    "Jaw_Up",
                    "Mouth_Pucker_Up_L", "Mouth_Pucker_Up_R"
                };
                break;

            case Expression.Mad:
                primaryBlendshapes = new string[] {
                    "Mouth_Frown_L", "Mouth_Frown_R",
                    "Brow_Compress_L", "Brow_Compress_R",
                    "Eye_Wide_L", "Eye_Wide_R"
                };
                secondaryBlendshapes = new string[] {
                    "Nose_Sneer_L", "Nose_Sneer_R",
                    "Cheek_Suck_L", "Cheek_Suck_R",
                    "Mouth_Press_L", "Mouth_Press_R"
                };
                tertiaryBlendshapes = new string[] {
                    "Jaw_Forward",
                    "Neck_Tighten_L", "Neck_Tighten_R",
                    "Head_Tilt_Down"
                };
                break;

            case Expression.Angry:
                primaryBlendshapes = new string[] {
                    "Brow_Compress_L", "Brow_Compress_R",  // Strong brow compression
                    "Nose_Sneer_L", "Nose_Sneer_R",        // Flared nostrils
                    "Mouth_Tighten_L", "Mouth_Tighten_R",  // Tightened mouth
                    "Eye_Squint_L", "Eye_Squint_R"         // Intense eye squint
                };
                secondaryBlendshapes = new string[] {
                "Cheek_Suck_L", "Cheek_Suck_R",        // Sunken cheeks
                "Brow_Drop_L", "Brow_Drop_R",          // Lowered outer brows
                "Mouth_Frown_L", "Mouth_Frown_R"       // Frowning mouth edges
                };
                tertiaryBlendshapes = new string[] {
                "Jaw_Forward",                         // Forward jaw for tension
                "Neck_Tighten_L", "Neck_Tighten_R",    // Neck tightening
                "Head_Tilt_Down"                       // Aggressive head tilt
                };
                break;

            case Expression.Sad:
                primaryBlendshapes = new string[] {
                    "Brow_Compress_L", "Brow_Compress_R",
                    "Mouth_Frown_L", "Mouth_Frown_R"
                };
                secondaryBlendshapes = new string[] {
                    "Eye_Squint_L", "Eye_Squint_R"
                };
                tertiaryBlendshapes = new string[] {
                    "Nose_Sneer_L", "Nose_Sneer_R"
                };
                break;

            case Expression.Serious:
                primaryBlendshapes = new string[] {
                    "Brow_Compress_L", "Brow_Compress_R",
                    "Mouth_Tighten_L", "Mouth_Tighten_R",
                    "Eye_Squint_L", "Eye_Squint_R"
                };
                secondaryBlendshapes = new string[] {
                    "Nose_Crease_L", "Nose_Crease_R",
                    "Cheek_Suck_L", "Cheek_Suck_R"
                };
                tertiaryBlendshapes = new string[] {
                    "Head_Forward",
                    "Neck_Swallow_Up",
                    "Jaw_Backward"
                };
                break;

            case Expression.Fear:
                primaryBlendshapes = new string[] {
                    "Eye_Wide_L", "Eye_Wide_R",
                    "Brow_Raise_Inner_L", "Brow_Raise_Inner_R",
                    "Mouth_Open"
                };
                secondaryBlendshapes = new string[] {
                    "Nose_Nostril_Dilate_L", "Nose_Nostril_Dilate_R",
                    "Cheek_Suck_L", "Cheek_Suck_R",
                    "Mouth_Frown_L", "Mouth_Frown_R"
                };
                tertiaryBlendshapes = new string[] {
                    "Jaw_Down",
                    "Head_Tilt_Back",
                    "Neck_Swallow_Up"
                };
                break;

            case Expression.Disgust:
                primaryBlendshapes = new string[] {
                    "Nose_Sneer_L", "Nose_Sneer_R",
                    "Mouth_Frown_L", "Mouth_Frown_R"
                };
                secondaryBlendshapes = new string[] {
                    "Cheek_Suck_L", "Cheek_Suck_R",
                    "Brow_Compress_L", "Brow_Compress_R"
                };
                tertiaryBlendshapes = new string[] {
                    "Mouth_Roll_In_Upper_L", "Mouth_Roll_In_Upper_R",
                    "Head_Tilt_Back",
                    "Jaw_Backward"
                };
                break;

            case Expression.Confused:
                primaryBlendshapes = new string[] {
                    "Brow_Compress_L",
                    "Brow_Raise_Inner_R",
                    "Mouth_Smile_L", "Mouth_Frown_R"
                };
                secondaryBlendshapes = new string[] {
                    "Eye_Squint_L", "Eye_Squint_R",
                    "Cheek_Raise_L"
                };
                tertiaryBlendshapes = new string[] {
                    "Jaw_Sideways",
                    "Head_Tilt_L"
                };
                break;

            case Expression.Neutral:
            default:
                break;
        }

        // Apply target weights
        if (primaryBlendshapes != null)
            ApplyBlendShapes(primaryBlendshapes, 1.0f);
        if (secondaryBlendshapes != null)
            ApplyBlendShapes(secondaryBlendshapes, 0.7f);
        if (tertiaryBlendshapes != null)
            ApplyBlendShapes(tertiaryBlendshapes, 0.4f);
    }

    private void ApplyBlendShapes(string[] blendshapes, float weightMultiplier)
    {
        foreach (string blendshapeName in blendshapes)
        {
            int blendShapeIndex = mesh.GetBlendShapeIndex(blendshapeName);
            if (blendShapeIndex >= 0)
            {
                targetWeights[blendShapeIndex] = expressionIntensity * weightMultiplier;
            }
        }
    }
}
