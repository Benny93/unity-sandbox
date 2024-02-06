using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace com.emptystate
{
    [CreateAssetMenu(fileName = "New PrefabDatabase", menuName = "ScriptableObjects/PrefabDatabase")]
    public class PrefabDatabase : ScriptableObject
    {
        public PropData[] Props;


        public PropData GetRandomPropByTags(params string[] tags)
        {
            // Create a list to store matching props
            List<PropData> matchingProps = new List<PropData>();

            // Find all props matching at least one of the provided tags
            foreach (var prop in Props)
            {
                // Check if any provided tag is present in the prop's Tags list
                if (tags.Any(tag => prop.Tags.Contains(tag)))
                {
                    matchingProps.Add(prop);
                }
            }

            // Check if there are matching props
            if (matchingProps.Count > 0)
            {
                // Return a random prop from the matching props
                return matchingProps[UnityEngine.Random.Range(0, matchingProps.Count)];
            }
            else
            {
                // No matching props found
                Debug.LogWarning($"No props found with any of the tags: {string.Join(", ", tags)}");
                return null;
            }
        }


    }


}