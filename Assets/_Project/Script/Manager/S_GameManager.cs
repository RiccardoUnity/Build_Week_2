using UnityEngine;

namespace SGM
{
    public enum Lane
    {
        Left,
        Center,
        Right
    }

    public static class S_GameManager
    {
        private static string[] _laneName = { "Left", "Center", "Right" };
        public static string GetLaneName(int id) => _laneName[id];

        public static string GetHorizontal() => "Horizontal";

        public static string GetVertical() => "Vertical";

        //0 facile --- 1 difficile
        private static float _difficulty = 0.5f;

        public static float Difficulty
        {
            get => _difficulty;
            set => Mathf.Clamp01(value);
        }
    }
}