using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManager.Domain.Entities.MachineStates
{
    public abstract class MachineStatus<T>
    {
        private readonly StateSettings[] _settings;
        private StateSettings _currentStateSettings;
        private readonly Action<T> _onStateChanged;
        private readonly Action<MachineStatus<T>> _onMachineStatusChanged;

        public T CurrentState => _currentStateSettings.FromState;

        protected MachineStatus(StateSettings[] setup, T initialState,
            Action<T> onStateChanged = null, Action<MachineStatus<T>> onMachineStatusChanged = null)
        {
            _settings = setup;
            _currentStateSettings = GetSettings(initialState);
            _onStateChanged = onStateChanged;
            _onMachineStatusChanged = onMachineStatusChanged;
        }

        public bool TrySetState(T nextState)
        {
            if (_currentStateSettings.PossibleToStates?.Contains(nextState) ?? false)
            {
                _currentStateSettings = GetSettings(nextState);
                _onStateChanged?.Invoke(CurrentState);
                _onMachineStatusChanged?.Invoke(this);

                return true;
            }

            return false;
        }

        private StateSettings GetSettings(T state)
            => _settings.FirstOrDefault(x => EqualityComparer<T>.Default.Equals(x.FromState, state));

        public struct StateSettings
        {
            public T FromState { get; set; }
            public IList<T> PossibleToStates { get; set; }
        }
    }
}
