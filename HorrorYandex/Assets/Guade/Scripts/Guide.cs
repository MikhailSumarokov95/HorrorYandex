using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ToxicFamilyGames.FirstPersonController;
using UnityEngine.SceneManagement;

public class Guide : MonoBehaviour
{
    [SerializeField] private Button goPauseButton;
    [SerializeField] private VideoCamera videoCamera;
    [SerializeField] private GameObject battery;
    [SerializeField] private Image handTutorialPickUpImage;
    [SerializeField] private TMP_Text videoCameraButtonText;
    [SerializeField] private Image handVideoCameraButtonImage;
    [SerializeField] private Button isOffVideoCameraButton;
    [SerializeField] private TMP_Text batteriesText;
    [SerializeField] private TMP_Text batteriesFindText;
    [SerializeField] private TMP_Text batteriesAdsTextMobile;
    [SerializeField] private TMP_Text batteriesAdsTextPC;
    [SerializeField] private GameObject monster;
    [SerializeField] private Character player;
    [SerializeField] private GameObject head;

    private void Awake()
    {
        FindObjectOfType<GameInput>(true).Awake();
        FindObjectOfType<GeneralSetting>(true).LoadSettings();
    }

    private void Start()
    {
        if (!PlatformManager.IsMobile) Cursor.lockState = CursorLockMode.Locked;
        goPauseButton.gameObject.SetActive(false);
        StartCoroutine(GuideScenario());
    }

    private IEnumerator GuideScenario()
    {
        videoCamera.SetActive(false);

        videoCameraButtonText.gameObject.SetActive(true);
        if (PlatformManager.IsMobile) handVideoCameraButtonImage.gameObject.SetActive(true);

        if (PlatformManager.IsMobile) yield return new WaitForButtonClick(isOffVideoCameraButton);
        else yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));

        videoCameraButtonText.gameObject.SetActive(false);
        if (PlatformManager.IsMobile) handVideoCameraButtonImage.gameObject.SetActive(false);

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

        if (PlatformManager.IsMobile)
        {
            batteriesAdsTextMobile.gameObject.SetActive(true);
            handVideoCameraButtonImage.gameObject.SetActive(true);
        }
        else batteriesAdsTextPC.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        if (PlatformManager.IsMobile)
        {
            batteriesAdsTextMobile.gameObject.SetActive(false);
            handVideoCameraButtonImage.gameObject.SetActive(false);
        }
        else batteriesAdsTextPC.gameObject.SetActive(false);

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

        StorageManager.SetGuideCompleted();

        SceneManager.LoadScene(1);
    }
}
