#if ENABLE_ANIMATION_COLLECTION && ENABLE_ANIMATION_BURST
#define ENABLE_SPRITESKIN_COMPOSITE
#endif


using Unity.Collections;
using UnityEngine.Profiling;

namespace UnityEngine.U2D.Animation
{
    public sealed  partial class SpriteSkin : MonoBehaviour
#if ENABLE_SPRITESKIN_COMPOSITE
    {
        int m_TransformId;
        NativeArray<int> m_BoneTransformId;
        int m_RootBoneTransformId;
        NativeCustomSlice<Vector3> m_SpriteVertices;
        NativeCustomSlice<Vector4> m_SpriteTangents;
        NativeCustomSlice<BoneWeight> m_SpriteBoneWeights;
        NativeCustomSlice<Matrix4x4> m_SpriteBindPoses;
        NativeCustomSlice<int> m_BoneTransformIdNativeSlice;
        bool m_SpriteHasTangents;
        int m_SpriteVertexStreamSize;
        int m_SpriteVertexCount;
        int m_SpriteTangentVertexOffset;
        int m_DataIndex = -1;
        bool m_BoneCacheUpdateToDate = false;

        void OnEnableBatch()
        {
            if (m_UseBatching && m_BatchSkinning == false)
            {
                SpriteSkinComposite.instance.AddSpriteSkin(this);
                m_BatchSkinning = true;
                CacheBoneTransformIds(true);
            }
            else
                SpriteSkinComposite.instance.AddSpriteSkinForLateUpdate(this);

            m_TransformId = gameObject.transform.GetInstanceID();
            UpdateSpriteDeform();
        }

        void OnResetBatch()
        {
            if (m_UseBatching)
            {
                CacheBoneTransformIds(true);
            }
        }

        void OnDisableBatch()
        {
            RemoveTransformFromSpriteSkinComposite();
            SpriteSkinComposite.instance.RemoveSpriteSkin(this);
            SpriteSkinComposite.instance.RemoveSpriteSkinForLateUpdate(this);
            
            m_BatchSkinning = false;
        }

        internal void UpdateSpriteDeform()
        {
            if (sprite == null)
            {
                m_SpriteVertices = NativeCustomSlice<Vector3>.Default();
                m_SpriteTangents = NativeCustomSlice<Vector4>.Default();
                m_SpriteBoneWeights = NativeCustomSlice<BoneWeight>.Default();
                m_SpriteBindPoses = NativeCustomSlice<Matrix4x4>.Default();
                m_SpriteHasTangents = false;
                m_SpriteVertexStreamSize = 0;
                m_SpriteVertexCount = 0;
                m_SpriteTangentVertexOffset = 0;
            }
            else
            {
                m_SpriteVertices = new NativeCustomSlice<Vector3>(sprite.GetVertexAttribute<Vector3>(UnityEngine.Rendering.VertexAttribute.Position));
                m_SpriteTangents = new NativeCustomSlice<Vector4>(sprite.GetVertexAttribute<Vector4>(UnityEngine.Rendering.VertexAttribute.Tangent));
                m_SpriteBoneWeights = new NativeCustomSlice<BoneWeight>(sprite.GetVertexAttribute<BoneWeight>(UnityEngine.Rendering.VertexAttribute.BlendWeight));
                m_SpriteBindPoses = new NativeCustomSlice<Matrix4x4>(sprite.GetBindPoses());
                m_SpriteHasTangents = sprite.HasVertexAttribute(Rendering.VertexAttribute.Tangent);
                m_SpriteVertexStreamSize = sprite.GetVertexStreamSize();
                m_SpriteVertexCount = sprite.GetVertexCount();
                m_SpriteTangentVertexOffset = sprite.GetVertexStreamOffset(Rendering.VertexAttribute.Tangent);
            }
            
        }

        void CacheBoneTransformIds(bool forceUpdate = false)
        {
            if (!m_BoneCacheUpdateToDate || forceUpdate)
            {
                SpriteSkinComposite.instance.RemoveTransformById(m_RootBoneTransformId);
                if (rootBone != null)
                {
                    m_RootBoneTransformId = rootBone.GetInstanceID();
                    if (this.enabled)
                        SpriteSkinComposite.instance.AddSpriteSkinRootBoneTransform(this);
                }
                else
                    m_RootBoneTransformId = 0;

                if (boneTransforms != null)
                {
                    int boneCount = 0;
                    for (int i = 0; i < boneTransforms.Length; ++i)
                    {
                        if (boneTransforms[i] != null)
                            ++boneCount;
                    }


                    if (m_BoneTransformId.IsCreated)
                    {
                        for (int i = 0; i < m_BoneTransformId.Length; ++i)
                            SpriteSkinComposite.instance.RemoveTransformById(m_BoneTransformId[i]);
                        NativeArrayHelpers.ResizeIfNeeded(ref m_BoneTransformId, boneCount);
                    }
                    else
                    {
                        m_BoneTransformId = new NativeArray<int>(boneCount, Allocator.Persistent);
                    }

                    m_BoneTransformIdNativeSlice = new NativeCustomSlice<int>(m_BoneTransformId);
                    for (int i = 0, j = 0; i < boneTransforms.Length; ++i)
                    {
                        if (boneTransforms[i] != null)
                        {
                            m_BoneTransformId[j] = boneTransforms[i].GetInstanceID();
                            ++j;
                        }
                    }
                    if (this.enabled)
                    {
                        SpriteSkinComposite.instance.AddSpriteSkinBoneTransform(this);
                    }
                }
                else
                {
                    if (m_BoneTransformId.IsCreated)
                        NativeArrayHelpers.ResizeIfNeeded(ref m_BoneTransformId, 0);
                    else
                        m_BoneTransformId = new NativeArray<int>(0, Allocator.Persistent);
                }
                CacheValidFlag();
                m_BoneCacheUpdateToDate = true;
            }
        }
        
        void UseBatchingBatch()
        {
            if (!this.enabled)
                return;

            if (m_UseBatching)
            {
                SpriteSkinComposite.instance.AddSpriteSkin(this);
                SpriteSkinComposite.instance.RemoveSpriteSkinForLateUpdate(this);
                m_BatchSkinning = true;
                CacheBoneTransformIds();
            }
            else
            {
                SpriteSkinComposite.instance.RemoveSpriteSkin(this);
                SpriteSkinComposite.instance.AddSpriteSkinForLateUpdate(this);
                RemoveTransformFromSpriteSkinComposite();
                m_BatchSkinning = false;
            }
        }

        void RemoveTransformFromSpriteSkinComposite()
        {
            if (m_BoneTransformId.IsCreated)
            {
                for (int i = 0; i < m_BoneTransformId.Length; ++i)
                    SpriteSkinComposite.instance.RemoveTransformById(m_BoneTransformId[i]);
                m_BoneTransformId.Dispose();
            }
            SpriteSkinComposite.instance.RemoveTransformById(m_RootBoneTransformId);
            m_RootBoneTransformId = -1;
            m_BoneCacheUpdateToDate = false;
        }

        internal bool GetSpriteSkinBatchData(ref NativeArray<SpriteSkinData> data, ref SpriteSkinBatchProcessData batchProcessData, ref PerSkinJobData perskinJobData, ref int vertexBufferSize, int dataIndex, int spriteSkinIndex)
        {
            CacheBoneTransformIds();
            CacheCurrentSprite();
            if (m_IsValid && (alwaysUpdate || spriteRenderer.isVisible) && spriteRenderer.enabled)
            {
                Profiler.BeginSample("SpriteSkinData");
                m_DataIndex = dataIndex;
                m_CurrentDeformVerticesLength = m_SpriteVertexCount * m_SpriteVertexStreamSize;
                data[dataIndex] = new SpriteSkinData()
                {
                    vertices = m_SpriteVertices,
                    boneWeights = m_SpriteBoneWeights,
                    bindPoses = m_SpriteBindPoses,
                    tangents = m_SpriteTangents,
                    hasTangents = m_SpriteHasTangents,
                    spriteVertexStreamSize = m_SpriteVertexStreamSize,
                    spriteVertexCount = m_SpriteVertexCount,
                    tangentVertexOffset = m_SpriteTangentVertexOffset,
                    spriteSkinIndex = spriteSkinIndex,
                    deformVerticesStartPos = vertexBufferSize,
                    rootBoneTransformId = m_RootBoneTransformId,
                    transformId = m_TransformId,
                    boneTransformId = m_BoneTransformIdNativeSlice
                };
                Profiler.EndSample();
                Profiler.BeginSample("BatchProcessData");
                batchProcessData.rootBoneTransformId[dataIndex] = m_RootBoneTransformId;
                batchProcessData.rootTransformId[dataIndex] = m_TransformId;
                batchProcessData.spriteBound[dataIndex] = bounds;
                Profiler.EndSample();

                Profiler.BeginSample("PerskinJobData");
                perskinJobData.verticesIndex.x = perskinJobData.verticesIndex.y;
                perskinJobData.verticesIndex.y = perskinJobData.verticesIndex.x + m_SpriteVertexCount;
                vertexBufferSize += m_CurrentDeformVerticesLength;
                perskinJobData.bindPosesIndex.x = perskinJobData.bindPosesIndex.y;
                perskinJobData.bindPosesIndex.y = perskinJobData.bindPosesIndex.x + m_SpriteBindPoses.Length;
                Profiler.EndSample();
                return true;
            }
            m_DataIndex = -1;
            return false;
        }

        void OnBoneTransformChanged()
        {
            if (this.enabled)
            {
                CacheBoneTransformIds(true);
            }
        }

        void OnRootBoneTransformChanged()
        {
            if (this.enabled)
            {
                CacheBoneTransformIds(true);
            }
        }
        
        void OnBeforeSerializeBatch()
        {}

        void OnAfterSerializeBatch()
        {
#if UNITY_EDITOR
            m_BoneCacheUpdateToDate = false;
#endif
        }


    }
#else
    {
        void OnEnableBatch(){}
        internal void UpdateSpriteDeform(){}
        void OnResetBatch(){}
        void UseBatchingBatch(){}
        void OnDisableBatch(){}
        void OnBoneTransformChanged(){}
        void OnRootBoneTransformChanged(){}
        void OnBeforeSerializeBatch(){}
        void OnAfterSerializeBatch(){}
    }
#endif

}
