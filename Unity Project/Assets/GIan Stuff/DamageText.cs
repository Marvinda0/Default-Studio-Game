using UnityEngine;
using TMPro; // Only needed if using TextMeshPro

public class DamageText : MonoBehaviour
{
    public float moveSpeed = 1f; // Speed at which the text moves upward
    public float fadeDuration = 1f; // Time it takes to fade out
    private TextMeshProUGUI textMesh; // For TextMeshPro
    private Color textColor;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textColor = textMesh.color; // Save the initial color
        Destroy(gameObject, fadeDuration); // Destroy the object after fading out
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime); // Move upward
        textColor.a -= Time.deltaTime / fadeDuration; // Fade out over time
        textMesh.color = textColor; // Apply the color change
    }

    public void SetText(string damageAmount)
    {
        if (textMesh != null)
        {
            textMesh.text = damageAmount;
        }
    }
}

