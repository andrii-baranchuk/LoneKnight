using System.Collections;
using UnityEngine;

namespace CodeBase.Logic
{
  public class LoadingCurtain : MonoBehaviour
  {
    public CanvasGroup Curtain;
    
    private Coroutine _fadeCoroutine;

    public void Show()
    {
      StopPreviousFade();
      _fadeCoroutine = StartCoroutine(DoFadeOut());
    }

    public void Hide()
    {
      StopPreviousFade();
      _fadeCoroutine = StartCoroutine(DoFadeIn());
    }

    private void Awake()
    {
      DontDestroyOnLoad(this);
    }

    private void Start()
    {
      Curtain.alpha = 1;
    }

    private void StopPreviousFade()
    {
      if (_fadeCoroutine != null)
      {
        StopCoroutine(_fadeCoroutine);
      }
    }

    private IEnumerator DoFadeIn()
    {
      while (Curtain.alpha > 0)
      {
        Curtain.alpha -= 0.03f;
        yield return new WaitForSeconds(0.03f);
      }
      
      gameObject.SetActive(false);
    }
    
    private IEnumerator DoFadeOut()
    {
      gameObject.SetActive(true);
     
      while (Curtain.alpha < 1)
      {
        Curtain.alpha += 0.03f;
        yield return new WaitForSeconds(0.03f);
      }
    }
  }
}