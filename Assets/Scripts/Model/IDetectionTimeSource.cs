using System;
using UnityEngine;

public interface IDetectionTimeSource
{
    event Action<float, float> OnDetectionTimeChange;
}
