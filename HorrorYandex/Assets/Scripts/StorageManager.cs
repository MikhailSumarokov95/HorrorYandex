using System;
using System.Collections.Generic;
using UnityEngine;

public static class StorageManager
{
    private static bool _isGuideCompleted;
    private static bool _isBoughtCamera;
    private static List<LevelParameters> _levelsOpened;

    public static void SetGuideCompleted()
    {
        GSPrefs.SetInt(nameof(_isGuideCompleted), 1);
        GSPrefs.Save();
    }

    public static bool IsGuideCompleted() => 
        GSPrefs.GetInt(nameof(_isGuideCompleted), 0) == 1;

    public static void SetOpenLevel(LevelParameters level)
    {
        _levelsOpened ??= GetOpenedLevels();
        _levelsOpened.Add(level);
        var shellList = new ShellListLevelParameters() { LevelsOpened = _levelsOpened };
        GSPrefs.SetString(nameof(_levelsOpened), JsonUtility.ToJson(shellList));
        GSPrefs.Save();
    }

    public static List<LevelParameters> GetOpenedLevels()
    {
        _levelsOpened ??= JsonUtility.FromJson<ShellListLevelParameters>
            (GSPrefs.GetString(nameof(_levelsOpened),
            JsonUtility.ToJson(new ShellListLevelParameters()))).LevelsOpened;
        return _levelsOpened;
    }

    public static void SetBoughtCamera()
    {
        GSPrefs.SetInt(nameof(_isBoughtCamera), 1);
        GSPrefs.Save();
    }

    public static bool IsBoughtCamera() =>
        GSPrefs.GetInt(nameof(_isBoughtCamera), 0) == 1;

    [Serializable]
    private class ShellListLevelParameters
    {
        public List<LevelParameters> LevelsOpened;
    }
}
