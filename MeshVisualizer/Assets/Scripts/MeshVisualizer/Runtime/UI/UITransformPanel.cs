using UnityEngine.Events;
using UnityEngine.UIElements;

namespace MeshVisualizer.UI {
    public class UITransformPanel : UIPanel {
        private const string horizontalPositionSliderName = "horizontal-position-slider";
        private const string verticalPositionSliderName = "vertical-position-slider";
        private const string scaleSliderName = "scale-slider";

        public UnityAction<float> onHorizontalPositionChanged;
        public UnityAction<float> onVerticalPositionChanged;
        public UnityAction<float> onScaleChanged;

        private void Start() {
            Slider horizontalSlider = contentContainer.Q<Slider>(horizontalPositionSliderName);
            horizontalSlider.RegisterValueChangedCallback(OnHorizontalPositionSliderChanged);
            
            Slider verticalSlider = contentContainer.Q<Slider>(verticalPositionSliderName);
            verticalSlider.RegisterValueChangedCallback(OnVerticalPositionSliderChanged);
            
            Slider scaleSlider = contentContainer.Q<Slider>(scaleSliderName);
            scaleSlider.RegisterValueChangedCallback(OnScaleSliderChanged);
        }


        private void OnHorizontalPositionSliderChanged(ChangeEvent<float> changeEvent) {
            onHorizontalPositionChanged.Invoke(changeEvent.newValue);
        }

        private void OnVerticalPositionSliderChanged(ChangeEvent<float> changeEvent) {
            onVerticalPositionChanged.Invoke(changeEvent.newValue);
        }
        
        private void OnScaleSliderChanged(ChangeEvent<float> changeEvent) {
            onScaleChanged.Invoke(changeEvent.newValue);
        }
    }
}
