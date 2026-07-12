using System;

namespace Assets.Scripts
{
    internal class ScoreManager : IDisposable
    {
        private int _currentScore;
        private int _totalScore;
        private int _strikeBonusesLeft = 0;
        private int _spareBonusesLeft = 0;

        private GameParameters _gameParameters;
        private EventManager _eventManager;
        private TextManager _textManager;

        public ScoreManager(GameParameters gameParameters, EventManager eventManager, TextManager textManager)
        {
            _currentScore = 0;
            _totalScore = 0; 
            _gameParameters = gameParameters;
            _eventManager = eventManager;
            _textManager = textManager;
            _eventManager.PinFell += PinFell;
            _eventManager.EndRound += HandleEndRound;
            _eventManager.EndCouple += HandleEndCouple;
        }

        private void SetCurrent(int points)
        {
            _currentScore = points;
            _textManager.WriteCurrent(points);
        }
        private void SetTotal(int points)
        {
            _totalScore = points;
            _textManager.WriteTotal(points);
        }

        private void PinFell() => SetCurrent(_currentScore + 1);

        private void HandleEndRound(bool isFirstRound)
        {
            if (_strikeBonusesLeft > 0)
            {
                SetCurrent(_currentScore + _gameParameters.StrikeBonus);
                _strikeBonusesLeft--;
            }
            if (_spareBonusesLeft > 0)
            {
                SetCurrent(_currentScore + _gameParameters.SpareBonus);
                _spareBonusesLeft--;
            }

            if (isFirstRound && _currentScore == _gameParameters.PinAmount) //STRIKE
            {
                _strikeBonusesLeft += 2;
                _eventManager.EndRoundInvoke();
            }
            else if (_currentScore == _gameParameters.PinAmount)
                _spareBonusesLeft += 1;
        }

        private void HandleEndCouple()
        {
            _textManager.WriteTotal(_totalScore + _currentScore);
            SetCurrent(0);
        }

        void IDisposable.Dispose()
        {
            _eventManager.PinFell -= PinFell;
            _eventManager.EndRound -= HandleEndRound;
            _eventManager.EndCouple -= HandleEndCouple;
        }
    }
}
