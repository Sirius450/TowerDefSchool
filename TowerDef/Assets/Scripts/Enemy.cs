using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

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
