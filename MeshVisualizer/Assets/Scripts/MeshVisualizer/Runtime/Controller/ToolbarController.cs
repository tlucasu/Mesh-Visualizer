using MeshVisualizer.Input;
using UnityEngine;

namespace MeshVisualizer.Controller {
    public class ToolbarController : MonoBehaviour {
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


        private void TranslateModel(Vector2 lastPointerScreenPosition, Vector2 currentPointerScreenPosition) {
            //The conceptional idea behind this function is to take two points on a plane, the previous point and the
            //current point and to move the model by the difference between them, along the plane, regardless of the model's
            //position in the world.

            //Create a plane that is facing the camera, and positioned at the model
            Plane viewPlane = new Plane(-activeCamera.transform.forward, transformController.transform.position);

            //Gets the last position of the pointer on the plane
            Vector3 lastWorldPoint = GetPointOnPlane(viewPlane, lastPointerScreenPosition);
            //Gets the current position of the pointer on the plane
            Vector3 currentWorldPoint = GetPointOnPlane(viewPlane, currentPointerScreenPosition);

            //Gets the delta between the points
            Vector3 worldDelta = currentWorldPoint - lastWorldPoint;

            //Because worldDelta is a delta between 2 points on a plane, the delta will be a vector3 that runs along the plane
            //This will move the model by that same amount
            transformController.TranslateWithConstraints(worldDelta);

            // //Update Transform Panel
            // transformPanel.SetPositionSliderValues(transformController.localPosition);
        }

        private void RotateModel(Vector2 lastPointerScreenPosition, Vector2 currentPointerScreenPosition) {
            //The conceptional idea behind this function is to take two points on a sphere find the angle between them and to rotate the model by that same angle

            Plane viewPlane = new Plane(-activeCamera.transform.forward, transformController.transform.position);
            Vector2 screenCenter = new Vector2(activeCamera.pixelWidth / 2, activeCamera.pixelHeight / 2);
            Vector3 screenCenterWorldPoint = GetPointOnPlane(viewPlane, screenCenter);

            var lastWorldPosition =
                activeCamera.ScreenToWorldPoint(
                    new Vector3(lastPointerScreenPosition.x, lastPointerScreenPosition.y, 1));
            var currentWorldPosition = activeCamera.ScreenToWorldPoint(new Vector3(currentPointerScreenPosition.x,
                currentPointerScreenPosition.y, 1));

            var adjustedLWP = lastWorldPosition - screenCenterWorldPoint;
            var adjustecCurrent = currentWorldPosition - screenCenterWorldPoint;

            Vector3 axis = Vector3.Cross(adjustecCurrent, adjustedLWP).normalized;
            axis = Quaternion.Inverse(transformController.transform.localRotation) * axis;

            float angle = -Vector3.Angle(adjustecCurrent, adjustedLWP) * rotationSpeed;

            transformController.Rotate(axis, angle);
        }

        private Vector3 GetPointOnPlane(Plane plane, Vector2 screenPoint) {
            Ray screenPointRay = activeCamera.ScreenPointToRay(screenPoint);

            if (plane.Raycast(screenPointRay, out float distanceFromCamera)) {
                Vector3 positionOnPlane = screenPointRay.GetPoint(distanceFromCamera);
                return positionOnPlane;
            }

            Debug.LogError("Ray did not hit plane");
            return Vector3.zero;
        }
    }
}
