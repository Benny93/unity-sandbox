using UnityEngine;

namespace com.emptystate
{
    public class PropSpawner : MonoBehaviour
    {
        public PrefabDatabase prefabDatabase;
        public bool enableLogging = true; // Property to enable or disable logging
        public LayerMask raycastLayerMask = ~0; // Layer mask for raycasting
        public float raycastDistance = 10f; // Max distance for raycasting
        public float spawnOffset = 0.2f; // Offset from the hit point to spawn the object

        // Sound related variables
        public AudioClip spawnSound; // Sound to play on spawn
        private AudioSource audioSource;
        private PropData currentSelectedProp;

        void Start()
        {
            // Check if PrefabDatabase is assigned
            if (prefabDatabase == null)
            {
                Debug.LogError("PrefabDatabase is not assigned to PropSpawner!");
                return;
            }

            // Initialize the AudioSource component
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                // If AudioSource component is not present, add one
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        public void SpawnRandomObject()
        {
            PropData propData = GetRandomPropData();
            SpawnObject(propData);
            
        }

        public void SpawnSelected()
        {
            if (currentSelectedProp == null) {
                currentSelectedProp = GetRandomPropData();
            }
            SpawnObject(currentSelectedProp);
        }

        public void DestroyAllSpawned()
        {
            SpawnedProp[] spawned = FindObjectsOfType<SpawnedProp>();
            foreach (SpawnedProp item in spawned)
            {
                Destroy(item.gameObject);
            }
        }

        internal void SpawnObject(PropData propData)
        {
            // TODO add api with only prop id

            // Raycast forward from the transform position
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance, raycastLayerMask))
            {
                // Instantiate the object at the hit point plus offset

                if (propData.Object != null)
                {

                    // Calculate the rotation to look towards the spawner
                    var lookPos = transform.position - hit.point; 
                    lookPos.y = 0; 
                    var rotation = Quaternion.LookRotation(lookPos);                
                    

                    Vector3 spawnPosition = hit.point + hit.normal * spawnOffset;
                    var go = Instantiate(propData.Object, spawnPosition, rotation) as GameObject;

                    var sp = go.AddComponent<SpawnedProp>();
                    sp.ID = propData.ID;
                    Debug.Log($"Spawned object: {propData.Name} at {spawnPosition}");
                    // Play the spawn sound
                    PlaySpawnSound();
                }
                else
                {
                    Debug.LogWarning($"Object in PropData is null.");
                }
            }
            else
            {
                Debug.LogWarning("No valid hit point found for spawning.");
            }
        }

        internal void SetSelectedProp(PropData selectedPropData)
        {
            currentSelectedProp = selectedPropData;
        }

        private PropData GetRandomPropData()
        {
            // Check if Props array is not empty
            if (prefabDatabase.Props.Length > 0)
            {
                // Get a random index
                int randomIndex = Random.Range(0, prefabDatabase.Props.Length);

                // Return the PropData at the random index
                return prefabDatabase.Props[randomIndex];
            }
            else
            {
                Debug.LogWarning("Props array is empty in PrefabDatabase.");
                return null;
            }
        }

        private void PlaySpawnSound()
        {
            // Check if a spawn sound is assigned
            if (spawnSound != null && audioSource != null)
            {
                // Play the spawn sound
                audioSource.PlayOneShot(spawnSound);
            }
        }

        // Draw a visible ray with Gizmos
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.forward * raycastDistance);
        }
    }
}
