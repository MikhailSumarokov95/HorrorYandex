using UnityEngine;
using TMPro;

public class EscapeMode : Level
{
    [Header("Object")]
    [SerializeField] private GameObject battery;
    [SerializeField] private int amountBattery = 3;
    [SerializeField] private GameObject key;
    [Header("Keys")]
    [SerializeField] private int[] numberOfKeysToWinDependingOnTheLevelNumber;
    [SerializeField] private TMP_Text _numberFoundKeysText;
    private int _numberFoundKeys;

    protected override void Start()
    {
        base.Start();
        SetTextNumberFoundKeys(0);
        var spawner = FindObjectOfType<SpawnManager>();
        spawner.CreateRandomObjectsOnLevel(key, numberOfKeysToWinDependingOnTheLevelNumber[_difficultyLevelNumber]);
        spawner.CreateRandomObjectsOnLevel(battery, amountBattery);
    }

    [ContextMenu("PickUpKey")]
    public void PickUpKey()
    {
        _numberFoundKeys++;
        SetTextNumberFoundKeys(_numberFoundKeys);
        if (_numberFoundKeys >= numberOfKeysToWinDependingOnTheLevelNumber[_difficultyLevelNumber]) WinLevel(); 
    }

    private void SetTextNumberFoundKeys(int number)
    {
        _numberFoundKeysText.text = 
            number.ToString() + "/" + numberOfKeysToWinDependingOnTheLevelNumber[_difficultyLevelNumber].ToString();
    }
}
