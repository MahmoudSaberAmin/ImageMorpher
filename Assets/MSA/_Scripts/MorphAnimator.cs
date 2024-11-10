
using System.Collections;
using UnityEngine;


namespace Morph.Smart.Algorithm
{
    public enum EaseFunction
    {
        Linear,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,
        EaseInElastic,
        EaseOutElastic,
        EaseInOutElastic
        // Add more ease functions as needed
    }


    public static class Easing
    {
        public static float Linear(float t) => t;

        public static float EaseInQuad(float t) => t * t;

        public static float EaseOutQuad(float t) => t * (2 - t);

        public static float EaseInOutQuad(float t) => t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;

        public static float EaseInCubic(float t) => t * t * t;

        public static float EaseOutCubic(float t) => (--t) * t * t + 1;

        public static float EaseInOutCubic(float t) => t < 0.5f ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;

        public static float EaseInQuart(float t) => t * t * t * t;

        public static float EaseOutQuart(float t) => 1 - (--t) * t * t * t;

        public static float EaseInOutQuart(float t) => t < 0.5f ? 8 * t * t * t * t : 1 - 8 * (--t) * t * t * t;

        public static float EaseInQuint(float t) => t * t * t * t * t;

        public static float EaseOutQuint(float t) => 1 + (--t) * t * t * t * t;

        public static float EaseInOutQuint(float t) => t < 0.5f ? 16 * t * t * t * t * t : 1 + 16 * (--t) * t * t * t * t;

        public static float EaseInSine(float t) => 1 - Mathf.Cos(t * Mathf.PI / 2);

        public static float EaseOutSine(float t) => Mathf.Sin(t * Mathf.PI / 2);

        public static float EaseInOutSine(float t) => -(Mathf.Cos(Mathf.PI * t) - 1) / 2;

        public static float EaseInBounce(float t) => 1 - EaseOutBounce(1 - t);

        public static float EaseOutBounce(float t)
        {
            if (t < 1 / 2.75f) return 7.5625f * t * t;
            if (t < 2 / 2.75f) return 7.5625f * (t -= 1.5f / 2.75f) * t + 0.75f;
            if (t < 2.5f / 2.75f) return 7.5625f * (t -= 2.25f / 2.75f) * t + 0.9375f;
            return 7.5625f * (t -= 2.625f / 2.75f) * t + 0.984375f;
        }

        public static float EaseInOutBounce(float t) => t < 0.5f ? EaseInBounce(t * 2) * 0.5f : EaseOutBounce(t * 2 - 1) * 0.5f + 0.5f;

        public static float EaseInBack(float t)
        {
            float s = 1.70158f;
            return t * t * ((s + 1) * t - s);
        }

        public static float EaseOutBack(float t)
        {
            float s = 1.70158f;
            return (--t) * t * ((s + 1) * t + s) + 1;
        }

        public static float EaseInOutBack(float t)
        {
            float s = 1.70158f * 1.525f;
            return t < 0.5f ? (t * 2) * (t * 2) * ((s + 1) * t * 2 - s) * 0.5f : ((t * 2 - 2) * (t * 2 - 2) * ((s + 1) * (t * 2 - 2) + s) + 2) * 0.5f;
        }

        public static float EaseInElastic(float t)
        {
            return Mathf.Sin(13 * Mathf.PI / 2 * t) * Mathf.Pow(2, 10 * (t - 1));
        }

        public static float EaseOutElastic(float t)
        {
            return Mathf.Sin(-13 * Mathf.PI / 2 * (t + 1)) * Mathf.Pow(2, -10 * t) + 1;
        }

        public static float EaseInOutElastic(float t)
        {
            return t < 0.5f
                ? 0.5f * Mathf.Sin(13 * Mathf.PI / 2 * (2 * t)) * Mathf.Pow(2, 10 * ((2 * t) - 1))
                : 0.5f * (Mathf.Sin(-13 * Mathf.PI / 2 * ((2 * t - 1) + 1)) * Mathf.Pow(2, -10 * (2 * t - 1)) + 2);
        }

        // Add more ease functions as needed
    }

    public class MorphAnimator : MonoBehaviour
    {
        [SerializeField] private ImageMorpher _morpher;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private EaseFunction _easeFunction;
        [SerializeField] private float _delayBetweenAnims = 0.2f;
        [SerializeField] private float _animDuration = 1f;

        private float _amount = 0f;
        private int _index = 1;

        private void Start()
        {
            StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            float duration = _animDuration; // Duration of the animation
            float elapsedTime = 0f;
            yield return new WaitForSeconds(_delayBetweenAnims);


            if (_amount < 0.5f)
            {
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / duration;
                    _amount = ApplyEasing(t);

                    _morpher.morphAmount = _amount;
                    yield return null;
                }

                _amount = 1f;
            }
            else
            {
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / duration;
                    _amount = ApplyEasing(1 - t);

                    _morpher.morphAmount = _amount;
                    yield return null;
                }

                _amount = 0f;
            }

            yield return new WaitForSeconds(_delayBetweenAnims);

            _index++;

            if (_index % 2 == 0)
                _morpher.imageA = _sprites[_index % _sprites.Length];
            else
                _morpher.imageB = _sprites[(_index) % _sprites.Length];

            _morpher.Initialize();

            yield return Animate();
        }

        private float ApplyEasing(float t)
        {
            switch (_easeFunction)
            {
                case EaseFunction.Linear: return Easing.Linear(t);
                case EaseFunction.EaseInQuad: return Easing.EaseInQuad(t);
                case EaseFunction.EaseOutQuad: return Easing.EaseOutQuad(t);
                case EaseFunction.EaseInOutQuad: return Easing.EaseInOutQuad(t);
                case EaseFunction.EaseInCubic: return Easing.EaseInCubic(t);
                case EaseFunction.EaseOutCubic: return Easing.EaseOutCubic(t);
                case EaseFunction.EaseInOutCubic: return Easing.EaseInOutCubic(t);
                case EaseFunction.EaseInQuart: return Easing.EaseInQuart(t);
                case EaseFunction.EaseOutQuart: return Easing.EaseOutQuart(t);
                case EaseFunction.EaseInOutQuart: return Easing.EaseInOutQuart(t);
                case EaseFunction.EaseInQuint: return Easing.EaseInQuint(t);
                case EaseFunction.EaseOutQuint: return Easing.EaseOutQuint(t);
                case EaseFunction.EaseInOutQuint: return Easing.EaseInOutQuint(t);
                case EaseFunction.EaseInSine: return Easing.EaseInSine(t);
                case EaseFunction.EaseOutSine: return Easing.EaseOutSine(t);
                case EaseFunction.EaseInOutSine: return Easing.EaseInOutSine(t);
                case EaseFunction.EaseInBounce: return Easing.EaseInBounce(t);
                case EaseFunction.EaseOutBounce: return Easing.EaseOutBounce(t);
                case EaseFunction.EaseInOutBounce: return Easing.EaseInOutBounce(t);
                case EaseFunction.EaseInBack: return Easing.EaseInBack(t);
                case EaseFunction.EaseOutBack: return Easing.EaseOutBack(t);
                case EaseFunction.EaseInOutBack: return Easing.EaseInOutBack(t);
                case EaseFunction.EaseInElastic: return Easing.EaseInElastic(t);
                case EaseFunction.EaseOutElastic: return Easing.EaseOutElastic(t);
                case EaseFunction.EaseInOutElastic: return Easing.EaseInOutElastic(t);
                // Add more ease functions as needed
                default: return t;
            }
        }
    }
}