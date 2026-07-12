using System;

namespace Assets.Scripts
{
    internal class ScoreManager : IDisposable //Logic is horrible here, but i am too tired of this class
    {
        private int _currentScore = 0;
        private int _totalScore = 0;
        private int _strikeBonusesLeft = 0;
        private int _spareBonusesLeft = 0;
        private int _pinsFellCount = 0;
        private bool _skipRoundForStrike = false;

        private GameParameters _gameParameters;
        private EventManager _eventManager;
        private TextManager _textManager;

        public ScoreManager(GameParameters gameParameters, EventManager eventManager, TextManager textManager)
        {
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
        private void DisplayBonuses() //ugly, but works well
        {
            var bonus = 0;
            if (_strikeBonusesLeft > 0)
                bonus += _gameParameters.StrikeBonus;
            if (_spareBonusesLeft > 0) 
                bonus += _gameParameters.SpareBonus;
            _textManager.WriteBonus(bonus);
        }

        private void PinFell()
        {
            _pinsFellCount++;
            SetCurrent(_currentScore + 1);
        }

        private void HandleEndRound(bool isFirstRound)
        {
            bool allThePinsAreDown = _pinsFellCount == _gameParameters.PinAmount;
            if (_skipRoundForStrike)
            {
                _skipRoundForStrike = false;
                return;
            }
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

            if (isFirstRound && allThePinsAreDown) //STRIKE
            {
                _skipRoundForStrike = true;
                _strikeBonusesLeft += 2;
                _eventManager.EndRoundInvoke();
            }
            else if (_currentScore == _gameParameters.PinAmount && allThePinsAreDown)
                _spareBonusesLeft += 1;
            DisplayBonuses();
        }

        private void HandleEndCouple()
        {
            _pinsFellCount = 0;
            SetTotal(_totalScore + _currentScore);
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
