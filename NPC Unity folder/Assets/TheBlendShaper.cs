using UnityEngine;

public class TheBlendShaper : MonoBehaviour
{
    // Enum for all possible emotions
    public enum Expression
    {
        Neutral, Smile, Mad, Angry, Serious, Fear, Disgust, Confused
    }

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
        // Reset target weights to zero
        for (int i = 0; i < targetWeights.Length; i++)
        {
            targetWeights[i] = 0f;
        }

        // Define target blendshapes for the new expression
        string[] blendshapes = null;

        switch (expression)
        {
            case Expression.Smile:
                blendshapes = new string[] {
                    "Mouth_Smile_L", "Mouth_Smile_R",            // Smile
                    "Cheek_Raise_L", "Cheek_Raise_R",           // Cheeks
                    "Eye_Squint_L", "Eye_Squint_R",             // Eyes squint slightly
                    "Brow_Raise_Inner_L", "Brow_Raise_Inner_R"  // Eyebrows slightly raised
                };
                break;

            case Expression.Mad:
                blendshapes = new string[] {
                    "Mouth_Frown_L", "Mouth_Frown_R",           // Mouth frown
                    "Cheek_Suck_L", "Cheek_Suck_R",            // Sunken cheeks
                    "Brow_Compress_L", "Brow_Compress_R",      // Brows compression
                    "Eye_Wide_L", "Eye_Wide_R"                 // Wide open eyes
                };
                break;

            case Expression.Serious:
                blendshapes = new string[] {
                    "Brow_Compress_L", "Brow_Compress_R",      // Brows compression
                    "Mouth_Tighten_L", "Mouth_Tighten_R",      // Tightened mouth
                    "Eye_Squint_L", "Eye_Squint_R",            // Squinted eyes
                    "Cheek_Suck_L", "Cheek_Suck_R"             // Cheek compression
                };
                break;

            case Expression.Angry:
                blendshapes = new string[] {
                    "Brow_Raise_Outer_L", "Brow_Raise_Outer_R",  // Raised outer brows
                    "Mouth_Tighten_L", "Mouth_Tighten_R",      // Tightened mouth
                    "Nose_Sneer_L", "Nose_Sneer_R",            // Flared nostrils
                    "Cheek_Suck_L", "Cheek_Suck_R"             // Aggressive cheek compression
                };
                break;

            case Expression.Fear:
                blendshapes = new string[] {
                    "Eye_Wide_L", "Eye_Wide_R",                 // Wide-open eyes
                    "Mouth_Open",                               // Open mouth
                    "Brow_Raise_Inner_L", "Brow_Raise_Inner_R"  // Raised inner brows
                };
                break;

            case Expression.Disgust:
                blendshapes = new string[] {
                    "Nose_Sneer_L", "Nose_Sneer_R",            // Flared nostrils
                    "Mouth_Frown_L", "Mouth_Frown_R",          // Frowned mouth
                    "Cheek_Suck_L", "Cheek_Suck_R"             // Compressed cheeks
                };
                break;

            case Expression.Confused:
                blendshapes = new string[] {
                    "Brow_Compress_L", "Brow_Raise_Inner_R",    // One raised brow
                    "Mouth_Smile_L", "Mouth_Frown_R"           // Asymmetrical mouth
                };
                break;

            case Expression.Neutral:
            default:
                // Neutral does not apply specific blendshapes
                break;
        }

        // Set target weights for the blendshapes of the new expression
        if (blendshapes != null)
        {
            foreach (string blendshapeName in blendshapes)
            {
                int blendShapeIndex = mesh.GetBlendShapeIndex(blendshapeName);
                if (blendShapeIndex >= 0)
                {
                    targetWeights[blendShapeIndex] = expressionIntensity;
                }
            }
        }
    }
}
