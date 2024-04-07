using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text hpText;
    [SerializeField] int BaseHp;

    private int totalHp;
    public static int bonusHP = 0;

    private void Awake()
    {
        totalHp = BaseHp + bonusHP;
        hpText.text = $"HP: {totalHp}";
    }

    public void OnTakeDamege(int damege)
    {
        totalHp -= damege;
        totalHp = (int)Mathf.Clamp(totalHp, 0, 999);

        hpText.text = $"HP: {totalHp}";
    }
}