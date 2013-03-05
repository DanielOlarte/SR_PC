using UnityEngine;
using System.Collections;

public class RatioFixer : MonoBehaviour {

	public float m_NativeRatio = 1.6F;
 
    void Start ()
    {
        float currentRatio = (float)Screen.width / (float)Screen.height;
        Vector3 scale = transform.localScale;
        scale.x *= currentRatio/m_NativeRatio;
        transform.localScale = scale;
    }
}
