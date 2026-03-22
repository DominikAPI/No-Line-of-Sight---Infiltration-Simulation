using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private Button restartButton;
    [SerializeField] private TMP_Text detectedText;
    [SerializeField] private List<GameObject> floors;
    [SerializeField] private GameObject player;

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

            SetUI(false);
        }
        else Destroy(gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetUI(bool visible)
    {
        restartButton.gameObject.SetActive(visible);
        detectedText.gameObject.SetActive(visible);
    }

    private void HandleDetection(IDetectable detectable, string message)
    {
        playerController.DisablePlayerControls();
        cameraFollow.Enabled = false;
        detectable.FocusOn();
        detectedText.text = message;
        SetUI(true);
        Time.timeScale = 0;
    }

    private void ResetFloor()
    {
        SetUI(false);
        playerController.ResetObject();
        activeFloor.ResetGuards();
        cameraFollow.Enabled = true;
        playerController.EnablePlayerControls();
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
    }
}
