using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ToxicFamilyGames.FirstPersonController;

public class Guade : MonoBehaviour
{
    public Transform PointSpawnPlayer;
    [SerializeField] private Button goPauseButton;
    [SerializeField] private Flashlight flashlight;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PickUp pickUp;
    [SerializeField] private Energy energy;
    [SerializeField] private GameObject battery;
    [SerializeField] private GameObject energyDrink;
    [SerializeField] private Image handForEyesButtonImage;
    [SerializeField] private TMP_Text forEyesButtonText;
    [SerializeField] private Button isCloseEyesButton;
    [SerializeField] private TMP_Text energyLevelText;
    [SerializeField] private Image handEnergyLevelImage;
    [SerializeField] private TMP_Text energyLevelLookForEnergyText;
    [SerializeField] private Image handTutorialPickUpImage;
    [SerializeField] private TMP_Text handTutorialPickUpText;
    [SerializeField] private Button pickUpButton;
    [SerializeField] private TMP_Text flashlightButtonText;
    [SerializeField] private Image handFlashlightButtonImage;
    [SerializeField] private Button isOffFlashlightButton;
    [SerializeField] private TMP_Text batteriesText;
    [SerializeField] private TMP_Text batteriesFindText;
    [SerializeField] private GameObject monster;
    [SerializeField] private Character player;
    [SerializeField] private GameObject head;

    private void OnEnable()
    {
        StartCoroutine(GuadeScenario());
        flashlight.gameObject.SetActive(false);
        goPauseButton.gameObject.SetActive(false);
        flashlight.OnDisable();
        gameManager.PauseKeyLock = true;
    }

    private void OnDisable()
    {
        goPauseButton.gameObject.SetActive(true);
        gameManager.PauseKeyLock = false;
    }

    private void Update()
    {
        if (energy.Value < 0.05) energy.SetFullEnergy();
    }

    private IEnumerator GuadeScenario()
    {
        if (gameManager.IsMobile) handForEyesButtonImage.gameObject.SetActive(true);
        forEyesButtonText.gameObject.SetActive(true);
        
        if (gameManager.IsMobile) yield return new WaitForButtonClick(isCloseEyesButton);
        else yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (gameManager.IsMobile) handForEyesButtonImage.gameObject.SetActive(false);
        forEyesButtonText.gameObject.SetActive(false);

        energyLevelText.gameObject.SetActive(true);
        handEnergyLevelImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        energyLevelText.gameObject.SetActive(false);
        handEnergyLevelImage.gameObject.SetActive(false);

        energyLevelLookForEnergyText.gameObject.SetActive(true);
        energyDrink.SetActive(true);

        while(energyDrink != null)
        {
            if (pickUp.OnSearchedObject)
            {
                if (gameManager.IsMobile) handTutorialPickUpImage.gameObject.SetActive(true);
                else handTutorialPickUpText.gameObject.SetActive(true);
            }

            else
            {
                if (gameManager.IsMobile) handTutorialPickUpImage.gameObject.SetActive(false);
                else handTutorialPickUpText.gameObject.SetActive(false);
            }
            yield return null;
        }

        if (gameManager.IsMobile) handTutorialPickUpImage.gameObject.SetActive(false);
        else handTutorialPickUpText.gameObject.SetActive(false);
        energyLevelLookForEnergyText.gameObject.SetActive(false);

        flashlight.gameObject.SetActive(true);
        flashlight.GetComponent<Flashlight>().SetActiveFlashlight(false);

        flashlightButtonText.gameObject.SetActive(true);
        if (gameManager.IsMobile) handFlashlightButtonImage.gameObject.SetActive(true);

        if (gameManager.IsMobile) yield return new WaitForButtonClick(isOffFlashlightButton);
        else yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));

        flashlightButtonText.gameObject.SetActive(false);
        if (gameManager.IsMobile) handFlashlightButtonImage.gameObject.SetActive(false);

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

        head.transform.localRotation = Quaternion.identity;
        player.isLocked = true;
        monster.SetActive(true);
        var monsterPosition = head.transform.position - head.transform.transform.forward * 2 + new Vector3(0, - 0.6f, 0);
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

        player.isLocked = false;

        PlayerPrefs.SetInt("guade", 1);
        FindObjectOfType<GameManager>().StartMenu();
    }
}
