using System.Collections.Generic;
using System.Linq;
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
        GSPrefs.SetString(nameof(_levelsOpened), JsonUtility.ToJson(_levelsOpened));
        GSPrefs.Save();
    }

    public static List<LevelParameters> GetOpenedLevels()
    {
        _levelsOpened ??= JsonUtility.FromJson<List<LevelParameters>>
            (GSPrefs.GetString(nameof(_levelsOpened),
            JsonUtility.ToJson(new List<LevelParameters>())));
        return _levelsOpened;
    }

    public static void SetBoughtCamera()
    {
        GSPrefs.SetInt(nameof(_isBoughtCamera), 1);
        GSPrefs.Save();
    }

    public static bool IsBoughtCamera() =>
        GSPrefs.GetInt(nameof(_isBoughtCamera), 0) == 1;
}
