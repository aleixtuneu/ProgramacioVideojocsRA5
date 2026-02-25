using UnityEngine;

public class KeyAnimation : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float flotationSpeed = 2f;
    public float flotationHeight = 0.5f;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Girar
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Pujar i baixar
        float moviment_y = Mathf.Sin(Time.time * flotationSpeed) * flotationHeight;
        transform.position = initialPosition + Vector3.up * moviment_y;
    }
}
