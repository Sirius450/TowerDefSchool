using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text MoneyText;
    [SerializeField] int BaseHp;
    [SerializeField] int BaseMoney;

    internal int totalHp; //retirer plus tard
    public static int bonusHP = 0;

    public static int totalMoney;
    public int bonusMoney;

    private void Awake()
    {
        totalHp = BaseHp + bonusHP;
        totalMoney = BaseMoney + bonusMoney;
        MoneyText.text = $"Money : {totalMoney}$";
        hpText.text = $"HP: {totalHp}"; //retirer plus tard
    }

    public void OnTakeDamege(int damege)
    {
        totalHp -= damege;
        totalHp = (int)Mathf.Clamp(totalHp, 0, 999);

        hpText.text = $"HP: {totalHp}";//retirer plus tard
    }

    public void  OnGetMoney(int money)
    {
        totalMoney += money;
        totalMoney = (int)Mathf.Clamp(totalMoney, 0, 999999999);

        MoneyText.text = $"Money : {totalMoney}$";
    }

    public bool OnCheckMoney(int money)
    {
        return totalMoney - money >=0;
    }

    public void OnSpendMoney(int money)
    {
        totalMoney -= money;
        totalMoney = (int)Mathf.Clamp(totalMoney, 0, 999999999);

        MoneyText.text = $"Money : {totalMoney}$";

    }    
}
