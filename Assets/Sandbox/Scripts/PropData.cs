using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.emptystate
{

    public class ScriptableObjectIdAttribute : PropertyAttribute { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ScriptableObjectIdAttribute))]
    public class ScriptableObjectIdDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;

            Object owner = property.serializedObject.targetObject;
            // This is the unity managed GUID of the scriptable object, which is always unique :3
            string unityManagedGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(owner));

            if (property.stringValue != unityManagedGuid)
            {
                property.stringValue = unityManagedGuid;
            }
            EditorGUI.PropertyField(position, property, label, true);

            GUI.enabled = true;
        }
    }
#endif



    [CreateAssetMenu(fileName = "New PropData", menuName = "ScriptableObjects/PropData")]
    public class PropData : ScriptableObject
    {
        [ScriptableObjectId]
        public string ID;
        public Object Object;
        public string Name;
        public List<string> Tags;
        public Sprite Image; 
    }
}
