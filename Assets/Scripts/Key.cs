using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collisió detectada amb: " + other.gameObject.name);
        Debug.Log("Tag del objecte: " + other.tag);

        if (other.CompareTag("Player"))
        {
            Debug.Log("És el Player! Clau recollegida!");

            PlayerController.keyCollected = true;
            Destroy(gameObject); // Destruir la clau del mapa
            Debug.Log("Clau recollida!");
        }
        else
        {
            Debug.Log("No és el Player, és: " + other.gameObject.name);
        }
    }
}
