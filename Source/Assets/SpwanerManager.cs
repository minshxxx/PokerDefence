using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpwanerManager : MonoBehaviour
{
    [SerializeField] GameObject m_monster;

    private Vector3 spwanPoint;
    private int wave;
    private float hp;
    private int SpwanCount = 10;
    // Start is called before the first frame update
    void Start()
    {
        spwanPoint = new Vector3(-8.388f, 0.53f, -6.42f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpwanMonster(int wave)
    {
        hp = (int)(wave * wave * 100 * ((wave / 5) * 0.5f + 1));
        StartCoroutine("MyCoroutine");
    }

    IEnumerator MyCoroutine()
    {
        for (int i = 0; i < SpwanCount; i++)
        {
            GameObject monster = Instantiate(m_monster, spwanPoint, Quaternion.Euler(0f, 180f, 0f));
            monster.GetComponent<monster>().SpwanInit(hp);
            monster.name = i + "monster";
            yield return new WaitForSeconds(1f);
        }
    }

}
