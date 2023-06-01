using UnityEngine;
using ToxicFamilyGames.FirstPersonController;

public class Level : MonoBehaviour
{
    [SerializeField] protected LevelParameters levelType;
    private BackRoundMusic _backRoundMusic;
    protected int _difficultyLevelNumber;
    protected bool _isGameOver;

    protected virtual void Start()
    {
        _backRoundMusic = FindObjectOfType<BackRoundMusic>();
    }

    public void WinLevel()
    {
        FindObjectOfType<LevelsProgress>().OpenLevel(levelType);
        FindObjectOfType<GameManager>().OnWin();
        _backRoundMusic.IsPause = true;
    }

    public void LossLevel()
    {
        if (_isGameOver) return;
        _isGameOver = true;
        FindObjectOfType<GameManager>().OnLoss();
        FindObjectOfType<Character>().IsBrokenNeck = true;
        _backRoundMusic.IsPause = true;
    }

    protected void SetActivateChildTransform(Transform transform, bool value)
    {
        for (var i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(value);
    }
}
