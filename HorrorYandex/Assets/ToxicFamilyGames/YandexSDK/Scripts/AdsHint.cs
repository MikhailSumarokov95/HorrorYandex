using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AdsHint : MonoBehaviour
{
    [SerializeField] private Image batteryIsDischarged;
    [SerializeField] private TMP_Text pcAdsBattery;
    [SerializeField] private TMP_Text mobileAdsBattery;
    private GameManager gameManager;
    private TMP_Text adsBattery;
    private Coroutine energyAdsHintCorotune;
    private Coroutine batteryAdsHintCorotune;
    private bool isShowBatteryAdsHint;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager.IsMobile)
        {
            adsBattery = mobileAdsBattery;
        }
        else
        {
            adsBattery = pcAdsBattery;
        }
    }

    private void Update()
    {
        if (batteryIsDischarged.gameObject.activeInHierarchy && !isShowBatteryAdsHint)
            batteryAdsHintCorotune = StartCoroutine(ActiveAdsHintBattery());
        else if (!batteryIsDischarged.gameObject.activeInHierarchy)
        {
            if (batteryAdsHintCorotune != null) StopCoroutine(batteryAdsHintCorotune);
            adsBattery.gameObject.SetActive(false);
            isShowBatteryAdsHint = false;
        }
    }

    private IEnumerator ActiveAdsHintBattery()
    {
        isShowBatteryAdsHint = true;
        adsBattery.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        adsBattery.gameObject.SetActive(false);
    }
}