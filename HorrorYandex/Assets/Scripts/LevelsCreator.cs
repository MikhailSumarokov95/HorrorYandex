using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LevelsCreator : MonoBehaviour
{
    public int NumberCurrentLevel { get; private set; }

    [SerializeField] private GameObject[] modeForLevelNumberPrefabs;
    [SerializeField] private GameObject[] mapPrefabs;
    [SerializeField] private GameObject menuRoom;
    [SerializeField] private GameObject guideMobile;
    [SerializeField] private GameObject guidePC;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] monsters;
    private GameObject _currentMonster;
    private GameObject _currentLevel;

    private void Start()
    {
        CheckPrefabs();
    }

    public void CreateLevel(int number)
    {
        menuRoom.SetActive(false);
        Destroy(_currentLevel);
        var numberMap = Random.Range(0, mapPrefabs.Length);
        _currentLevel = Instantiate(mapPrefabs[numberMap], Vector3.zero, Quaternion.identity);
        player.SetActive(false);
        Destroy(_currentMonster);
        StartCoroutine(WaitOneFrameAndInitializationLevel(number));
    }

    public void ReturnMenu()
    {
        if (_currentLevel != null) Destroy(_currentLevel);
        menuRoom.SetActive(true);
        Destroy(_currentMonster);
        player.SetActive(false);
    }

    public void CreateGuideLevel()
    {
        menuRoom.SetActive(false);
        player.SetActive(true);
        if (FindObjectOfType<GameManager>().IsMobile)
        {
            guideMobile.SetActive(true);
            _currentLevel = guideMobile;
        }
        else
        {
            guidePC.SetActive(true);
            _currentLevel = guidePC;
        }
        var guide = _currentLevel.GetComponent<Guade>();
        player.transform.SetPositionAndRotation(guide.PointSpawnPlayer.position, guide.PointSpawnPlayer.rotation);
    }

    private void InitializationLevel(int number)
    {
        var map = _currentLevel.GetComponent<Map>();
        Instantiate(modeForLevelNumberPrefabs[number], _currentLevel.transform);
        var level = FindObjectOfType<Level>();
        level.NumberLevel = NumberCurrentLevel = number;
        map.GetComponent<NavMeshSurface>().BuildNavMesh();
        _currentMonster = Instantiate(monsters[Random.Range(0, monsters.Length)]);
        player.SetActive(true);
        var playerSpawnPoint = map.GetSpawnPoint();
        var monsterSpawnPoint = map.GetSpawnPoint();
        player.transform.SetPositionAndRotation(playerSpawnPoint.position, playerSpawnPoint.rotation);
        _currentMonster.transform.SetPositionAndRotation(monsterSpawnPoint.position, monsterSpawnPoint.rotation);
    }

    private IEnumerator WaitOneFrameAndInitializationLevel(int number)
    {
        yield return null;
        InitializationLevel(number);
    }

    private void CheckPrefabs()
    {
        foreach (var mode in modeForLevelNumberPrefabs)
            if (mode.GetComponent<Level>() == null) Debug.LogError("No script inherited from \"Level\" script");

        foreach (var map in mapPrefabs)
            if (map.GetComponent<Map>() == null) Debug.LogError("Map does not contain script \"Map\"");
    }

    [ContextMenu("ClearSaveGuide")]
    public void ClearSaveGuide() => PlayerPrefs.SetInt("guide", 0);
}