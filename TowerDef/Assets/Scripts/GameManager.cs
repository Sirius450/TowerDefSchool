using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameTilePrefab;
    [SerializeField] GameObject enemyPrefab;
    GameTile[,] gameTiles;
    private GameTile spawnTile;
    const int ColCount = 20;
    const int RowCount = 10;

    public GameTile TargetTile { get; internal set; }

    private void Awake()
    {
        gameTiles = new GameTile[ColCount, RowCount];

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

        spawnTile = gameTiles[1, 7];
        spawnTile.SetEnemySpawn();
        StartCoroutine(SpawnEnemyCoroutine());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)&& TargetTile != null )
        {
            PathFinfing(spawnTile, TargetTile);
        }
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

        }


    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.6f);
                Instantiate(enemyPrefab, spawnTile.transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
