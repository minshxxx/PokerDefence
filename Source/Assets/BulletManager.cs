using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private GameObject target = null;
    private float damage = 100;
    private bool isCritical;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    

    public void Shuting(GameObject monster)
    {
        target = monster;
        StartCoroutine("MyCoroutine");
    }

    IEnumerator MyCoroutine()
    {
        while (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * 40f);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }

    public float getDamage()
    {
        return damage;
    }

    public bool getIsCritical()
    {
        return isCritical;
    }

    public void setDamage(float value, float criticalRatio, float criticalDamage)
    {
        int criticalValue = Random.Range(0, 1000);
        Debug.Log(criticalValue + "<" + criticalRatio);
        if(criticalValue < criticalRatio)
        {
            isCritical = true;
            damage = (int)(value * criticalDamage);
        }
        else
        {
            isCritical = false;
            damage = value;
        }
    }
}
