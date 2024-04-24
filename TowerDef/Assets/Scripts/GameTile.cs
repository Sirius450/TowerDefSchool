using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class GameTile : MonoBehaviour, IPointerEnterHandler,
    IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] GameObject machinegunTurret;
    [SerializeField] GameObject mortarTurret;
    [SerializeField] GameObject EMPGenerator;
    public bool machinegun = false;
    public bool mortar = false;
    public bool IEM = false;
    public bool delete = false;


    [SerializeField] SpriteRenderer hoverRenderer;
    [SerializeField] SpriteRenderer turretRenderer;
    [SerializeField] SpriteRenderer spawnRenderer;
    [SerializeField] SpriteRenderer createRenderer;
    [SerializeField] SpriteRenderer exitRenderer;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    [SerializeField] private int damage;
    [SerializeField] private int range;

    public GameManager GM { get; internal set; }
    public int X { get; internal set; }
    public int Y { get; internal set; }
    public bool IsBloced { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        turretRenderer.enabled = false;
        originalColor = spriteRenderer.color;
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

    internal void TurnGrey()
    {
        spriteRenderer.color = Color.gray;
        originalColor = spriteRenderer.color;
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
                if (machinegun) //spawn machingun
                {
                    Instantiate(machinegunTurret, this.transform.position, Quaternion.identity);
                }
                if (mortar) //spawn mortier
                {
                    Instantiate(mortarTurret, this.transform.position, Quaternion.identity);
                }
                if (IEM) //spawn EMPGernerator
                {
                    Instantiate(EMPGenerator, this.transform.position, Quaternion.identity);
                }
                IsBloced = true;
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
        spriteRenderer.color = isPath ? Color.yellow : originalColor;
    }
}
