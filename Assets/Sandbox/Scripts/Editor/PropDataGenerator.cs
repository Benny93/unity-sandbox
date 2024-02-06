using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.emptystate.Editor
{
    public class PropDataGenerator : EditorWindow
    {
        private string selectedFolder = "Assets/Prefabs"; // Default source folder path
        private string targetDirectory = "Assets/Sandbox/PropData"; // Default target directory
        private Sprite defaultSprite; // Default sprite to be used as an image for all created PropData

        [MenuItem("Tools/Generate PropData from Prefabs")]
        public static void ShowWindow()
        {
            GetWindow<PropDataGenerator>("PropData Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("PropData Generator", EditorStyles.boldLabel);

            // Display the selected source folder path
            EditorGUILayout.LabelField("Selected Source Folder:", selectedFolder);

            // Button to open the source folder selection dialog
            if (GUILayout.Button("Select Source Folder"))
            {
                string folderPath = EditorUtility.OpenFolderPanel("Select Source Folder for Prefabs", "Assets", "");
                if (!string.IsNullOrEmpty(folderPath))
                {
                    selectedFolder = MakePathRelativeToProject(folderPath);
                }
            }

            // Display the selected target directory
            EditorGUILayout.LabelField("Selected Target Directory:", targetDirectory);

            // Button to open the target directory selection dialog
            if (GUILayout.Button("Select Target Directory"))
            {
                string folderPath = EditorUtility.OpenFolderPanel("Select Target Directory for PropData", "Assets", "");
                if (!string.IsNullOrEmpty(folderPath))
                {
                    targetDirectory = MakePathRelativeToProject(folderPath);
                }
            }

            // Field to set the default sprite
            defaultSprite = EditorGUILayout.ObjectField("Default Sprite:", defaultSprite, typeof(Sprite), false) as Sprite;

            // Button to generate PropData from Prefabs
            if (GUILayout.Button("Generate PropData from Prefabs"))
            {
                GeneratePropData();
            }

            // Button to add all PropData in the target folder to the specified PrefabDatabase
            if (GUILayout.Button("Add PropData to PrefabDatabase"))
            {
                AddPropDataToPrefabDatabase();
            }

        }

        private void GeneratePropData()
        {
            // Get all prefabs in the specified source folder
            string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab", new[] { selectedFolder });
            foreach (var prefabPath in prefabPaths)
            {
                string fullPath = AssetDatabase.GUIDToAssetPath(prefabPath);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(fullPath);

                if (prefab != null)
                {
                    CreatePropData(prefab);
                }
            }

            Debug.Log("PropData generation complete!");
        }

        private void CreatePropData(GameObject prefab)
        {
            // Create a new PropData asset
            PropData propData = ScriptableObject.CreateInstance<PropData>();

            // Set properties of the PropData
            propData.ID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(prefab));
            propData.Object = prefab;
            propData.Name = prefab.name; // You may want to customize this based on your requirements
            // Set other properties as needed

            // Set the default sprite if it's not null
            if (defaultSprite != null)
            {
                propData.Image = defaultSprite;
            }

            // Create a new asset for PropData in the specified target directory
            string assetPath = $"{targetDirectory}/PropData_{prefab.name}.asset";
            AssetDatabase.CreateAsset(propData, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void AddPropDataToPrefabDatabase()
        {
            // Get the specified PrefabDatabase
            PrefabDatabase prefabDatabase = Selection.activeObject as PrefabDatabase;
            if (prefabDatabase == null)
            {
                Debug.LogError("Please select a PrefabDatabase asset in the Project window.");
                return;
            }

            // Get all PropData assets in the specified target directory
            string[] propDataPaths = AssetDatabase.FindAssets("t:PropData", new[] { targetDirectory });
            List<PropData> propDataList = new List<PropData>();

            foreach (var propDataPath in propDataPaths)
            {
                string fullPath = AssetDatabase.GUIDToAssetPath(propDataPath);
                PropData propData = AssetDatabase.LoadAssetAtPath<PropData>(fullPath);

                if (propData != null)
                {
                    propDataList.Add(propData);
                }
            }

            // Assign the PropData assets to the PrefabDatabase
            prefabDatabase.Props = propDataList.ToArray();

            // Save changes to the PrefabDatabase
            EditorUtility.SetDirty(prefabDatabase);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("PropData added to PrefabDatabase.");
        }

        private string MakePathRelativeToProject(string fullPath)
        {
            return fullPath.Replace(Application.dataPath, "Assets");
        }
    }
}
