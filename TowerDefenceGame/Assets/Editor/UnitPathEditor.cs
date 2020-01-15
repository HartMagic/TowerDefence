using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitPath))]
public sealed class UnitPathEditor : Editor
{
    private UnitPath _handlePath;

    private Transform _handleTransform;
    private Quaternion _handleRotation;

    private const int _stepsPerSegment = 10;

    private const float _directionScale = 0.5f;

    private const float _handleSize = 0.04f;
    private const float _pickSize = 0.06f;

    private int _selectedIndex = -1;

    private static bool _showDirections;

    private static Color[] _modeColors =
    {
        Color.white,
        Color.red,
        Color.blue,
    };

    public override void OnInspectorGUI()
    {
        _handlePath = target as UnitPath;

        if (_handlePath != null)
        {
            _showDirections = EditorGUILayout.Toggle("Show Directions", _showDirections);
            
            EditorGUI.BeginChangeCheck();
            var loop = EditorGUILayout.Toggle("IsLoop", _handlePath.IsLoop);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_handlePath, "Toggle IsLoop");
                EditorUtility.SetDirty(_handlePath);

                _handlePath.IsLoop = loop;
            }

            if (_selectedIndex >= 0 && _selectedIndex < _handlePath.Count)
            {
                DrawSelectedPointOnInspector();
            }

            if (GUILayout.Button("Add Segment"))
            {
                EditorGUI.BeginChangeCheck();
                Undo.RecordObject(_handlePath, "Add Segment");
                _handlePath.AddSegment();
                EditorUtility.SetDirty(_handlePath);
            }
        }
    }

    private void OnSceneGUI()
    {
        _handlePath = target as UnitPath;

        if (_handlePath != null)
        {
            _handleTransform = _handlePath.transform;
            _handleRotation = Tools.pivotRotation == PivotRotation.Local
                ? _handleTransform.rotation
                : Quaternion.identity;

            var p0 = DrawControlPoint(0);

            for (var i = 1; i < _handlePath.Count; i += 3)
            {
                var p1 = DrawControlPoint(i);
                var p2 = DrawControlPoint(i + 1);
                var p3 = DrawControlPoint(i + 2);
                
                Handles.color = Color.gray;
                Handles.DrawLine(p0, p1);
                Handles.DrawLine(p2, p3);
                
                Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2.0f);

                p0 = p3;
            }

            if (_showDirections)
                DrawDirections();
        }
    }

    private void DrawDirections()
    {
        Handles.color = Color.green;

        var point = _handlePath.GetPoint(0.0f);
        Handles.DrawLine(point, point + _handlePath.GetDirection(0.0f) * _directionScale);

        var steps = _stepsPerSegment * _handlePath.SegmentCount;
        for (var i = 1; i <= steps; i++)
        {
            point = _handlePath.GetPoint(i / (float) steps);
            Handles.DrawLine(point, point + _handlePath.GetDirection(i / (float)steps) * _directionScale);
        }
    }
    
    private Vector3 DrawControlPoint(int index)
    {
        var point = _handleTransform.TransformPoint(_handlePath[index]);
        var size = HandleUtility.GetHandleSize(point);

        if (index == 0)
            size *= 2.0f;

        Handles.color = _modeColors[(int) _handlePath.GetControlPointMode(index)];

        if (Handles.Button(point, _handleRotation, _handleSize * size, _pickSize * size, Handles.DotHandleCap))
        {
            _selectedIndex = index;
            Repaint();
        }

        if (_selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, _handleRotation);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_handlePath, "Move Point");
                EditorUtility.SetDirty(_handlePath);
                _handlePath[index] = _handleTransform.InverseTransformPoint(point);
            }
        }

        return point;
    }

    private void DrawSelectedPointOnInspector()
    {
        GUILayout.Label("Selected Point:");
        
        EditorGUI.BeginChangeCheck();
        var point = EditorGUILayout.Vector3Field("Position", _handlePath[_selectedIndex]);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_handlePath, "Move Point");
            EditorUtility.SetDirty(_handlePath);

            _handlePath[_selectedIndex] = point;
        }
        
        EditorGUI.BeginChangeCheck();
        var mode = (ControlPointMode) EditorGUILayout.EnumPopup("Mode",
            _handlePath.GetControlPointMode(_selectedIndex));

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_handlePath, "Change Point Mode");
            EditorUtility.SetDirty(_handlePath);
            
            _handlePath.SetControlPointMode(_selectedIndex, mode);
        }
    }
    
    [DrawGizmo(GizmoType.NotInSelectionHierarchy)]
    private static void RenderCustomGizmo(Transform objectTransform, GizmoType gizmoType)
    {
        var path = objectTransform.GetComponent<UnitPath>();
        if (path != null)
        {
            var p0 = objectTransform.TransformPoint(path[0]);

            for (var i = 1; i < path.Count; i += 3)
            {
                var p1 = objectTransform.TransformPoint(path[i]);
                var p2 = objectTransform.TransformPoint(path[i + 1]);
                var p3 = objectTransform.TransformPoint(path[i + 2]);
                
                Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2.0f);
                p0 = p3;
            }
        }
    }
}
