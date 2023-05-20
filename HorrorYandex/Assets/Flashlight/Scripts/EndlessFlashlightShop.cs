using UnityEngine;

public class EndlessFlashlightShop : MonoBehaviour, IGSPurchase
{
    private void Awake()
    {
        if (GSPrefs.GetInt(nameof(GSConnect.PurchaseTag.EndlessFlashlight), 0) == 1)
        {
            FindObjectOfType<Flashlight>(true).IsEndless = true;
            gameObject.SetActive(false);
        }
    }

    public void TryBuy()
    {
        GSConnect.Purchase(GSConnect.PurchaseTag.EndlessFlashlight, this);
    }

    public void RewardPerPurchase()
    {
        GSPrefs.SetInt(nameof(GSConnect.PurchaseTag.EndlessFlashlight), 1);
        GSPrefs.Save();
        var flashlight = FindObjectOfType<Flashlight>(true);
        flashlight.IsEndless = true;
        flashlight.SetFullCharge();
        gameObject.SetActive(false);
    }
}
