using System;

namespace Assets.Scripts
{
    public class SharedDataManager
    {
        public Team CurrentPlayer { get; private set; }
        public State CurrentState { get; private set; }
        public Cell Destination { get; private set; }
        public Unit Target { get; private set; }
        public event Action OnTargetChosen;
        public event Action OnDestinationChosen;
        public event Action OnLockIsOver;
        public event Action OnNextPlayer;
        public event Action OnWaitForConfirm;
        public event Action OnAbort;
        public SharedDataManager()
        {
            CurrentPlayer = Team.Player1;
            CurrentState = State.ChoosingCell;
            Destination = null;
            Target = null;
        }
        public void ChooseTarget(Unit target)
        {
            Target = target;
            OnTargetChosen?.Invoke();
            CurrentState = State.Lock;
        }
        public void ChooseDestination(Cell destination)
        {
            Destination = destination;
            OnTargetChosen?.Invoke();
            CurrentState = State.Lock;
        }
        public void WaitForConfirmation()
        {
            OnWaitForConfirm?.Invoke();
            CurrentState = State.WaitingForConfirm;
        }
        public void Abort()
        {
            OnAbort.Invoke();
            CurrentState = State.ChoosingCell;
        }
        public void EndTheLock()
        {
            OnLockIsOver?.Invoke();
        }
        public void NextPlayer()
        {
            CurrentPlayer =
                (CurrentPlayer == Team.Player1) ?
                                    Team.Player2 :
                                    Team.Player1;
            OnNextPlayer.Invoke();
            CurrentState = State.ChoosingCell;
            Destination = null;
            Target = null;
        }
    }
}
