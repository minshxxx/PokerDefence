using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDiscount : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("Disabling");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Disabling()
    {
        for (double i = 1f; i > 0f; i -= 0.01)
        {
            gameObject.transform.position += new Vector3(0f, 0f, 0.005f);
            yield return null;
        }

        Destroy(gameObject);
    }
}
