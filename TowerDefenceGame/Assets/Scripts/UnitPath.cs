using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class UnitPath : MonoBehaviour
{
    public int Count
    {
        get { return _controlPoints.Length; }
    }
    
    public int SegmentCount
    {
        get { return (_controlPoints.Length - 1) / 3; }
    }

    public bool IsLoop
    {
        get { return _isLoop; }
        set
        {
            _isLoop = value;
            if (value)
            {
                _controlPointModes[_controlPointModes.Length - 1] = _controlPointModes[0];
                
                this[0] = _controlPoints[0];
            }
        }
    }

    public Vector3 this[int index]
    {
        get
        {
            index = Mathf.Clamp(index, 0, _controlPoints.Length);
            return _controlPoints[index];
        }
        set
        {
            if (index % 3 == 0)
            {
                var delta = value - _controlPoints[index];

                if (_isLoop)
                {
                    if (index == 0)
                    {
                        _controlPoints[1] += delta;
                        _controlPoints[_controlPoints.Length - 2] += delta;
                        _controlPoints[_controlPoints.Length - 1] = value;
                    }
                    else if (index == _controlPoints.Length - 1)
                    {
                        _controlPoints[0] = value;
                        _controlPoints[1] += delta;
                        _controlPoints[index - 1] += delta;
                    }
                    else
                    {
                        _controlPoints[index - 1] += delta;
                        _controlPoints[index + 1] += delta;
                    }
                }
                else
                {
                    if (index > 0)
                        _controlPoints[index - 1] += delta;
                    if (index + 1 < _controlPoints.Length)
                        _controlPoints[index + 1] += delta;
                }
            }

            _controlPoints[index] = value;
            EnforceMode(index);
        }
    }
    
    [SerializeField]
    private Vector3[] _controlPoints = new Vector3[]
    {
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(2.0f, 0.0f, 0.0f),
        new Vector3(3.0f, 0.0f, 0.0f),
        new Vector3(4.0f, 0.0f, 0.0f) 
    };
    
    [SerializeField]
    private ControlPointMode[] _controlPointModes = new ControlPointMode[]
    {
        ControlPointMode.Free,
        ControlPointMode.Free
    };

    [SerializeField]
    private bool _isLoop;

    public void Reset()
    {
        _controlPoints = new Vector3[]
        {
            new Vector3(1.0f, 0.0f, 0.0f),
            new Vector3(2.0f, 0.0f, 0.0f),
            new Vector3(3.0f, 0.0f, 0.0f),
            new Vector3(4.0f, 0.0f, 0.0f) 
        };
        
        _controlPointModes = new ControlPointMode[]
        {
            ControlPointMode.Free,
            ControlPointMode.Free
        };
    }

    public void AddSegment()
    {
        var point = _controlPoints[_controlPoints.Length - 1];
        Array.Resize(ref _controlPoints, _controlPoints.Length + 3);

        point.x += 1.0f;
        _controlPoints[_controlPoints.Length - 3] = point;
        point.x += 1.0f;
        _controlPoints[_controlPoints.Length - 2] = point;
        point.x += 1.0f;
        _controlPoints[_controlPoints.Length - 1] = point;

        Array.Resize(ref _controlPointModes, _controlPointModes.Length + 1);
        _controlPointModes[_controlPointModes.Length - 1] = _controlPointModes[_controlPointModes.Length - 2];
        
        EnforceMode(_controlPoints.Length - 4);

        if (_isLoop)
        {
            _controlPoints[_controlPoints.Length - 1] = _controlPoints[0];
            _controlPointModes[_controlPointModes.Length - 1] = _controlPointModes[0];
            
            EnforceMode(0);
        }
    }

    public ControlPointMode GetControlPointMode(int index)
    {
        return _controlPointModes[(index + 1) / 3];
    }

    public void SetControlPointMode(int index, ControlPointMode mode)
    {
        var modeIndex = (index + 1) / 3;
        _controlPointModes[modeIndex] = mode;

        if (_isLoop)
        {
            if (modeIndex == 0)
                _controlPointModes[_controlPointModes.Length - 1] = mode;
            else if (modeIndex == _controlPointModes.Length - 1)
                _controlPointModes[0] = mode;
        }
        
        EnforceMode(index);
    }

    public Vector3 GetPoint(float t)
    {
        int i;
        if (t >= 1.0f)
        {
            t = 1.0f;
            i = _controlPoints.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * SegmentCount;
            i = (int) t;
            t -= i;
            i *= 3;
        }

        return transform.TransformPoint(GetPoint(_controlPoints[i], _controlPoints[i + 1], _controlPoints[i + 2],
            _controlPoints[i + 3], t));
    }

    public Vector3 GetVelocity(float t)
    {
        int i;
        if (t >= 1.0f)
        {
            t = 1.0f;
            i = _controlPoints.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * SegmentCount;
            i = (int) t;
            t -= i;
            i *= 3;
        }

        return transform.TransformPoint(GetVelocity(_controlPoints[i], _controlPoints[i + 1], _controlPoints[i + 2],
            _controlPoints[i + 3], t)) - transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    private void EnforceMode(int index)
    {
        var modeIndex = (index + 1) / 3;
        var mode = _controlPointModes[modeIndex];
        
        if(mode == ControlPointMode.Free || !_isLoop && (modeIndex == 0 || modeIndex == _controlPointModes.Length - 1))
            return;

        var middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;

        if (index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            if (fixedIndex < 0)
                fixedIndex = _controlPoints.Length - 2;

            enforcedIndex = middleIndex + 1;
            if (enforcedIndex >= _controlPoints.Length)
                enforcedIndex = 1;
        }
        else
        {
            fixedIndex = middleIndex + 1;
            if (fixedIndex >= _controlPoints.Length)
                fixedIndex = 1;

            enforcedIndex = middleIndex - 1;
            if (enforcedIndex < 0)
                enforcedIndex = _controlPoints.Length - 2;
        }

        var middle = _controlPoints[middleIndex];
        var enforcedTangent = middle - _controlPoints[fixedIndex];

        if (mode == ControlPointMode.Aligned)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, _controlPoints[enforcedIndex]);
        }

        _controlPoints[enforcedIndex] = middle + enforcedTangent;
    }

    // Bezier curve
    private Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);

        var oneMinusT = 1.0f - t;
        return oneMinusT * oneMinusT * oneMinusT * p0 + 3.0f * oneMinusT * oneMinusT * t * p1 +
               3.0f * oneMinusT * t * t * p2 + t * t * t * p3;
    }

    private Vector3 GetVelocity(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);

        var oneMinusT = 1.0f - t;
        return 3.0f * oneMinusT * oneMinusT * (p1 - p0) + 6.0f * oneMinusT * t * (p2 - p1) + 3.0f * t * t * (p3 - p2);
    }
}