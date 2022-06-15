using System.Collections;
using MeshVisualizer.Runtime.UI;
using MeshVisualizer.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Scene.Main.UserInterface {
    public class UserInterfaceTestBase {
        protected const string tabClassName = "tab-button";
        protected const string panelName = "panel";
        protected const string screenName = "screen";
        protected const string spacerName = "spacer";
        
        protected UIDocument uiDocument { get; set; }
        protected UITabbedMenu tabbedMenu { get; set; }
        
        [UnitySetUp]
        protected virtual IEnumerator Setup() {
            SceneManager.LoadScene("Scenes/Main");
            
            yield return null;
            
            var menuGameObject = GameObject.FindGameObjectWithTag("Menu");
            tabbedMenu = menuGameObject.GetComponent<UITabbedMenu>();
            uiDocument = menuGameObject.GetComponent<UIDocument>();
        }
        
        protected void MouseOver(VisualElement element) {
            var mouseOverEvent = MouseOverEvent.GetPooled();
            mouseOverEvent.target = element;
            element.SendEvent(mouseOverEvent);
        }
        
        protected void SendDisplayEvent(VisualElement element) {
            var mouseOverEvent = DisplayEvent.GetPooled();
            mouseOverEvent.target = element;
            element.SendEvent(mouseOverEvent);
        }
        
        protected void SendClickEvent(VisualElement element) {
            var mouseOverEvent = ClickEvent.GetPooled();
            mouseOverEvent.target = element;
            element.SendEvent(mouseOverEvent);
        }
    }
}