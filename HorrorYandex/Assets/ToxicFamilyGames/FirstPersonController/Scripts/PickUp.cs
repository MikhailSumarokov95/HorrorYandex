using UnityEngine;
using HighlightPlus;
using TMPro;

public class PickUp : MonoBehaviour
{
    public bool OnSearchedObject { get; private set; }
    [SerializeField] private float distanceSearchObject;
    [SerializeField] private GameObject pickUpButton;
    [SerializeField] private TMP_Text pickUpText;
    [SerializeField] private VideoCamera videoCamera;
    [SerializeField] private Coins coins;
    private GameObject searchedObject;
    Reward reward;
    private EscapeMode escapeMode;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        pickUpButton.SetActive(false);
    }

    private void Update()
    {
        SearchObject();
        if (!gameManager.IsMobile && GameInput.Key.GetKeyDown("PickUp")) PickUpObject();
    }

    public void PickUpObject()
    {
        if (reward == null) return;
        reward.Invoke();
        Destroy(searchedObject);
    }

    private void SearchObject()
    {
       
        RaycastHit hit;
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (!Physics.Raycast(ray, out hit, distanceSearchObject))
        {
            ShowPlayerTheyCanTakeItem(false, searchedObject);
            searchedObject = null;
            reward = null;
            return;
        }
        else if (hit.collider.CompareTag("Batteries"))
        {
            searchedObject = hit.collider.gameObject;
            reward = videoCamera.SetFullCharge;
            ShowPlayerTheyCanTakeItem(true, searchedObject);
        }
        else if (hit.collider.CompareTag("Coin"))
        {
            searchedObject = hit.collider.gameObject;
            reward = coins.PickUpCoin;
            ShowPlayerTheyCanTakeItem(true, searchedObject);
        }
        else if (hit.collider.CompareTag("Key"))
        {
            if (escapeMode == null) escapeMode = FindObjectOfType<EscapeMode>();
            searchedObject = hit.collider.gameObject;
            reward = escapeMode.PickUpKey;
            ShowPlayerTheyCanTakeItem(true, searchedObject);
        }
        else
        {
            ShowPlayerTheyCanTakeItem(false, searchedObject);
            searchedObject = null;
            reward = null;
        }
        print(hit.collider.tag + hit.collider.name);
    }

    private void ShowPlayerTheyCanTakeItem(bool value, GameObject searchedObject)
    {
        try
        {
            searchedObject.GetComponent<HighlightEffect>().SetHighlighted(value);
        }
        catch { }
        if (gameManager.IsMobile) pickUpButton.SetActive(value);
        else pickUpText.gameObject.SetActive(value);
        OnSearchedObject = value;
    }

    delegate void Reward();
}
