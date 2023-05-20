using UnityEngine;
using ToxicFamilyGames.YandexSDK;
public class GameManager : MonoBehaviour
{
    public bool PauseKeyLock { get; set; }
    public bool IsMobile;
    [SerializeField] private GameObject menuRoom;
    [SerializeField] private GameObject winTable;
    [SerializeField] private GameObject lossTable;
    [SerializeField] private GameObject pauseTable;
    [SerializeField] private GameObject gameTable;
    [SerializeField] private GameObject menuTable;
    [SerializeField] private GameObject goPauseButton;
    [SerializeField] private GeneralSetting generalSetting;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private KeyCode keyPause;
    private LevelsCreator _levelCreator;

    public bool IsPause { get; private set; }

    private void Awake()
    {
        IsMobile = AuthorizationYandex.IsMobile();
    }

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        gameInput.Awake();
        generalSetting.LoadSettings();
        _levelCreator = FindObjectOfType<LevelsCreator>();
        goPauseButton.SetActive(IsMobile);

        if (PlayerPrefs.GetInt("guade", 0) == 0)
        {
            _levelCreator.CreateGuideLevel();
            gameTable.SetActive(true);
        }
        else StartMenu();
    }

    private void Update()
    {
        if (!IsMobile && Input.GetKeyDown(keyPause) && !menuRoom.activeInHierarchy && !PauseKeyLock) 
            OnPausePanel(!IsPause);
    }

    public void StartMenu()
    {
        PauseKeyLock = true;
        menuTable.SetActive(true);
        gameTable.SetActive(false);
        _levelCreator.ReturnMenu();
        OnPause(false);
        pauseTable.SetActive(false);
    }

    public void StartLevel()
    {
        PauseKeyLock = false;
        menuTable.SetActive(false);
        gameTable.SetActive(true);
        OnPause(false);
    }

    public void RestartLevel()
    {
        PauseKeyLock = false;
        _levelCreator.CreateLevel(_levelCreator.NumberCurrentLevel);
        Cursor.lockState = CursorLockMode.Locked;
        OnPause(false);
    }

    public void OnWin()
    {
        PauseKeyLock = true;
        gameTable.SetActive(false);
        winTable.SetActive(true);
        OnPause(true);
        if (!IsMobile) Cursor.lockState = CursorLockMode.None;
    }

    public void OnLoss()
    {
        PauseKeyLock = true;
        gameTable.SetActive(false);
        lossTable.SetActive(true);
        OnPause(true);
        if (!IsMobile) Cursor.lockState = CursorLockMode.None;
    }

    public void OnPausePanel(bool value)
    {
        pauseTable.SetActive(value);
        OnPause(value);
        if (!IsMobile) Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void OnPause(bool value)
    {
        IsPause = value;
        Time.timeScale = value ? 0 : 1;
    }
}
