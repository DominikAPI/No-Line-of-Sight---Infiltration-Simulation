using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button howToButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text instructions;

    private bool instructionsVisible = false;

    private void Awake()
    {
        newGameButton.onClick.AddListener(NewGame);
        continueGameButton.onClick.AddListener(ContinueGame);
        howToButton.onClick.AddListener(LoadHowTo);
        exitButton.onClick.AddListener(ExitGame);
        continueGameButton.enabled = SaveSystem.HasSave();
        instructions.gameObject.SetActive(false);
    }

    private void ToggleInstructionsVisibility()
    {
        instructionsVisible = !instructionsVisible;
        instructions.gameObject.SetActive(instructionsVisible);
    }

    private void NewGame()
    {
        SaveSystem.DeleteSave();
        SceneManager.LoadScene("GameScene");
    }

    private void ContinueGame()
    {
        SessionData.ContinueGame = true;
        SceneManager.LoadScene("GameScene");
    }

    private void LoadHowTo() => ToggleInstructionsVisibility();

    private void ExitGame() => Application.Quit();

}
