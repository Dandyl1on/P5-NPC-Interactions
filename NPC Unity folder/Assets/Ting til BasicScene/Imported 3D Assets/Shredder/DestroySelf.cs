using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] GameObject paperTrash;

    void Awake()
    {
        StartCoroutine(DestroyAndSpawn(paperTrash));
    }

    IEnumerator DestroyAndSpawn(GameObject gb)
    {

        yield return new WaitForSeconds(2);

        Instantiate(gb);
        Destroy(gameObject);
    }
}
