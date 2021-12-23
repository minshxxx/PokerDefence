using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBlink : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BlinkLight());
    }

    IEnumerator BlinkLight()
    {
        while (true)
        {
            for (float i = 4f; i >= 1f; i -= 0.1f)
            {
                gameObject.GetComponent<Light>().intensity = i;
                yield return new WaitForSeconds(0.05f);
            }
            for (float i = 1f; i < 4f; i += 0.1f)
            {
                gameObject.GetComponent<Light>().intensity = i;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
