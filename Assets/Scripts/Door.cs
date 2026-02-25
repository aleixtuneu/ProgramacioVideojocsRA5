using UnityEngine;

public class Door : MonoBehaviour
{
    private bool open_door = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collisió detectada amb: " + other.gameObject.name);
        Debug.Log("Tag: " + other.tag);
        Debug.Log("Clau recollida: " + PlayerController.keyCollected);

        if (other.CompareTag("Player") && !open_door)
        {
            if (PlayerController.keyCollected)
            {
                OpenDoor();
            }
            else
            {
                Debug.Log("No tens la clau!");
            }
        }
    }

    private void OpenDoor()
    {
        open_door = true;
        Debug.Log("Porta oberta!");
        Destroy(gameObject); // La porta desapareix
        
    }
}
