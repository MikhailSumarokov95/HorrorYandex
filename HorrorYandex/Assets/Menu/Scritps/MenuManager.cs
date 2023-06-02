using GameScore;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GeneralSetting generalSetting;
    private void Awake()
    {
        if (!Application.isEditor) GS_Device.IsMobile();
        if (!Application.isEditor) PlayerPrefs.SetString("selectedLanguage", GS_Language.Current());
    }

    private void Start()
    {
        if (!PlatformManager.IsMobile) Cursor.lockState = CursorLockMode.None;
        generalSetting.LoadSettings();
        if (!StorageManager.IsGuideCompleted())
            StartGuide();
    }

    public void StartGuide() => SceneManager.LoadScene(2);
}
