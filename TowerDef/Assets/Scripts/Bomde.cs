using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomde : MonoBehaviour
{
    internal float damage =0;
    internal float range =0;

    bool explode = false;
    ParticleSystem explosion;
    List<Enemy> enemylist = new List<Enemy>();

    internal void SetValue(float newDamege, float newRange)
    {
        damage = newDamege;
        range = newRange;
    }

    private void Start()
    {
        explosion = GetComponent<ParticleSystem>();
        foreach (Enemy enemy in Enemy.allEnemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < range)
            {
                enemylist.Add(enemy);
            }
        }
        explosion.Play();
        explode = true;
    }

    private void Update()
    {
        foreach (Enemy enemy in enemylist)
        {
            enemy.OnTakeDamage(damage);
            Destroy(gameObject);

        }
    }
}
