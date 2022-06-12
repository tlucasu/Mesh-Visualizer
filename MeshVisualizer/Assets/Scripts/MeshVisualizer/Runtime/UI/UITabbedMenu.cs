using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace MeshVisualizer.UI {
    [RequireComponent(typeof(UIDocument))]
    public class UITabbedMenu : MonoBehaviour {
        //UI Element Names
        private const string panelName = "panel";
        private const string panelTitleName = "panel-title";
        
        //Class names
        private const string tabClassName = "tab-button";
        private const string tabSelectedClassName = "selected-tab-button";
        private const string contentContainerClassName = "scrollable-content";

        //Name Suffixes
        private const string tabSuffix = "tab-button";
        private const string contentSuffix = "content-container";

        private VisualElement root { get; set; }
        private VisualElement panel { get; set; }
        private Label panelTitle { get; set; }
        
        private void Awake() {
            var document = GetComponent<UIDocument>();
            if (document == null) {
                Debug.LogError($"Failed to initialize {this}, must contain UIDocument component");
                return;
            }
            
            root = document.rootVisualElement;
            if (root == null) {
                Debug.LogError($"Failed to initialize {this}. UI Document is not configured correctly. " +
                               $"Check that an uxml visual tree asset has been assigned");
                return;
            }

            panelTitle = root.Q<Label>(panelTitleName);
            if (panelTitle == null) {
                Debug.LogError($"Failed to initialize {this}. UI Document does not contain label named '{panelTitleName}'");
                return;
            }
            
            panel = root.Q<VisualElement>(panelName);
            if (panel == null) {
                Debug.LogError($"Failed to initialize {this}. UI Document does not contain '{panelName}'");
                return;
            }
            
            root
                .Query<Button>(className: tabClassName)
                .ForEach(tabButton => {
                    string contentContainerName = GetContentContainerName(tabButton);
                    if (root.Q<VisualElement>(contentContainerName) == null) {
                        Debug.LogError($"Failed to initialize {this}. UI Document does not contain " +
                                       $"'{contentContainerName}' for tab button '{tabButton.name}'");
                        return;
                    }

                    tabButton.RegisterCallback<ClickEvent>(OnTabClicked);
                });
        }

        private void OnTabClicked(ClickEvent evt) {
            if(evt.currentTarget is Button tab) {
                ToggleTab(tab);
            }
        }

        /// <summary>
        /// Toggles the selection of the tab, either off if already selected or selects if not currently selected.
        /// It will also unselect any other tabs currently selected.
        /// </summary>
        /// <param name="tab"></param>
        private void ToggleTab(Button tab) {
            HideAllContentContainers();
            
            if (IsSelected(tab)) {
                //Hide panel
                panel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);

                //Remove selected class from tab
                tab.RemoveFromClassList(tabSelectedClassName);
            }
            else {
                //Remove selected class from previously selected tab
                root.Q<Button>(className: tabSelectedClassName)?.RemoveFromClassList(tabSelectedClassName);
                
                //Display content container
                root.Q<VisualElement>(GetContentContainerName(tab)).style.display 
                    = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);

                //Display panel
                panel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);

                panelTitle.text = tab.text;
                
                tab.AddToClassList(tabSelectedClassName);
            }
        }

        private void HideAllContentContainers() {
            root
                .Query<VisualElement>(className: contentContainerClassName)
                .ForEach(element => {
                    //Hide content container
                    element.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                });
        }

        //Checks to see if the tab contains the selected class name
        private bool IsSelected(VisualElement tab) => tab.ClassListContains(tabSelectedClassName);

        //Returns the contain container name for the provided tab
        private string GetContentContainerName(VisualElement tab) => tab.name.Replace(tabSuffix, contentSuffix);
    }
}