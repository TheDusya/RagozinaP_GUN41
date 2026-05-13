public enum NeighbourType { TopRight, BottomRight, BottomLeft, TopLeft}
public enum Team { Player1, Player2 }
public enum State { ChoosingUnit, ChoosingCell, Lock, WaitingForConfirm, Restarting }
public enum GameEvent { NewTurn, SelectUnit, SelectCell, Confirm, CancelCell, CancelUnit, MovementStart, MovementEnd, Attack, Restart, RestartOver }