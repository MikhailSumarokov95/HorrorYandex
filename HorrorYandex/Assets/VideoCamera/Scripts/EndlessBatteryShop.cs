using UnityEngine;

public class EndlessBatteryShop : MonoBehaviour, IGSPurchase
{
    private void Awake()
    {
        if (StorageManager.IsBoughtCamera())
            gameObject.SetActive(false);
    }

    public void TryBuy()
    {
        GSConnect.Purchase(GSConnect.PurchaseTag.EndlessFlashlight, this);
    }

    public void RewardPerPurchase()
    {
        StorageManager.SetBoughtCamera();
        var videoCamera = FindObjectOfType<VideoCamera>(true);
        if (videoCamera != null) videoCamera.SetEndless();
        gameObject.SetActive(false);
    }
}
