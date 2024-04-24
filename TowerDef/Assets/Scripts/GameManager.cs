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
    [SerializeField] Player player;
    [SerializeField] GameObject kamikaze;
    GameTile[,] gameTiles;
    internal GameTile spawnTile;
    internal GameTile endTile;
    const int ColCount = 16;
    const int RowCount = 10;
    [SerializeField] private string mapName = "Test";
    [SerializeField] private int mapIndex = 0;

    public GameTile TargetTile { get; internal set; }
    List<GameTile> pathToGoal = new List<GameTile>();

    public static GameManager Singleton;
    LevelLayout layout;
    SpawningEnemy spwaning;

    private void Awake()
    {
        layout = GetComponent<LevelLayout>();
        spwaning = GetComponent<SpawningEnemy>();

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
    }

    private void Start()
    {
        gameTiles = new GameTile[RowCount, ColCount];
        layout.ChargerCarte(mapIndex, gameTilePrefab, this, ref spawnTile, ref endTile, ref gameTiles, ref mapName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && TargetTile != null)
        {
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
            spwaning.Spawning(spawnTile, pathToGoal);
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

        //if (u.X - 1 >= 0)
        //{ result.Add(gameTiles[u.X - 1, u.Y]); }
        //if (u.X + 1 < ColCount)
        //{ result.Add(gameTiles[u.X + 1, u.Y]); }
        //if (u.Y - 1 >= 0)
        //{ result.Add(gameTiles[u.X, u.Y - 1]); }
        //if (u.Y + 1 < RowCount)
        //{ result.Add(gameTiles[u.X, u.Y + 1]); }

        if (u.X - 1 >= 0)
        { result.Add(gameTiles[u.Y, u.X - 1]); }
        if (u.X + 1 < ColCount)
        { result.Add(gameTiles[u.Y, u.X + 1]); }
        if (u.Y - 1 >= 0)
        { result.Add(gameTiles[u.Y - 1, u.X]); }
        if (u.Y + 1 < RowCount)
        { result.Add(gameTiles[u.Y + 1, u.X]); }

        return result;
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
        spwaning.NewPath(pathToGoal);
    }
}
