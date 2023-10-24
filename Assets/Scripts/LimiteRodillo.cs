using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimiteRodillo : MonoBehaviour
{
    [SerializeField] Transform spawnIconos;
    float alturaSpawn;

    void Start()
    {
        alturaSpawn = spawnIconos.position.y;
    }

    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Iconos"))
        {
            collision.transform.position = new Vector3(collision.transform.position.x, alturaSpawn, collision.transform.position.z);
            Debug.Log("Colisiono");
        }
    }
}
