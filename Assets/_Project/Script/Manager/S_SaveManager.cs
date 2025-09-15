using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

namespace SGM
{
    [System.Serializable]
    public class Leaderboard
    {
        private const int _maxPosition = 5;
        public static int GetMaxPosition() => _maxPosition;

        private int[] _position = new int[_maxPosition];

        public Leaderboard(int[] position)
        {
            int indexPosition = position.Length;
            if (indexPosition > _maxPosition)
            {
                for (int i = 0; i < _maxPosition; ++i)
                {
                    _position[i] = position[i];
                }
            }
            else if (indexPosition == _maxPosition)
            {
                _position = position;
            }
            else
            {
                for (int i = 0; i < _maxPosition; ++i)
                {
                    if (i < indexPosition)
                    {
                        _position[i] = position[i];
                    }
                    else
                    {
                        _position[i] = 0;
                    }
                }
            }
        }

        public Leaderboard()
        {
            for (int i = 0; i < _maxPosition; ++i)
            {
                _position[i] = 0;
            }
        }

        public int GetPosition(int index)
        {
            if (index >= 0 && index < _maxPosition)
            {
                return _position[index];
            }

            return -1;
        }

        public void InsertPosition(int index, int value)
        {
            int sort;
            for (int i = index; i < _maxPosition; ++i)
            {
                sort = _position[i];
                _position[i] = value;
                value = sort;
            }
        }
    }

    public static class S_SaveManager
    {
        #region Option
        private static string _effects = "effects";
        private static string _music = "music";
        private static string _brightness = "brightness";
        private static float _defaultValue = 0.5f;

        public static float GetEffects() => PlayerPrefs.GetFloat(_effects, _defaultValue);
        public static float GetMusic() => PlayerPrefs.GetFloat(_music, _defaultValue);
        public static float GetBrightness() => PlayerPrefs.GetFloat(_brightness, _defaultValue);

        public static void SetEffects(float value) => PlayerPrefs.SetFloat(_effects, value);
        public static void GetMusic(float value) => PlayerPrefs.SetFloat(_music, value);
        public static void SetBrightness(float value) => PlayerPrefs.SetFloat(_brightness, value);
        #endregion

        #region Leaderboard
        private static string _path = Application.persistentDataPath + "/leaderboard.txt";

        public static Leaderboard ResetLeaderboard()
        {
            try
            {
                Leaderboard leaderboard = new Leaderboard();
                string stringLeaderboard = JsonConvert.SerializeObject(leaderboard, Formatting.Indented);
                File.WriteAllText(_path, stringLeaderboard);
                return leaderboard;
            }
            catch
            {
                Debug.LogError("Errore nel resettare il file di salvataggio");
                return null;
            }
        }

        public static Leaderboard GetLeaderboard()
        {
            if (File.Exists(_path))
            {
                try
                {
                    string stringLeaderboard;
                    Leaderboard leaderboard;
                    stringLeaderboard = File.ReadAllText(_path);
                    leaderboard = JsonConvert.DeserializeObject<Leaderboard>(stringLeaderboard);
                    return leaderboard;
                }
                catch
                {
                    Debug.LogError("Qualcosa nella lettura del file di salvataggio è andata ESTREMAMENTE storta");
                    return null;
                }
            }
            else
            {
                return ResetLeaderboard();
            }
        }

        public static void SaveRecord()
        {
            Leaderboard leaderboard;
            leaderboard = GetLeaderboard();
            for (int i = 0; i < Leaderboard.GetMaxPosition(); ++i)
            {
                if (CoinManager.Instance.GetCoinPickUp() > leaderboard.GetPosition(i))
                {
                    leaderboard.InsertPosition(i, CoinManager.Instance.GetCoinPickUp());
                    break;
                }
            }

            string stringLeaderboard = JsonConvert.SerializeObject(leaderboard, Formatting.Indented);
            File.WriteAllText(_path, stringLeaderboard);
        }

        #endregion
    }
}