using UnityEngine;
using UnityEditor;

namespace com.emptystate.Editor
{
    [CustomEditor(typeof(PropSpawner))]
    public class PropSpawnerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();

            // Add a button to invoke SpawnRandomObject
            if (GUILayout.Button("Spawn Random Object"))
            {
                PropSpawner propSpawner = (PropSpawner)target;
                propSpawner.SpawnRandomObject();
            }

            if (GUILayout.Button("Destroy all spawned"))
            {
                PropSpawner propSpawner = (PropSpawner)target;
                propSpawner.DestroyAllSpawned();
            }
        }
    }
}
