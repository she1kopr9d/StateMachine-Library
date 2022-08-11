using UnityEngine;
using UnityEngine.Events;

namespace smf.Unity
{
    public class StateMachineLink
    {
        private string _stateName;
        private SurvivalGame.Multiplayer.StateMachine.StateMachine _stateMachine;

        private SurvivalGame.Multiplayer.StateMachine.StateMachine _StateMachine
        {
            get
            {
                if (_stateMachine == null)
                    _stateMachine = StateFinder.Find(_stateName);
                return _stateMachine;
            }
            set { _stateMachine = value; }
        }


        public StateMachineLink(string stateName)
        {
            _stateName = stateName;
        }

        public void AddListener(UnityAction<string> listener)
        {
            _StateMachine.StateChangedEvent.AddListener(listener);
        }

        public bool Move(string link)
        {
            bool state = _StateMachine.Move(link);
            Debug.Log(_StateMachine.Machine._nowAddres);
            return state;
        }

        public bool Cheak(string link)
        {
            return _StateMachine.Machine._nowAddres == link;
        }
    }
}