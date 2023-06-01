using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AdsHint : MonoBehaviour
{
    [SerializeField] private Image batteryIsDischarged;
    [SerializeField] private TMP_Text pcAdsBattery;
    [SerializeField] private TMP_Text mobileAdsBattery;
    private TMP_Text adsBattery;
    private Coroutine batteryAdsHintCoroutine;
    private bool isShowBatteryAdsHint;

    private void Start()
    {
        if (PlatformManager.IsMobile)
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
            batteryAdsHintCoroutine = StartCoroutine(ActiveAdsHintBattery());
        else if (!batteryIsDischarged.gameObject.activeInHierarchy)
        {
            if (batteryAdsHintCoroutine != null) StopCoroutine(batteryAdsHintCoroutine);
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
