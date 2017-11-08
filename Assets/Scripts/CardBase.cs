using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CardBase : MonoBehaviour {

    public Vector3 originalPos;
    
    void Start()
    {
        originalPos = transform.position;
    }

    public void StartMove(Transform dest)
    {
        Tween myTween = transform.DOMove(dest.position, 0.5f);
        myTween.SetEase(Ease.InOutQuint);
        //myTween.OnComplete(myFunction);
    }

    void myFunction()
    {
        Debug.Log("Complete");
        StartCoroutine("moveOriginalPos");
    }

    IEnumerator moveOriginalPos()
    {
        yield return new WaitForSeconds(1f);
        transform.position = originalPos;
    }
}



