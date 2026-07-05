using UnityEngine;

namespace CupkekGames.Fadeables
{
    public enum EasingType
    {
        Linear,
        SineIn,
        SineOut,
        SineInOut,
        QuadIn,
        QuadOut,
        QuadInOut,
        ExpoIn,
        ExpoOut,
        ExpoInOut,
        BackOut,
        BackInOut,
        ElasticOut,
        BounceOut
    }
    public static class EasingTypeExtensions
    {
        public static float Apply(this EasingType type, float t)
        {
            switch (type)
            {
                case EasingType.QuadIn:
                    return t * t; // Quadratic ease-in
                case EasingType.QuadOut:
                    return t * (2 - t); // Quadratic ease-out
                case EasingType.QuadInOut:
                    return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t; // Quadratic ease-in-out
                case EasingType.ExpoIn:
                    return t == 0f ? 0f : Mathf.Pow(2f, 10f * (t - 1f)); // Exponential ease-in
                case EasingType.ExpoOut:
                    return t == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t); // Exponential ease-out
                case EasingType.ExpoInOut:
                    if (t == 0f) return 0f;
                    if (t == 1f) return 1f;
                    return t < 0.5f
                        ? 0.5f * Mathf.Pow(2f, 20f * t - 10f) // First half: exponential ease-in
                        : 1f - 0.5f * Mathf.Pow(2f, -20f * t + 10f); // Second half: exponential ease-out
                case EasingType.SineIn:
                    return 1f - Mathf.Cos(t * Mathf.PI * 0.5f); // Sine ease-in
                case EasingType.SineOut:
                    return Mathf.Sin(t * Mathf.PI * 0.5f); // Sine ease-out
                case EasingType.SineInOut:
                    return 0.5f * (1f - Mathf.Cos(Mathf.PI * t)); // Sine ease-in-out
                case EasingType.BackOut:
                {
                    const float c1 = 1.70158f;
                    const float c3 = c1 + 1f;
                    float u = t - 1f;
                    return 1f + c3 * u * u * u + c1 * u * u; // Back ease-out (overshoots past 1)
                }
                case EasingType.BackInOut:
                {
                    const float c1 = 1.70158f;
                    const float c2 = c1 * 1.525f;
                    if (t < 0.5f)
                    {
                        float u = 2f * t;
                        return u * u * ((c2 + 1f) * u - c2) * 0.5f;
                    }
                    else
                    {
                        float u = 2f * t - 2f;
                        return (u * u * ((c2 + 1f) * u + c2) + 2f) * 0.5f;
                    }
                }
                case EasingType.ElasticOut:
                {
                    if (t == 0f) return 0f;
                    if (t == 1f) return 1f;
                    const float c4 = 2f * Mathf.PI / 3f;
                    return Mathf.Pow(2f, -10f * t) * Mathf.Sin((10f * t - 0.75f) * c4) + 1f; // Elastic ease-out (overshoots past 1)
                }
                case EasingType.BounceOut:
                {
                    const float n1 = 7.5625f;
                    const float d1 = 2.75f;
                    if (t < 1f / d1) return n1 * t * t;
                    if (t < 2f / d1) { float u = t - 1.5f / d1; return n1 * u * u + 0.75f; }
                    if (t < 2.5f / d1) { float u = t - 2.25f / d1; return n1 * u * u + 0.9375f; }
                    float v = t - 2.625f / d1;
                    return n1 * v * v + 0.984375f;
                }
                default:
                    return t; // Linear
            }
        }
    }
}