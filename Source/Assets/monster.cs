using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class monster : MonoBehaviour
{
    public GameObject goal;
    public GameObject m_hp;
    [SerializeField] GameObject m_damage;

    private GameObject m_gameManager;
    private float hp = 100f;
    private NavMeshAgent agent = null;

    private bool isDie = false;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.transform.position;
        Debug.Log(goal.transform.position);
        m_gameManager = GameObject.Find("GameManager");
    }

    public void SpwanInit(float value)
    {
        hp = value;
        syncronizeHP();
    }
    public void TurretHit(float hitValue)
    {
        hp -= hitValue;
        if(hp <= 0)
        {
            if (isDie == false)
            {
                isDie = true;
                m_gameManager.GetComponent<gameManager>().CheckEndDefence();
                Destroy(gameObject);
            }
        }
        syncronizeHP();
    }

    public void syncronizeHP()
    {
        m_hp.GetComponent<TextMeshPro>().text = hp.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            bool isCritical = other.GetComponent<BulletManager>().getIsCritical();
            float damage = other.GetComponent<BulletManager>().getDamage();
            Destroy(other.gameObject);

            Vector3 spwanPoint = gameObject.transform.position + new Vector3(0f, 0f, 1f);
            spwanPoint.y = 0.5f;
            GameObject damageText = Instantiate(m_damage, spwanPoint, Quaternion.Euler(90f, 0f, 0f));

            damageText.GetComponent<TextMeshPro>().text = damage.ToString();
            if (isCritical)
            {
                damageText.GetComponent<TextMeshPro>().color = Color.red;
            }

            TurretHit(damage);
        }
    }
}
