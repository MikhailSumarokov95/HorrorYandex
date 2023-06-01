using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private GameObject headPlayers;
    private Transform target;

    private void Start()
    {
        target = headPlayers.transform;
    }

    private void LateUpdate()
    {
        transform.SetPositionAndRotation(target.position, target.rotation);
    }
}