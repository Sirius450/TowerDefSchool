using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Kamikaze : MonoBehaviour
{
    [SerializeField] internal float speed;
    [SerializeField] internal int damage;
    [SerializeField] internal float range;
    [SerializeField] GameObject explosion;
    [SerializeField] internal int cost = 4500;
    private bool isExplode = false;
    private Stack<GameTile> path = new Stack<GameTile>();
    public static HashSet<Kamikaze> allKamikaze = new HashSet<Kamikaze>();
    List<GameTile> pathList = new List<GameTile>();

    private void Awake()
    {
        allKamikaze.Add(this);
    }
    internal void SetPath(List<GameTile> pathToGoal)
    {
        path.Clear();

        for(int i = pathToGoal.Count-1; i != 0; i--)
        {
            path.Push(pathToGoal[i]);
        }
    }

    private void Update()
    {
        if (path.Count > 0)
        {
            Vector3 desPos = path.Peek().transform.position;

            transform.position = Vector3.MoveTowards(transform.position, desPos, speed * Time.deltaTime);


            if (Vector3.Distance(transform.position, desPos) < 0.1f)
            {
                path.Pop();
            }
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Enemy enemy in Enemy.allEnemies)
        {
            if(Vector3.Distance(transform.position, enemy.transform.position) < 0.1)
            {
                isExplode = true;
                Explose();
            }
        }

        if(isExplode)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            allKamikaze.Remove(this);
            Destroy(gameObject);
        }
    }

    private void Explose()
    {
        foreach (Enemy enemy in Enemy.allEnemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < range)
            {
                enemy.OnTakeDamage(damage);
            }
        }
    }

    internal void NewPath(List<GameTile> tempPathToGoal, List<GameTile> pathToGoal)
    {
        //si la liste n'est pas pareil
        if (!tempPathToGoal.SequenceEqual(pathToGoal))
        {
            //pathList.Clear();
            Vector3 currentPosition = transform.position;

            // Trouver l'indice de la tuile la plus proche.
            int indexNearestTile = FindIndexOfNearestTile(tempPathToGoal, currentPosition);

            // Mettre à jour le chemin avec les nouvelles tuiles depuis la tuile la plus proche.
            path.Clear();
            for (int i = pathToGoal.Count - 1; i != indexNearestTile; i--)
            {
                pathList.Add(tempPathToGoal[i]);
            }

            foreach (GameTile tile in pathList)
            {
                path.Push(tile);
            }
        }
    }

    private int FindIndexOfNearestTile(List<GameTile> tiles, Vector3 position)
    {
        float minDistance = float.MaxValue;
        int indexNearest = 0;

        for (int i = 0; i < tiles.Count; i++)
        {
            float distance = Vector3.Distance(position, tiles[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                indexNearest = i;
            }
        }

        return indexNearest;
    }

    internal void OnRemove()
    {
        allKamikaze.Remove(this);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        allKamikaze.Remove(this );
    }
}
