using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Vector3 direction = Vector3.right;

    private void Awake()
    {
        StartCoroutine(DirectionCoroutine());
    }

    IEnumerator DirectionCoroutine()
    {
        yield return new WaitForSeconds(8f);
        direction = Vector3.down;
        yield return new WaitForSeconds(3f);
        direction = Vector3.left;
        yield return new WaitForSeconds(6f);
        direction = Vector3.up;
        yield return new WaitForSeconds(30f);
        Destroy(gameObject);
    }

    void Update()
    {
        transform.position += direction * Time.deltaTime * 2f;
    }
}
