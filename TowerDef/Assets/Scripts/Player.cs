using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text MoneyText;
    [SerializeField] int BaseHp;
    [SerializeField] internal int currentMoney;

    public static Player Singleton;

    internal int totalHp; //retirer plus tard
    public static int bonusHP = 0;


    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        Debug.Log("resetMoney");
        totalHp = BaseHp + bonusHP;
        MoneyText.text = $"Money : {currentMoney}$";
        hpText.text = $"HP: {totalHp}"; //retirer plus tard
    }


    private void Update()
    {
        if(hpText == null || MoneyText == null)
        {
            hpText = GameObject.Find("HPBar").GetComponent<TMP_Text>();
            MoneyText = GameObject.Find("MoneyBar").GetComponent<TMP_Text>();
        }

        hpText.text = $"HP: {totalHp}";
        MoneyText.text = $"${currentMoney}";
    }

    public void OnTakeDamege(int damege)
    {
        totalHp -= damege;
        totalHp = (int)Mathf.Clamp(totalHp, 0, 999);   
    }

    public void  OnGetMoney(int money)
    {
        currentMoney += money;
        currentMoney = (int)Mathf.Clamp(currentMoney, 0, 999999999);
    }

    public bool OnCheckMoney(int money)
    {
        return currentMoney - money >=0;
    }

    public void OnSpendMoney(int money)
    {
        currentMoney -= money;
        currentMoney = (int)Mathf.Clamp(currentMoney, 0, 999999999);
    }    
}
