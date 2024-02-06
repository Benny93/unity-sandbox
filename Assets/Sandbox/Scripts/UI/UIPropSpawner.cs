using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

namespace com.emptystate
{
    public class UIPropSpawner : MonoBehaviour
    {
        public ScrollRect scrollView;
        public GameObject buttonPrefab;
        public PrefabDatabase prefabDatabase;
        public PropSpawner spawner;
        public TextMeshProUGUI CurrentSelectedText;
        public Button DeleteAllButton;

        void Start()
        {
            // Populate the ScrollView with PropData buttons
            PopulateScrollView();
            UpdateSelectedText("Random");

            DeleteAllButton.onClick.AddListener(() => HandleDeleteAllClick());
        }

        private void HandleDeleteAllClick()
        {
            spawner.DestroyAllSpawned();
        }

        void PopulateScrollView()
        {
            // Clear existing content
            foreach (Transform child in scrollView.content)
            {
                Destroy(child.gameObject);
            }

            // Get all PropData items from the PrefabDatabase
            foreach (PropData propData in prefabDatabase.Props)
            {
                
                // Create a button for each PropData item
                GameObject button = Instantiate(buttonPrefab, scrollView.content);
                var buttonC = button.GetComponent<UIScrollViewButtonWithImage>();
                buttonC.SetText(propData.Name);
                buttonC.SetImageSprite(propData.Image);

                // Add a listener to handle button click
                button.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(propData));
                
            }
        }

        void OnButtonClick(PropData selectedPropData)
        {
            if (selectedPropData.Object != null)
            {
                spawner.SetSelectedProp(selectedPropData);
                UpdateSelectedText(selectedPropData.Name);
            }
            else
            {
                Debug.LogWarning($"Object in selected PropData is null.");
            }
        }

        private void UpdateSelectedText(string name)
        {
            CurrentSelectedText.text = name;
        }
    }
}
