using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : MonoBehaviour
{
    [SerializeField]
    private float fallSpeed = 1;

    private void Update()
    {
        if(transform.position.y > 0.4f)
        {
            transform.position += Time.deltaTime * Vector3.down * fallSpeed;
        }
        else
        {
            transform.position = new Vector3 (transform.position.x, 0.28f, transform.position.z);
        }
    }
}
