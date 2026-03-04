using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collisió detectada amb: " + other.gameObject.name);
        Debug.Log("Tag del objecte: " + other.tag);

        if (other.CompareTag("Player"))
        {
            Debug.Log("És el Player! Clau recollida!");

            if (GameManager.Instance != null)
            {
                Debug.Log("GameManager trobat!");
                // GameManager per recollir la clau
                GameManager.Instance.CollectKey(gameObject);
            }
            else
            {
                Debug.LogError("ERROR: GameManager NO ha estat trobat a la escena!");
                // Destruir la clau de totes maneres
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("No és el Player, és: " + other.gameObject.name);
        }
    }
}
