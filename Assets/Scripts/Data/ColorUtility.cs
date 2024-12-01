using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class HSLColor
{
    public readonly float H;
    public readonly float S;
    public readonly float L;

    public HSLColor(float h, float s, float l)
    {
        H = h;
        S = s;
        L = l;
    }
}

public static class ColorUtility
{
    public static readonly Color green = ToColor32("1baa48FF");
    public static readonly Color purple = ToColor32("ab86c4FF");
    public static readonly Color selectedGold = ToColor32("BD924BFF");
    public static readonly Color plantGreen = ToColor32("00a245ff");
    public static readonly Color earthBrown = ToColor32("ba8a82ff");
    public static readonly Color caveBlue = ToColor32("00a0b1ff");
    public static readonly Color blueprintBlue = ToColor32("0090f7ff");
    public static readonly Color simRed = ToColor32("dc358fff");
    public static readonly Color warningYellow = ToColor32("eae135ff");
    public static readonly Color warningRed = ToColor32("c12e2eff");
    public static readonly Color commentGreen = ToColor32("4bed21ff");
    public static readonly Color voidPurple = ToColor32("a947c8ff");

    public static Color32 ToColor32(string hex)
    {
        if (string.IsNullOrEmpty(hex))
            return new Color32();

        hex = hex.Replace("#", "");

        byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);

        return new Color32(r, g, b, 255);
    }

#if ODD_REALM_APP
    public static Color Random(float minLuminance, float maxLuminance)
    {
        return new Color(TinyBeast.Random.FloatRange(minLuminance, maxLuminance),
                         TinyBeast.Random.FloatRange(minLuminance, maxLuminance),
                         TinyBeast.Random.FloatRange(minLuminance, maxLuminance),
                         1f);
    }

    public static IEnumerator FadeInRoutine(UnityEngine.UI.Image image, float duration, System.Action callback)
    {
        Color c = image.color;
        float start = Time.realtimeSinceStartup;

        while (true)
        {
            float d = Time.realtimeSinceStartup - start;
            image.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(1.0f - d / duration));

            yield return null;

            if (d >= duration) { break; }
        }

        image.color = new Color(c.r, c.g, c.b, 1.0f);
        if (callback != null) { callback(); }
    }

    public static Color Mult(Color a, float b)
    {
        return new Color(a.r * b, a.g * b, a.b * b, a.a);
    }

    public static IEnumerator FadeOutRoutine(UnityEngine.UI.Image image, float duration, System.Action callback)
    {
        Color c = image.color;
        float start = Time.realtimeSinceStartup;

        while (true)
        {
            float d = Time.realtimeSinceStartup - start;
            image.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(d / duration));

            yield return null;

            if (d >= duration) { break; }
        }

        image.color = new Color(c.r, c.g, c.b, 0.0f);
        if (callback != null) { callback(); }
    }

    public static Color FromFloatList(List<float> l)
    {
        return new Color(l[0] / 255.0f, l[1] / 255.0f, l[2] / 255.0f, 1.0f);
    }

    public static HSLColor FromRGB(float R, float G, float B)
    {
        float r = (R / 255f);
        float g = (G / 255f);
        float b = (B / 255f);

        float min = Mathf.Min(Mathf.Min(r, g), b);
        float max = Mathf.Max(Mathf.Max(r, g), b);
        float delta = max - min;

        float H = 0;
        float S = 0;
        float L = (float)((max + min) / 2.0f);

        if (delta != 0)
        {
            if (L < 0.5f)
            {
                S = (float)(delta / (max + min));
            }
            else
            {
                S = (float)(delta / (2.0f - max - min));
            }


            if (r == max)
            {
                H = (g - b) / delta;
            }
            else if (g == max)
            {
                H = 2f + (b - r) / delta;
            }
            else if (b == max)
            {
                H = 4f + (r - g) / delta;
            }
        }

        return new HSLColor(H, S, L);
    }
#endif
}
