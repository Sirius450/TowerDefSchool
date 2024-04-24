using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourel : MonoBehaviour
{
    [SerializeField] float range = 2f;
    [SerializeField] float minRange = 0.01f;
    [SerializeField] float damege = 2f;
    [SerializeField] float reloadTime = 0.2f;

    internal int x;
    internal int y;
    bool shoot = false;

    LineRenderer lineRenderer;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
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
            lineRenderer.SetPosition(0, this.transform.position);
            lineRenderer.SetPosition(1, target.transform.position);
            lineRenderer.enabled = true;
            if (!shoot)
            {
                StartCoroutine(OnShoot(target));
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
}
