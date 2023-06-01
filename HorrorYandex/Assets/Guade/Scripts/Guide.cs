using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ToxicFamilyGames.FirstPersonController;
using UnityEngine.SceneManagement;

public class Guide : MonoBehaviour
{
    public Transform PointSpawnPlayer;
    [SerializeField] private Button goPauseButton;
    [SerializeField] private VideoCamera videoCamera;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PickUp pickUp;
    [SerializeField] private GameObject battery;
    [SerializeField] private Image handTutorialPickUpImage;
    [SerializeField] private TMP_Text handTutorialPickUpText;
    [SerializeField] private Button pickUpButton;
    [SerializeField] private TMP_Text flashlightButtonText;
    [SerializeField] private Image handFlashlightButtonImage;
    [SerializeField] private Button isOffFlashlightButton;
    [SerializeField] private TMP_Text batteriesText;
    [SerializeField] private TMP_Text batteriesFindText;
    [SerializeField] private TMP_Text batteriesAdsText;
    [SerializeField] private GameObject monster;
    [SerializeField] private Character player;
    [SerializeField] private GameObject head;

    private void OnEnable()
    {
        goPauseButton.gameObject.SetActive(false);
        StartCoroutine(GuideScenario());
    }

    private void OnDisable()
    {
        goPauseButton.gameObject.SetActive(true);
    }

    private IEnumerator GuideScenario()
    {
        videoCamera.SetActiveFlashlight(false);

        flashlightButtonText.gameObject.SetActive(true);
        if (PlatformManager.IsMobile) handFlashlightButtonImage.gameObject.SetActive(true);

        if (PlatformManager.IsMobile) yield return new WaitForButtonClick(isOffFlashlightButton);
        else yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));

        flashlightButtonText.gameObject.SetActive(false);
        if (PlatformManager.IsMobile) handFlashlightButtonImage.gameObject.SetActive(false);

        batteriesText.gameObject.SetActive(true);
        battery.SetActive(true);

        yield return new WaitForSeconds(1f);

        batteriesFindText.gameObject.SetActive(true);

        while(battery != null)
        {
            yield return null;
        }

        batteriesFindText.gameObject.SetActive(false);
        batteriesText.gameObject.SetActive(false);

        batteriesAdsText.gameObject.SetActive(true);
        if (PlatformManager.IsMobile) handFlashlightButtonImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        batteriesAdsText.gameObject.SetActive(false);
        if (PlatformManager.IsMobile) handFlashlightButtonImage.gameObject.SetActive(false);

        head.transform.localRotation = Quaternion.identity;
        player.IsLocked = true;
        monster.SetActive(true);
        var monsterPosition = head.transform.position - head.transform.transform.forward * 3 + new Vector3(0, -1.8f, 0);
        var monsterRotation = player.transform.rotation * monster.transform.rotation;
        monster.transform.SetPositionAndRotation(monsterPosition, monsterRotation);

        while (true)
        {
            var rotationForLookAnMonster = Quaternion.LookRotation(- monster.transform.forward);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotationForLookAnMonster, 0.1f);
            if (Mathf.Abs(player.transform.rotation.eulerAngles.y - rotationForLookAnMonster.eulerAngles.y) < 1f) break;
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        player.IsLocked = false;

        SceneManager.LoadScene(1);
    }
}
