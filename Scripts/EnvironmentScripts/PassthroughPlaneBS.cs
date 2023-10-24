using UnityEngine;

public class PassthroughPlaneBS : MonoBehaviour
{
    void Start()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_ANDROID
        OVRManager.eyeFovPremultipliedAlphaModeEnabled = false;
#endif
    }
}
