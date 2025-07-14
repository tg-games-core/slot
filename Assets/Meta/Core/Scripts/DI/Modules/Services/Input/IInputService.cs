using System;
using UnityEngine;

namespace Core.Services
{
    public interface IInputService : IService
    {
        Action Clicked
        {
            get; set;
        }
        
        Action<Vector2> Dragged
        {
            get; set;
        }
        
        Action Released
        {
            get; set;
        }
        
        float Speed
        {
            get;
        }
        
        Vector2 InputDirection
        {
            get;
        }

        void Init();
    }
}