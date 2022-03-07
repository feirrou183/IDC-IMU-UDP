using System.Collections;
using UnityEngine;

namespace DataVisualization.Charts
{
    [System.Serializable]
    public class TwoDimensionalData
    {
        public Vector2 dataValue;
        public TwoDimensionalData(float x,float y)
        {
            dataValue.x = x;
            dataValue.y = y;
        }
    }
}