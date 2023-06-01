using UnityEngine;
using TMPro;

public class SurvivalMode : Level
{
    [Header("Object")]
    [SerializeField] private GameObject battery;
    [SerializeField] private int amountBattery = 3;
    [Header("Timer")]
    [SerializeField] private int[] timerTimeDependingOnTheLevelNumber;
    [SerializeField] private TMP_Text _timerText;
    private float _timer;

    public float Timer
    {
        get
        {
            return _timer;
        }
        private set
        {
            _timer = value;
            _timerText.text = _timer.ToString("F0");
        }
    }

    protected override void Start()
    {
        base.Start();
        _timerText.text = "0";
        Timer = timerTimeDependingOnTheLevelNumber[levelType.Number];
        FindObjectOfType<SpawnManager>().CreateRandomObjectsOnLevel(battery, amountBattery);
    }

    private void Update()
    {
        if (_isGameOver) return;
        if (Timer < 0)
        {
            WinLevel();
            _isGameOver = true;
        }
        Timer -= Time.deltaTime;
    }
}