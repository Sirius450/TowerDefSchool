using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int damege = 0;
    [SerializeField] private float PV = 3;
    [SerializeField] internal float currentPV;
    [SerializeField] private GameObject HpBar;

    private Vector3 maxSizeHpBar;
    private Vector3 curentSize;
    public static HashSet<Enemy> allEnemies = new HashSet<Enemy>();

    private Stack<GameTile> path = new Stack<GameTile>();

    internal void SetPath(List<GameTile> pathToGoal)
    {
        path.Clear();

        foreach (GameTile tile in pathToGoal)
        {
            path.Push(tile);
        }
    }

    private void Awake()
    {
        allEnemies.Add(this);
        currentPV = PV;
        maxSizeHpBar = HpBar.transform.localScale;
        curentSize = HpBar.transform.localScale;

    }

    private void Update()
    {
        if (path.Count > 0)
        {
            Vector3 desPos = path.Peek().transform.position;

            transform.position = Vector3.MoveTowards(transform.position, desPos, 2 * Time.deltaTime);


            if (Vector3.Distance(transform.position, desPos) < 0.1f)
            {
                path.Pop();
            }
        }
        else
        {
            GameObject player = GameObject.Find("Player");
            Player healt = player.GetComponent<Player>();
            healt.OnTakeDamege(damege);
            allEnemies.Remove(this);
            Destroy(gameObject);
        }
    }


    public void OnTakeDamage(int damege)
    {
        currentPV -= damege;
        AdjustHpBar();


        if (currentPV <= 0)
        {
            Destroy(gameObject);
            allEnemies.Remove(this);
        }

    }

    public void Onheal(int heal)
    {
        currentPV += heal;
        AdjustHpBar();

        if (currentPV >= PV)
        { currentPV = PV; }
    }

    private void AdjustHpBar()
    {
        curentSize.x = (currentPV / PV) * maxSizeHpBar.x;

        HpBar.transform.localScale = curentSize;
    }

}
