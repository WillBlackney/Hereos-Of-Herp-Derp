using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.U2D.Interface;
using UnityEditor.U2D.Interface;
using System;
using System.Collections.Generic;

namespace UnityEditor.U2D.Animation
{
    internal class GenericVertexSelector : ISelector<int>
    {
        public ISelection<int> selection { get; set; }
        public ISpriteMeshData spriteMeshData { get; set; }
        public Func<int, bool> SelectionCallback;

        public void Select()
        {
            Debug.Assert(selection != null);
            Debug.Assert(spriteMeshData != null);
            Debug.Assert(SelectionCallback != null);

            for (var i = 0; i < spriteMeshData.vertexCount; i++)
                selection.Select(i, SelectionCallback(i));
        }
    }
}
