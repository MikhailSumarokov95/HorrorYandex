using UnityEngine;
using TMPro;

public class EscapeMode : Level
{
    [SerializeField] private GameObject key;
    [SerializeField] private int[] numberOfKeysToWinDependingOnTheLevelNumber;
    private int _numberFoundKeys;
    private GameObject _numberFoundKeysGO;
    private TMP_Text _numberFoundKeysText;

    private void Start()
    {
        base.Start();
        _numberFoundKeysGO = GameObject.FindGameObjectWithTag("NumberFoundKeys");
        SetActivateChildTransform(_numberFoundKeysGO.transform, true);
        _numberFoundKeysText = _numberFoundKeysGO.transform.GetChild(0).GetComponent<TMP_Text>();
        SetTextNumberFoundKeys(0);
        _map.CreateRandomObjectsOnLevel(key, numberOfKeysToWinDependingOnTheLevelNumber[_difficultyLevelNumber]);
    }

    private void OnDestroy()
    {
        SetActivateChildTransform(_numberFoundKeysGO.transform, false);
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
