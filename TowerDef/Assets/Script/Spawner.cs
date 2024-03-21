using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Metadata;

public class Spawner : MonoBehaviour
{
    [SerializeField] internal List<Transform> node;
    [SerializeField] private GameObject path;
    [SerializeField] private List<GameObject> prefadListEnnemy;
    [SerializeField] private float waiteTime = 05f;


    private void Awake()
    {
        foreach (Transform child in path.transform)
        {
            if (child.tag != "Spawner")
            {
                node.Add(child);
            }
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {

        int indexEnemy = Random.Range(0, prefadListEnnemy.Count);
        GameObject Enemy = Instantiate(prefadListEnnemy[indexEnemy], this.transform.position, Quaternion.identity);
        Enemy.transform.parent = this.transform;

        yield return new WaitForSeconds(waiteTime);
        StartCoroutine(SpawnEnemy());
    }
}
