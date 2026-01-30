using Runtime.System.InputSystem;
using UnityEngine;

namespace Runtime.Player
{
    public class CameraController : MonoBehaviour
    {
        public Transform rotateObject;
        public float minX = -60f;
        public float maxX = 60f;
        public float sensitivity = 2f;
        public float rotationSpeed = 2f;
        public float tiltAngle = 10f; // Maksimum tilt açısı
        public float tiltSpeed = 5f; // Tilt hareket hızı

        private float rotY;
        private float rotX;
        private float currentTilt = 0f;


        void Update()
        {
            if (InputManager.Instance.blockLookInput) return;

            RotateCamera();
            ApplyTilt();
        }

        void RotateCamera()
        {
            Vector2 lookInput = InputManager.Instance.GetLookInput();
            rotY += lookInput.x * sensitivity;
            rotX += lookInput.y * sensitivity;
            rotX = Mathf.Clamp(rotX, minX, maxX);

            Quaternion targetRotation = Quaternion.Euler(-rotX, rotY, 0);
            rotateObject.rotation =
                Quaternion.Slerp(rotateObject.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        void ApplyTilt()
        {
            Vector2 moveInput = InputManager.Instance.GetMovementInput();

            float targetTilt = 0f;

            if (moveInput.x > 0) targetTilt = -tiltAngle; // Sağ hareket → sağa eğ
            else if (moveInput.x < 0) targetTilt = tiltAngle; // Sol hareket → sola eğ

            currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);

            rotateObject.localRotation = Quaternion.Euler(-rotX, rotY, currentTilt);
        }
    }
}