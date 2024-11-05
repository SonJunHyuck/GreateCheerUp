using UnityEngine;
using System.Collections.Generic;

public class AllySpawner : MonoBehaviour
{
    // Reference to the spawn location (can be set via the inspector)
    public Transform spawnLocation;

    // Key를 이용해 소환, Button에 이벤트 호출
    public void SpawnAlly(string key)
    {
        // Call the GetFromPool function to get the unit from the object pool
        GameObject ally = PoolManager.Instance.AllyPool.GetFromPool(key);

        if (ally != null)
        {
            // Set the ally's position to the spawn location and activate it
            ally.transform.position = spawnLocation.position;
            ally.GetComponent<Collider2D>().enabled = true;
            ally.SetActive(true);

            Debug.Log($"{key} has been spawned at {spawnLocation.position}.");
        }
        else
        {
            Debug.LogWarning($"No ally found in the pool with key: {key}");
        }
    }
}