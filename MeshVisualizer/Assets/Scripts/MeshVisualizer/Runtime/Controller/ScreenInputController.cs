using MeshVisualizer.Input;
using UnityEngine;

namespace MeshVisualizer.Controller {
    public class ScreenInputController : MonoBehaviour {
        [SerializeField] private CameraController cameraController;

        [SerializeField] private ModelTransformController transformController;

        [Range(1, 100)] [SerializeField] private float rotationSpeed = 10f;

        private Camera activeCamera => cameraController.activeCamera;

        public void OnScreenDrag(Vector2 pointerDelta) {
            Vector2 currentPointerScreenPosition = ScreenInputManager.Instance.pointerScreenPosition;
            Vector2 lastPointerScreenPosition = currentPointerScreenPosition - pointerDelta;

            //TranslateModel(lastPointerScreenPosition, currentPointerScreenPosition);
            RotateModel(lastPointerScreenPosition, currentPointerScreenPosition);
        }


        /// <summary>
        /// Moves the model along a plane by the same amount between two points on the screen
        /// </summary>
        private void TranslateModel(Vector2 lastScreenPoint, Vector2 currentScreenPoint) {
            //The conceptional idea behind this function is to take two points on a plane, the previous point and the
            //current point and to move the model by the difference between them, along the plane, regardless of the model's
            //position in the world.

            //Create a plane that is facing the camera, and positioned at the model
            Plane viewPlane = new Plane(-activeCamera.transform.forward, transformController.transform.position);

            //Gets the last position of the pointer on the plane
            Vector3 lastWorldPoint = GetPointOnPlane(viewPlane, lastScreenPoint);
            //Gets the current position of the pointer on the plane
            Vector3 currentWorldPoint = GetPointOnPlane(viewPlane, currentScreenPoint);

            //Gets the delta between the points
            Vector3 worldDelta = currentWorldPoint - lastWorldPoint;

            //Because worldDelta is a delta between 2 points on a plane, the delta will be a vector3 that runs along the plane
            //This will move the model by that same amount
            transformController.TranslateWithConstraints(worldDelta);
        }

        /// <summary>
        /// Rotates the model using the axis and angle between two points on the screen
        /// </summary>
        private void RotateModel(Vector2 lastScreenPoint, Vector2 currentScreenPoint) {
            var lastWorldPoint = activeCamera
                .ScreenToWorldPoint(new Vector3(lastScreenPoint.x, lastScreenPoint.y, 1));
            lastWorldPoint -= activeCamera.transform.position;
            
            var currentWorldPoint = activeCamera
                .ScreenToWorldPoint(new Vector3(currentScreenPoint.x, currentScreenPoint.y, 1));
            currentWorldPoint -= activeCamera.transform.position;

            //Get the axis for the rotation
            Vector3 axis = Vector3.Cross(currentWorldPoint, lastWorldPoint).normalized;
            //Transform the axis to be along the model's current rotation
            axis = Quaternion.Inverse(transformController.transform.localRotation) * axis;

            //Get the angle difference between the two points
            float angle = Vector3.Angle(currentWorldPoint, lastWorldPoint) * rotationSpeed;

            //Rotate the model
            transformController.Rotate(axis, angle);
        }

        /// <summary>
        /// From a point on the screen, returns a point on the plane
        /// </summary>
        private Vector3 GetPointOnPlane(Plane plane, Vector2 screenPoint) {
            Ray screenPointRay = activeCamera.ScreenPointToRay(screenPoint);

            if (plane.Raycast(screenPointRay, out float distanceFromCamera)) {
                Vector3 positionOnPlane = screenPointRay.GetPoint(distanceFromCamera);
                return positionOnPlane;
            }

            Debug.LogError($"Error: {this} ray {screenPointRay} cast did not hit the plane {plane}");
            return Vector3.zero;
        }
    }
}
