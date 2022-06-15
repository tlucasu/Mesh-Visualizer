using System;
using UnityEngine;

namespace MeshVisualizer.Controller {
    public class ModelTransformController : MonoBehaviour {

        [Header("Local Position Constraints")] [InspectorName("X Axis")]
        public MinMax xPositionConstraint = new MinMax(-1, 1);

        [InspectorName("Y Axis")] public MinMax yPositionConstraint = new MinMax(-1, 1);
        [InspectorName("Z Axis")] public MinMax zPositionConstraint = new MinMax(-1, 1);


        public void SetLocalPositionWithConstraints(Vector3 position) {
            //Local position is used to allow for the model anchor to be nested in another
            //game object and be localed else where in the world 
            transform.localPosition = ClampPosition(position);
        }

        public void TranslateWithConstraints(Vector3 translation) {
            transform.Translate(translation);
            transform.localPosition = ClampPosition(transform.localPosition);
        }

        private Vector3 ClampPosition(Vector3 position) {
            return new Vector3() {
                x = Mathf.Clamp(position.x, xPositionConstraint.min, xPositionConstraint.max),
                y = Mathf.Clamp(position.y, yPositionConstraint.min, yPositionConstraint.max),
                z = Mathf.Clamp(position.z, zPositionConstraint.min, zPositionConstraint.max),
            };
        }

        public void SetLocalRotation(Vector3 newRotation) {
            transform.localRotation = Quaternion.Euler(newRotation);
        }

        public void Rotate(Vector3 axis, float angle) {
            transform.Rotate(axis, angle);
        }

        public void SetLocalScale(float scale) {
            transform.localScale = Vector3.one * scale;
        }

        [Serializable]
        public struct MinMax {
            public float max;
            public float min;

            public MinMax(float min, float max) {
                this.min = min;
                this.max = max;
            }
        }
    }
}