using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class GameTile : MonoBehaviour, IPointerEnterHandler,
    IPointerExitHandler, IPointerDownHandler
{
    [Header("Spawnig asset")]
    [SerializeField] GameObject machinegunTurret;
    [SerializeField] GameObject mortarTurret;
    [SerializeField] GameObject EMPGenerator;
    public bool machinegun = false;
    public bool mortar = false;
    public bool IEM = false;
    public bool delete = false;

    [Header("Sprite")]
    [SerializeField] SpriteRenderer hoverRenderer;
    [SerializeField] SpriteRenderer spawnRenderer;
    [SerializeField] SpriteRenderer createRenderer;
    [SerializeField] SpriteRenderer exitRenderer;

    private SpriteRenderer spriteRenderer;
    private Player player;
    private Color originalColor;

    public GameManager GM { get; internal set; }
    public int X { get; internal set; }
    public int Y { get; internal set; }
    public bool IsBloced { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        var temp = GameObject.Find("Player");
        player = temp.GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            machinegun = true;
            mortar = false;
            IEM = false;
            delete = false;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            mortar = true;
            machinegun = false;
            IEM = false;
            delete = false;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            IEM = true;
            machinegun = false;
            mortar = false;
            delete = false;
        }
        if (Input.GetKeyUp(KeyCode.Delete))
        {
            delete = true;
            machinegun = false;
            mortar = false;
            IEM = false;
        }
    }


    internal void TurnGrey(float alphaValue)
    {
        //spriteRenderer.color = Color.gray;
        //originalColor = spriteRenderer.color;

        Color currentColor = spriteRenderer.color;
        Color newColor = new Color(currentColor.grayscale, currentColor.grayscale, currentColor.grayscale, 0.5f);
        spriteRenderer.color = newColor;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        hoverRenderer.enabled = true;
        GM.TargetTile = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverRenderer.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsBloced)
        {
            IsBloced = true;
            GM.GamePath();
            if (GM.GetPathLeght() > 1)
            {
                IsBloced = true;
                if (machinegun && player.OnCheckMoney(machinegunTurret.GetComponent<Tourel>().cost)) //spawn machingun
                {
                    Instantiate(machinegunTurret, this.transform.position, Quaternion.identity);
                    player.OnSpendMoney(machinegunTurret.GetComponent<Tourel>().cost);
                }
                else if (mortar && player.OnCheckMoney(mortarTurret.GetComponent<Tourel>().cost)) //spawn mortier
                {
                    Instantiate(mortarTurret, this.transform.position, Quaternion.identity);
                    player.OnSpendMoney(mortarTurret.GetComponent<Tourel>().cost);
                }
                else if (IEM && player.OnCheckMoney(EMPGenerator.GetComponent<Tourel>().cost)) //spawn EMPGernerator
                {
                    Instantiate(EMPGenerator, this.transform.position, Quaternion.identity);
                    player.OnSpendMoney(EMPGenerator.GetComponent<Tourel>().cost);
                }
                else
                {
                    IsBloced = false;
                }

            }
            else
            { IsBloced = false; }
        }


        if (IsBloced && delete)
        {
            foreach (Tourel tourel in Tourel.allTourel)
            {

                if (Vector3.Distance(this.transform.position, tourel.transform.position) < 1)
                {
                    IsBloced = false;
                    player.OnGetMoney(tourel.cost);
                    tourel.OnRevome();
                    break;
                }
            }
        }

        //Debug.Log($"IsBloced = {IsBloced}");
        GM.GamePath();
    }

    internal void SetEnemySpawn()
    {
        spawnRenderer.enabled = true;
    }

    internal void SetWall()
    {
        createRenderer.enabled = true;
        IsBloced = createRenderer.enabled;
    }

    internal void SetExit()
    {
        exitRenderer.enabled = true;
    }

    internal void SetPath(bool isPath)
    {
        Color transparentOrange = new Color(1, 0.375f, 0, 0.5f);

        spriteRenderer.color = isPath ? transparentOrange : originalColor;
    }

    internal void Reset()
    {
        IsBloced = false;
        hoverRenderer.enabled = false;
        spawnRenderer.enabled = false;
        createRenderer.enabled = false;
        exitRenderer.enabled = false;
        spriteRenderer.color = originalColor;
    }
}
