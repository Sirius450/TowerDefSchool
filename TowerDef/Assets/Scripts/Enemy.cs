using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private int damage = 0;
    [SerializeField] private float PV = 3;
    [SerializeField] internal float currentPV;
    [SerializeField] private GameObject HpBar;
    [SerializeField] private bool showDirection = false;

    [SerializeField] private bool heal;
    [SerializeField] private int range;
    [SerializeField] private int healAmount;
    private bool shoot;
    private LineRenderer lineRenderer;

    private Vector3 maxSizeHpBar;
    private Vector3 curentSize;
    public static HashSet<Enemy> allEnemies = new HashSet<Enemy>();

    private Stack<GameTile> path = new Stack<GameTile>();
    [SerializeField] List<GameTile> pathList = new List<GameTile>();

    internal void SetPath(List<GameTile> pathToGoal)
    {
        path.Clear();

        foreach (GameTile tile in pathToGoal)
        {
            path.Push(tile);
        }
    }

    internal void NewPath(List<GameTile> tempPathToGoal, List<GameTile> pathToGoal)
    {

        //si la liste n'est pas pareil
        if (!tempPathToGoal.SequenceEqual(pathToGoal))
        {
            //GameTile currentTile = path.Peek();

            //float dist = float.MaxValue; 
            //int indexTile = 0;

            //for (int i = 0; i < tempPathToGoal.Count; i++)
            //{
            //    float currentDist = Vector3.Distance(transform.position, tempPathToGoal[i].transform.position);
            //    if (currentDist <= dist)
            //    {
            //        dist = currentDist;
            //        indexTile = i;
            //        //Debug.Log($"index de la tuile a plus ptoche = {indexTile}");
            //    }
            //}

            //path.Clear();
            //foreach (GameTile tile in tempPathToGoal.Skip(indexTile))
            //{
            //    path.Push(tile);
            //}



            Vector3 currentPosition = transform.position;

            // Trouver l'indice de la tuile la plus proche.
            int indexNearestTile = FindIndexOfNearestTile(tempPathToGoal, currentPosition);

            // Mettre à jour le chemin avec les nouvelles tuiles depuis la tuile la plus proche.
            path.Clear();
            for (int i = indexNearestTile; i /*< tempPathToGoal.Count*/ !=-1; i--)
            {
                pathList.Add(tempPathToGoal[i]);
                //path.Push(tempPathToGoal[i]);
            }
            pathList.Reverse();

            foreach (GameTile tile in pathList)
            {
                path.Push(tile);
            }


            //for (int i = pathList.Count - 1; i !=0; i--)
            //{

            //    path.Push(pathList[i]);
            //}
            

        }
        else
        { Debug.Log("meme chemin"); }


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

    private void Awake()
    {
        allEnemies.Add(this);
        currentPV = PV;
        maxSizeHpBar = HpBar.transform.localScale;
        curentSize = HpBar.transform.localScale;

        if (heal)
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = false;
        }

    }

    private void Update()
    {

        if (path.Count > 0)
        {
            if (showDirection)
            {
                Debug.DrawLine(transform.position, path.Peek().transform.position, Color.blue);
            }

            Vector3 desPos = path.Peek().transform.position;

            transform.position = Vector3.MoveTowards(transform.position, desPos, speed * Time.deltaTime);


            if (Vector3.Distance(transform.position, desPos) < 0.1f)
            {
                path.Pop();
            }
        }
        else
        {
            GameObject player = GameObject.Find("Player");
            Player healt = player.GetComponent<Player>();
            healt.OnTakeDamege(damage);
            allEnemies.Remove(this);
            Destroy(gameObject);
        }

        if (heal)
        {
            Heal();
        }

    }

    private void Heal()
    {
        Enemy target = null;
        float prevPv = 999;
        foreach (var enemy in allEnemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < range && enemy.gameObject != this.gameObject)
            {
                if (enemy.currentPV < prevPv)
                {
                    target = enemy;
                    prevPv = target.currentPV;
                }
            }
        }

        if (target != null)
        {
            lineRenderer.SetPosition(1, target.transform.position);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.enabled = true;
            if (!shoot)
            {
                StartCoroutine(OnHeal(target));
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    IEnumerator OnHeal(Enemy target)
    {
        shoot = true;
        target.OntakeHeal(healAmount);
        yield return new WaitForSeconds(0.5f);
        shoot = false;
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

    public void OntakeHeal(int heal)
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
