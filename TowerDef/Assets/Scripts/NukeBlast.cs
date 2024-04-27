using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.VFX;

public class NukeBlast : MonoBehaviour
{
    [SerializeField] float damege = 100f;
    [SerializeField] float duration = 2f;
    [SerializeField] internal int cost = 15000;
    VisualEffect nukeBlast;

    private void Start()
    {
        nukeBlast = GetComponent<VisualEffect>();
        transform.rotation = Quaternion.Euler(0, 0, 147);
        transform.position = new Vector3(17, 10, 0);
    }
    private void Update()
    {
        if (duration > 0)
        {
            if (duration < 1)
            {
                foreach (Enemy enemy in Enemy.allEnemies)
                {
                    enemy.OnTakeDamage(damege);
                }
            }
        }
        else
        {
            nukeBlast.Stop();
            StartCoroutine(WaitAndContinue());
        }

        duration -= Time.deltaTime;
    }

    IEnumerator WaitAndContinue()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }


}
