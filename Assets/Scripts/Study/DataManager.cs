using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manager, Controller
public class DataManager// : MonoBehaviour ==> Awake, Start, Update를 쓰지 않으므로 삭제를 한다.
                        // 모노헤비어를 쓰지 않으므로 하이라키 창에 넣지 않고 사용한다.
{
    #region Awake없는 싱글톤
    private static DataManager instance;
    
    public static DataManager Instance //기존 싱글톤
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager();
            }
            return instance;
        }
    }
    #endregion

    private List<Component> listControllers = new List<Component>();
    //컨트롤러는 다양한 자료형을 가지고 있어 특정해내기 힘들다.
    //그렇기에 유니티는 스크립트를 Component라는 자료형으로 사용하기 때문에 Component자료형으로 리스트를 만든다.

    public void Add(Component _value)
    {
        listControllers.Add(_value);
    }

    public void Remove(Component _value)
    {
        listControllers.Add(_value);
    }

    //T는 임의의 데이터 자료형
    public T Get<T>(System.Type _value) where T : class //class를 넣어 콜 바이 레퍼런스 형태로 사용
    {
        return listControllers.Find(x => x.GetType() == _value) as T; //as를 넣어 형변환을 시도하여 만약 찾지 못하면 null로 변환
    }
}
