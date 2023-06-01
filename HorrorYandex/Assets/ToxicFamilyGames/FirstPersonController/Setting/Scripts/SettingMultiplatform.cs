using UnityEngine;

public class SettingMultiplatform : MonoBehaviour
{
    [SerializeField] private GameObject pcSetting;
    [SerializeField] private GameObject mobileSetting;

    private void OnEnable()
    {
        var isMobile = PlatformManager.IsMobile;
        mobileSetting.SetActive(isMobile);
        pcSetting.SetActive(!isMobile);
    }
}
