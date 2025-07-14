using System;
using Core.UI;
using UnityEngine;

namespace Core.Services
{
    public class MobileInputService : IInputService
    {
        private readonly JoystickController _joystickController;
        
        private IInputService _inputService;
        
        Action IInputService.Clicked { get; set; }
        Action<Vector2> IInputService.Dragged { get; set; }
        Action IInputService.Released { get; set; }

        float IInputService.Speed
        {
            get => _joystickController.Speed;
        }

        Vector2 IInputService.InputDirection
        {
            get => _joystickController.InputDirection;
        }

        public MobileInputService(JoystickController joystickController)
        {
            _joystickController = joystickController;
        }
        
        public void Init()
        {
            _inputService = this;
            
            _joystickController.Clicked += JoystickController_Clicked;
            _joystickController.Dragged += JoystickController_Dragged;
            _joystickController.Released += JoystickController_Released;
        }

        private void JoystickController_Clicked()
        {
            _inputService.Clicked?.Invoke();
        }

        private void JoystickController_Dragged(Vector2 delta)
        {
            _inputService.Dragged?.Invoke(delta);
        }

        private void JoystickController_Released()
        {
            _inputService.Released?.Invoke();
        }
    }
}