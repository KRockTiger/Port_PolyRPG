using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    DataManager dataManager;

    private void Awake()
    {
        dataManager = DataManager.Instance;

        dataManager.Add(this);
    }

    private void Start()
    {
        GameController gameController = DataManager.Instance.Get<GameController>(typeof(GameController));        
    }

    private void Update()
    {
        
    }
}
