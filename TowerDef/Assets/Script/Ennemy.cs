using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Queue<Transform> path;
    Spawner spawner;
    public List<Transform> target;
    int index = 0;

    private void Start()
    {
        spawner = GetComponentInParent<Spawner>();
        target = spawner.node;
    }

    private void FixedUpdate()
    {
        if(transform.position == target[index].position)
        {
            index++;

            if(index == target.Count)
            {
                Destroy(this.gameObject);
            }
        }

        transform.LookAt(target[index].position);
        //transform.position = Vector2.up * speed;



    }



}
