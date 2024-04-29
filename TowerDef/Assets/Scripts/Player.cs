using System;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text MoneyText;
    [SerializeField] TMP_Text ExpText;
    [SerializeField] TMP_Text waveText;
    [SerializeField] int BaseHp;
    [SerializeField] internal int currentMoney;
    [SerializeField] internal int currentExp;
    [SerializeField] internal int currentWave;

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
        ExpText.text = $"Exp {currentExp}";
        hpText.text = $"HP: {totalHp}"; //retirer plus tard
        waveText.text = $"Round {currentWave}";
    }


    private void Update()
    {
        if(hpText == null || MoneyText == null || ExpText == null)
        {
            hpText = GameObject.Find("HEALTH").GetComponentInChildren<TMP_Text>();
            MoneyText = GameObject.Find("MONEY").GetComponentInChildren<TMP_Text>();
            ExpText = GameObject.Find("Exp").GetComponentInChildren<TMP_Text>();
            waveText = GameObject.Find("RoundNumber").GetComponent<TMP_Text>();
        }

        hpText.text = $"{totalHp}";
        MoneyText.text = $"{currentMoney}";
        ExpText.text = $"{currentExp}";
        waveText.text = $"Round {currentWave}";
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

    internal bool OnCheckExp(int expCost)
    {
        return currentExp - expCost >= 0;
    }

    internal void OnSpendExp(int expCost)
    {
        currentExp -= expCost;
        currentExp = (int)Mathf.Clamp(currentExp, 0, 999999999);
    }
}
