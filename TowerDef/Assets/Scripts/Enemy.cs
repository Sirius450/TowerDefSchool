using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int damege = 0;

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

        //StartCoroutine(DirectionCoroutine());
    }

    private void Update()
    {
        if (path.Count > 0)
        {
            Vector3 desPos = path.Peek().transform.position; 

            transform.position = Vector3.MoveTowards(transform.position, desPos, 2* Time.deltaTime);


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

    //IEnumerator DirectionCoroutine()
    //{
    //    yield return new WaitForSeconds(8f);
    //    direction = Vector3.down;
    //    yield return new WaitForSeconds(3f);
    //    direction = Vector3.left;
    //    yield return new WaitForSeconds(6f);
    //    direction = Vector3.up;
    //    yield return new WaitForSeconds(30f);
    //    Destroy(gameObject);
    //}
}
