using System.Collections.Generic;
using ToxicFamilyGames.FirstPersonController;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winTable;
    [SerializeField] private GameObject lossTable;
    [SerializeField] private GameObject pauseTable;
    [SerializeField] private GameObject gameTable;
    [SerializeField] private GameObject goPauseButton;
    [SerializeField] private GeneralSetting generalSetting;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private KeyCode keyPause;
    [SerializeField] private Monster[] monsters;
    [SerializeField] private Character player;

    private Monster _currentMonster;
    public Monster CurrentMonster { get { return _currentMonster; } private set { _currentMonster = value; } }

    public static bool IsPause { get; set; }

    private void Awake()
    {
        gameInput.Awake();
        var spawner = FindObjectOfType<SpawnManager>();
        spawner.TransformObjectOnRandomPoint(player.gameObject);
        CurrentMonster = spawner.CreateRandomObjectsOnLevel(monsters[Random.Range(0, monsters.Length)].gameObject, 1)[0].GetComponent<Monster>();
    }

    private void Start()
    {
        generalSetting.LoadSettings();
        goPauseButton.SetActive(PlatformManager.IsMobile);
        OnPause(false);
    }

    private void Update()
    {
        if (!PlatformManager.IsMobile && Input.GetKeyDown(keyPause) && !IsPause) 
            OnPausePanel(!IsPause);
    }

    public void StartMenu()
    {
        OnPause(false);
        pauseTable.SetActive(false);
        GSConnect.ShowMidgameAd();
        SceneManager.LoadScene(1);
    }

    public void RestartLevel()
    {
        OnPause(false);
        GSConnect.ShowMidgameAd();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnWin()
    {
        gameTable.SetActive(false);
        winTable.SetActive(true);
        OnPause(true);
    }

    public void OnLoss()
    {
        gameTable.SetActive(false);
        lossTable.SetActive(true);
        OnPause(true);
    }

    public void OnPausePanel(bool value)
    {
        pauseTable.SetActive(value);
        OnPause(value);
    }

    private void OnPause(bool value)
    {
        IsPause = value;
        Time.timeScale = value ? 0 : 1;
        if (!PlatformManager.IsMobile) Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
