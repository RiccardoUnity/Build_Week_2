using System.Collections;
using UnityEngine;

public class BiomePool : MonoBehaviour
{
    [System.Serializable]
    public class Biome
    {
        public GameObject gameObject;
        public float lenghtZ;
    }

    [Header("Prefab")]
    [SerializeField] private Biome[] _biomes = new Biome[4];
    private Biome[] _biomesInstantiate;

    [Header("In Scene")]
    [SerializeField] private GameObject _player;
    [SerializeField] private Vector3 _startPoint;
    private Camera _camera;

    private int _index;
    private int _nextIndex;
    private WaitForSeconds _sleep;
    private float _relativeStartPlayer;
    private bool _isPoolBiome;

    private const float _deltaV = 10f;

    void Start()
    {
        _camera = Camera.main;

        _biomesInstantiate = new Biome[_biomes.Length];
        for (int i = 0; i < _biomes.Length; ++i)
        {
            _biomesInstantiate[i] = new Biome();
            _biomesInstantiate[i].gameObject = Instantiate(_biomes[i].gameObject);
            _biomesInstantiate[i].gameObject.SetActive(false);
            _biomesInstantiate[i].lenghtZ = _biomes[i].lenghtZ;
        }

        int random = Random.Range(0, _biomes.Length);

        _biomesInstantiate[random].gameObject.transform.position = _startPoint;
        _biomesInstantiate[random].gameObject.SetActive(true);
        _player.transform.position = _startPoint;

        _index = random;
        _sleep = new WaitForSeconds(1);
        _relativeStartPlayer = _player.transform.position.z;
        StartCoroutine(MyUpdate());
    }

    IEnumerator MyUpdate()
    {
        while(_player.transform.position.y > -5)
        {
            yield return _sleep;

            if (!_isPoolBiome && _biomesInstantiate[_index].lenghtZ - _camera.farClipPlane - _deltaV < _player.transform.position.z - _relativeStartPlayer)
            {
                int random;
                do
                {
                    random = Random.Range(0, _biomes.Length);
                }
                while (random == _index);
                _nextIndex = random;

                _biomesInstantiate[_nextIndex].gameObject.transform.position =
                    _biomesInstantiate[_index].gameObject.transform.position + new Vector3(0f, 0f, _biomesInstantiate[_index].lenghtZ);
                _biomesInstantiate[_nextIndex].gameObject.SetActive(true);

                _isPoolBiome = true;
            }

            if (_isPoolBiome && _biomesInstantiate[_index].lenghtZ < _player.transform.position.z - _relativeStartPlayer)
            {
                _isPoolBiome = false;
                _relativeStartPlayer += _biomesInstantiate[_index].lenghtZ;

                yield return _sleep;
                _biomesInstantiate[_index].gameObject.SetActive(false);
                _index = _nextIndex;
            }
        }
    }
}
