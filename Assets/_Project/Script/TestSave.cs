using System.Collections;
using UnityEngine;
using Save = SGM.S_SaveManager;

public class TestSave : MonoBehaviour
{
    IEnumerator Start()
    {
        //Save.ResetCoin();
        //Save.SaveCoin(500);
        yield return null;
        Debug.Log(Save.GetCoin());
    }
}
