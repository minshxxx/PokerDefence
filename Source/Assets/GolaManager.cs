using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolaManager : MonoBehaviour
{
    [SerializeField] GameObject m_gameManager;
    [SerializeField] GameObject m_LifeDiscount;

    [SerializeField] AudioClip LifeCountSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "monster")
        {

            Vector3 spwanPoint = gameObject.transform.position + new Vector3(0f, 0.8f, 0.5f);
            GameObject damageText = Instantiate(m_LifeDiscount, spwanPoint, Quaternion.Euler(90f, 0f, 0f));

            GetComponent<AudioSource>().PlayOneShot(LifeCountSFX);
            m_gameManager.GetComponent<gameManager>().LifeDiscount();
            m_gameManager.GetComponent<gameManager>().CheckEndDefence();
            Destroy(other.gameObject);
        }
    }
}
