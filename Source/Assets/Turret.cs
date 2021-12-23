using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] Transform m_tfGunBody = null;
    [SerializeField] float m_range = 0f;
    [SerializeField] LayerMask m_layerMask = 0;
    [SerializeField] float m_spinSpeed = 0f;
    [SerializeField] Transform m_tfHead = null;
    [SerializeField] float m_fireRate = 0f;
    [SerializeField] GameObject m_bullet = null;
    [SerializeField] GameObject m_geneology = null;
    [SerializeField] GameObject m_geneologyNumber = null;

    float m_currentFireRate;

    [SerializeField] AudioClip shutSfx;
    [SerializeField] float ShutSpeed = 0.5f;

    private AudioSource source = null;

    [SerializeField] float AttackSpeed = 0f;
    [SerializeField] float AttackDamage = 0f;
    [SerializeField] float criticalRatio = 50f;
    [SerializeField] float criticalDamage = 2f;

    [SerializeField] int UpgradeAttackCnt = 0;
    [SerializeField] int UpgradeSpeedCnt = 0;
    [SerializeField] int UpgradeCriticalRatio = 0;
    [SerializeField] int UpgradeCriticalDamage = 0;

    GameObject m_tfTarget = null;

    public void setUpgradeAttackCnt(int value)
    {
        UpgradeAttackCnt = value;
    }
    public void setUpgradeSpeedCnt(int value)
    {
        UpgradeSpeedCnt = value;
    }
    public void setUpgradeCriticalRatio(int value)
    {
        UpgradeCriticalRatio = value;
    }
    public void setUpgradeCriticalDamage(int value)
    {
        UpgradeCriticalDamage = value;
    }


    void SearchEnemy()
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, m_range, m_layerMask);
        Transform t_shortestTarget = null;

        if(t_cols.Length > 0)
        {
            float t_shortestDistance = Mathf.Infinity;
            foreach(Collider t_colTarget in t_cols)
            {
                float t_distance = Vector3.SqrMagnitude(transform.position - t_colTarget.transform.position);
                Debug.Log(transform.position - t_colTarget.transform.position);
                if(t_shortestDistance > t_distance)
                {
                    t_shortestDistance = t_distance;
                    t_shortestTarget = t_colTarget.transform;
                }
            }
        }

        if(t_shortestTarget != null)
            m_tfTarget = t_shortestTarget.transform.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_currentFireRate = m_fireRate;
        InvokeRepeating("SearchEnemy", 0f, ShutSpeed);
        SoundInit();
    }

    void SoundInit()
    {
        source = GetComponent<AudioSource>();
    }

    public void setInfo(string geneology, string number)
    {
        m_geneology.GetComponent<TextMeshPro>().text = geneology.ToString();
        if (number.Equals("11")) number = "J";
        if (number.Equals("12")) number = "Q";
        if (number.Equals("13")) number = "K";
        if (number.Equals("14")) number = "A";
        m_geneologyNumber.GetComponent<TextMeshPro>().text = number.ToString();
        gameObject.name = "Turret";

        AttackSpeed = 1f;

        if (geneology.Equals("탑"))
        {
            AttackDamage = 50f;
            GameObject outlineColor = gameObject.transform.GetChild(4).gameObject;
            outlineColor.GetComponent<MeshRenderer>().material.color = Color.white;
        }
        else if (geneology.Equals("원페어"))
        {
            AttackDamage = 100f;
            GameObject outlineColor = gameObject.transform.GetChild(4).gameObject;
            outlineColor.GetComponent<MeshRenderer>().material.color = new Color(0.67f, 0.79f, 1f);
        }
        else if (geneology.Equals("투페어"))
        {
            AttackDamage = 200f;
            GameObject outlineColor = gameObject.transform.GetChild(4).gameObject;
            outlineColor.GetComponent<MeshRenderer>().material.color = new Color(0.34f, 0.58f, 1f);
        }
        else if (geneology.Equals("트리플"))
        {
            AttackDamage = 400f;
            GameObject outlineColor = gameObject.transform.GetChild(4).gameObject;
            outlineColor.GetComponent<MeshRenderer>().material.color = new Color(0f, 0.27f, 0.77f);
        }
        else if (geneology.Equals("스트레이트"))
        {
            AttackDamage = 600f;
            GameObject outlineColor = gameObject.transform.GetChild(4).gameObject;
            outlineColor.GetComponent<MeshRenderer>().material.color = new Color(0.64f, 1f, 0.59f);
        }
        else if (geneology.Equals("마운틴"))
        {
            AttackDamage = 800f;
            GameObject outlineColor = gameObject.transform.GetChild(4).gameObject;
            outlineColor.GetComponent<MeshRenderer>().material.color = new Color(0.09f, 1f, 0f);
        }
        else if (geneology.Equals("빽스트레이트"))
        {
            GameObject outlineColor = gameObject.transform.GetChild(4).gameObject;
            outlineColor.GetComponent<MeshRenderer>().material.color = new Color(0.04f, 0.41f, 0f);
            AttackDamage = 1000f;
        }
        else if (geneology.Equals("플러시"))
        {
            GameObject outlineColor = gameObject.transform.GetChild(4).gameObject;
            outlineColor.GetComponent<MeshRenderer>().material.color = new Color(1f, 0.08f, 0f);
            AttackDamage = 1200f;
        }
        else if (geneology.Equals("풀하우스"))
        {
            GameObject outlineColor = gameObject.transform.GetChild(4).gameObject;
            outlineColor.GetComponent<MeshRenderer>().material.color = new Color(1f, 0.08f, 0f);
            AttackDamage = 1500f;
        }
        else if (geneology.Equals("포카드"))
        {
            GameObject outlineColor = gameObject.transform.GetChild(4).gameObject;
            outlineColor.GetComponent<MeshRenderer>().material.color = new Color(1f, 0f, 0.80f);
            AttackDamage = 2000f;
        }
        else if (geneology.Equals("스트레이트플러시"))
        {
            GameObject outlineColor = gameObject.transform.GetChild(4).gameObject;
            outlineColor.GetComponent<MeshRenderer>().material.color = Color.red;
            AttackDamage = 3000f;
        }
        else if (geneology.Equals("로얄스트레이트플러시"))
        {
            GameObject outlineColor = gameObject.transform.GetChild(4).gameObject;
            outlineColor.GetComponent<MeshRenderer>().material.color = Color.red;
            AttackDamage = 5000f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_tfTarget == null)
        {
            m_tfGunBody.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
        }
        else
        {
            Quaternion t_lookRotation = Quaternion.LookRotation(m_tfTarget.transform.position - m_tfHead.position);
            Vector3 t_euler = Quaternion.RotateTowards(m_tfGunBody.rotation,
                                                        t_lookRotation, 
                                                        m_spinSpeed * Time.deltaTime).eulerAngles;
            m_tfGunBody.rotation = Quaternion.Euler(0, t_euler.y, 0);

            Quaternion t_fireRotation = Quaternion.Euler(0, t_lookRotation.eulerAngles.y, 0);
            if(Quaternion.Angle(m_tfGunBody.rotation, t_fireRotation) < 51)
            {
                m_currentFireRate -= (Time.deltaTime * (AttackSpeed + (float)(0.004 * UpgradeSpeedCnt)));
                if(m_currentFireRate <= 0)
                {
                    m_currentFireRate = m_fireRate;
                    GetComponent<AudioSource>().PlayOneShot(shutSfx);
                    // m_tfTarget.GetComponent<monster>().TurretHit(20);

                    // 총알 생성
                    Vector3 position = gameObject.transform.position;
                    GameObject bullet = Instantiate(m_bullet, new Vector3(position.x, position.y + 0.3f, position.z), Quaternion.identity);
                    bullet.GetComponent<BulletManager>().setDamage((int)(AttackDamage + UpgradeAttackCnt * (float)(AttackDamage * 0.05)), 50 + (UpgradeCriticalRatio * 10), (2 + UpgradeCriticalDamage * 0.1f));
                    bullet.GetComponent<BulletManager>().Shuting(m_tfTarget);
                }
            }
        }
    }
}
