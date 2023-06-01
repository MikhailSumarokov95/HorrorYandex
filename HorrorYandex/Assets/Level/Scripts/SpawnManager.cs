using System.Collections.Generic;
using ToxicFamilyGames.FirstPersonController;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    public GameObject[] CreateRandomObjectsOnLevel(GameObject obj, int amount)
    {
        var objects = new List<GameObject>();
        for (var i = 0; i < amount; i++)
        {
            var numberPointSpawn = Random.Range(0, spawnPoints.Length);
            if (spawnPoints[numberPointSpawn] == null)
            {
                i--;
                continue;
            }
            var positionSpawn = spawnPoints[numberPointSpawn].position;
            spawnPoints[numberPointSpawn] = null;
            objects.Add(Instantiate(obj, positionSpawn + obj.transform.position, obj.transform.rotation));
        }
        return objects.ToArray();
    }

    public void TransformObjectOnRandomPoint(GameObject obj)
    {
        while (true)
        {
            var numberPointSpawn = Random.Range(0, spawnPoints.Length);
            if (spawnPoints[numberPointSpawn] == null)
                continue;
            var positionSpawn = spawnPoints[numberPointSpawn].position;
            spawnPoints[numberPointSpawn] = null;
            obj.transform.position = positionSpawn;
            break;
        }
    }
}
