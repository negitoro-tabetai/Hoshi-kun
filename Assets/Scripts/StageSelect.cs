using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    public void Save(int stage)
    {
        DataManager.Instance.Save(stage);
    }
}
