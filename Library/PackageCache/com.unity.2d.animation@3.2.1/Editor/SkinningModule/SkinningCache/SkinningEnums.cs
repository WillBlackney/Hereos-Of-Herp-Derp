using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.U2D.Layout;
using UnityEngine.U2D;
using UnityEngine.UIElements;
using UnityEngine.Events;

namespace UnityEditor.U2D.Animation
{
    internal enum SkinningMode
    {
        SpriteSheet,
        Character
    }

    internal enum Tools
    {
        EditGeometry,
        CreateVertex,
        CreateEdge,
        SplitEdge,
        GenerateGeometry,
        EditPose,
        EditJoints,
        CreateBone,
        SplitBone,
        ReparentBone,
        WeightSlider,
        WeightBrush,
        GenerateWeights,
        BoneInfluence,
        CopyPaste,
        Visibility,
        SwitchMode
    }
}
