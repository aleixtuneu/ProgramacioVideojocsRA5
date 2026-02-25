using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FinalZone : MonoBehaviour
{
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activated)
        {
            activated = true;
            Debug.Log("Ha entrat! Animació de ball!");

            // Obtenir Animator del peronatge
            Animator animator = other.GetComponent<Animator>();
            if (animator != null)
            {
                // Animació de ball
                animator.SetBool("IsDancing", true);
            }

            StartCoroutine(PauseAfter(3f));
        }
    }

    private IEnumerator PauseAfter(float delay) 
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Joc Acabat!");
        Time.timeScale = 0f;
    }
}
