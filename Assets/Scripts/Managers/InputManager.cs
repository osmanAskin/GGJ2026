using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Runtime.System.InputSystem
{
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        public bool blockLookInput = false;
        public bool blockMovementInput = false;


        public Vector2 GetMovementInput()
        {
            float moveX = Input.GetAxis("Horizontal"); // A, D
            float moveZ = Input.GetAxis("Vertical"); // W, S
            return new Vector2(moveX, moveZ);
        }

        public bool IsRunning()
        {
            return Input.GetKey(KeyCode.LeftShift);
        }

        public bool IsJumping()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }


        public Vector2 GetLookInput()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            return new Vector2(mouseX, mouseY);
        }

        public void BlockLookInput(bool block)
        {
            blockLookInput = block;
        }


        public bool IsLeftClick()
        {
            return Input.GetMouseButtonDown(0);
        }
    }
}