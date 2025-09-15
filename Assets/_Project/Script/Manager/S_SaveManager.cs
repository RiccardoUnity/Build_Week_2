using Newtonsoft.Json;
using System.IO;
using UnityEngine;

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

    [System.Serializable]
    public class PowerUp
    {
        private static int _doubleCoinsLevel;
        public static int DoubleCoinsLevel
        {
            get => _doubleCoinsLevel;
            private set => _doubleCoinsLevel = value;
        }

        private static int _wingsLevel;
        public static int WingsLevel
        {
            get => _wingsLevel;
            private set => _wingsLevel = value;
        }

        private static int _slowTimeLevel;
        public static int SlowTimeLevel
        {
            get => _slowTimeLevel;
            private set => _slowTimeLevel = value;
        }

        public PowerUp()
        {
            DoubleCoinsLevel = 0;
            WingsLevel = 0;
            SlowTimeLevel = 0;
        }

        public PowerUp(int doubleCoins, int wings, int slowTimeLevel)
        {
            DoubleCoinsLevel = doubleCoins;
            WingsLevel = wings;
            SlowTimeLevel = slowTimeLevel;
        }

        public void IncreasePowerUp(PowerUpType powerUpType)
        {
            switch (powerUpType)
            {
                case PowerUpType.DoubleCoin:
                    ++DoubleCoinsLevel;
                    break;
                case PowerUpType.Wings:
                    ++WingsLevel;
                    break;
                case PowerUpType.SlowTime:
                    ++SlowTimeLevel;
                    break;
            }
            S_SaveManager.SavePowerUp();
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
        private static string _pathLeaderboard = Application.persistentDataPath + "/leaderboard.txt";

        public static Leaderboard ResetLeaderboard()
        {
            try
            {
                Leaderboard leaderboard = new Leaderboard();
                string stringLeaderboard = JsonConvert.SerializeObject(leaderboard, Formatting.Indented);
                File.WriteAllText(_pathLeaderboard, stringLeaderboard);
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
            if (File.Exists(_pathLeaderboard))
            {
                try
                {
                    string stringLeaderboard;
                    Leaderboard leaderboard;
                    stringLeaderboard = File.ReadAllText(_pathLeaderboard);
                    leaderboard = JsonConvert.DeserializeObject<Leaderboard>(stringLeaderboard);
                    return leaderboard;
                }
                catch
                {
                    Debug.LogError("Qualcosa è andato ESTREMAMENTE storto nella lettura della Leaderboard");
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
            File.WriteAllText(_pathLeaderboard, stringLeaderboard);
        }
        #endregion

        #region PowerUp
        public static PowerUp powerUp;

        private static string _pathPowerUp = Application.persistentDataPath + "/powerUp.txt";

        public static void ResetPowerUp()
        {
            powerUp = new PowerUp();
            string stringPowerUp = JsonConvert.SerializeObject(powerUp);
            File.WriteAllText(_pathPowerUp, stringPowerUp);
        }

        public static PowerUp GetPowerUp()
        {
            if (powerUp == null)
            {
                if (File.Exists(_pathPowerUp))
                {
                    try
                    {
                        string stringPowerUp = File.ReadAllText(_pathPowerUp);
                        powerUp = JsonConvert.DeserializeObject<PowerUp>(stringPowerUp);
                    }
                    catch
                    {
                        Debug.LogError("Qualcosa è andato ESTREMAMENTE storto nella lettura dei PowerUp");
                    }
                }
                else
                {
                    ResetPowerUp();
                }
            }
            return powerUp;
        }

        public static void SavePowerUp()
        {
            string stringPowerUp = JsonConvert.SerializeObject(powerUp, Formatting.Indented);
            File.WriteAllText(_pathPowerUp, stringPowerUp);
        }
        #endregion
    }
}