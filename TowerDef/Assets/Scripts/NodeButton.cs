using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum NodeState
{
    Obtained, // Vert
    Accessible, // Jaune
    Unaccessible // Rouge
}

public class NodeButton : MonoBehaviour
{
    [SerializeField] NodeButton parentNode;
    [SerializeField] List<GameObject> nodeLiset = new List<GameObject>();
    List<NodeButton> ParentNodeList = new List<NodeButton>();
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TMP_Text valueText;
    [SerializeField] int bonusHP = 1;
    [SerializeField] bool actif = false;
    [SerializeField] float effect = 0.3f;
    [SerializeField] int expCost = 100;
    [SerializeField] bool range;
    [SerializeField] bool power;
    [SerializeField] bool realod;
    [SerializeField] bool Ultimate;


    LineRenderer lineRenderer;
    NodeState currentState = NodeState.Unaccessible;
    List<NodeButton> children = new List<NodeButton>();
    Player player;
    private void Awake()
    {
        //valueText.text = $"+{bonusHP} HP";
        lineRenderer = GetComponent<LineRenderer>();

        if (parentNode != null)
        {
            parentNode.children.Add(this);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, parentNode.transform.position);
        }

    }

    private void Start()
    {
        SetupLine();

        if (nodeLiset.Count != 0)
        {
            foreach (GameObject nodeLiset in nodeLiset)
            {
                ParentNodeList.Add(nodeLiset.GetComponent<NodeButton>());
            }
        }

        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }
    }

    private void Update()
    {
        bool allGood = true;
        foreach (NodeButton button in ParentNodeList)
        {
            if (button.Optained())
            { allGood = true; }
            else
            { allGood = false; }
        }

        if (allGood)
        {
            currentState = NodeState.Accessible;
        }

        if (actif)
        {
            currentState = NodeState.Obtained;
        }
    }


    void SetupLine()
    {
        if (nodeLiset.Count != 0)
        {
            // Calcule le nombre de points nécessaires
            int pointCount = nodeLiset.Count * 2;
            lineRenderer.positionCount = pointCount;

            int posIndex = 0;
            foreach (GameObject startPoint in nodeLiset)
            {
                lineRenderer.SetPosition(posIndex++, startPoint.transform.position);
                lineRenderer.SetPosition(posIndex++, transform.position);
            }
        }
        else
        {
            if (parentNode == null)
            {
                // On est à la racine
                SetState(NodeState.Accessible);
            }

        }

    }



    private void SetState(NodeState nodeState)
    {
        currentState = nodeState;
        switch (currentState)
        {
            case NodeState.Obtained:
                if (player.OnCheckExp(expCost))
                {
                    actif = true;

                    foreach (Tourel tourel in Tourel.allTourel)
                    {
                        if (range)
                        {
                            tourel.OnGetRange(effect);
                            player.OnSpendExp(expCost);
                        }
                        if (power)
                        {
                            tourel.OnGetPower(effect);
                            player.OnSpendExp(expCost);
                        }
                        if (realod)
                        {
                            tourel.OnGetRealod(effect);
                            player.OnSpendExp(expCost);
                        }
                        if (Ultimate)
                        {
                            tourel.OnGetUltimate();
                            player.OnSpendExp(expCost);
                        }
                    }

                    spriteRenderer.color = Color.green;
                    foreach (var child in children)
                        child.SetState(NodeState.Accessible);

                }
                else
                {  currentState = NodeState.Accessible; }

                break;
            case NodeState.Accessible:

                spriteRenderer.color = new Color(1, 0.75f, 0);
                foreach (var child in children)
                    child.SetState(NodeState.Unaccessible);
                break;
            case NodeState.Unaccessible:
                spriteRenderer.color = Color.red;
                foreach (var child in children)
                    child.SetState(NodeState.Unaccessible);
                break;
        }
    }

    private void OnMouseDown()
    {
        if (!actif)
        {
            spriteRenderer.color = new Color(1, 0.75f, 0);
            foreach (var child in children)
                child.SetState(NodeState.Unaccessible);

            if (currentState == NodeState.Accessible)
            {
                SetState(NodeState.Obtained);
            }
        }
    }

    public bool Optained()
    {
        if (currentState == NodeState.Obtained)
        { return true; }
        else
        { return false; }
    }
}
