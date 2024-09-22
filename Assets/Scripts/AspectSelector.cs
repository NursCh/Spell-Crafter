using System;
using System.Diagnostics.Contracts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AspectSelector : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject prefab4;
    public GameObject prefab5;
    public GameObject prefab6;
    public GameObject[] prefabsSpells;
    public GameObject prefabSpell1;
    public GameObject prefabSpell2;
    public GameObject prefabSpell3;
    public GameObject prefabSpell4;
    public GameObject Spell;
    public GameObject selectedAspect1 = null;
    public GameObject selectedAspect2 = null;
    public GameManager gameManager;
    public Camera camera;
    private float timeThreshold = 0.2f;    
    private float clickTime;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 initialMousePosition;
    private RaycastHit2D hit;
    public TextMeshProUGUI text;
    public AudioClip clueSoundLeft;  
    private AudioSource audioSourceLeft;
    public AudioClip clueSoundRight;
    private AudioSource audioSourceRight;

    
    void Start()
    {
        audioSourceLeft = gameObject.AddComponent<AudioSource>();
        audioSourceRight = gameObject.AddComponent<AudioSource>();
;
        prefabs = new GameObject[6];
        prefabs[0] = prefab1;
        prefabs[1] = prefab2;
        prefabs[2] = prefab3;
        prefabs[3] = prefab4;
        prefabs[4] = prefab5;
        prefabs[5] = prefab6;
        prefabsSpells = new GameObject[4];
        prefabsSpells[0] = prefabSpell1;
        prefabsSpells[1] = prefabSpell2;
        prefabsSpells[2] = prefabSpell3;
        prefabsSpells[3] = prefabSpell4;
        gameManager = GetComponent<GameManager>();
        camera = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayAudioLeft();
            Vector3 WorldPosition = GetMouseWorldPosition();
            initialMousePosition = WorldPosition;
            Vector2 origin = new Vector2(WorldPosition.x, WorldPosition.y);
            hit = Physics2D.Raycast(origin, Vector2.zero, 0f);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Aspect"))
                {
                    isDragging = false;
                    offset = hit.collider.transform.position - WorldPosition;
                    clickTime = Time.time;
                }
            }
            clickTime = Time.time;
        }
        if (Input.GetMouseButtonUp(0))
        {
            float timeHeld = Time.time - clickTime;
            float distanceMoved = Vector2.Distance(initialMousePosition, camera.ScreenToWorldPoint(Input.mousePosition));
            
            if (timeHeld < timeThreshold)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Aspect"))
                    {
                        SelectAspect(hit);
                    }
                }
            }
            else
            {
                if (isDragging)
                {
                    isDragging = false;  
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            float distanceMoved = Vector2.Distance(initialMousePosition, camera.ScreenToWorldPoint(Input.mousePosition));

                isDragging = true;
                if (hit.collider != null)
                {
                    MoveObject(hit);
                }
        }
        void MoveObject(RaycastHit2D hit)
        {
            if (hit.collider.CompareTag("Aspect"))
            {
                RuneAspect runeAspect = hit.collider.gameObject.GetComponent<RuneAspect>();
                if (hit.collider.transform.parent == null && runeAspect.Line == null)
                {
                    Vector2 newPosition = GetMouseWorldPosition() + offset;
                    if (newPosition.x > -8.5 && newPosition.x < -4 && newPosition.y < 4.5f && newPosition.y > -2.3f)
                    {
                        hit.collider.transform.position = newPosition;
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            PlayAudioRight();
            DeselectAspect();
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 WorldPosition = camera.ScreenToWorldPoint(mousePosition);
        return WorldPosition;
    }
    public void PlayAudioLeft()
    {
        if (audioSourceLeft != null && clueSoundLeft != null)
        {
            audioSourceLeft.PlayOneShot(clueSoundLeft); 
        }
    }
    public void PlayAudioRight()
    {
        if (audioSourceRight != null && clueSoundRight != null)
        {
            audioSourceRight.PlayOneShot(clueSoundRight); 
        }
    }


    void SelectAspect(RaycastHit2D hit)
    {
            GameObject aspect = hit.collider.GameObject();
            //Debug.Log(aspect.name);
            if (selectedAspect1 == null)
            {
                selectedAspect1 = aspect;
            }
            else
            {
                selectedAspect2 = aspect;
                if (!CheckCombination())
                {
                    CheckSpell();
                }
            }
    }

    bool CheckSpell()
    {
        RuneAspect runeAspect1 = selectedAspect1.GetComponent<RuneAspect>();
        RuneAspect runeAspect2 = selectedAspect2.GetComponent<RuneAspect>();
        string name1 = runeAspect1.name;
        string name2 = runeAspect2.name;
        string result = gameManager.GetSpellsResult(name1, name2);
        //Debug.Log("abboa");
        if (!string.IsNullOrEmpty(result))
        {
            //Debug.Log("1");
            if (Spell == null)
            {
                DrawLine(Color.yellow, Color.blue);
                CreateSpell(result);
                selectedAspect1 = null;
                selectedAspect2 = null;
                Debug.Log(result);
                return true;
            }
            selectedAspect1 = null;
            selectedAspect2 = null;
            ShowCombinationResult(result);
        }
        selectedAspect1 = null;
        selectedAspect2 = null;
        return false;
    }

    public void CastSpell()
    {
        gameManager.UpdateGameState(GameState.CastSpell);
    }
    public bool CheckSpell(string spell)
    {
        if (Spell == null)
        {
            IncorrectSpell();
            gameManager.UpdateGameState(GameState.CustomerLeave);
            return false;
        }
        if (spell == Spell.name)
        {
            CorrectSpell();
            gameManager.UpdateGameState(GameState.CustomerLeave);
            return true;
        }
        else
        {
            IncorrectSpell();
        }
        gameManager.UpdateGameState(GameState.CustomerLeave);
        return false;
    }

    public void CorrectSpell()
    {
        text.text = "Thanks man";
    }

    public void IncorrectSpell()
    {
        text.text = "WHAT HAVE YOU DONE?!!?!!?";
    }
    bool CheckCombination()
    {
            RuneAspect runeAspect1 = selectedAspect1.GetComponent<RuneAspect>();
            RuneAspect runeAspect2 = selectedAspect2.GetComponent<RuneAspect>();
            string name1 = runeAspect1.name;
            string name2 = runeAspect2.name;
            string result = gameManager.GetCombinationResult(name1, name2);
            Debug.Log(result);
            if (!string.IsNullOrEmpty(result))
            {
                DrawLine(Color.green, Color.red);
                CreateAspect(result);
                ShowCombinationResult(result);
                selectedAspect1 = null;
                selectedAspect2 = null;
                return true;
            }
            else
            {
                ShowCombinationResult(result);
            }
            return false;
    }

    void CreateSpell(string result)
    {
        Vector2 pos1 = selectedAspect1.transform.position;
        Vector2 pos2 = selectedAspect2.transform.position;
        RuneAspect runeAspect1 = selectedAspect1.GetComponent<RuneAspect>();
        RuneAspect runeAspect2 = selectedAspect2.GetComponent<RuneAspect>();
        Vector2 mid = pos1 + (pos2 - pos1) * 0.5f;
        GameObject newAspect = new GameObject();
        foreach (GameObject aspect in prefabsSpells)
        {
            if (result == aspect.name)
            {
                newAspect.transform.position = mid;
                RuneAspect runeOfAspect = newAspect.AddComponent<RuneAspect>();
                newAspect.tag = "Spell";
                CircleCollider2D circleCollider = newAspect.AddComponent<CircleCollider2D>();
                circleCollider.radius = 0.18f;
                circleCollider.isTrigger = false;
                newAspect.transform.localScale = new Vector3(2f, 2f, 1);
                newAspect.AddComponent<SpriteRenderer>().sprite = aspect.GetComponent<SpriteRenderer>().sprite;
                newAspect.name = aspect.name;
                break;
            }
        }
        Spell = newAspect;
        newAspect.transform.SetParent(selectedAspect1.transform);
    }

    void CreateAspect(string result)
    {
        Vector2 pos1 = selectedAspect1.transform.position;
        Vector2 pos2 = selectedAspect2.transform.position;
        RuneAspect runeAspect1 = selectedAspect1.GetComponent<RuneAspect>();
        RuneAspect runeAspect2 = selectedAspect2.GetComponent<RuneAspect>();
        Vector2 mid = pos1 + (pos2 - pos1) * 0.5f;
        GameObject newAspect = new GameObject();
        foreach (GameObject aspect in prefabs)
        {
            if (result == aspect.name)
            {
                newAspect.transform.position = mid;
                newAspect.transform.localScale = new Vector3(2f, 2f, 1);
                RuneAspect runeOfAspect = newAspect.AddComponent<RuneAspect>();
                newAspect.tag = "Aspect";
                CircleCollider2D circleCollider = newAspect.AddComponent<CircleCollider2D>();
                circleCollider.radius = 0.18f;
                circleCollider.isTrigger = false;
                newAspect.AddComponent<SpriteRenderer>().sprite = aspect.GetComponent<SpriteRenderer>().sprite;
                runeOfAspect.name = aspect.name;
                newAspect.name = aspect.name;
                break;
            }
            
        }
        newAspect.transform.SetParent(selectedAspect1.transform);
    }

    public void DestroyAllChildren(GameObject gameObject)
    {
        // Loop through all child objects of runeAspect
        foreach (Transform child in gameObject.transform)
        {
            if (child.transform.CompareTag("Spell"))
            {
                Spell = null;
            }
            RuneAspect runeAspect = child.GetComponent<RuneAspect>();
            if (runeAspect.Pair != null)
            {
                Destroy(runeAspect.Line);
                runeAspect.Line = null;
                if (runeAspect.Pair != null)
                {
                    DestroyAllChildren(runeAspect.Pair);
                    RuneAspect runeAspect3 = runeAspect.Pair.GetComponent<RuneAspect>();
                    runeAspect3.Line = null;
                    runeAspect3.Pair = null;
                }
                DestroyAllChildren(child.gameObject);
                runeAspect.Pair = null;
            }
            Destroy(child.gameObject); 
        }
    }


    void DrawLine(Color startColor, Color endColor)
    {
        GameObject line = new GameObject();
        RuneAspect runeAspect1 = selectedAspect1.GetComponent<RuneAspect>();
        RuneAspect runeAspect2 = selectedAspect2.GetComponent<RuneAspect>();
        if (runeAspect1.Line != null)
        {
            Destroy(runeAspect1.Line);
            runeAspect1.Line = null;
            if (runeAspect1.Pair != selectedAspect2)
            {
                RuneAspect runeAspect3 = runeAspect1.Pair.GetComponent<RuneAspect>();
                runeAspect3.Line = null;
                runeAspect3.Pair = null;
            }
            DestroyAllChildren(selectedAspect1);
            DestroyAllChildren(runeAspect1.Pair);
            runeAspect1.Pair = null;
        }
        if (runeAspect2.Line != null)
        {
            Destroy(runeAspect2.Line); 
            runeAspect2.Line = null;
            if (runeAspect2.Pair != selectedAspect1)
            {
                RuneAspect runeAspect4 = runeAspect2.Pair.GetComponent<RuneAspect>();
                runeAspect4.Line = null;
                runeAspect4.Pair = null;
            }
            DestroyAllChildren(selectedAspect2);
            DestroyAllChildren(runeAspect2.Pair);
            runeAspect2.Pair = null;
        }
        Vector2 pos1 = selectedAspect1.transform.position;
        Vector2 pos2 = selectedAspect2.transform.position;
        LineRenderer lineRenderer;
        lineRenderer = line.AddComponent<LineRenderer>();
        //lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        
        lineRenderer.startWidth = 0.05f; 
        lineRenderer.endWidth = 0.05f;  
        lineRenderer.positionCount = 2;  
        
        Gradient gradient = new Gradient();
        float alpha = 1.0f;
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;

        
        lineRenderer.SetPosition(0, new Vector3(pos1.x, pos1.y, 1));  
        lineRenderer.SetPosition(1, new Vector3(pos2.x, pos2.y, 1));  
        selectedAspect1.GetComponent<RuneAspect>().Line = line;
        selectedAspect2.GetComponent<RuneAspect>().Line = line;
        selectedAspect1.GetComponent<RuneAspect>().Pair = selectedAspect2;
        selectedAspect2.GetComponent<RuneAspect>().Pair = selectedAspect1;
    }

    void ShowCombinationResult(string result)
    {
        if (result != null)
        {
            //Debug.Log(result);
        }
        else
        {
            //.Log(result);
        }
    }

    void DeselectAspect()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 WorldPosition = camera.ScreenToWorldPoint(mousePosition);
        Vector2 origin = new Vector2(WorldPosition.x, WorldPosition.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 0f); 
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Aspect"))
            {
                GameObject aspect = hit.collider.GameObject();
                RuneAspect runeAspect = aspect.GetComponent<RuneAspect>();
                if (runeAspect.Pair != null)
                {
                    Destroy(runeAspect.Line);
                    runeAspect.Line = null;
                    if (runeAspect.Pair != null)
                    {
                        RuneAspect runeAspect3 = runeAspect.Pair.GetComponent<RuneAspect>();
                        runeAspect3.Line = null;
                        DestroyAllChildren(runeAspect.Pair);
                        runeAspect3.Pair = null;
                    }
                    DestroyAllChildren(aspect);
                    runeAspect.Pair = null;
                }
                selectedAspect1 = null;
                selectedAspect2 = null;
            }
        }
        else
        {
            selectedAspect1 = null;
            selectedAspect2 = null;
        }
    }
}