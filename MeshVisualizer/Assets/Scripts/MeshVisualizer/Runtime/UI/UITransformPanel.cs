using MeshVisualizer.Runtime.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace MeshVisualizer.UI {
    /// <summary>
    /// Adds panel to parent UI document under 'panel-content' and initializes the elements. Unity Events are
    /// called when elements on the panel are changed. 
    /// </summary>
    public class UITransformPanel : UIPanel {
        //UXML UI Constants
        private const string xPositionSliderName = "x-position-slider";
        private const string yPositionSliderName = "y-position-slider";
        private const string zPositionSliderName = "z-position-slider";
        
        private const string xRotationSliderName = "x-rotation-slider";
        private const string yRotationSliderName = "y-rotation-slider";
        private const string zRotationSliderName = "z-rotation-slider";
        
        private const string scaleSliderName = "scale-slider";
        
        private const string positionResetButtonName = "position-reset-button";
        private const string rotationResetButtonName = "rotation-reset-button";
        private const string scaleResetButtonName = "scale-reset-button";

        [Header("Target")]
        public ModelTransformController transformController;

        //Positional Sliders
        private Slider xPositionSlider { get; set; }
        private Slider yPositionSlider { get; set; }
        private Slider zPositionSlider { get; set; }
        
        //Rotation Sliders
        private Slider xRotationSlider { get; set; }
        private Slider yRotationSlider { get; set; }
        private Slider zRotationSlider { get; set; }
        
        //Scale Slider
        private Slider scaleSlider { get; set; }
        
        //Initialize Panel
        private void Start() {
            //Initialization failed
            if (contentContainer == null)
                return;
            
            if(transformController == null) {
                Debug.LogError($"Transform Controller not set on {this}. Cannot initialize Transform Panel");
                return;
            }

            contentContainer.RegisterCallback<DisplayEvent>(OnDisplayEvent);
            
            //Register Position Sliders
            xPositionSlider = RegisterSlider(xPositionSliderName, OnPositionSliderChanged);
            yPositionSlider = RegisterSlider(yPositionSliderName, OnPositionSliderChanged);
            zPositionSlider = RegisterSlider(zPositionSliderName, OnPositionSliderChanged);
            
            //Register Rotation Sliders
            xRotationSlider = RegisterSlider(xRotationSliderName, OnRotationSliderChanged);
            yRotationSlider = RegisterSlider(yRotationSliderName, OnRotationSliderChanged);
            zRotationSlider = RegisterSlider(zRotationSliderName, OnRotationSliderChanged);
            
            //Register Scale Slider
            scaleSlider = RegisterSlider(scaleSliderName, OnScaleSliderChanged);

            //Initialize Reset Buttons
            InitializeResetButton(positionResetButtonName, OnPositionResetClicked);
            InitializeResetButton(rotationResetButtonName, OnRotationResetClicked);
            InitializeResetButton(scaleResetButtonName, OnScaleResetClicked);
            
            InitializePositionSliderMinMax();
        }

        private void OnDisplayEvent(DisplayEvent evt) => UpdaterSlideValues();

        private void InitializePositionSliderMinMax() {
            xPositionSlider.lowValue = transformController.xPositionConstraint.min;
            xPositionSlider.highValue = transformController.xPositionConstraint.max;
            
            yPositionSlider.lowValue = transformController.yPositionConstraint.min;
            yPositionSlider.highValue = transformController.yPositionConstraint.max;
            
            zPositionSlider.lowValue = transformController.zPositionConstraint.min;
            zPositionSlider.highValue = transformController.zPositionConstraint.max;
        }

        private void UpdaterSlideValues() {
            SetPositionSliderValues(transformController.transform.localPosition);
            SetRotationSliderValues(transformController.transform.localRotation.eulerAngles);
            SetScaleSliderValue(transformController.transform.localScale.x);
        }

        /// <summary>
        /// Finds the slider by '<paramref name="sliderName"/>' then registers value changed event with '<paramref name="callback"/>'
        /// </summary>
        private Slider RegisterSlider(string sliderName, EventCallback<ChangeEvent<float>> callback) {
            Slider slider = contentContainer.Q<Slider>(sliderName);
            if (slider == null) {
                Debug.LogError($"Unable to find '{sliderName}' in content container. Make sure the correct VisualTreeAsset is assigned {this}.");
                return null;
            }
            slider.RegisterValueChangedCallback(callback);
            return slider;
        }

        /// <summary>
        /// Finds the button by '<paramref name="buttonName"/>' then registers clicked event with '<paramref name="callback"/>'
        /// </summary>
        private void InitializeResetButton(string buttonName, EventCallback<ClickEvent> callback) {
            Button button = contentContainer.Q<Button>(buttonName);
            if (button == null) {
                Debug.LogError($"Unable to find '{buttonName}' in content container. Make sure the correct VisualTreeAsset is assigned {this}.");
                return;
            }
            button.RegisterCallback(callback);
        }


        /// <summary>
        /// Invoked when any position-slider's value is changed
        /// </summary>
        private void OnPositionSliderChanged(ChangeEvent<float> changeEvent) {
            Vector3 newPosition = new () {
                x = xPositionSlider.value,
                y = yPositionSlider.value,
                z = zPositionSlider.value,
            };
            transformController.SetLocalPositionWithConstraints(newPosition);
        }
        
        /// <summary>
        /// Invoked when any rotation-slider's value is changed
        /// </summary>
        /// <param name="changeEvent"></param>
        private void OnRotationSliderChanged(ChangeEvent<float> changeEvent) {
            Vector3 newRotation = new () {
                x = xRotationSlider.value,
                y = yRotationSlider.value,
                z = zRotationSlider.value,
            };
            transformController.SetLocalRotation(newRotation);
        }

        /// <summary>
        /// Invoked when scale-slider's value is changed
        /// </summary>
        private void OnScaleSliderChanged(ChangeEvent<float> changeEvent) {
            transformController.SetLocalScale(changeEvent.newValue);
        }

        /// <summary>
        /// Invoked when position-reset-button is clicked
        /// </summary>
        private void OnPositionResetClicked(ClickEvent evt) {
            SetPositionSliderValues(Vector3.zero);
            transformController.SetLocalPositionWithConstraints(Vector3.zero);
        }
        
        /// <summary>
        /// Invoked when rotation-reset-button is clicked
        /// </summary>
        private void OnRotationResetClicked(ClickEvent evt) {
            SetRotationSliderValues(Vector3.zero);
            transformController.SetLocalRotation(Vector3.zero);
        }
        
        /// <summary>
        /// Invoked when scale-reset-button is clicked
        /// </summary>
        private void OnScaleResetClicked(ClickEvent evt) {
            SetScaleSliderValue(1);
            transformController.SetLocalScale(1);
        }

        /// <summary>
        /// Adjusts the positional slider values on the Transform Panel to the specified values (x,y,z)
        /// without triggering OnValueChanged
        /// </summary>
        /// <param name="values"></param>
        public void SetPositionSliderValues(Vector3 values) {
            xPositionSlider.SetValueWithoutNotify(values.x);
            yPositionSlider.SetValueWithoutNotify(values.y);
            zPositionSlider.SetValueWithoutNotify(values.z);
        }

        /// <summary>
        /// Sets the rotational slider values on the Transform Panel to the specified values (x,y,z)
        /// /// without triggering OnValueChanged
        /// </summary>
        /// <param name="values"></param>
        public void SetRotationSliderValues(Vector3 values) {
            if (values.x >= 180)
                values.x -= 360;
            if (values.y >= 180)
                values.y -= 360;
            if (values.z >= 180)
                values.z -= 360;
            
            xRotationSlider.SetValueWithoutNotify(values.x);
            yRotationSlider.SetValueWithoutNotify(values.y);
            zRotationSlider.SetValueWithoutNotify(values.z);
        }

        /// <summary>
        /// Sets the scale slider value on the Transform Panel to the specified value without triggering OnValueChanged
        /// </summary>
        /// <param name="value"></param>
        public void SetScaleSliderValue(float value) {
            scaleSlider.SetValueWithoutNotify(value);
        }
    }
}
