using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public GameObject m_cardPanel;
    public GameObject m_upgradePanel;

    private int wave;
    // status -> Suffle, Defence
    private string status;
    private Card[] curCard = new Card[5];
    private int GenealogyNumber;
    private Genealogy genealogy;
    private string genealogyString;
    private int Life;
    private GameObject selectedTile;
    private int ClearCount;
    private int RemainMonster;
    private int money;

    public int UpgradePower = 0;
    public int UpgradeSpeed = 0;
    public int UpgradeCriticalRatio = 0;
    public int UpgradeCriticalDamage = 0;

    [SerializeField] GameObject spwanerManager;

    [SerializeField] GameObject m_ChoiceMsg;
    [SerializeField] GameObject m_SuffleMsg;
    [SerializeField] GameObject m_DefenceMsg;
    [SerializeField] GameObject m_GameoverMsg;

    [SerializeField] AudioClip SuffleBGM;
    [SerializeField] AudioClip DefenceBGM;

    [SerializeField] GameObject m_upgradeAttackPowerText;
    [SerializeField] GameObject m_upgradeAttackSpeedText;
    [SerializeField] GameObject m_upgradeCriticalRatioText;
    [SerializeField] GameObject m_upgradeCriticalDamageText;


    public GameObject m_turret;
    public Text m_text;
    public Text m_life;
    public Text m_wave;
    public Text m_money;

    class Genealogy
    {
        string genealogy;
        int number;

        public Genealogy(string genealogy, int number)
        {
            this.genealogy = genealogy;
            this.number = number;
        }

        public string getGenealogy()
        {
            return genealogy;
        }
        public int getNumber()
        {
            return number;
        }
        public void setGenealogy(string value)
        {
            this.genealogy = value;
        }

        public void setNumber(int value)
        {
            this.number = value;
        }
    }
    class Card
    {
        string shape;
        int number;

        public Card(int shape, int number)
        {
            string ret = null;
            if (shape == 0) ret = "◆";
            else if (shape == 1) ret = "♥";
            else if (shape == 2) ret = "♣";
            else if (shape == 3) ret = "♠";

            this.shape = ret;
            this.number = number;
        }

        public string getShape()
        {
            return shape;
        }
        public int getNumber()
        {
            return number;
        }
        public void setShape(int value)
        {
            string ret = null;
            if (value == 0) ret = "◆";
            else if (value == 1) ret = "♥";
            else if (value == 2) ret = "♣";
            else if (value == 3) ret = "♠";
            shape = ret;
        }

        public void setNumber(int value)
        {
            number = value;
        }

        public bool isEquals(Card card)
        {
            return (getShape() == card.getShape() && getNumber() == card.getNumber());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitGame();
    }
    public void InitGame()
    {
        GetComponent<AudioSource>().PlayOneShot(SuffleBGM);
        wave = 0;
        money = 0;
        setLife(20);

        ButtonInit();
        m_cardPanel.SetActive(false);
        //m_upgradePanel.SetActive(false);

        m_ChoiceMsg.SetActive(true);
        m_DefenceMsg.GetComponent<TextMeshProUGUI>().text = RemainMonster + " 남음";
        m_SuffleMsg.SetActive(false);

        syncUpgradeButton();
    }
    public void InitSelectPhase(GameObject selectTile)
    {
        WaveCount();
        Suffle();
        syncUpgradeButton();
        selectedTile = selectTile;

        m_cardPanel.SetActive(true);
        //m_upgradePanel.SetActive(false);

        ButtonInit();
        m_ChoiceMsg.SetActive(false);
        m_DefenceMsg.GetComponent<TextMeshProUGUI>().text = RemainMonster + " 남음";
        m_SuffleMsg.SetActive(true);
    }
    public void InitDefencePhase()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(DefenceBGM);
        m_ChoiceMsg.SetActive(false);
        m_DefenceMsg.SetActive(true);
        m_SuffleMsg.SetActive(false);
        //m_upgradePanel.SetActive(true);

        RemainMonster = 10;
        m_DefenceMsg.GetComponent<TextMeshProUGUI>().text = RemainMonster + " 남음";

        // 터렛 생성
        GameObject SelectedLight = selectedTile.transform.GetChild(0).gameObject;
        SelectedLight.SetActive(false);
        Vector3 position = selectedTile.transform.position;
        Destroy(selectedTile.GetComponent<MeshRenderer>());


        GameObject new_turret = Instantiate(m_turret, new Vector3(position.x, position.y, position.z + 0.1f), Quaternion.identity);
        new_turret.transform.parent = selectedTile.transform;
        new_turret.GetComponent<Turret>().setInfo(genealogy.getGenealogy(), genealogy.getNumber().ToString());
        
        syncUpgradeTurret();

        // 카드 제거
        m_cardPanel.SetActive(false);

        // 몬스터 생성
        ClearCount = 0;
        spwanerManager.GetComponent<SpwanerManager>().SpwanMonster(wave);

        // 족보 숫자 돈 지급
        
        if (!genealogy.getGenealogy().Equals("탑"))
        {
            MoneyCount(GetGenealogyMoney());
        }
        syncUpgradeButton();
    }
    
    public void InitChoiceTilePhase()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(SuffleBGM);
        m_ChoiceMsg.SetActive(true);
        m_DefenceMsg.GetComponent<TextMeshProUGUI>().text = RemainMonster + " 남음";
        m_SuffleMsg.SetActive(false);
        //m_upgradePanel.SetActive(false);


        GameObject m_clickManager = GameObject.Find("ClickManager");
        m_clickManager.GetComponent<ClickManager>().ResetIsSelected();
    }

    public void CheckEndDefence()
    {
        ClearCount++;
        RemainMonster--;
        m_DefenceMsg.GetComponent<TextMeshProUGUI>().text = RemainMonster + " 남음";
        if (ClearCount == 10 && Life != 0)
        {
            InitChoiceTilePhase();
        }
    }

    void GameOver()
    {
        m_cardPanel.SetActive(false);

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("monster");
        foreach(GameObject monster in monsters){
            Destroy(monster);
        }
    }

    public int GetGenealogyMoney()
    {
        int ret;

        ret = genealogy.getNumber() - 6;

        return ret;
    }

    public void LifeDiscount()
    {
        setLife(Life - 1);
        if(Life == 0)
        {
            m_ChoiceMsg.SetActive(false);
            m_DefenceMsg.GetComponent<TextMeshProUGUI>().text = "남은 몬스터 수: " + RemainMonster;
            m_SuffleMsg.SetActive(false);
            m_GameoverMsg.SetActive(true);

            GameOver();
        }
    }
    public void WaveCount()
    {
        setWave(wave + 1);
    }
    public int getWave()
    {
        return wave;
    }
    public void setWave(int value)
    {
        wave = value;
        m_wave.text = "Wave: " + wave.ToString();
    }
    public int getMoney()
    {
        return money;
    }
    public void MoneyCount(int value)
    {
        setMoeny(money + value);
    }
    public void setMoeny(int value)
    {
        money = value;
        m_money.text = "$ " + money.ToString();
    }
    void ButtonInit()
    {
        GameObject cardPanel = m_cardPanel;
        for (int i = 0; i < 5; i++)
        {
            GameObject card = cardPanel.transform.GetChild(i).gameObject;
            GameObject Button = card.transform.GetChild(1).gameObject;
            Button.GetComponent<Image>().color = Color.blue;
            Button.GetComponent<Button>().interactable = true;
        }
    }

    public int getUpgradePower()
    {
        return UpgradePower;
    }
    public int getUpgradeSpeed()
    {
        return UpgradeSpeed;
    }
    public int getUpgradeCriticalRatio()
    {
        return UpgradeCriticalRatio;
    }
    public int getUpgradeCriticalDamage()
    {
        return UpgradeCriticalDamage;
    }
    public void setUpgradePower(int value)
    {
        UpgradePower = value;
    }
    public void setUpgradeSpeed(int value)
    {
        UpgradeSpeed = value;
    }
    public void setUpgradeCriticalRatio(int value)
    {
        UpgradeCriticalRatio = value;
    }
    public void setUpgradeCriticalDamage(int value)
    {
        UpgradeCriticalDamage = value;
    }

    public void upgradePower()
    {
        if (money > 0)
        {
            setUpgradePower(getUpgradePower() + 1);
            setMoeny(getMoney() - 1);
        }
        syncUpgradeButton();
    }
    public void upgradeSpeed()
    {
        if (money > 0)
        {
            setUpgradeSpeed(getUpgradeSpeed() + 1);
            setMoeny(getMoney() - 1);
        }
        syncUpgradeButton();
    }
    public void upgradeCriticalRatio()
    {
        if (money > 0)
        {
            setUpgradeCriticalRatio(getUpgradeCriticalRatio() + 1);
            setMoeny(getMoney() - 1);
        }
        syncUpgradeButton();
    }
    public void upgradeCiriticalDamage()
    {
        if (money > 0)
        {
            setUpgradeCriticalDamage(getUpgradeCriticalDamage() + 1);
            setMoeny(getMoney() - 1);
        }
        syncUpgradeButton();
    }

    void syncUpgradeButton()
    {
        if (money > 0)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject Button = m_upgradePanel.transform.GetChild(i).gameObject;
                Button.GetComponent<Image>().color = Color.yellow;
                Button.GetComponent<Button>().interactable = true;
            }
        }
        else if (money == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject Button = m_upgradePanel.transform.GetChild(i).gameObject;
                Button.GetComponent<Image>().color = Color.black;
                Button.GetComponent<Button>().interactable = false;
            }
        }
        syncUpgradePanel();
        syncUpgradeTurret();
    }

    void syncUpgradePanel()
    {
        m_upgradeAttackPowerText.GetComponent<TextMeshProUGUI>().text = "+" + (UpgradePower * 5).ToString() + "%";
        m_upgradeAttackSpeedText.GetComponent<TextMeshProUGUI>().text = "+" + (UpgradeSpeed * 4).ToString() + "%";
        m_upgradeCriticalRatioText.GetComponent<TextMeshProUGUI>().text = "+" + (UpgradeCriticalRatio * 1).ToString() + "%";
        m_upgradeCriticalDamageText.GetComponent<TextMeshProUGUI>().text = "+" + (UpgradeCriticalDamage * 10).ToString() + "%";

    }

    void syncUpgradeTurret()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
        foreach (GameObject child in turrets)
        {
            Debug.Log(child.name);
            child.GetComponent<Turret>().setUpgradeAttackCnt(UpgradePower);
            child.GetComponent<Turret>().setUpgradeSpeedCnt(UpgradeSpeed);
            child.GetComponent<Turret>().setUpgradeCriticalRatio(UpgradeCriticalRatio);
            child.GetComponent<Turret>().setUpgradeCriticalDamage(UpgradeCriticalDamage);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public int getLife()
    {
        return Life;
    }
    public void setLife(int value)
    {
        Life = value;
        m_life.text = "LIFE " + Life.ToString();
    }
    string numberToString(int number)
    {
        string retNumber = null;

        if (number == 11) retNumber = "J";
        else if (number == 12) retNumber = "Q";
        else if (number == 13) retNumber = "K";
        else if (number == 14) retNumber = "A";
        else retNumber = number.ToString();

        return retNumber;
    }
    void Suffle()
    {
        status = "suffle";
        GameObject cardPanel = m_cardPanel;
        for (int i = 0; i < 5; i++)
        {
            curCard[i] = new Card(-1, -1);
            GameObject card = cardPanel.transform.GetChild(i).gameObject;
            SetCard(card, i);
        }
        GradeGenealogy();
    }
    public void ChangeCard(int butIdx)
    {
        GameObject cardPanel = m_cardPanel;
        GameObject card = cardPanel.transform.GetChild(butIdx).gameObject;
        SetCard(card, butIdx);

        GameObject Button = card.transform.GetChild(1).gameObject;
        Button.GetComponent<Image>().color = Color.grey;
        Button.GetComponent<Button>().interactable = false;
        GradeGenealogy();
    }
    void SetCard(GameObject card, int curidx)
    {
        GameObject CardInfo = card.transform.GetChild(0).gameObject;
        GameObject Button = card.transform.GetChild(1).gameObject;

        int shape = 0;
        int number = 0;

        while (true)
        {
            bool independent = true;

            shape = Random.Range(0, 4);
            number = Random.Range(7, 15);

            Card newCard = new Card(shape, number);

            for (int i = 0; i < 5; i++)
            {
                if (curCard[i] == null) break;
                if (i == curidx) continue;
                if (curCard[i].isEquals(newCard))
                {
                    independent = false;
                    break;
                }
            }

            if (independent == true) break;
        }

        curCard[curidx].setShape(shape);
        curCard[curidx].setNumber(number);

        CardInfo.GetComponent<Text>().text = setCardInfo(shape, number);
        if (shape == 0 || shape == 1)
        {
            CardInfo.GetComponent<Text>().color = Color.red;
        }
        else
        {
            CardInfo.GetComponent<Text>().color = Color.black;
        }
    }
    string setCardInfo(int shape, int number)
    {
        string retShape = null, retNumber = null;

        if (shape == 0) retShape = "◆";
        else if (shape == 1) retShape = "♥";
        else if (shape == 2) retShape = "♣";
        else if (shape == 3) retShape = "♠";

        if (number == 11) retNumber = "J";
        else if (number == 12) retNumber = "Q";
        else if (number == 13) retNumber = "K";
        else if (number == 14) retNumber = "A";
        else retNumber = number.ToString();

        return (retShape + retNumber);
    }
    void GradeGenealogy()
    {
        genealogy = new Genealogy(null, 0);

        if (isRoyalStrateFlush()) { }
        else if (isStrateFlush()) { }
        else if (isFiveCard()) { }
        else if (isFourCard()) { }
        else if (isFullHouse()) { }
        else if (isFlush()) { }
        else if (isMountain()) { }
        else if (isbackStrate()) { }
        else if (isStrate()) { }
        else if (isTripple()) { }
        else { WhatPair(); }

        genealogyString = numberToString(genealogy.getNumber()) + " " + genealogy.getGenealogy();
        m_text.text = genealogyString;
    }
    void GradeDebug()
    {
        m_text.text = "파이브카드: " + isFiveCard() + "\n" +
                    "포카드: " + isFourCard() + "\n" +
                    "포카드: " + isFullHouse() + "\n" +
                    "플러시: " + isFlush() + "\n" +
                    "마운틴: " + isMountain() + "\n" +
                    "스트레이트: " + isStrate() + "\n" +
                    "트리플: " + isTripple() + "\n" +
                    WhatPair() + "페어";
    }
    bool isRoyalStrateFlush()
    {
        bool ret = isMountain() && isFlush();

        List<Card> CardInfo = new List<Card>();
        for (int i = 0; i < 5; i++)
        {
            CardInfo.Add(curCard[i]);
        }
        CardInfo.Sort(delegate (Card A, Card B)
        {
            if (A.getNumber() < B.getNumber()) return 1;
            else if (A.getNumber() > B.getNumber()) return -1;
            else return 0;
        });

        if (ret)
        {
            genealogy.setGenealogy("로얄스트레이트플러시");
            genealogy.setNumber(14);
        }

        return ret;
    }
    bool isStrateFlush()
    {
        bool ret = (isStrate() || isbackStrate() )&& isFlush();

        List<Card> CardInfo = new List<Card>();
        for (int i = 0; i < 5; i++)
        {
            CardInfo.Add(curCard[i]);
        }
        CardInfo.Sort(delegate (Card A, Card B)
        {
            if (A.getNumber() < B.getNumber()) return 1;
            else if (A.getNumber() > B.getNumber()) return -1;
            else return 0;
        });

        if (ret)
        {
            genealogy.setGenealogy("스트레이트플러시");
            genealogy.setNumber(CardInfo[0].getNumber());
        }

        return ret;
    }
    bool isFiveCard()
    {
        bool ret = true;

        for (int i = 1; i < 5; i++)
        {
            if (curCard[0].getNumber() != curCard[i].getNumber())
            {
                ret = false;
            }
        }

        if (ret)
        {
            genealogy.setGenealogy("파이브카드");
            genealogy.setNumber(curCard[0].getNumber());
        }
        return ret;
    }
    bool isFourCard()
    {
        bool ret = false;

        List<Card> CardInfo = new List<Card>();
        for (int i = 0; i < 5; i++)
        {
            CardInfo.Add(curCard[i]);
        }
        CardInfo.Sort(delegate (Card A, Card B)
        {
            if (A.getNumber() < B.getNumber()) return 1;
            else if (A.getNumber() > B.getNumber()) return -1;
            else return 0;
        });

        for (int i = 0; i < 2; i++)
        {
            ret = (CardInfo[i].getNumber() == CardInfo[i + 1].getNumber()
            && CardInfo[i + 1].getNumber() == CardInfo[i + 2].getNumber()
            && CardInfo[i + 2].getNumber() == CardInfo[i + 3].getNumber());
            if (ret)
            {
                genealogy.setGenealogy("포카드");
                genealogy.setNumber(curCard[i].getNumber());
                break;
            }
        }

        return ret;
    }
    bool isFullHouse()
    {
        bool ret = false;
        bool ret2 = false;

        List<Card> CardInfo = new List<Card>();
        for (int i = 0; i < 5; i++)
        {
            CardInfo.Add(curCard[i]);
        }
        CardInfo.Sort(delegate (Card A, Card B)
        {
            if (A.getNumber() < B.getNumber()) return 1;
            else if (A.getNumber() > B.getNumber()) return -1;
            else return 0;
        });

        ret = (CardInfo[0].getNumber() == CardInfo[1].getNumber()
        && CardInfo[1].getNumber() == CardInfo[2].getNumber()
        && CardInfo[3].getNumber() == CardInfo[4].getNumber());

        if(ret)
        {
            genealogy.setGenealogy("풀하우스");
            genealogy.setNumber(CardInfo[0].getNumber());
        }

        ret2 = (CardInfo[0].getNumber() == CardInfo[1].getNumber()
        && CardInfo[2].getNumber() == CardInfo[3].getNumber()
        && CardInfo[3].getNumber() == CardInfo[4].getNumber());

        if (ret2)
        {
            genealogy.setGenealogy("풀하우스");
            genealogy.setNumber(CardInfo[2].getNumber());
        }
        
        return ret || ret2;
    }
    bool isFlush()
    {
        bool ret = true;

        List<Card> CardInfo = new List<Card>();
        for (int i = 0; i < 5; i++)
        {
            CardInfo.Add(curCard[i]);
        }
        CardInfo.Sort(delegate (Card A, Card B)
        {
            if (A.getNumber() < B.getNumber()) return 1;
            else if (A.getNumber() > B.getNumber()) return -1;
            else return 0;
        });

        for (int i = 0; i < 4; i++)
        {
            if (CardInfo[i].getShape() != CardInfo[i + 1].getShape()) ret = false;
        }

        if (ret)
        {
            genealogy.setGenealogy("플러시");
            genealogy.setNumber(CardInfo[0].getNumber());
        }
        return ret;
    }
    bool isbackStrate()
    {
        List<Card> CardInfo = new List<Card>();
        for (int i = 0; i < 5; i++)
        {
            CardInfo.Add(curCard[i]);
        }
        CardInfo.Sort(delegate (Card A, Card B)
        {
            if (A.getNumber() < B.getNumber()) return 1;
            else if (A.getNumber() > B.getNumber()) return -1;
            else return 0;
        });

        bool ret = (CardInfo[0].getNumber() == 14 && CardInfo[1].getNumber() == 10
            && CardInfo[2].getNumber() == 9 && CardInfo[3].getNumber() == 8
            && CardInfo[4].getNumber() == 7);

        if (ret)
        {
            genealogy.setGenealogy("빽스트레이트");
            genealogy.setNumber(14);
        }
        return ret;
    }
    bool isMountain()
    {
        List<Card> CardInfo = new List<Card>();
        for (int i = 0; i < 5; i++)
        {
            CardInfo.Add(curCard[i]);
        }
        CardInfo.Sort(delegate (Card A, Card B)
        {
            if (A.getNumber() < B.getNumber()) return 1;
            else if (A.getNumber() > B.getNumber()) return -1;
            else return 0;
        });

        bool ret = CardInfo[0].getNumber() == 14 && isStrate();
        if (ret)
        {
            genealogy.setGenealogy("마운틴");
            genealogy.setNumber(14);
        }

        return ret;
    }
    bool isStrate()
    {
        List<Card> CardInfo = new List<Card>();
        for (int i = 0; i < 5; i++)
        {
            CardInfo.Add(curCard[i]);
        }
        CardInfo.Sort(delegate (Card A, Card B)
        {
            if (A.getNumber() < B.getNumber()) return 1;
            else if (A.getNumber() > B.getNumber()) return -1;
            else return 0;
        });

        int strateCount = 0;
        for (int i = 0; i < 4; i++)
        {
            if (CardInfo[i].getNumber() == CardInfo[i + 1].getNumber() + 1)
            {
                strateCount++;
            }
        }

        bool ret = strateCount == 4;

        if (ret)
        {
            genealogy.setGenealogy("스트레이트");
            genealogy.setNumber(CardInfo[0].getNumber());
        }
        return ret;
    }
    bool isTripple()
    {
        bool ret = false;

        List<Card> CardInfo = new List<Card>();
        for (int i = 0; i < 5; i++)
        {
            CardInfo.Add(curCard[i]);
        }
        CardInfo.Sort(delegate (Card A, Card B)
        {
            if (A.getNumber() < B.getNumber()) return 1;
            else if (A.getNumber() > B.getNumber()) return -1;
            else return 0;
        });

        for (int i = 0; i < 3; i++)
        {
            ret = ( CardInfo[i].getNumber() == CardInfo[i + 1].getNumber() &&
                    CardInfo[i + 1].getNumber() == CardInfo[i + 2].getNumber());
            if (ret)
            {
                genealogy.setGenealogy("트리플");
                genealogy.setNumber(CardInfo[i].getNumber());
                break;
            }
        }

        return ret;
    }
    int WhatPair()
    {
        int ret = 0;
        int number_pair = 0;

        List<Card> CardInfo = new List<Card>();
        for (int i = 0; i < 5; i++)
        {
            CardInfo.Add(curCard[i]);
        }
        CardInfo.Sort(delegate (Card A, Card B)
        {
            if (A.getNumber() < B.getNumber()) return 1;
            else if (A.getNumber() > B.getNumber()) return -1;
            else return 0;
        });

        for (int i = 0; i < 4; i++)
        {
            if(CardInfo[i].getNumber() == CardInfo[i + 1].getNumber())
            {
                if(CardInfo[i].getNumber() > number_pair)
                {
                    number_pair = CardInfo[i].getNumber();
                }
                ret++;
                i++;
            }
        }

        if (ret == 2)
        {
            genealogy.setGenealogy("투페어");
            genealogy.setNumber(number_pair);
        }
        else if (ret == 1)
        {
            genealogy.setGenealogy("원페어");
            genealogy.setNumber(number_pair);
        }
        else
        {
            genealogy.setGenealogy("탑");
            genealogy.setNumber(CardInfo[0].getNumber());
        }
        return ret;
    }
}

