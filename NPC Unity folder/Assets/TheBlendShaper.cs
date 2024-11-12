using UnityEngine;

public class TheBlendShaper : MonoBehaviour
{
    // Smile parameters (weights for blendshapes)
    [Range(0, 100)] public float smileIntensity = 100f;

    public SkinnedMeshRenderer skinnedMeshRenderer;

    void Start()
    {
        // Define the blendshapes involved in a smile
        string[] smileBlendshapes = {
            "Mouth_Smile_L", "Mouth_Smile_R",            // Smile
            "Cheek_Raise_L", "Cheek_Raise_R",           // Cheeks
            "Eye_Squint_L", "Eye_Squint_R",             // Eyes squint slightly
            "Brow_Raise_Inner_L", "Brow_Raise_Inner_R", // Eyebrows slightly raised
            "Brow_Raise_Outer_L", "Brow_Raise_Outer_R"  // Eyebrows slightly raised (outer)
        };

        // Get the mesh associated with the SkinnedMeshRenderer
        Mesh mesh = skinnedMeshRenderer.sharedMesh;

        // Apply the smile to the relevant blendshapes
        foreach (string blendshapeName in smileBlendshapes)
        {
            int blendShapeIndex = mesh.GetBlendShapeIndex(blendshapeName);
            if (blendShapeIndex >= 0)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, smileIntensity);
            }
        }
    }
}
