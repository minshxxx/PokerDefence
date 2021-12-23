using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class damage : MonoBehaviour
{
    // Start is called before the first frame update
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
        for (double i = 1f; i > 0f; i -= 0.02)
        {
            gameObject.transform.position += new Vector3(0f, 0f, 0.007f);
            Color c = gameObject.GetComponent<TextMeshPro>().color;
            c.a = (float)i + 0.5f;
            gameObject.GetComponent<TextMeshPro>().color = c;

            yield return null;
        }

        Destroy(gameObject);
    }
}
