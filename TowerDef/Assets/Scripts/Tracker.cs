using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField] List<Transform> destinations = new List<Transform>();
    int destinationIndex;
    private void Update()
    {
        if(destinationIndex < destinations.Count)
        {
            Vector3 destpos = destinations[destinationIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, destpos, 2*Time.deltaTime);

            if(Vector3.Distance(transform.position, destpos) < 0.1f)
            {
                destinationIndex++;
            }
        }
    }


}
