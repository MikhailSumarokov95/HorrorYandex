using GameScore;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private static bool isMobile;
    public static bool IsMobile { get { return isMobile; } private set { isMobile = value; } }

    private void Awake()
    {
        if (!Application.isEditor) IsMobile = GS_Device.IsMobile();
    }
}
