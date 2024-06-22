using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manager, Controller
public class DataManager// : MonoBehaviour ==> Awake, Start, Update�� ���� �����Ƿ� ������ �Ѵ�.
                        // ������ ���� �����Ƿ� ���̶�Ű â�� ���� �ʰ� ����Ѵ�.
{
    #region Awake���� �̱���
    private static DataManager instance;
    
    public static DataManager Instance //���� �̱���
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
    //��Ʈ�ѷ��� �پ��� �ڷ����� ������ �־� Ư���س��� �����.
    //�׷��⿡ ����Ƽ�� ��ũ��Ʈ�� Component��� �ڷ������� ����ϱ� ������ Component�ڷ������� ����Ʈ�� �����.

    public void Add(Component _value)
    {
        listControllers.Add(_value);
    }

    public void Remove(Component _value)
    {
        listControllers.Add(_value);
    }

    //T�� ������ ������ �ڷ���
    public T Get<T>(System.Type _value) where T : class //class�� �־� �� ���� ���۷��� ���·� ���
    {
        return listControllers.Find(x => x.GetType() == _value) as T; //as�� �־� ����ȯ�� �õ��Ͽ� ���� ã�� ���ϸ� null�� ��ȯ
    }
}
