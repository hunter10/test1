using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CenterCube : MonoBehaviour {

    // 게임스타트 버튼 이벤트 받으면
    // 48장 카드 랜덤하게 섞어서 가운데에 쌓기
    // 오브젝트 풀 사용 준비

    public GameObject cardBase;

    float cardheight = 0.02f;

    List<GameObject> _cardDesk = new List<GameObject>();
    public List<Transform> _userPos = new List<Transform>();


    public void MakeDeck()
    {
        for (int i = 0; i < 48; i++)
        {
            GameObject card = Instantiate(cardBase, new Vector3(transform.position.x, transform.position.y, transform.position.z + cardheight * i), Quaternion.identity) as GameObject;
            _cardDesk.Add(card);
        }
    }

    public void MoveToUser()
    {
        Debug.Log(_userPos[0].position);
        //_cardDesk[_cardDesk.Count-1].GetComponent<CardBase>().StartMove(_userPos[0]);

        Debug.Log(_cardDesk[_cardDesk.Count - 1].GetComponent<CardBase>().originalPos);
    }
}

