using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public struct GameSaveData
{
    public int floorIndex;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text detectedText;
    [SerializeField] private List<GameObject> floors;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject mask;

    private GuardManager activeFloor;
    private PlayerController playerController;
    private CameraFollow cameraFollow;
    private int floorIndex;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playerController = player.GetComponent<PlayerController>();
            cameraFollow = Camera.main.GetComponent<CameraFollow>();

            for (int i = 1; i < floors.Count; i++) floors[i].SetActive(false);

            floorIndex = 0;
            activeFloor = floors[floorIndex].GetComponent<GuardManager>();
            activeFloor.OnDetection += HandleDetection;

            restartButton.onClick.AddListener(ResetFloor);
            continueButton.onClick.AddListener(ContinueGame);
            exitButton.onClick.AddListener(ExitGame);
            quitButton.onClick.AddListener(QuitToMenu);

            SetMousePointer(false);
            SetDetectionUI(false);
            SetPauseUI(false);
        }
        else Destroy(gameObject);
        
    }

    private void Start()
    {
        if (SessionData.ContinueGame)
        {
            SessionData.ContinueGame = false;
            SaveSystem.LoadGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) PauseGame();
    }

    private void SetDetectionUI(bool visible)
    {
        restartButton.gameObject.SetActive(visible);
        detectedText.gameObject.SetActive(visible);
    }

    private void SetPauseUI(bool visible)
    {
        exitButton.gameObject.SetActive(visible);
        quitButton.gameObject.SetActive(visible);
        continueButton.gameObject.SetActive(visible);
    }

    private void HandleDetection(IDetectable detectable, string message)
    {
        playerController.DisablePlayerControls();
        cameraFollow.Enabled = false;
        detectable.FocusOn();
        mask.SetActive(false);
        detectedText.text = message;
        SetDetectionUI(true);
        SetMousePointer(true);
        Time.timeScale = 0;
    }

    private void ResetFloor()
    {
        SetMousePointer(false);
        SetDetectionUI(false);
        playerController.ResetObject();
        activeFloor.ResetGuards();
        cameraFollow.Enabled = true;
        playerController.EnablePlayerControls();
        mask.SetActive(true);
        Time.timeScale = 1;
    }

    public void ActivateNextFloor()
    {
        activeFloor = floors[floorIndex + 1].GetComponent<GuardManager>();
        activeFloor.OnDetection += HandleDetection;
        floors[floorIndex + 1].SetActive(true);
    }

    public void IncrementFloor()
    {
        floors[floorIndex++].SetActive(false);
        playerController.SetCheckpoint();
        SaveSystem.SaveGame();
    }

    private void HandleFloorLoad(int newFloorIndex)
    {
        activeFloor = floors[newFloorIndex].GetComponent<GuardManager>();
        activeFloor.OnDetection += HandleDetection;
        floors[floorIndex].SetActive(false);
        floors[newFloorIndex].SetActive(true);
        floorIndex = newFloorIndex;
    }

    public void Save(ref SaveData saveData)
    {
        saveData.gameSaveData.floorIndex = floorIndex;
        playerController.Save(ref saveData.playerSaveData);
    }

    public void Load(SaveData saveData) => StartCoroutine(HandleLoad(saveData));

    public IEnumerator HandleLoad(SaveData saveData)
    {
        playerController.DisablePlayerControls();
        HandleFloorLoad(saveData.gameSaveData.floorIndex);
        playerController.Load(saveData.playerSaveData);
        yield return floors[floorIndex].GetComponentInChildren<ReceivingElevator>().OpenDoor();
        playerController.EnablePlayerControls();
    }

    private void PauseGame()
    {
        playerController.DisablePlayerControls();
        SetMousePointer(true);
        Time.timeScale = 0f;
        SetPauseUI(true);
    }

    private void ContinueGame()
    {
        SetMousePointer(false);
        Time.timeScale = 1.0f;
        playerController.EnablePlayerControls();
        SetPauseUI(false);
    }

    private void ExitGame() => Application.Quit();

    private void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void SetMousePointer(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Confined;
    }
}
