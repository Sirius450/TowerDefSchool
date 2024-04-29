using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningEnemy : MonoBehaviour
{
    [Header("Spawning info")]
    [SerializeField] List<GameObject> enemyPrefab = new List<GameObject>();
    [SerializeField] int currentWave = 1;
    [SerializeField] int enemyWave = 8;
    [SerializeField] int maxWave = 20;
    [SerializeField] int expGain = 100;

    [Header("Wave seting")]
    [SerializeField] float multiDificultyWave = 0.3f;
    [SerializeField] float timeBetweenEnemy = 0.5f;
    [SerializeField] float timeBetweenWave = 5f;
    [SerializeField] float reduceTimeWave = 0.02f;
    [SerializeField] float timeBetweenMap = 10f;
    internal bool nextWave = true;
    internal bool reset = false;

    [Header("player")]
    [SerializeField] Player player;

    GameManager gameManager;
    List<GameTile> pathToGoal = new List<GameTile>();

    private void Start()
    {

        gameManager = GetComponent<GameManager>();
    }

    private void Update()
    {
        if (currentWave == maxWave && Enemy.allEnemies.Count == 0 && !reset)
        {
            nextWave = false;
            reset = true;
            StartCoroutine(Fisnish());
        }

        if (player == null)
        {
            var hp = GameObject.Find("Player");
            player = hp.GetComponent<Player>();
        }

    }
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
        if (nextWave)
        {
            player.currentExp += Mathf.RoundToInt(expGain * Mathf.Pow(currentWave, multiDificultyWave));
            timeBetweenEnemy -= reduceTimeWave;
            if (timeBetweenEnemy <= 0)
            { timeBetweenEnemy = 0.1f; }
            return Mathf.RoundToInt(enemyWave * Mathf.Pow(currentWave, multiDificultyWave));
        }
        return 0;
    }

    IEnumerator SpawnEnemyCoroutine(GameTile spawnTile)
    {
        while (player.totalHp != 0 && nextWave)
        {
            yield return new WaitUntil(() => Enemy.allEnemies.Count == 0);

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
        int i = UnityEngine.Random.Range(0, 22);

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
        else if (i > 16 && i <= 20)    //heal camarade robot
        {
            return 3;
        }
        else if (i > 20)    //Giant camarade robot
        {
            return 4;
        }
        else
        {
            return 0;
        }
    }

    IEnumerator Fisnish()
    {
        yield return new WaitForSeconds(timeBetweenMap);
        gameManager.NextMap();
        currentWave = 1;
    }
}
