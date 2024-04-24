using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomde : MonoBehaviour
{
    internal float damage =0;
    internal float range =0;


    internal void SetValue(float newDamege, float newRange)
    {
        damage = newDamege;
        range = newRange;
    }

    private void Start()
    {
        foreach (Enemy enemy in Enemy.allEnemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < range)
            {
                enemy.OnTakeDamage(damage);
            }
        }
    }
}
