using UnityEngine;
using UnityEngine.UI;

namespace com.emptystate
{
    [RequireComponent(typeof(Button))]
    public class UIButtonSound : MonoBehaviour
    {
        private Button button;
        private AudioSource audioSource;

        [SerializeField] private AudioClip clickSound;

        void Start()
        {
            // Get or add the Button component
            button = GetComponent<Button>();

            // Ensure there's an AudioSource component
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                // Set 2D spatial settings
                audioSource.spatialBlend = 0.0f;
            }

            // Attach the method to the button's click event
            button.onClick.AddListener(PlayButtonClickSound);
        }

        private void PlayButtonClickSound()
        {
            if (clickSound != null && audioSource != null)
            {
                // Play the assigned click sound
                audioSource.PlayOneShot(clickSound);
            }
        }
    }
}
