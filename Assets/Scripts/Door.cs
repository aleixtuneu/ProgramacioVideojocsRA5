using UnityEngine;

public class Door : MonoBehaviour
{
    private bool open_door = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collisió detectada amb: " + other.gameObject.name);
        Debug.Log("Tag: " + other.tag);

        if (other.CompareTag("Player") && !open_door)
        {
            Debug.Log("GameManager trobat!");
            Debug.Log($"Estat clau: {GameManager.Instance.IsKeyCollected}");

            // GameManager per intentar obrir la porta
            if (GameManager.Instance.TryOpenDoor())
            {
                OpenDoor();
            }
            else
            {
                Debug.Log("No tens la clau!");
            }
        }
        else
        {
            Debug.LogError("ERROR: GameManager NO ha estat trobat a la escena!");
            Debug.LogError("Assegura't que hi ha un GameObject amb el component GameManager!");
        }
    }

    private void OpenDoor()
    {
        open_door = true;
        Debug.Log("Porta oberta!");
        Destroy(gameObject); // La porta desapareix
        
    }
}
