using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    DataManager dataManager;

    private void Awake()
    {
        //DataManager dataManager = DataManager.Instance; //½Ì±ÛÅæ °¡Á®¿Ã ¼ö ÀÖÀ½

        dataManager = DataManager.Instance;

        dataManager.Add(this);
    }

    private void Start()
    {
        InputController inputController = DataManager.Instance.Get<InputController>(typeof(InputController));        
    }

    private void Update()
    {
        
    }
}
