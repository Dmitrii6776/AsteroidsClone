
using AsteroidsClone;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviour
{
   [Header("Managers")]
   [SerializeField] private InputManager inputManager;
   [Header("UI")]
   [SerializeField] private GameObject menu;
   [SerializeField] private TextMeshProUGUI scoreText;
   [SerializeField] private TextMeshProUGUI playerLivesText;
   [SerializeField] private TextMeshProUGUI switchControlButtonText;
   [SerializeField] private GameState gameState;
   private int _score;
   private int _playerLives;
   public bool isInMenu;


   public void ActivatePauseMenu()
   {
      Time.timeScale = 0;
      menu.SetActive(true);
      isInMenu = true;

   }

   public void NewGame()
   {
      gameState.isGameStarted = true;
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      DisableMenu();
   }

   public void Resume()
   {
      if (!gameState.isGameStarted) return;
      DisableMenu();
      
   }

   public void SwitchControl()
   {
      inputManager.SwitchControl();
      SetSwitchControlButtonText();
   }
   public void Exit()
   {
      gameState.isGameStarted = false;
      Debug.Log("Exit");
   }

   public void GameOver()
   {
      gameState.isGameStarted = false;
      ActivatePauseMenu();
   }
   
   

   private void DisableMenu()
   {
      menu.SetActive(false);
      Time.timeScale = 1;
      isInMenu = false;
   }

   

   private void SetSwitchControlButtonText()
   {
      switchControlButtonText.text = inputManager.GetCurrentControlState() ? "Control: Mouse + Keyboard" : "Control: Keyboard";
   }
   
   public void SetScore(int score)
   {
      _score += score;
      SetScoreText();
   }

   public void SetLives(int lives)
   {
      _playerLives = lives;
      SetLivesText();
   }

   private void Start()
   {
      SetSwitchControlButtonText();
      
      ActivatePauseMenu();
      if (gameState.isGameStarted)
      {
         DisableMenu();
      }
      
   }
   

   private void SetScoreText()
   {
      scoreText.text = _score.ToString();
   }

   private void SetLivesText()
   {
      playerLivesText.text = _playerLives.ToString();
   }
   
   
}
