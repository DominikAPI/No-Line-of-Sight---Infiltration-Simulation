using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private TMP_Text detectedText;
    [SerializeField] private GameObject testFloor;
    [SerializeField] private GameObject player;

    private GuardManager activeFloor;
    private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerController = player.GetComponent<PlayerController>();

        GameObject floor = Instantiate(testFloor);
        activeFloor = floor.GetComponent<GuardManager>();
        activeFloor.OnDetection += HandleDetection;

        restartButton.onClick.AddListener(ResetFloor);

        SetUI(false);
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
        detectable.FocusOn();
        detectedText.text = message;
        SetUI(true);
        Time.timeScale = 0;
    }

    private void ResetFloor()
    {
        SetUI(false);
        activeFloor.ResetGuards();
        playerController.ResetObject();
        Time.timeScale = 1;
    }
}
