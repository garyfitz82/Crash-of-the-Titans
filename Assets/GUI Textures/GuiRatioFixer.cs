using UnityEngine;
using System.Collections;

// Use this on a guiText or guiTexture object to automatically have them
// adjust their aspect ratio when the game starts.

public class GuiRatioFixer : MonoBehaviour
{
    public float m_NativeRatio = 1.3333333333333F;

    void Start()
    {
        float currentRatio = (float)Screen.width / (float)Screen.height;
        Vector3 scale = transform.localScale;
        scale.x *= m_NativeRatio / currentRatio;
        transform.localScale = scale;
    }

}