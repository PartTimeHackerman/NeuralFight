using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class RewardFunctions
{

    public static double gaussian(double x, float valueAt1)
    {
        double scale = Math.Sqrt(-2f * Mathf.Log(valueAt1));
        return Math.Exp(-.5f * Math.Pow(x * scale, 2f));
    }

    public static double hyperbolic(double x, float valueAt1)
    {
        double scale = MathFns.HArccos(1 / valueAt1);
        return 1 / Math.Cosh(x * scale);
    }

    public static double longTail(double x, float valueAt1)
    {
        double scale = Math.Sqrt(1 / valueAt1 - 1);
        return 1 / (Math.Pow((x * scale), 2) + 1);
    }

    public static double cosine(double x, float valueAt1)
    {
        double scale = Math.Acos(2 * valueAt1 - 1) / Math.PI;
        double scaledX = x * scale;
        return Math.Abs(scaledX) < 1 ? (1 + Math.Cos(Math.PI * scaledX)) / 2 : 0.0;
    }

    public static double linear(double x, float valueAt1)
    {
        double scale = 1 - valueAt1;
        double scaledX = x * scale;
        return Math.Abs(scaledX) < 1 ? 1 - scaledX : 0.0;
    }

    public static double quadratic(double x, float valueAt1)
    {
        double scale = Math.Sqrt(1 - valueAt1);
        double scaledX = x * scale;
        return Math.Abs(scaledX) < 1 ? 1 - Math.Pow(scaledX, 2) : 0.0;
    }

    public static double tanhSqrt(double x, float valueAt1)
    {
        double scale = MathFns.HArctan(Math.Sqrt(1 - valueAt1));
        return 1 - Math.Pow(Math.Tanh(x * scale), 2);
    }

    
    ///<summary>
    ///Returns 1 when `x` falls inside the bounds, between 0 and 1 otherwise.
    ///Args:
    ///x: A scalar or numpy array.
    ///    bounds: A tuple of floats specifying inclusive `(lower, upper)` bounds for
    ///the target interval.These can be infinite if the interval is unbounded
    ///    at one or both ends, or they can be equal to one another if the target
    ///value is exact.
    ///    margin: Float.Parameter that controls how steeply the output decreases as
    ///`x` moves out-of-bounds.
    ///* If `margin == 0` then the output will be 0 for all values of `x`
    ///outside of `bounds`.
    ///* If `margin > 0` then the output will decrease sigmoidally with
    ///    increasing distance from the nearest bound.
    ///    sigmoid: String, choice of sigmoid type.Valid values are: 'gaussian',
    ///'linear', 'hyperbolic', 'long_tail', 'cosine', 'tanh_squared'.
    ///value_at_margin: A float between 0 and 1 specifying the output value when
    ///    the distance from `x` to the nearest bound is equal to `margin`. Ignored
    ///if `margin == 0`.
    ///Returns:
    ///A float or numpy array with values between 0.0 and 1.0.
    ///Raises:
    ///ValueError: If `bounds[0] > bounds[1]`.
    ///ValueError: If `margin` is negative.
    ///</summary>
    public static double tolerance(double x, float lower, float upper, float margin, float valueAtMargin, RewardFunction rewardFunction)
    {
        if (lower > upper)
            throw new Exception("Lower bound must be <= upper bound.");
        if (margin < 0)
            throw new Exception("margin must be non-negative.");

        bool inBounds = (lower <= x && x <= upper);

        double value, d = 0;

        if (margin == 0)
            value = inBounds ? 1.0 : 0.0;
        else
        {
            d = (x < lower ? lower - x : x - upper) / margin;
            value = inBounds ? 1.0 : fnByName(d, valueAtMargin, rewardFunction);
        }

        return value;
    }

    public static double toleranceInv(double x, float lower, float upper, float margin, float valueAtMargin, RewardFunction rewardFunction)
    {
        x = Math.Abs(x - 1f);
        return tolerance(x, lower, upper, margin, valueAtMargin, rewardFunction);
    }

    public static float toleranceInvNoBounds(double x, float margin, float valueAtMargin, RewardFunction rewardFunction)
    {
        return (float)toleranceInv(x, 0f, 0f, margin, valueAtMargin, rewardFunction);
    }

    private static double fnByName(double x, float valueAt1, RewardFunction rewardFunction)
    {
        switch (rewardFunction)
        {
            case RewardFunction.GAUSSIAN: return gaussian(x, valueAt1);
            case RewardFunction.HYPERBOLIC: return hyperbolic(x, valueAt1);
            case RewardFunction.LONGTAIL: return longTail(x, valueAt1);
            case RewardFunction.COSINE: return cosine(x, valueAt1);
            case RewardFunction.LINEAR: return linear(x, valueAt1);
            case RewardFunction.QUADRATIC: return quadratic(x, valueAt1);
            case RewardFunction.TANHSQRT: return tanhSqrt(x, valueAt1);
            default: return 0;
        }
    }

}

public enum RewardFunction
{
    GAUSSIAN,
    HYPERBOLIC,
    LONGTAIL,
    COSINE,
    LINEAR,
    QUADRATIC,
    TANHSQRT
}
