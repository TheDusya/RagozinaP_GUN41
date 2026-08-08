namespace Assets.Scripts.Parameters
{
    public static class NameConstants
    {
        public static class LayerNames
        {
            public const string PlayerLayer = "Player";
            public const string GroundLayer = "Ground";
            public const string SceneryLayer = "Scenery";
            public const string WallsLayer = "Walls";
        }
        public static class StateNames
        {
            public const string IdleState = "Idle";
            public const string WalkState = "Walk";
            public const string RunState = "Run";
            public const string JumpState = "Jump";
            public const string CrouchState = "Crouch";
        }
        public static class AnimatorParametersNames
        {
            public const string MoveXParameter = "MoveX";
            public const string MoveZParameter = "MoveZ";
            public const string IsRunningParameter = "IsRunning";
            public const string IsWalkingParameter = "IsWalking";
            public const string IsJumpingParameter = "IsJumping";
            public const string IsCrouchingParameter = "IsCrouching";
        }
    }
}
