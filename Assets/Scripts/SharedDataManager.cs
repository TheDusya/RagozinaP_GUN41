using System;

namespace Assets.Scripts
{
    public class SharedDataManager
    {
        public Team CurrentPlayer { get; private set; }
        public State CurrentState;
        public Cell Cell { get; private set; }
        public Unit Unit { get; private set; }
        public Unit AttackedUnit { get; private set; }
        bool IsAttack  => AttackedUnit != null;
        public event Action<GameEvent> OnGameEvent;
        
        /*public event Action OnUnitChosen;
        public event Action OnDestinationChosen;
        public event Action OnLockIsOver;
        public event Action OnNextPlayer;
        public event Action OnWaitForConfirm;
        public event Action OnCancel;*/
        public SharedDataManager()
        {
            CurrentPlayer = Team.Player1;
            CurrentState = State.ChoosingUnit;
            Cell = null;
            Unit = null;
        }
        public void SelectUnit(Unit unit)
        {
            if (unit.Team != CurrentPlayer)
                return;
            Unit = unit;
            OnGameEvent.Invoke(GameEvent.SelectUnit);
            CurrentState = State.ChoosingCell;
        }
        public void SelectCell(Cell destination, Unit attacked)
        {
            Cell = destination;
            AttackedUnit = attacked;
            OnGameEvent.Invoke(GameEvent.SelectCell);
            CurrentState = State.WaitingForConfirm;
        }
        public void Confirm()
        {
            OnGameEvent.Invoke(GameEvent.Confirm);
            CurrentState = State.Lock;
            MovementStart();
        }
        public void MovementStart()
        {
            OnGameEvent.Invoke(GameEvent.MovementStart);
        }
        public void MovementEnd()
        {
            OnGameEvent.Invoke(GameEvent.MovementEnd);
            if (IsAttack)
                OnGameEvent.Invoke(GameEvent.Attack);
            //TODO: add multiple attacks
            NextPlayer();
        }
        public void NextPlayer()
        {
            CurrentPlayer =
                (CurrentPlayer == Team.Player1) ?
                                    Team.Player2 :
                                    Team.Player1;
            OnGameEvent.Invoke(GameEvent.NewTurn);
            CurrentState = State.ChoosingUnit;
            Cell = null;
            Unit = null;
        }
    }
}
