using System.Collections.Generic;
using UnityEngine;

namespace Globo.Scripts
{
    public class EntityStateManager : MonoBehaviour
    {
        [SerializeField] private EntityBaseState _currentState;

        [SerializeField] private EntityBaseState[] _states =
        {
            new EntityHappyState(),
            new EntityAngryState(),
            new EntitySadState(),
            new EntityDisgustState()
        };
        
        private void Start()
        {
            _currentState = new EntityHappyState();
            _currentState.EnterState(this);
        }


        private void Update()
        {
            _currentState.UpdateState(this);
        }

        public void SwitchState(EntityBaseState newState)
        {
            _currentState = newState;
            _currentState.EnterState(this);
        }
        
    }
}