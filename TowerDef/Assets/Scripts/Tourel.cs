using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourel : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float range = 2f;
    [SerializeField] float minRange = 0.01f;
    [SerializeField] float reloadTime = 0.2f;

    [Header("Physic damege")]
    [SerializeField] bool physic = false;
    [SerializeField] float damege = 2f;
    [SerializeField] bool AOI = false;
    [SerializeField] float explosionRange = 0f;
    [SerializeField] GameObject bomde;

    [Header("IEM")]
    [SerializeField] bool IEM = false;
    [SerializeField] float swoling = 0.5f;
    [SerializeField] float swolingTime = 0.2f;


    internal int x;
    internal int y;
    bool shoot = false;

    LineRenderer lineRenderer;

    public static HashSet<Tourel> allTourel = new HashSet<Tourel>();


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        allTourel.Add(this);
    }

    void Update()
    {
        Enemy target = null;
        float distance = 999f;
        foreach (var enemy in Enemy.allEnemies)
        {
            distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < range && distance > minRange)
            {
                target = enemy;
                break;
            }
        }

        if (target != null)
        {
            if (!IEM)
            {
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, target.transform.position);
                lineRenderer.enabled = true;
            }
            if (!shoot && physic)
            {
                StartCoroutine(OnShoot(target));
            }
            if (!shoot && AOI)
            {
                StartCoroutine(OnBomde(target));
            }
            if (!shoot && IEM)
            {
                StartCoroutine(OnSlowing());
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
    IEnumerator OnShoot(Enemy target)
    {
        shoot = true;
        target.OnTakeDamage(damege);
        yield return new WaitForSeconds(reloadTime);
        shoot = false;
    }

    IEnumerator OnBomde(Enemy target)
    {
        shoot = true;
        var projectile = Instantiate(bomde, target.transform.position, Quaternion.identity);
        var value = projectile.GetComponent<Bomde>();
        value.SetValue(damege, explosionRange);
        yield return new WaitForSeconds(reloadTime);
        shoot = false;
    }

    IEnumerator OnSlowing()
    {
        shoot = true;
        foreach (Enemy enemy in Enemy.allEnemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < range)
            {
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, enemy.transform.position);
                lineRenderer.enabled = true;
                enemy.OnSlowing(swoling, swolingTime);
            }
        }
        yield return new WaitForSeconds(reloadTime);

        lineRenderer.enabled=false;
        shoot = false;
    }

    internal void OnRevome()
    {
        allTourel.Remove(this);
        Destroy(gameObject);
    }

}
