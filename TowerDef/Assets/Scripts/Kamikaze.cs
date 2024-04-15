using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Kamikaze : MonoBehaviour
{
    [SerializeField] CircleCollider2D explosionRange;
    [SerializeField] internal float speed;
    [SerializeField] internal int damage;
    [SerializeField] internal float range;
    [SerializeField] GameObject explosion;
    private bool isExplode = false;
    public List<Enemy> enemyList = new List<Enemy>();

    private Stack<GameTile> path = new Stack<GameTile>();
    private void Awake()
    {
        explosionRange.radius = range;
        
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
            if (Vector3.Distance(transform.position, enemy.transform.position) < range)
            {
                enemyList.Add(enemy);
            }
            else
            {
                enemyList.Remove(enemy);
            }    

        }

        foreach (Enemy enemy in enemyList)
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
            Destroy(gameObject);
        }
    }

    private void Explose()
    {
        foreach (Enemy enemy in enemyList)
        {
            enemy.OnTakeDamage(damage);
        }
    }
}
