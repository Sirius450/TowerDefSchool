using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject UI;

    [SerializeField] internal GameObject gameTilePrefab;
    [SerializeField] List<GameObject> enemyPrefab = new List<GameObject>();
    [SerializeField] Player player;
    [SerializeField] GameObject kamikaze;
    GameTile[,] gameTiles;
    internal GameTile spawnTile;
    internal GameTile endTile;
    const int ColCount = 16;
    const int RowCount = 10;
    [SerializeField] private string mapName = "Test";

    public GameTile TargetTile { get; internal set; }
    List<GameTile> pathToGoal = new List<GameTile>();

    public static GameManager Singleton;
    LevelLayout layout;

    private void Awake()
    {
        layout = GetComponent<LevelLayout>();

        //Creation of singleton
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }


        gameTiles = new GameTile[ColCount, RowCount];



        layout.ChargerCarte(mapName);

        //do this pussy
        //for pour tout les tuille et check le caracthere pour le remplcer avec le bon
        //faire une fonction


        for (int x = 0; x < ColCount; x++)
        {
            for (int y = 0; y < RowCount; y++)
            {
                var spawnPosition = new Vector3(x, y, 0);
                var tile = Instantiate(gameTilePrefab, spawnPosition, Quaternion.identity);
                gameTiles[x, y] = tile.GetComponent<GameTile>();
                gameTiles[x, y].GM = this;
                gameTiles[x, y].X = x;
                gameTiles[x, y].Y = y;
                if ((x + y) % 2 == 0)
                {
                    gameTiles[x, y].TurnGrey();
                }
            }
        }

        //spawnTile = gameTiles[1, 7];
        spawnTile.SetEnemySpawn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && TargetTile != null)
        {
            endTile = TargetTile;
            foreach (var t in gameTiles)
            {
                t.SetPath(false);
            }

            var path = PathFinfing(spawnTile, endTile);
            var tile = endTile;

            while (tile != null)
            {
                pathToGoal.Add(tile);
                tile.SetPath(true);
                tile = path[tile];
            }
            StartCoroutine(SpawnEnemyCoroutine());
        }

        //pause the game
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    OnPauseMenu();
        //}
        //if (SceneManager.GetActiveScene().buildIndex == 0)
        //{
        //    pauseMenu.SetActive(false);
        //}
        //if (SceneManager.GetActiveScene().buildIndex != 0)
        //{
        //    UI.SetActive(true);
        //}

        if (Input.GetKeyDown(KeyCode.K))
        {
            var kamikazeObject = Instantiate(kamikaze, endTile.transform.position, Quaternion.identity);
            kamikazeObject.GetComponent<Kamikaze>().SetPath(pathToGoal);
        }
    }

    public void OnPauseMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (pauseMenu.active == false)
            {
                pauseMenu.SetActive(true);
            }
            else
            {
                pauseMenu.SetActive(false);
            }
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private Dictionary<GameTile, GameTile> PathFinfing(GameTile sourceTile, GameTile targetTile)
    {
        var dist = new Dictionary<GameTile, int>();
        var prev = new Dictionary<GameTile, GameTile>();

        var Q = new List<GameTile>();

        foreach (var v in gameTiles)
        {
            dist.Add(v, 999);
            prev.Add(v, null);
            Q.Add(v);
        }

        dist[sourceTile] = 0;

        while (Q.Count > 0)
        {
            GameTile u = null;
            int minDistance = int.MaxValue;

            foreach (var v in Q)
            {
                if (dist[v] < minDistance)
                {
                    minDistance = dist[v];
                    u = v;
                }

            }

            Q.Remove(u);

            foreach (var v in FindNeighbor(u))
            {
                if (!Q.Contains(v) || v.IsBloced)
                {
                    continue;
                }

                int alt = dist[u] + 1;

                if (alt < dist[v])
                {
                    dist[v] = alt;

                    prev[v] = u;
                }
            }

        }

        return prev;
    }

    private List<GameTile> FindNeighbor(GameTile u)
    {
        var result = new List<GameTile>();

        if (u.X - 1 >= 0)
        { result.Add(gameTiles[u.X - 1, u.Y]); }
        if (u.X + 1 < ColCount)
        { result.Add(gameTiles[u.X + 1, u.Y]); }
        if (u.Y - 1 >= 0)
        { result.Add(gameTiles[u.X, u.Y - 1]); }
        if (u.Y + 1 < RowCount)
        { result.Add(gameTiles[u.X, u.Y + 1]); }

        return result;
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (player.totalHp != 0)
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.6f);
                var enemy = Instantiate(enemyPrefab[RNGRobot()], spawnTile.transform.position, Quaternion.identity);
                enemy.GetComponent<Enemy>().SetPath(pathToGoal);
            }
            yield return new WaitForSeconds(2f);
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

    internal void GamePath()
    {
        List<GameTile> tempPathToGoal = new List<GameTile>();

        foreach (var t in gameTiles)
        {
            t.SetPath(false);
        }

        var path = PathFinfing(spawnTile, endTile);
        var tile = endTile;

        while (tile != null)
        {
            tempPathToGoal.Add(tile);
            tile.SetPath(true);
            tile = path[tile];
        }

        foreach (Enemy enemy in Enemy.allEnemies)
        {
            enemy.NewPath(tempPathToGoal, pathToGoal);
        }
        foreach (Kamikaze kamikaze in Kamikaze.allKamikaze)
        {
            kamikaze.NewPath(tempPathToGoal, pathToGoal);
        }

        pathToGoal = tempPathToGoal;

    }
}
