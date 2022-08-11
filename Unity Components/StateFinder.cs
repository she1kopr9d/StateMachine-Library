using UnityEngine;

namespace smf.Unity
{
    public class StateFinder
    {
        public static SurvivalGame.Multiplayer.StateMachine.StateMachine Find(string stateName)
        {
            SurvivalGame.Multiplayer.StateMachine.StateMachine[] states = MonoBehaviour.FindObjectsOfType<SurvivalGame.Multiplayer.StateMachine.StateMachine>();
            foreach (SurvivalGame.Multiplayer.StateMachine.StateMachine state in states)
            {
                if (state.Name != stateName)
                    continue;
                return state;
            }
            return null;
        }
    }
}
