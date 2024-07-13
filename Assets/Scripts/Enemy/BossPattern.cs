using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossPattern
{
    public float patternNum; //보스 패턴 번호
    public string patternName; //보스 패턴 이름
    public GameObject attackTrigger; //공격 트리거 오브젝트
}
