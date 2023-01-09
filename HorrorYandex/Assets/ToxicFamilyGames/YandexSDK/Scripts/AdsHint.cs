using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AdsHint : MonoBehaviour
{
    [SerializeField] private Image energyIsDischarged;
    [SerializeField] private Image batteryIsDischarged;
    [SerializeField] private TMP_Text pcAdsEnergy;
    [SerializeField] private TMP_Text pcAdsBattery;
    [SerializeField] private TMP_Text mobileAdsEnergy;
    [SerializeField] private TMP_Text mobileAdsBattery;
    private GameManager gameManager;
    private TMP_Text adsEnergy;
    private TMP_Text adsBattery;
    private Coroutine energyAdsHintCorotine;
    private Coroutine batteryAdsHintCorotine;
    private bool isShowEnergyAdsHint;
    private bool isShowBatteryAdsHint;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager.IsMobile)
        {
            adsEnergy = mobileAdsEnergy;
            adsBattery = mobileAdsBattery;
        }
        else
        {
            adsEnergy = pcAdsEnergy;
            adsBattery = pcAdsBattery;
        }
    }

    private void Update()
    {
        if (energyIsDischarged.gameObject.activeInHierarchy && !isShowEnergyAdsHint)
            energyAdsHintCorotine = StartCoroutine(ActiveAdsHintEnergy());
        else if (!energyIsDischarged.gameObject.activeInHierarchy)
        {
            if (energyAdsHintCorotine != null )StopCoroutine(energyAdsHintCorotine);
            adsEnergy.gameObject.SetActive(false);
            isShowEnergyAdsHint = false;
        }

        if (batteryIsDischarged.gameObject.activeInHierarchy && !isShowBatteryAdsHint)
            batteryAdsHintCorotine = StartCoroutine(ActiveAdsHintBattery());
        else if (!batteryIsDischarged.gameObject.activeInHierarchy)
        {
            if (batteryAdsHintCorotine != null) StopCoroutine(batteryAdsHintCorotine);
            adsBattery.gameObject.SetActive(false);
            isShowBatteryAdsHint = false;
        }
    }

    private IEnumerator ActiveAdsHintEnergy()
    {
        isShowEnergyAdsHint = true;
        adsEnergy.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        adsEnergy.gameObject.SetActive(false);
    }

    private IEnumerator ActiveAdsHintBattery()
    {
        isShowBatteryAdsHint = true;
        adsBattery.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        adsBattery.gameObject.SetActive(false);
    }
}