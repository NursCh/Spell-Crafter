using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameManager gameManager;

    public void Start()
    {
        gameManager = GetComponent<GameManager>();
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Shop"); 
        gameManager.UpdateGameState(GameState.CustomerArrive);
    }
}