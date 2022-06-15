using System.Collections;
using MeshVisualizer.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scene.Main.UserInterface {
    public class TransformPanelTests : UserInterfaceTestBase {
        
        private const string positionResetButtonName = "position-reset-button";
        private const string rotationResetButtonName = "rotation-reset-button";
        private const string scaleResetButtonName = "scale-reset-button";
        
        private UITransformPanel testObject { get; set; }
        
        protected override IEnumerator Setup() {
            yield return base.Setup();
            
            var panelGameObject = GameObject.Find("Transform Panel");
            testObject = panelGameObject.GetComponent<UITransformPanel>();
        }

        [Test]
        public void OnDisplay_Sliders_UpdateValues() {
            testObject.contentContainer
                      .Query<Slider>()
                      .ForEach(slider => {
                          slider.value = slider.lowValue;
                      });

            var target = testObject.transformController.transform;
            target.localPosition = Vector3.one;
            target.localRotation = Quaternion.Euler(Vector3.up*90);
            target.localScale = Vector3.one * 2;
            
            SendDisplayEvent(testObject.contentContainer);
            
            testObject.contentContainer
                      .Query<Slider>()
                      .ForEach(slider => {
                          Assert.AreNotEqual(slider.lowValue, slider.value);
                      });
        }
        
        [Test]
        public void OnValueChange_Sliders_UpdateTargetTransform() {
            var target = testObject.transformController.transform;
            Vector3 startingPosition = target.localPosition;
            Quaternion startingRotation = target.localRotation;
            Vector3 startingScale = target.localScale;
            
            testObject.contentContainer
                      .Query<Slider>()
                      .ForEach(slider => {
                          slider.value = 0.5f;
                          var changeEvent = ChangeEvent<float>.GetPooled(0, 0.5f);
                          changeEvent.target = slider;
                          slider.SendEvent(changeEvent);
                      });

            //Assert that position has changed
            float mag = Vector3.SqrMagnitude(target.localPosition - startingPosition);
            Assert.That(!Mathf.Approximately(mag, 0), $"Expected {target.localPosition} position to change but it did not");
            
            //Assert that rotation has changed
            Assert.That(startingRotation != target.localRotation, 
                $"Expected {target.localRotation} rotation to change but it did not");
            
            //Assert that scale has changed
            mag = Vector3.SqrMagnitude(target.localScale - startingScale);
            Assert.That(!Mathf.Approximately(mag, 0), $"Expected {target.localScale} scale to change but it did not");
        }

        [Test]
        public void OnClick_PositionResetButton_ResetsModelPosition() {
            testObject.transformController.transform.localPosition = Vector3.one;

            var resetButton = testObject.contentContainer.Q<Button>(positionResetButtonName);
            
            SendClickEvent(resetButton);
            
            float mag = Vector3.SqrMagnitude(testObject.transformController.transform.localPosition);
            Assert.That(Mathf.Approximately(mag, 0), $"Expected {Vector3.zero} got {testObject.transformController.transform.localPosition}");
        }
        
        [Test]
        public void OnClick_RotationResetButton_ResetsModelRotation() {
            testObject.transformController.transform.localRotation = Quaternion.Euler(Vector3.up * 90);

            var resetButton = testObject.contentContainer.Q<Button>(rotationResetButtonName);
            
            SendClickEvent(resetButton);
            
            Assert.That(Quaternion.identity == testObject.transformController.transform.localRotation, 
                $"Expected {Quaternion.identity} got {testObject.transformController.transform.localRotation}");
        }
        
        [Test]
        public void OnClick_ScaleResetButton_ResetsModelScale() {
            testObject.transformController.transform.localScale = Vector3.one * 2;

            var resetButton = testObject.contentContainer.Q<Button>(scaleResetButtonName);
            
            SendClickEvent(resetButton);
            
            float mag = Vector3.SqrMagnitude(testObject.transformController.transform.localScale - Vector3.one);
            Assert.That(Mathf.Approximately(mag, 0), $"Expected {Vector3.one} got {testObject.transformController.transform.localScale} with magnitude of {mag}");
        }
    }
}
