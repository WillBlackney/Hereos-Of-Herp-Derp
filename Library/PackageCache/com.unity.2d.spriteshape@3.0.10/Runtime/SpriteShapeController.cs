using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Collections.LowLevel.Unsafe;
#if UNITY_EDITOR
using UnityEditor.U2D;
#endif

namespace UnityEngine.U2D
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteShapeRenderer))]
    [DisallowMultipleComponent]
    [HelpURLAttribute("https://docs.unity3d.com/Packages/com.unity.2d.spriteshape@latest/index.html")]
    public class SpriteShapeController : MonoBehaviour
    {
        const float s_DistanceTolerance = 0.001f;
        EdgeCollider2D m_EdgeCollider2D;
        PolygonCollider2D m_PolygonCollider2D;

        Sprite[] m_SpriteArray;
        Sprite[] m_EdgeSpriteArray;
        Sprite[] m_CornerSpriteArray;
        AngleRangeInfo[] m_AngleRangeInfoArray;

        bool m_DynamicOcclusionLocal;
        bool m_DynamicOcclusionOverriden;
        bool m_LegacyGenerator = false;
        SpriteShapeRenderer m_SpriteShapeRenderer;

        SpriteShape m_ActiveSpriteShape;
        SpriteShapeParameters m_ActiveShapeParameters;
        NativeArray<float2> m_ColliderData;
        NativeArray<Vector4> m_TangentData;

        int m_AngleRangeHash = 0;
        int m_ActiveCornerSpritesHash = 0;
        int m_ActiveSplineHash = 0;

        [SerializeField]
        Spline m_Spline = new Spline();
        [SerializeField]
        SpriteShape m_SpriteShape;

        [SerializeField]
        float m_FillPixelPerUnit = 100.0f;
        [SerializeField]
        float m_StretchTiling = 1.0f;
        [SerializeField]
        int m_SplineDetail;
        [SerializeField]
        bool m_AdaptiveUV;
        [SerializeField]
        bool m_StretchUV;
        [SerializeField]
        bool m_WorldSpaceUV;

        [SerializeField]
        float m_CornerAngleThreshold = 30.0f;

        [SerializeField]
        int m_ColliderDetail;
        [SerializeField, Range(-0.5f, 0.5f)]
        float m_ColliderOffset;
        [SerializeField]
        bool m_UpdateCollider = true;
        [SerializeField]
        bool m_OptimizeCollider = true;
        [SerializeField]
        bool m_OptimizeGeometry = true;
        [SerializeField]
        bool m_EnableTangents = false;

        public int spriteShapeHashCode
        {
            get { return m_ActiveSplineHash; }
        }
        public bool worldSpaceUVs
        {
            get { return m_WorldSpaceUV; }
            set { m_WorldSpaceUV = value; }
        }
        public float fillPixelsPerUnit
        {
            get { return m_FillPixelPerUnit; }
            set { m_FillPixelPerUnit = value; }
        }
        public bool enableTangents
        {
            get { return m_EnableTangents; }
            set { m_EnableTangents = value; }
        }
        public float stretchTiling
        {
            get { return m_StretchTiling; }
            set { m_StretchTiling = value; }
        }
        public Spline spline
        {
            get { return m_Spline; }
        }
        public SpriteShape spriteShape
        {
            get { return m_SpriteShape; }
            set { m_SpriteShape = value; }
        }

        public SpriteShapeRenderer spriteShapeRenderer
        {
            get
            {
                if (!m_SpriteShapeRenderer)
                    m_SpriteShapeRenderer = GetComponent<SpriteShapeRenderer>();

                return m_SpriteShapeRenderer;
            }
        }

        public PolygonCollider2D polygonCollider
        {
            get
            {
                if (!m_PolygonCollider2D)
                    m_PolygonCollider2D = GetComponent<PolygonCollider2D>();

                return m_PolygonCollider2D;
            }
        }

        public EdgeCollider2D edgeCollider
        {
            get
            {
                if (!m_EdgeCollider2D)
                    m_EdgeCollider2D = GetComponent<EdgeCollider2D>();

                return m_EdgeCollider2D;
            }
        }

        public int splineDetail
        {
            get { return m_SplineDetail; }
            set { m_SplineDetail = Mathf.Max(0, value); }
        }

        public int colliderDetail
        {
            get { return m_ColliderDetail; }
            set { m_ColliderDetail = Mathf.Max(0, value); }
        }

        public float colliderOffset
        {
            get { return m_ColliderOffset; }
            set { m_ColliderOffset = value; }
        }

        public float cornerAngleThreshold
        {
            get { return m_CornerAngleThreshold; }
            set { m_CornerAngleThreshold = value; }
        }

        public bool hasCollider
        {
            get { return (edgeCollider != null) || (polygonCollider != null); }
        }

        public bool autoUpdateCollider
        {
            get { return m_UpdateCollider; }
            set { m_UpdateCollider = value; }
        }

        public bool optimizeCollider
        {
            get { return m_OptimizeCollider; }
        }
        public bool optimizeGeometry
        {
            get { return m_OptimizeGeometry; }
        }

        private void DisposeNativeArrays()
        {
            if (m_ColliderData.IsCreated)
                m_ColliderData.Dispose();
            if (m_TangentData.IsCreated)
                m_TangentData.Dispose();
        }

        private void OnApplicationQuit()
        {
            DisposeNativeArrays();
        }

        static void SmartDestroy(UnityEngine.Object o)
        {
            if (o == null)
                return;

#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(o);
            else
#endif
                Destroy(o);
        }

        void Reset()
        {
            m_SplineDetail = (int)QualityDetail.High;
            m_AdaptiveUV = true;
            m_StretchUV = false;
            m_FillPixelPerUnit = 100f;

            m_Spline.InsertPointAt(0, Vector2.left + Vector2.down);
            m_Spline.InsertPointAt(1, Vector2.left + Vector2.up);
            m_Spline.InsertPointAt(2, Vector2.right + Vector2.up);
            m_Spline.InsertPointAt(3, Vector2.right + Vector2.down);

            m_ColliderDetail = (int)QualityDetail.High;
        }

        void OnEnable()
        {
            spriteShapeRenderer.enabled = true;
            m_DynamicOcclusionOverriden = true;
            m_DynamicOcclusionLocal = spriteShapeRenderer.allowOcclusionWhenDynamic;
            spriteShapeRenderer.allowOcclusionWhenDynamic = false;
        }

        void OnDisable()
        {
            spriteShapeRenderer.enabled = false;
            DisposeNativeArrays();
        }

        void OnDestroy()
        {

        }

        void ValidateSpriteShapeData()
        {
            if (spriteShape == null)
            {
                if (m_EdgeSpriteArray != null)
                    m_EdgeSpriteArray = null;
                if (m_CornerSpriteArray != null)
                    m_CornerSpriteArray = null;
                if (m_AngleRangeInfoArray != null)
                    m_AngleRangeInfoArray = null;
                if (m_SpriteArray != null)
                    m_SpriteArray = null;
            }
        }

        int GetAngleRangeHashCode()
        {
            unchecked
            {
                int hashCode = (int)2166136261;

                hashCode = hashCode * 16777619 ^ spriteShape.angleRanges.Count;

                for (int i = 0; i < spriteShape.angleRanges.Count; ++i)
                {
                    hashCode = hashCode * 16777619 ^ (spriteShape.angleRanges[i].GetHashCode() + i);
                }

                return hashCode;
            }
        }

        int GetCornerSpritesHashCode()
        {
            unchecked
            {
                int hashCode = (int)2166136261;

                hashCode = hashCode * 16777619 ^ spriteShape.cornerSprites.Count;

                for (int i = 0; i < spriteShape.cornerSprites.Count; ++i)
                {
                    hashCode = hashCode * 16777619 ^ (spriteShape.cornerSprites[i].GetHashCode() + i);
                }

                return hashCode;
            }
        }

        bool SpriteShapeChanged()
        {
            return m_ActiveSpriteShape != spriteShape;
        }

        bool NeedUpdateSprites()
        {
            if (m_EdgeSpriteArray == null || m_CornerSpriteArray == null || m_AngleRangeInfoArray == null)
                return true;

            bool updateSprites = SpriteShapeChanged();
            if (spriteShape && !updateSprites)
            {
                var angleRangeHashCode = GetAngleRangeHashCode();
                if (m_AngleRangeHash != angleRangeHashCode)
                {
                    m_AngleRangeHash = angleRangeHashCode;
                    updateSprites = true;
                }

                var cornerSpritesHashCode = GetCornerSpritesHashCode();
                if (m_ActiveCornerSpritesHash != cornerSpritesHashCode)
                {
                    m_ActiveCornerSpritesHash = cornerSpritesHashCode;
                    updateSprites = true;
                }
            }
            return updateSprites;
        }

        bool HasSplineChanged()
        {
            unchecked
            {
                int hashCode = (int)2166136261 ^ m_Spline.GetHashCode();
                hashCode = hashCode * 16777619 ^ (m_OptimizeGeometry ? 1 : 0);
                hashCode = hashCode * 16777619 ^ (m_EnableTangents ? 1 : 0);

                if (spriteShapeHashCode != hashCode)
                {
                    m_ActiveSplineHash = hashCode;
                    return true;
                }
            }
            return false;
        }

        void OnWillRenderObject()
        {
            ValidateSpriteShapeData();
            bool needUpdateSprites = NeedUpdateSprites();
            bool spriteShapeParametersChanged = UpdateSpriteShapeParameters();
            bool splineChanged = HasSplineChanged();

            BakeCollider();
            if (needUpdateSprites || spriteShapeParametersChanged || splineChanged)
                BakeMesh(needUpdateSprites);
            m_ActiveSpriteShape = spriteShape;
        }

        public void RefreshSpriteShape()
        {
            m_ActiveSplineHash = 0;
        }

        public JobHandle BakeMesh()
        {
            UpdateSpriteShapeParameters();
            return BakeMesh(NeedUpdateSprites());
        }

        // Ensure Neighbor points are not too close to each other.
        private bool ValidatePoints(NativeArray<ShapeControlPoint> shapePoints)
        {
            for (int i = 0; i < shapePoints.Length - 1; ++i)
            {
                var vec = shapePoints[i].position - shapePoints[i + 1].position;
                if (vec.sqrMagnitude < s_DistanceTolerance)
                {
                    Debug.LogWarningFormat("Control points {0} & {1} are too close to each other. SpriteShape will not be generated.", i, i + 1);
                    return false;
                }
            }
            return true;
        }

        unsafe JobHandle BakeMesh(bool needUpdateSpriteArrays)
        {
            JobHandle jobHandle = default;
            if (needUpdateSpriteArrays)
                UpdateSprites();

            int pointCount = m_Spline.GetPointCount();
            if (pointCount < 2)
                return jobHandle;
            
            NativeArray<ShapeControlPoint> shapePoints  = new NativeArray<ShapeControlPoint>(pointCount, Allocator.Temp);
            NativeArray<SpriteShapeMetaData> shapeMetaData = new NativeArray<SpriteShapeMetaData>(pointCount, Allocator.Temp);

            for (int i = 0; i < pointCount; ++i)
            {
                ShapeControlPoint shapeControlPoint;
                shapeControlPoint.position = m_Spline.GetPosition(i);
                shapeControlPoint.leftTangent = m_Spline.GetLeftTangent(i);
                shapeControlPoint.rightTangent = m_Spline.GetRightTangent(i);
                shapeControlPoint.mode = (int)m_Spline.GetTangentMode(i);
                shapePoints[i] = shapeControlPoint;

                SpriteShapeMetaData metaData;
                metaData.corner = m_Spline.GetCorner(i);
                metaData.height = m_Spline.GetHeight(i);
                metaData.spriteIndex = (uint)m_Spline.GetSpriteIndex(i);
                metaData.bevelCutoff = 0;
                metaData.bevelSize = 0;
                shapeMetaData[i] = metaData;
            }

            if (spriteShapeRenderer != null && ValidatePoints(shapePoints))
            {
                if (m_LegacyGenerator)
                {
                    SpriteShapeUtility.GenerateSpriteShape(spriteShapeRenderer, m_ActiveShapeParameters,
                        shapePoints.ToArray(), shapeMetaData.ToArray(), m_AngleRangeInfoArray, m_EdgeSpriteArray,
                        m_CornerSpriteArray);
                }
                else
                {
                    bool hasSprites = false;
                    float smallestWidth = 99999.0f;
                    foreach (var sprite in m_SpriteArray)
                    {
                        if (sprite != null)
                        {
                            hasSprites = true;
                            float pixelWidth = BezierUtility.GetSpritePixelWidth(sprite);
                            smallestWidth = (smallestWidth > pixelWidth) ? pixelWidth : smallestWidth;
                        }
                    }
                    
                    // Approximate vertex Array Count.
                    float shapeLength = BezierUtility.BezierLength(shapePoints, splineDetail * splineDetail) * 2.0f;
                    int adjustWidth = hasSprites ? ((int)(shapeLength / smallestWidth) * 6) + (pointCount * 6 * splineDetail) : 0;
                    int adjustShape = pointCount * 4 * splineDetail;
                    adjustShape = optimizeGeometry ? (adjustShape) : (adjustShape * 2);
#if !UNITY_EDITOR
                    adjustShape = (spriteShape != null && spriteShape.fillTexture != null) ? adjustShape : 0;
#endif                    
                    int maxArrayCount = adjustShape + adjustWidth;

                    // Collider Data
                    if (m_ColliderData.IsCreated)
                        m_ColliderData.Dispose();
                    m_ColliderData = new NativeArray<float2>(maxArrayCount, Allocator.Persistent);

                    // Tangent Data
                    if (!m_TangentData.IsCreated)
                        m_TangentData = new NativeArray<Vector4>(1, Allocator.Persistent);

                    NativeArray<ushort> indexArray;
                    NativeSlice<Vector3> posArray;
                    NativeSlice<Vector2> uv0Array;
                    NativeArray<Bounds> bounds = spriteShapeRenderer.GetBounds();
                    NativeArray<SpriteShapeSegment> geomArray = spriteShapeRenderer.GetSegments(shapePoints.Length * 8);
                    NativeSlice<Vector4> tanArray = new NativeSlice<Vector4>(m_TangentData);

                    if (m_EnableTangents)
                    { 
                        spriteShapeRenderer.GetChannels(maxArrayCount, out indexArray, out posArray, out uv0Array, out tanArray);
                    }
                    else
                    {
                        spriteShapeRenderer.GetChannels(maxArrayCount, out indexArray, out posArray, out uv0Array);
                    }

                    var spriteShapeJob = new SpriteShapeGenerator()
                    {
                        m_Bounds = bounds,
                        m_PosArray = posArray,
                        m_Uv0Array = uv0Array,
                        m_TanArray = tanArray,
                        m_GeomArray = geomArray,
                        m_IndexArray = indexArray,
                        m_ColliderPoints = m_ColliderData
                    };
                    spriteShapeJob.Prepare(this, m_ActiveShapeParameters, maxArrayCount, shapePoints, shapeMetaData, m_AngleRangeInfoArray, m_EdgeSpriteArray, m_CornerSpriteArray);
                    jobHandle = spriteShapeJob.Schedule();
                    spriteShapeRenderer.Prepare(jobHandle, m_ActiveShapeParameters, m_SpriteArray);
                    JobHandle.ScheduleBatchedJobs();
                }
            }

            if (m_DynamicOcclusionOverriden)
            {
                spriteShapeRenderer.allowOcclusionWhenDynamic = m_DynamicOcclusionLocal;
                m_DynamicOcclusionOverriden = false;
            }

            shapePoints.Dispose();
            shapeMetaData.Dispose();
            return jobHandle;
        }

        public void BakeCollider()
        {
            if (m_ColliderData.IsCreated)
            {
                if (autoUpdateCollider)
                {
                    if (hasCollider)
                    {
                        int maxCount = short.MaxValue - 1;
                        float2 last = (float2)0;
                        List<Vector2> m_ColliderSegment = new List<Vector2>();
                        for (int i = 0; i < maxCount; ++i)
                        {
                            float2 now = m_ColliderData[i];
                            if (!math.any(last) && !math.any(now))
                                break;
                            m_ColliderSegment.Add(new Vector2(now.x, now.y));
                        }

                        EdgeCollider2D edge = GetComponent<EdgeCollider2D>();
                        if (edge != null)
                            edge.points = m_ColliderSegment.ToArray();
                        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
                        if (poly != null)
                            poly.points = m_ColliderSegment.ToArray();
                    }
                }
                m_ColliderData.Dispose();
#if UNITY_EDITOR
                if (UnityEditor.SceneView.lastActiveSceneView != null)
                    UnityEditor.SceneView.lastActiveSceneView.Repaint();
#endif
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
#else        
        void OnGUI()
#endif
        {
            if (spriteShapeRenderer != null)
            {
                var hasSplineChanged = HasSplineChanged();
                if (!spriteShapeRenderer.isVisible && hasSplineChanged)
                {
                    BakeMesh();
                    Rendering.CommandBuffer rc = new Rendering.CommandBuffer();
                    rc.GetTemporaryRT(0, 256, 256, 0);
                    rc.SetRenderTarget(0);
                    rc.DrawRenderer(spriteShapeRenderer, spriteShapeRenderer.sharedMaterial);
                    rc.ReleaseTemporaryRT(0);
                    Graphics.ExecuteCommandBuffer(rc);
                }
            }
        }

        public bool UpdateSpriteShapeParameters()
        {
            Matrix4x4 transformMatrix = Matrix4x4.identity;
            Texture2D fillTexture = null;
            uint fillScale = 0;
            uint splineDetail = (uint)m_SplineDetail;
            float angleThreshold = (m_CornerAngleThreshold >= 0 && m_CornerAngleThreshold < 90) ? m_CornerAngleThreshold : 89.9999f;
            float borderPivot = 0f;
            bool smartSprite = true;
            bool carpet = !m_Spline.isOpenEnded;
            bool adaptiveUV = m_AdaptiveUV;
            bool stretchUV = m_StretchUV;
            bool spriteBorders = false;

            if (spriteShape)
            {
                if (worldSpaceUVs)
                    transformMatrix = transform.localToWorldMatrix;

                fillTexture = spriteShape.fillTexture;
                fillScale = stretchUV ? (uint)stretchTiling : (uint)fillPixelsPerUnit;
                borderPivot = spriteShape.fillOffset;
                spriteBorders = spriteShape.useSpriteBorders;
                // If Corners are enabled, set smart-sprite to false.
                if (spriteShape.cornerSprites.Count > 0)
                    smartSprite = false;
            }
            else
            {
#if UNITY_EDITOR
                if (fillTexture == null)
                    fillTexture = UnityEditor.EditorGUIUtility.whiteTexture;
                fillScale = 100;
#endif
            }

            bool changed = m_ActiveShapeParameters.adaptiveUV != adaptiveUV ||
                m_ActiveShapeParameters.angleThreshold != angleThreshold ||
                m_ActiveShapeParameters.borderPivot != borderPivot ||
                m_ActiveShapeParameters.carpet != carpet ||
                m_ActiveShapeParameters.fillScale != fillScale ||
                m_ActiveShapeParameters.fillTexture != fillTexture ||
                m_ActiveShapeParameters.smartSprite != smartSprite ||
                m_ActiveShapeParameters.splineDetail != splineDetail ||
                m_ActiveShapeParameters.spriteBorders != spriteBorders ||
                m_ActiveShapeParameters.transform != transformMatrix ||
                m_ActiveShapeParameters.stretchUV != stretchUV;

            m_ActiveShapeParameters.adaptiveUV = adaptiveUV;
            m_ActiveShapeParameters.stretchUV = stretchUV;
            m_ActiveShapeParameters.angleThreshold = angleThreshold;
            m_ActiveShapeParameters.borderPivot = borderPivot;
            m_ActiveShapeParameters.carpet = carpet;
            m_ActiveShapeParameters.fillScale = fillScale;
            m_ActiveShapeParameters.fillTexture = fillTexture;
            m_ActiveShapeParameters.smartSprite = smartSprite;
            m_ActiveShapeParameters.splineDetail = splineDetail;
            m_ActiveShapeParameters.spriteBorders = spriteBorders;
            m_ActiveShapeParameters.transform = transformMatrix;

            return changed;
        }

        void UpdateSprites()
        {
            List<Sprite> edgeSpriteList = new List<Sprite>();
            List<Sprite> cornerSpriteList = new List<Sprite>();
            List<AngleRangeInfo> angleRangeInfoList = new List<AngleRangeInfo>();
            
            if (spriteShape)
            {
                List<AngleRange> sortedAngleRanges = new List<AngleRange>(spriteShape.angleRanges);
                sortedAngleRanges.Sort((a, b) => a.order.CompareTo(b.order));

                for (int i = 0; i < sortedAngleRanges.Count; i++)
                {
                    bool validSpritesFound = false;
                    AngleRange angleRange = sortedAngleRanges[i];
                    foreach (Sprite edgeSprite in angleRange.sprites)
                    {
                        if (edgeSprite != null)
                        {
                            validSpritesFound = true;
                            break;
                        }
                    }

                    if (validSpritesFound)
                    {
                        AngleRangeInfo angleRangeInfo = new AngleRangeInfo();
                        angleRangeInfo.start = angleRange.start;
                        angleRangeInfo.end = angleRange.end;
                        angleRangeInfo.order = (uint)angleRange.order;
                        List<int> spriteIndices = new List<int>();
                        foreach (Sprite edgeSprite in angleRange.sprites)
                        {
                            edgeSpriteList.Add(edgeSprite);
                            spriteIndices.Add(edgeSpriteList.Count - 1);
                        }
                        angleRangeInfo.sprites = spriteIndices.ToArray();
                        angleRangeInfoList.Add(angleRangeInfo);
                    }
                }

                bool validCornerSpritesFound = false;
                foreach (CornerSprite cornerSprite in spriteShape.cornerSprites)
                {
                    if (cornerSprite.sprites[0] != null)
                    {
                        validCornerSpritesFound = true;
                        break;
                    }
                }

                if (validCornerSpritesFound)
                {
                    for (int i = 0; i < spriteShape.cornerSprites.Count; i++)
                    {
                        CornerSprite cornerSprite = spriteShape.cornerSprites[i];
                        cornerSpriteList.Add(cornerSprite.sprites[0]);
                    }
                }
            }

            m_EdgeSpriteArray = edgeSpriteList.ToArray();
            m_CornerSpriteArray = cornerSpriteList.ToArray();
            m_AngleRangeInfoArray = angleRangeInfoList.ToArray();

            List<Sprite> spriteList = new List<Sprite>();
            spriteList.AddRange(m_EdgeSpriteArray);
            spriteList.AddRange(m_CornerSpriteArray);
            m_SpriteArray = spriteList.ToArray();
        }

        Texture2D GetTextureFromIndex(int index)
        {
            if (index == 0)
                return spriteShape ? spriteShape.fillTexture : null;

            --index;
            if (index < m_EdgeSpriteArray.Length)
                return GetSpriteTexture(m_EdgeSpriteArray[index]);

            index -= m_EdgeSpriteArray.Length;
            return GetSpriteTexture(m_CornerSpriteArray[index]);
        }

        Texture2D GetSpriteTexture(Sprite sprite)
        {
            if (sprite)
            {
#if UNITY_EDITOR
                return UnityEditor.Sprites.SpriteUtility.GetSpriteTexture(sprite, sprite.packed);
#else
                return sprite.texture;
#endif
            }

            return null;
        }
    }
}
