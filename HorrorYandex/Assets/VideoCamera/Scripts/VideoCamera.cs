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
    private bool _isOnFlashlight = true;
    private float _batteryCharge = 1f;
    private float _maxIntensityLight;
    private Animator _animator;
    private bool _isEndless;

    private void Awake()
    {
        _maxIntensityLight = spotLight.intensity;
    }

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        SetFullCharge();
        isDischargedImage.transform.parent.gameObject.SetActive(true);
        isOnFlashlightButton.transform.parent.gameObject.SetActive(PlatformManager.IsMobile);
        _isEndless = StorageManager.IsBoughtCamera();
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
        if (!PlatformManager.IsMobile && GameInput.Key.GetKeyDown("OnFlashlight"))
            SetActive(!spotLight.gameObject.activeInHierarchy);
        if (_isOnFlashlight && !_isEndless) DischargingBattery();
    }

    public void RewardPerPurchase() => SetFullCharge();

    public void TryRewardAd() => GSConnect.ShowRewardedAd(this);

    public void SetFullCharge()
    {
        _batteryCharge = 1f;
        batteryChargeSlider.value = _batteryCharge;
        spotLight.intensity = _maxIntensityLight;
        isDischargedFlashlightButton.gameObject.SetActive(false);
        isDischargedImage.gameObject.SetActive(false);
        isNotDischargedImage.gameObject.SetActive(true);
        SetActive(true);
    }

    public void SetActive(bool value)
    {
        spotLight.gameObject.SetActive(value);
        _isOnFlashlight = value;
        isOnFlashlightButton.gameObject.SetActive(value);
        isOffFlashlightButton.gameObject.SetActive(!value);
        if (value) _animator.SetTrigger("On");
        else _animator.SetTrigger("Off");
        if (!PlatformManager.IsMobile) pcFlashlightButtonText.gameObject.SetActive(!value);
    }

    public void SetEndless()
    {
        _isEndless = true;
        SetFullCharge();
    }

    private void DischargingBattery()
    {
        _batteryCharge -= Time.deltaTime * dischargingFactor;
        spotLight.intensity = _maxIntensityLight * _batteryCharge;
        batteryChargeSlider.value = _batteryCharge;
    }

    private bool BatteryChargeCheck()
    {
        if (_batteryCharge < 0.01f)
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
