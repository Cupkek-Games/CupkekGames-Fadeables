using System;
using UnityEngine;
using System.Collections;

namespace CupkekGames.Fadeables
{
  [Serializable]
  public class Fadeable
  {
    [Header("Fade Settings")] [SerializeField]
    public float _out = 0f;

    [SerializeField] public float _in = 1f;
    [SerializeField] public float _fadeOutDuration = 1f;
    [SerializeField] public float _fadeInDuration = 1f;
    [SerializeField] public float _fadeInDelay = 0f;
    [SerializeField] public float _fadeOutDelay = 0f;
    [SerializeField] private EasingType _easingIn = EasingType.Linear;
    [SerializeField] private EasingType _easingOut = EasingType.Linear;

    [Header("Fade Controller")] [SerializeField]
    private float _value = 0f;

    public float Value => _value;
    public event Action OnApply;
    public event Action OnFadeOutStart;
    public event Action OnFadeOutComplete;
    public event Action OnFadeInStart;

    public event Action OnFadeInComplete;

    // State
    private MonoBehaviour _parent;
    private bool _reversed = false;
    private Coroutine _fadeCoroutine;
    public bool IsFading => _fadeCoroutine != null;
    public bool IsFadedIn => Mathf.Approximately(_value, _in);
    public bool IsFadedOut => Mathf.Approximately(_value, _out);

    public Fadeable(MonoBehaviour parent)
    {
      Initialize(parent);
    }

    public void Initialize(MonoBehaviour parent)
    {
      _parent = parent;
    }

    public float GetValue()
    {
      return _value;
    }

    public void SetValue(float value)
    {
      _value = value;
    }

    private IEnumerator StartFade(float delay, float duration, bool fromCurrent)
    {
      Kill();

      if (delay > 0)
      {
        yield return new WaitForSeconds(delay);
      }

      float startValue;
      float endValue;
      EasingType easingType;

      if (_reversed)
      {
        startValue = _in;
        endValue = _out;
        easingType = _easingOut;
      }
      else
      {
        startValue = _out;
        endValue = _in;
        easingType = _easingIn;
      }

      // A fade interrupting a running fade continues from where the value is
      // (a hover that returns mid fade-out grows back from the current width,
      // no snap to the far end); the duration covers the distance left at the
      // full fade's pace.
      if (fromCurrent)
      {
        float full = Mathf.Abs(endValue - startValue);
        if (full > Mathf.Epsilon)
        {
          duration *= Mathf.Clamp01(Mathf.Abs(endValue - _value) / full);
        }

        startValue = _value;
      }

      float time = 0f;

      while (time < duration)
      {
        float t = time / duration;

        t = easingType.Apply(t);

        _value = Mathf.Lerp(startValue, endValue, t);

        OnApply?.Invoke();

        time += Time.deltaTime;

        yield return null;
      }

      OnCompleteInner();
    }

    private void OnCompleteInner()
    {
      if (_reversed)
      {
        OnFadedOut();
      }
      else
      {
        OnFadedIn();
      }
    }

    public void FadeIn()
    {
      bool fromCurrent = IsFading;
      Kill();
      OnFadeInStart?.Invoke();

      _reversed = false;

      // An inactive object cannot run a coroutine: land on the end state.
      if (!CanAnimate)
      {
        OnFadedIn();
        return;
      }

      _fadeCoroutine = _parent.StartCoroutine(StartFade(_fadeInDelay, _fadeInDuration, fromCurrent));
    }

    public void FadeOut()
    {
      bool fromCurrent = IsFading;
      Kill();
      OnFadeOutStart?.Invoke();

      _reversed = true;

      // An inactive object cannot run a coroutine: land on the end state.
      if (!CanAnimate)
      {
        OnFadedOut();
        return;
      }

      _fadeCoroutine = _parent.StartCoroutine(StartFade(_fadeOutDelay, _fadeOutDuration, fromCurrent));
    }

    // A fade animates through the parent's coroutine, which an inactive
    // GameObject cannot start (e.g. a fade-out requested from OnDisable).
    private bool CanAnimate => _parent != null && _parent.gameObject.activeInHierarchy;

    public void SetFadedIn()
    {
      _reversed = false;
      OnFadedIn();
    }

    private void OnFadedIn()
    {
      Kill();

      _value = _in;
      OnApply?.Invoke();
      OnFadeInComplete?.Invoke();
    }

    public void SetFadedOut()
    {
      _reversed = true;
      OnFadedOut();
    }

    private void OnFadedOut()
    {
      Kill();

      _value = _out;
      OnApply?.Invoke();
      OnFadeOutComplete?.Invoke();
    }

    public void Kill()
    {
      if (_fadeCoroutine != null && _parent != null)
      {
        _parent.StopCoroutine(_fadeCoroutine);
      }

      _fadeCoroutine = null;
    }
  }
}