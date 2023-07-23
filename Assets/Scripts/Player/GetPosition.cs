using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPosition : MonoBehaviour
{

   [SerializeField] private Transform position;

    void Update()
    {
        transform.position = position.position;
    }
}
