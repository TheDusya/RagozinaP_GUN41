using System;

namespace Assets
{
    public static class InGameEventManager
    {
        public static event Action<Scripts.Path> PathWasChosen;
        public static event Action PathWasFinished;

        public static void ChoosePath(Scripts.Path path) => PathWasChosen?.Invoke(path);
        public static void FinishPath() => PathWasFinished?.Invoke();
    }
}
