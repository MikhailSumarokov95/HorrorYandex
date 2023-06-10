using GameScore;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GeneralSetting generalSetting;

    private void Awake()
    {
        if (!Application.isEditor) PlayerPrefs.SetString("selectedLanguage", GS_Language.Current());
    }

    private void Start()
    {
        if (!PlatformManager.IsMobile) Cursor.lockState = CursorLockMode.None;
        generalSetting.LoadSettings();
    }
}
