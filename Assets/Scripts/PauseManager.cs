using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;
    public static PauseManager instance;

    private GameObject pausePanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        // Premer ESC per pausar/despausar
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pausar el joc
        Debug.Log("Joc pausat!");
        ShowPausePanel();
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f; // Reprendre el joc
        Debug.Log("Joc reprès!");
        HidePausePanel();
    }

    private void ShowPausePanel()
    {
        // De moment només debug
        Debug.Log("Mostrar menú de pausa");
    }

    private void HidePausePanel()
    {
        Debug.Log("Amagar menú de pausa");
    }
}
