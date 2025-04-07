using UnityEngine;

public class AudioManager : MonoBehaviour {
   public static AudioManager Instance { get; private set; }

   [SerializeField] private AudioSource audioSource;

   private void Awake() {
      if (Instance && Instance != this) {
         Destroy(gameObject);
         return;
      }

      Instance = this;
      DontDestroyOnLoad(gameObject);

      audioSource.Play();
   }
}