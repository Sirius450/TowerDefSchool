using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameTile : MonoBehaviour, IPointerEnterHandler,
    IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] SpriteRenderer hoverRenderer;
    [SerializeField] SpriteRenderer turretRenderer;
    [SerializeField] SpriteRenderer spawnRenderer;
    [SerializeField] SpriteRenderer createRenderer;
    [SerializeField] SpriteRenderer exitRenderer;
    private LineRenderer lineRenderer;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool shoot;
    [SerializeField] private int damage;
    [SerializeField] private int range;

    public GameManager GM { get; internal set; }
    public int X { get; internal set; }
    public int Y { get; internal set; }
    public bool IsBloced { get; private set; }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.SetPosition(0, transform.position);

        spriteRenderer = GetComponent<SpriteRenderer>();
        turretRenderer.enabled = false;
        originalColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (turretRenderer.enabled)
        {
            OnAttack();
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
        turretRenderer.enabled = !turretRenderer.enabled;
        IsBloced = turretRenderer.enabled;

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

    private void OnAttack()
    {
        Enemy target = null;
        foreach (var enemy in Enemy.allEnemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < range)
            {
                target = enemy;
                break;
            }
        }

        if (target != null)
        {
            lineRenderer.SetPosition(1, target.transform.position);
            lineRenderer.enabled = true;
            if(!shoot)
            {
                StartCoroutine(OnShoot(target));
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    IEnumerator OnShoot(Enemy target)
    {
        shoot = true;
        target.OnTakeDamage(damage);
        yield return new WaitForSeconds(0.5f);
        shoot = false;
    }
}
