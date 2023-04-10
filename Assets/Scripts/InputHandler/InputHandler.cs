using Assets.Scripts.InputHandler.Interfaces;
using UnityEngine;

namespace Assets.Scripts.InputHandler
{
    public class InputHandler : MonoBehaviour, IInputHandler
    {
        public bool GetRestartInput() => Input.GetKeyDown(KeyCode.R);
        public bool GetRightMouseButtomDown() => Input.GetMouseButtonDown(1);
        public bool GetLeftMouseButtomDown() => Input.GetMouseButtonDown(0);
    }
}