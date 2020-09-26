using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public TextMeshProUGUI infoText;
    private bool tapToRestart = false;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = GetComponent<GameManager>();
        Time.timeScale = 1;
    }

    private void Start()
    {
        //AdjustAspectRatio();
    }

    // Update is called once per frame
    private void Update()
    {
        if (tapToRestart)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                    
            }
        }

    }

    public void OnLevelFinished()
    {
        infoText.text = "Level finished with " + PlayerManager.instance.followerList.Count + " chicks." +
                "\nTap to retry.";
        infoText.gameObject.SetActive(true);
        tapToRestart = true;
        Time.timeScale = 0;
    }

    public void OnLevelFailed()
    {
        infoText.text = "Level failed. Got caught." +
                "\nTap to retry.";
        infoText.gameObject.SetActive(true);
        tapToRestart = true;
        Time.timeScale = 0;
    }

    public void ToggleInfoText(bool toggle)
    {
        infoText.gameObject.SetActive(toggle);
    }

    private void AdjustAspectRatio()
    {
        Vector2 deviceScreenResolution = new Vector2(Screen.width, Screen.height);

        float screenHeight = Screen.height;
        float screenWidth = Screen.width;

        float DEVICE_SCREEN_ASPECT = screenWidth / screenHeight;

        Camera.main.aspect = DEVICE_SCREEN_ASPECT;
    }
}
