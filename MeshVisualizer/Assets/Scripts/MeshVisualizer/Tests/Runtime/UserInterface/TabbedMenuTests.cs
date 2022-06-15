using NUnit.Framework;
using UnityEngine.UIElements;

namespace Scene.Main.UserInterface {
    public class TabbedMenuTests : UserInterfaceTestBase {

        [Test]
        public void MouseOver_Tab_DisplaysContent() {
            var root = uiDocument.rootVisualElement;
            
            root
                .Query<Button>(className: tabClassName)
                .ForEach(tab => {
                    var contentContainerName = tabbedMenu.GetContentContainerName(tab);
                    var contentContainer = root.Q<VisualElement>(contentContainerName);
                    contentContainer.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                    
                    MouseOver(tab);
                    
                    Assert.That(contentContainer.style.display.value == DisplayStyle.Flex);
                });
        }
        
        [Test]
        public void MouseOver_Tab_DisplaysPanel() {
            var root = uiDocument.rootVisualElement;

            var panel = root.Q<VisualElement>(panelName);
            
            root
                .Query<Button>(className: tabClassName)
                .ForEach(tab => {
                    panel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                    
                    MouseOver(tab);

                    Assert.That(panel.style.display.value == DisplayStyle.Flex);
                });
        }
        
        [Test]
        public void MouseOver_Screen_HidesPanel() {
            var root = uiDocument.rootVisualElement;

            var panel = root.Q<VisualElement>(panelName);
            var screen = root.Q<VisualElement>(screenName);
            var firstTab = root.Q<Button>(className: tabClassName);
            
            //Display Panel (needs to enable 'previewingPanel')
            MouseOver(firstTab);
            
            MouseOver(screen);
            
            Assert.That(panel.style.display.value == DisplayStyle.None);
        }
        
        [Test]
        public void MouseOver_Spacer_HidesPanel() {
            var root = uiDocument.rootVisualElement;

            var panel = root.Q<VisualElement>(panelName);
            var firstTab = root.Q<Button>(className: tabClassName);
            
            root
                .Query<VisualElement>(spacerName)
                .ForEach(spacer => {
                    //Display Panel (needs to enable 'previewingPanel')
                    MouseOver(firstTab);
            
                    MouseOver(spacer);
                
                    Assert.That(panel.style.display.value == DisplayStyle.None);
            });
            
        }
    }
}