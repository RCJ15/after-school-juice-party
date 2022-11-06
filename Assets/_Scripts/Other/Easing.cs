using UnityEngine;
/// <summary>
/// Easing library
/// </summary>
public static class Easing

{
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>x</returns>
    public static float Same(float x)
    {
        return x;
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>1 - Mathf.Cos((x * Mathf.PI) / 2);</returns>
    public static float InSine(float x)
    {
        return 1 - Mathf.Cos((x * Mathf.PI) / 2);
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>Mathf.Sin((x * Mathf.PI) / 2);</returns>
    public static float OutSine(float x)
    {
        return Mathf.Sin((x * Mathf.PI) / 2);
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns> -(Mathf.Cos(Mathf.PI * x) - 1) / 2;</returns>
    public static float InOutSine(float x)
    {
        return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>x * x;</returns>
    public static float InQuad(float x)
    {
        return x * x;
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>1 - (1 - x) * (1 - x);</returns>
    public static float OutQuad(float x)
    {
        return 1 - (1 - x) * (1 - x);
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;</returns>
    public static float InOutQuad(float x)
    {
        return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns> x * x * x;</returns>
    public static float InCubic(float x)
    {
        return x * x * x;
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>1 - Mathf.Pow(1 - x, 3);</returns>
    public static float OutCubic(float x)
    {
        return 1 - Mathf.Pow(1 - x, 3);
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;</returns>
    public static float InOutCubic(float x)
    {
        return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>x * x * x * x;</returns>
    public static float InQuart(float x)
    {
        return x * x * x * x;
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>1 - Mathf.Pow(1 - x, 4);</returns>
    public static float OutQuart(float x)
    {
        return 1 - Mathf.Pow(1 - x, 4);
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2;</returns>
    public static float InOutQuart(float x)
    {
        return x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2;
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>x * x * x * x * x;</returns>
    public static float InQuint(float x)
    {
        return x * x * x * x * x;
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>1 - Mathf.Pow(1 - x, 5);</returns>
    public static float OutQuint(float x)
    {
        return 1 - Mathf.Pow(1 - x, 5);
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;</returns>
    public static float InOutQuint(float x)
    {
        return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);</returns>
    public static float InExpo(float x)
    {
        return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns>x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);</returns>
    public static float OutExpo(float x)
    {
        return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }
    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <returns> x == 0? 0: x == 1? 1: x< 0.5 ? Mathf.Pow(2, 20 * x - 10) / 2: (2 - Mathf.Pow(2, -20 * x + 10)) / 2;</returns>
    public static float InOutExpo(float x)
    {
        return x == 0
     ? 0
     : x == 1
     ? 1
     : x < 0.5 ? Mathf.Pow(2, 20 * x - 10) / 2
     : (2 - Mathf.Pow(2, -20 * x + 10)) / 2;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns>1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));</returns>
    public static float InCirc(float x)
    {
        return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns>Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));</returns>
    public static float OutCirc(float x)
    {
        return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns>return x < 0.5? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2: (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;</returns>
    public static float InOutCirc(float x)
    {
        return x < 0.5
     ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
     : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns> c3* x * x* x - c1* x * x;</returns>
    public static float InBack(float x)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;

        return c3 * x * x * x - c1 * x * x;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns>1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);</returns>
    public static float OutBack(float x)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;

        return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns>x < 0.5? (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2: (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x* 2 - 2) + c2) + 2) / 2;</returns>
    public static float InOutBack(float x)
    {
        float c1 = 1.70158f;
        float c2 = c1 * 1.525f;

        return x < 0.5
          ? (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
          : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns>x == 0? 0: x == 1? 1: -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x* 10f - 10.75f) * c4);</returns>
    public static float InElastic(float x)
    {
        float c4 = (2 * Mathf.PI) / 3;

        return x == 0
          ? 0
          : x == 1
          ? 1
          : -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10f - 10.75f) * c4);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns>x == 0? 0: x == 1? 1: Mathf.Pow(2, -10 * x) * Mathf.Sin((x* 10f - 0.75f) * c4) + 1;</returns>
    public static float OutElastic(float x)
    {
        float c4 = (2 * Mathf.PI) / 3;

        return x == 0
          ? 0
          : x == 1
          ? 1
          : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10f - 0.75f) * c4) + 1;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns>x == 0? 0: x == 1? 1: x< 0.5? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2: (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2 + 1;</returns>
    public static float InOutElastic(float x)
    {
        float c5 = (2 * Mathf.PI) / 4.5f;

        return x == 0
          ? 0
          : x == 1
          ? 1
          : x < 0.5
          ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2
          : (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2 + 1;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns>1 - OutBounce(1 - x);</returns>
    public static float InBounce(float x)
    {
        return 1 - OutBounce(1 - x);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns>if (x < 1 / d1){return n1* x * x;}else if (x < 2 / d1){return n1 * (x -= 1.5f / d1) * x + 0.75f;}else if (x < 2.5 / d1){return n1 * (x -= 2.25f / d1) * x + 0.9375f;}else{return n1 * (x -= 2.625f / d1) * x + 0.984375f;}</returns>
    public static float OutBounce(float x)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }
        else if (x < 2.5 / d1)
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns>x < 0.? (1 - OutBounce(1 - 2 * x)) / 2: (1 + OutBounce(2 * x - 1)) / 2;</returns>
    public static float InOutBounce(float x)
    {
        return x < 0.5
     ? (1 - OutBounce(1 - 2 * x)) / 2
     : (1 + OutBounce(2 * x - 1)) / 2;
    }
}
/*
 public static float Ease(float x)
    {
        return x * x;
    }
 */