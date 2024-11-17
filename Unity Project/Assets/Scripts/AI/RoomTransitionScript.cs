using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class RoomTransition : MonoBehaviour
{
    public string sceneToLoad; // Name of the scene to load
    public float fadeDuration = 1f; // Duration of the fade effect
    public Image fadeImage; // Reference to a black screen Image to use when fading into next room

    private void Start()
    {
        // Set the fade image to be completely transparent at the start
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    // Triggered when another collider enters the trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Get the WaveSpawnnerScript instance to check if the room is complete
            WaveSpawnnerScript waveSpawner = FindObjectOfType<WaveSpawnnerScript>();

            if (waveSpawner != null && waveSpawner.isRoomComplete)
            {
                // If room is complete, start the transition
                StartCoroutine(TransitionToRoom());
            }
            else
            {
                Debug.Log("Room not complete. Defeat all enemies first.");
            }
        }
    }

    private IEnumerator TransitionToRoom()
    {
        //Make the player and enemies inactive or stop movement so no actions are done while transitioning
        Time.timeScale = 0; // Stop all movement
        fadeImage.color = new Color(0, 0, 0, 0); // Reset to fully transparent

        // Fade to black
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            float normalizedTime = t / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, normalizedTime);
            yield return null;
        }
        fadeImage.color = Color.black; // Ensure it's fully black

        // Load the new scene
        SceneManager.LoadScene(sceneToLoad);

        // Optional: Restore time scale
        Time.timeScale = 1; // Resume normal time
    }
}

