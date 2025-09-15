using UnityEngine;

namespace SGM
{
    public enum Lane
    {
        Left,
        Center,
        Right
    }

    public enum PowerUpType
    {
        DoubleCoin,
        Wings,
        SlowTime
    }

    public static class S_GameManager
    {
        #region Coin Logic
        private static string[] _laneName = { "Left", "Center", "Right" };
        public static string GetLaneName(int id) => _laneName[id];
        #endregion

        #region Difficulty
        private const float _multiplyDifficulty = 0.01f;

        public static float Difficulty() => 1f + CoinManager.Instance.GetCoinPickUp() * _multiplyDifficulty;
        #endregion

        #region String
        public static string GetHorizontal() => "Horizontal";

        public static string GetVertical() => "Vertical";

        public static string GetTagPlayer() => "Player";
        #endregion
    }
}