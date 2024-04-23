using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument document;
    private Button button;

    //to play a sound for the button click, for example
    private List<Button> menuButtons = new List<Button>();
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        document= GetComponent<UIDocument>();

        button = document.rootVisualElement.Q("StartGameButton") as Button;
        button.RegisterCallback<ClickEvent>(OnPlayGameClick);

        menuButtons = document.rootVisualElement.Query<Button>().ToList();

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonClick);
        }
    }

    private void OnDisable()
    {
        button.UnregisterCallback<ClickEvent>(OnPlayGameClick);

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonClick);
        }
    }

    private void OnPlayGameClick(ClickEvent evt)
    {
        Debug.Log("You started the game!");
    }

    private void OnAllButtonClick(ClickEvent evt)
    {
        audioSource.Play();
    }
}
