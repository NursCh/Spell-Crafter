using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static CombinationLoader CombinationLoader;
    public static SpellsLoader spellsLoader;
    public static Book Book;
    private Customer customer;
    public TextMeshProUGUI customerText;
    public static AspectSelector AspectSelector;
    public List<GameObject> GeneratedAspects;
    public static Player Player;
    public GameState state;
    public GameObject stickman;
    public StickmanMovement stickmanMovement;
    public AudioClip Correct;
    private AudioSource audioSourceCorrect;
    public AudioClip Incorrect;
    private AudioSource audioIncorrect;
    private bool correct = false;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            customer = gameObject.AddComponent<Customer>();
            CombinationLoader = gameObject.AddComponent<CombinationLoader>();
            spellsLoader = gameObject.AddComponent<SpellsLoader>();
            Book = gameObject.GetComponent<Book>();
            Player = gameObject.GetComponent<Player>();
            AspectSelector = gameObject.GetComponent<AspectSelector>();
            audioSourceCorrect = gameObject.AddComponent<AudioSource>();
            audioIncorrect = gameObject.AddComponent<AudioSource>();
            Book.GenerateBaseAspects();
            CombinationLoader.LoadCombinations();  // Load combinations during initialization
            spellsLoader.LoadSpells();
            stickmanMovement = stickman.GetComponent<StickmanMovement>();
        } 
        UpdateGameState(state);
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;
        switch (state)
        {
            case GameState.MainMenu:
                break;
            case GameState.CustomerArrive:
                stickmanMovement.StartMovement(10);
                StartCoroutine(waitToCharacterArrive());
                customer.CreateCustomer(Book);
                GeneratedAspects = Book.GenerateAspects(Book.BaseAspects, customer);
                showAspects(GeneratedAspects);
                break;
            case GameState.CastSpell:
                correct = AspectSelector.CheckSpell(customer.spell.name);
                break;
            case GameState.CustomerLeave:
                stickmanMovement.StartMovement(20);
                StartCoroutine(waitToCharacterLeave());
                break;
            case GameState.EndDay:
                break;
        }
    }

    
    public void showAspects(List<GameObject> GeneratedAspects)
    {
        float spacing = 0.5f; 
        Vector2 startPosition = new Vector2(-8.5f, 4.5f); 
        for (int i = 0; i < GeneratedAspects.Count; i++)
        {
            Vector2 spawnPosition = new Vector2(startPosition.x, startPosition.y - (i * spacing));
            Instantiate(GeneratedAspects[i], spawnPosition, Quaternion.identity);
        }
    }
    IEnumerator waitToCharacterArrive()
    {
        Debug.Log("Waiting for 3 seconds...");
        
        yield return new WaitForSeconds (10/8f);
        customerText.text = customer.text;
        Debug.Log("3 seconds have passed!");
        
    }
    IEnumerator waitToCharacterLeave()
    {
        Debug.Log("Waiting for 3 seconds...");
        if (correct)
        {
            PlayAudioCorrect();
        }
        else
        {
            PlayAudioIncorrect();
        }
        yield return new WaitForSeconds (3.0f);
        audioSourceCorrect.Pause();
        audioIncorrect.Pause();
        SceneManager.LoadScene("Shop"); 
        Debug.Log("3 seconds have passed!");
        
    }

    public string GetCombinationResult(string element1, string element2)
    {
        return CombinationLoader.GetCombinationResult(element1, element2);
    }

    public string GetSpellsResult(string element1, string element2)
    {
        return spellsLoader.GetSpellResult(element1, element2);
    }
    public void PlayAudioIncorrect()
    {
        if (audioIncorrect != null && Incorrect != null)
        {
            audioIncorrect.PlayOneShot(Incorrect); 
        }
    }
    public void PlayAudioCorrect()
    {
        if (audioSourceCorrect != null && Correct != null)
        {
            audioSourceCorrect.PlayOneShot(Correct); 
        }
    }
}

public enum GameState
{
    MainMenu,
    CustomerArrive,
    CastSpell,
    CustomerLeave,
    EndDay,
}
