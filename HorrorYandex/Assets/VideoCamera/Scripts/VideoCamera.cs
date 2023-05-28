using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoCamera : MonoBehaviour, IGSPurchase
{
    [SerializeField] private float dischargingFactor = 0.005f;
    [SerializeField] private Light spotLight;
    [SerializeField] private Button isOnFlashlightButton;
    [SerializeField] private Button isOffFlashlightButton;
    [SerializeField] private Button isDischargedFlashlightButton;
    [SerializeField] private Image isDischargedImage;
    [SerializeField] private Image isNotDischargedImage;
    [SerializeField] private Slider batteryChargeSlider;
    [SerializeField] private TMP_Text pcFlashlightButtonText;
    private bool isOnFlashlight = true;
    private float batteryCharge = 1f;
    private float maxIntensityLight;
    private GameManager gameManager;
    private Animator animator;

    public bool IsEndless { get; set; }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        maxIntensityLight = spotLight.intensity;
    }

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        SetFullCharge();
        isDischargedImage.transform.parent.gameObject.SetActive(true);
        isOnFlashlightButton.transform.parent.gameObject.SetActive(gameManager.IsMobile);
        SetActiveFlashlight(true);
    }

    public void OnDisable()
    {
        isDischargedImage.transform.parent.gameObject.SetActive(false);
        isOnFlashlightButton.transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!BatteryChargeCheck())
        {
            if (GameInput.Key.GetKeyDown("OnFlashlight")) GSConnect.ShowRewardedAd(this);
            return;
        }
        if (!gameManager.IsMobile && GameInput.Key.GetKeyDown("OnFlashlight"))
            SetActiveFlashlight(!spotLight.gameObject.activeInHierarchy);
        if (isOnFlashlight && !IsEndless) DischargingBattery();
    }

    public void RewardPerPurchase() => SetFullCharge();

    public void SetFullCharge()
    {
        batteryCharge = 1f;
        batteryChargeSlider.value = batteryCharge;
        isDischargedFlashlightButton.gameObject.SetActive(false);
        isDischargedImage.gameObject.SetActive(false);
        isNotDischargedImage.gameObject.SetActive(true);
        SetActiveFlashlight(true);
    }

    public void SetActiveFlashlight(bool value)
    {
        spotLight.gameObject.SetActive(value);
        isOnFlashlight = value;
        isOnFlashlightButton.gameObject.SetActive(value);
        isOffFlashlightButton.gameObject.SetActive(!value);
        if (value) animator.SetTrigger("On");
        else animator.SetTrigger("Off");
        if (!gameManager.IsMobile) pcFlashlightButtonText.gameObject.SetActive(!value);
    }

    private void DischargingBattery()
    {
        batteryCharge -= Time.deltaTime * dischargingFactor;
        spotLight.intensity = maxIntensityLight * batteryCharge;
        batteryChargeSlider.value = batteryCharge;
    }

    private bool BatteryChargeCheck()
    {
        if (batteryCharge < 0.01f)
        {
            isDischargedImage.gameObject.SetActive(true);
            isNotDischargedImage.gameObject.SetActive(false);
            isOffFlashlightButton.gameObject.SetActive(false);
            isOnFlashlightButton.gameObject.SetActive(false);
            isDischargedFlashlightButton.gameObject.SetActive(true);
            return false;
        }
        return true;
    }
}
