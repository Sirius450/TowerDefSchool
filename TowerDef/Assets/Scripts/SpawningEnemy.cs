using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningEnemy : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefab = new List<GameObject>();
    [SerializeField] Player player;
    [SerializeField] int currentWave = 1;
    [SerializeField] int enemyWave = 8;
    [SerializeField] float multiDificultyWave = 0.3f;
    [SerializeField] float timeBetweenEnemy = 0.5f;
    [SerializeField] float timeBetweenWave = 5f;

    List<GameTile> pathToGoal = new List<GameTile>();

    internal void Spawning(GameTile spawnTile, List<GameTile> NewPathToGoal)
    {
        pathToGoal = NewPathToGoal;
        StartCoroutine(SpawnEnemyCoroutine(spawnTile));
    }

    internal void NewPath(List<GameTile> NewPathToGoal)
    {
        pathToGoal = NewPathToGoal;
    }

    private int NextWave()
    {
        return Mathf.RoundToInt(enemyWave * Mathf.Pow(currentWave, multiDificultyWave));
    }




    IEnumerator SpawnEnemyCoroutine(GameTile spawnTile)
    {
        while (player.totalHp != 0)
        {
            for (int i = 0; i < enemyWave; i++)
            {
                yield return new WaitForSeconds(timeBetweenEnemy);
                var enemy = Instantiate(enemyPrefab[RNGRobot()], spawnTile.transform.position, Quaternion.identity);
                enemy.GetComponent<Enemy>().SetPath(pathToGoal);
            }
            currentWave++;
            enemyWave = NextWave();
            yield return new WaitForSeconds(timeBetweenWave);
        }
    }
    private int RNGRobot()
    {
        int i = UnityEngine.Random.Range(0, 21);

        if (i <= 6)  //hover Camarade Robot
        {
            return 0;
        }
        else if (i > 6 && i <= 12) // light Camarade Robot
        {
            return 1;
        }
        else if (i > 12 && i <= 16)  // heavy camarade robot
        {
            return 2;
        }
        else if (i > 16)    //heal camarade robot
        {
            return 3;
        }
        else
        {
            return 0;
        }
    }
}
