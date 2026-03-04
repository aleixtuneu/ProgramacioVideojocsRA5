using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Transform _keyEquippedPosition; // Posició on apareixerà la clau (mà, cintura, etc)
    [SerializeField] private bool _destroyKeyOnDoorOpen = false; // Destruir la clau quan obres la porta

    private bool _keyCollected = false;
    private GameObject _equippedKey;
    private bool _gameEnded = false;

    public bool IsKeyCollected => _keyCollected;
    public bool IsGameEnded => _gameEnded;
    public GameObject EquippedKey => _equippedKey;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Ja existia una instància de GameManager! Eliminant la duplicada.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("GameManager inicialitzat correctament!");
    }

    private void Start()
    {
        // Mostrar l'estat inicial
        Debug.Log($"GameManager Start - KeyCollected: {_keyCollected}");

        if (_keyEquippedPosition == null)
        {
            Debug.LogWarning("No s'ha assignat _keyEquippedPosition! La clau equipada no es veurà.");
        }
    }

    /// <summary>
    /// Recull la clau i l'equipa al personatge
    /// </summary>
    public void CollectKey(GameObject keyGameObject)
    {
        Debug.Log($"CollectKey cridat. Estat anterior: {_keyCollected}");

        if (_keyCollected)
        {
            Debug.LogWarning("La clau ja ha estat recollida!");
            return;
        }

        _keyCollected = true;
        Debug.Log("Clau recollida!");
        Debug.Log($"Nou estat - KeyCollected: {_keyCollected}");

        // Equipa la clau al personatge ABANS de destruir l'original
        EquipKey(keyGameObject);

        // Destruir la clau del mapa
        Destroy(keyGameObject);
    }

    /// <summary>
    /// Equipa la clau al personatge (la mostra a la mà o prop del cos)
    /// </summary>
    private void EquipKey(GameObject originalKey)
    {
        if (_keyEquippedPosition == null)
        {
            Debug.LogWarning("No s'ha assignat _keyEquippedPosition! No es pot mostrar la clau equipada.");
            return;
        }

        // Crear una còpia de la clau per mostrar-la equipada
        _equippedKey = Instantiate(originalKey, _keyEquippedPosition);

        // Desactivar scripts de física i col·lisió
        Collider[] colliders = _equippedKey.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        // Desactivar el script Key si existeix
        Key keyScript = _equippedKey.GetComponent<Key>();
        if (keyScript != null)
        {
            keyScript.enabled = false;
        }

        // Desactivar animacions si existeixen
        Animator animator = _equippedKey.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }

        // Desactivar Rigidbody si existeix
        Rigidbody rb = _equippedKey.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Debug.Log("Clau equipada al personatge!");
    }

    /// <summary>
    /// Obrir la porta si el jugador té la clau
    /// </summary>
    public bool TryOpenDoor()
    {
        Debug.Log($"TryOpenDoor cridat. Clau recollida: {_keyCollected}");

        if (!_keyCollected)
        {
            Debug.Log("No tens la clau! No pots obrir la porta.");
            return false;
        }

        Debug.Log("Porta oberta!");

        // Destruir la clau equipada si es demana
        if (_destroyKeyOnDoorOpen && _equippedKey != null)
        {
            Destroy(_equippedKey);
            Debug.Log("Clau destruïda!");
        }

        return true;
    }

    /// <summary>
    /// Acabar el joc (victòria)
    /// </summary>
    public void EndGame()
    {
        if (_gameEnded)
        {
            Debug.LogWarning("El joc ja havia acabat!");
            return;
        }

        _gameEnded = true;
        Debug.Log("Joc Acabat! Victòria!");
        Time.timeScale = 0f; // Pausar el joc
    }

    /// <summary>
    /// Reiniciar el joc
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        _keyCollected = false;
        _gameEnded = false;

        if (_equippedKey != null)
        {
            Destroy(_equippedKey);
        }

        Debug.Log("Joc reiniciat!");
    }
}