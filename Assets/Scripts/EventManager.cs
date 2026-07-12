using System;
using UnityEngine.UI;

public class EventManager : IDisposable
{
    public event Action PinFell;
    public event Action<bool> EndRound;
    public event Action EndCouple;
    private readonly Button _endRoundButton;
    private bool _isFirstRound = true;

    public EventManager(Button endRoundButton)
    {
        _endRoundButton = endRoundButton;
        _endRoundButton.onClick?.AddListener(EndRoundInvoke);
    }

    public void PinFellInvoke() => PinFell?.Invoke();
    public void EndRoundInvoke()
    {
        EndRound?.Invoke(_isFirstRound);
        if (!_isFirstRound)
            EndCouple?.Invoke();
        _isFirstRound = !_isFirstRound;
    }
    public void EndCoupleInvoke()
    {
        _isFirstRound = true;
        EndCouple?.Invoke();
    }

    void IDisposable.Dispose()
    {
        _endRoundButton.onClick?.RemoveListener(EndRoundInvoke);
    }
}
