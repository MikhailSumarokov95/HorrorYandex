using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsProgress : MonoBehaviour
{
    [SerializeField] private Level[] levels;

    private void OnEnable()
    {
        ActivationOfOpenLevelButtons();
    }

    [ContextMenu("OpenLevelTest")]
    public void OpenLevelTest()
    {
        OpenLevel(new LevelParameters() { Type = LevelParameters.LevelType.Survival, Number = 2 });
        OpenLevel(new LevelParameters() { Type = LevelParameters.LevelType.Escape, Number = 2 });

    }

    public void OpenLevel(LevelParameters levelType)
    {
        StorageManager.SetOpenLevel(levelType);
        ActivationOfOpenLevelButtons();
    }

    public void StartSurvivalLevel(int number) => SceneManager.LoadScene("SurvivalLevel" + number);

    public void StartEscapeLevel(int number) => SceneManager.LoadScene("EscapeLevel" + number);

    private void DisableAllLevelButtons()
    {
        for (var i = 0; i < levels.Length; i++)
        {
            levels[i].Opened.gameObject.SetActive(false);
            levels[i].Closed.gameObject.SetActive(true);
        }
    }

    private void ActivationOfOpenLevelButtons()
    {
        DisableAllLevelButtons();
        var levelsOpened = StorageManager.GetOpenedLevels();
        foreach (var level in levels)
            if (level.IsStarting)
            {
                level.Opened.gameObject.SetActive(true);
                level.Closed.gameObject.SetActive(false);
            }
            else 
                foreach (var levelOpened in levelsOpened)
                    if (levelOpened.Equals(level.Parameters) || level.IsStarting)
                    {
                        level.Opened.gameObject.SetActive(true);
                        level.Closed.gameObject.SetActive(false);
                    }
    }

    [Serializable]
    public class Level
    {
        public bool IsStarting;
        public LevelParameters Parameters;
        public Button Opened;
        public Button Closed;
    }
}