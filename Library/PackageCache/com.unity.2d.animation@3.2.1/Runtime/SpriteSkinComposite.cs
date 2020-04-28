#if ENABLE_ANIMATION_COLLECTION
using System;
using Unity.Mathematics;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.U2D.Common;
using UnityEngine.Profiling;
using Unity.Burst;

namespace UnityEngine.U2D.Animation
{
    internal struct PerSkinJobData
    {
        public int2 bindPosesIndex;
        public int2 verticesIndex;
    }

    internal struct SpriteSkinBatchProcessData
    {
        public NativeArray<int> rootBoneTransformId;
        public NativeArray<int> rootTransformId;
        public NativeArray<Bounds> spriteBound;
        public NativeArray<Bounds> newSpriteBound;

        public SpriteSkinBatchProcessData(int size)
        {
            rootBoneTransformId = new NativeArray<int>(size, Allocator.Persistent);
            rootTransformId = new NativeArray<int>(size, Allocator.Persistent);
            spriteBound = new NativeArray<Bounds>(size, Allocator.Persistent);
            newSpriteBound = new NativeArray<Bounds>(size, Allocator.Persistent);
        }

        public void Destory()
        {
            rootBoneTransformId.DisposeIfCreated();
            rootTransformId.DisposeIfCreated();
            spriteBound.DisposeIfCreated();
            newSpriteBound.DisposeIfCreated();
        }

        public void ResizeIfNeeded(int size)
        {
            NativeArrayHelpers.ResizeIfNeeded(ref rootBoneTransformId, size);
            NativeArrayHelpers.ResizeIfNeeded(ref rootTransformId, size);
            NativeArrayHelpers.ResizeIfNeeded(ref spriteBound, size);
            NativeArrayHelpers.ResizeIfNeeded(ref newSpriteBound, size);
        }
    }

    internal struct SpriteSkinData
    {
        public NativeCustomSlice<Vector3> vertices;
        public NativeCustomSlice<BoneWeight> boneWeights;
        public NativeCustomSlice<Matrix4x4> bindPoses;
        public NativeCustomSlice<Vector4> tangents;
        public bool hasTangents;
        public int spriteVertexStreamSize;
        public int spriteVertexCount;
        public int tangentVertexOffset;
        public int spriteSkinIndex;
        public int deformVerticesStartPos;
        public int rootBoneTransformId;
        public int transformId;
        public NativeCustomSlice<int> boneTransformId;
    }

#if ENABLE_ANIMATION_BURST
    [BurstCompile]
#endif 
    internal struct PrepareDeformJob :IJob
    {
        [ReadOnly]
        public NativeArray<SpriteSkinData> spriteSkinData;
        [ReadOnly]
        public NativeArray<PerSkinJobData> perSkinJobData;
        [ReadOnly]
        public int batchDataSize;
        // Lookup Data for Bones.
        public NativeArray<int2> boneLookupData;
        // VertexLookup
        public NativeArray<int2> vertexLookupData;

        public void Execute()
        {
            for (int i = 0; i < batchDataSize; ++i)
            {
                var jobData = perSkinJobData[i];
                var skinData = spriteSkinData[i];
                for (int j = 0; j < skinData.bindPoses.Length; ++j)
                {
                    int x = jobData.bindPosesIndex.x + j;
                    boneLookupData[x] = new int2(i, j);
                }
                for (int k = 0, j = jobData.verticesIndex.x; j < jobData.verticesIndex.y; ++j, ++k)
                {
                    vertexLookupData[j] = new int2(i, k);
                }
            }
        }
    }

#if ENABLE_ANIMATION_BURST
    [BurstCompile]
#endif 
    internal struct BoneDeformBatchedJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<float4x4> boneTransform;
        [ReadOnly]
        public NativeArray<float4x4> rootTransform;
        [ReadOnly]
        public NativeArray<int2> boneLookupData;
        [ReadOnly]
        public NativeArray<int> rootTransformId;
        [ReadOnly]
        public NativeArray<SpriteSkinData> spriteSkinData;
        [ReadOnly]
        public NativeHashMap<int, TransformAccessJob.TransformData> rootTransformIndex;
        [ReadOnly]
        public NativeHashMap<int, TransformAccessJob.TransformData> boneTransformIndex;
        // Output and Input.
        public NativeArray<float4x4> finalBoneTransforms;

        public void Execute(int i)
        {
            int x = boneLookupData[i].x;
            int y = boneLookupData[i].y;
            var v = spriteSkinData[x].boneTransformId[y];
            var index = boneTransformIndex[v].transformIndex;
            if (index < 0)
                return;
            var aa = boneTransform[index];
            var bb = spriteSkinData[x].bindPoses[y];
            var cc = rootTransformIndex[rootTransformId[x]].transformIndex;
            finalBoneTransforms[i] = math.mul(rootTransform[cc], math.mul(aa, bb));
        }
    }

#if ENABLE_ANIMATION_BURST
    [BurstCompile]
#endif 

    internal struct SkinDeformBatchedJob : IJobParallelFor
    {
        public NativeSlice<byte> vertices;
        [ReadOnly]
        public NativeArray<float4x4> finalBoneTransforms;
        [ReadOnly]
        public NativeArray<PerSkinJobData> perSkinJobData;
        [ReadOnly]
        public NativeArray<SpriteSkinData> spriteSkinData;
        [ReadOnly]
        public NativeArray<int2> vertexLookupData;

        public unsafe void Execute(int i)
        {
            int j = vertexLookupData[i].x;
            int k = vertexLookupData[i].y;
            PerSkinJobData perSkinData = perSkinJobData[j];
            float3 srcVertex = spriteSkinData[j].vertices[k];
            float4 tangents = spriteSkinData[j].tangents[k];
            var influence = spriteSkinData[j].boneWeights[k];

            int bone0 = influence.boneIndex0 + perSkinData.bindPosesIndex.x;
            int bone1 = influence.boneIndex1 + perSkinData.bindPosesIndex.x;
            int bone2 = influence.boneIndex2 + perSkinData.bindPosesIndex.x;
            int bone3 = influence.boneIndex3 + perSkinData.bindPosesIndex.x;
            var spriteSkin = spriteSkinData[j];
            byte* deformedPosOffset = (byte*)vertices.GetUnsafePtr();
            NativeSlice<float3> deformableVerticesFloat3 = NativeSliceUnsafeUtility.ConvertExistingDataToNativeSlice<float3>(deformedPosOffset + spriteSkin.deformVerticesStartPos, spriteSkin.spriteVertexStreamSize, spriteSkin.spriteVertexCount);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            NativeSliceUnsafeUtility.SetAtomicSafetyHandle(ref deformableVerticesFloat3, NativeSliceUnsafeUtility.GetAtomicSafetyHandle(vertices));
#endif
            if (spriteSkinData[j].hasTangents)
            {
                byte* deformedTanOffset = deformedPosOffset + spriteSkin.tangentVertexOffset + spriteSkin.deformVerticesStartPos;
                var deformableTangentsFloat4 = NativeSliceUnsafeUtility.ConvertExistingDataToNativeSlice<float4>(deformedTanOffset , spriteSkin.spriteVertexStreamSize, spriteSkin.spriteVertexCount);
                var tangent = new float4(tangents.xyz, 0.0f);

                tangent =
                    math.mul(finalBoneTransforms[bone0], tangent) * influence.weight0 +
                    math.mul(finalBoneTransforms[bone1], tangent) * influence.weight1 +
                    math.mul(finalBoneTransforms[bone2], tangent) * influence.weight2 +
                    math.mul(finalBoneTransforms[bone3], tangent) * influence.weight3;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                NativeSliceUnsafeUtility.SetAtomicSafetyHandle(ref deformableTangentsFloat4, NativeSliceUnsafeUtility.GetAtomicSafetyHandle(vertices));
#endif
                deformableTangentsFloat4[k] = new float4(math.normalize(tangent.xyz), tangents.w);
            }
            
            deformableVerticesFloat3[k] =
                math.transform(finalBoneTransforms[bone0], srcVertex) * influence.weight0 +
                math.transform(finalBoneTransforms[bone1], srcVertex) * influence.weight1 +
                math.transform(finalBoneTransforms[bone2], srcVertex) * influence.weight2 +
                math.transform(finalBoneTransforms[bone3], srcVertex) * influence.weight3;
        }
    }

#if ENABLE_ANIMATION_BURST
    [BurstCompile]
#endif 
    internal struct CalculateSpriteSkinAABBJob : IJobParallelFor
    {
        public NativeSlice<byte> vertices;
        public NativeArray<Bounds> bounds;
        [ReadOnly]
        public NativeArray<SpriteSkinData> spriteSkinData;
        public unsafe void Execute(int i)
        {
            var spriteSkin = spriteSkinData[i];
            byte* deformedPosOffset = (byte*)vertices.GetUnsafePtr();
            NativeSlice<float3> deformableVerticesFloat3 = NativeSliceUnsafeUtility.ConvertExistingDataToNativeSlice<float3>(deformedPosOffset + spriteSkin.deformVerticesStartPos, spriteSkin.spriteVertexStreamSize, spriteSkin.spriteVertexCount);
            
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            NativeSliceUnsafeUtility.SetAtomicSafetyHandle(ref deformableVerticesFloat3, NativeSliceUnsafeUtility.GetAtomicSafetyHandle(vertices));
#endif

            bounds[i] = SpriteSkinUtility.CalculateSpriteSkinBounds(deformableVerticesFloat3);
        }
    }    
    internal class SpriteSkinComposite : ScriptableObject
    {

        static SpriteSkinComposite m_Instance;

        public static SpriteSkinComposite instance
        {
            get
            {
                if (m_Instance == null)
                {
                    var composite = Resources.FindObjectsOfTypeAll<SpriteSkinComposite>();
                    if (composite.Length > 0)
                        m_Instance = composite[0];
                    else
                        m_Instance = ScriptableObject.CreateInstance<SpriteSkinComposite>();
                    m_Instance.hideFlags = HideFlags.HideAndDontSave;
                    m_Instance.Init();
                }
                return m_Instance;
            }
        }

        List<SpriteSkin> m_SpriteSkins = new List<SpriteSkin>();
        List<SpriteSkin> m_SpriteSkinLateUpdate = new List<SpriteSkin>();
        DeformVerticesBuffer m_DeformedVerticesBuffer;
        NativeArray<float4x4> m_FinalBoneTransforms;
        SpriteSkinBatchProcessData m_SpriteSkinBatchProcessData;
        
        NativeArray<PerSkinJobData> m_PerSkinJobData;
        NativeArray<SpriteSkinData> m_SpriteSkinData;
        NativeArray<int2> m_BoneLookupData;
        NativeArray<int2> m_VertexLookupData;
        PerSkinJobData m_SkinBatch = new PerSkinJobData();
        TransformAccessJob m_LocalToWorldTransformAccessJob;
        TransformAccessJob m_WorldToLocalTransformAccessJob;
        JobHandle m_BoundJobHandle;
        JobHandle m_DeformJobHandle;
        [SerializeField]
        GameObject m_Helper;

        Action<SpriteRenderer, NativeArray<byte>> SetDeformableBuffer = InternalEngineBridge.SetDeformableBuffer;

        internal Action<SpriteRenderer, NativeArray<byte>> spriteRendererSetDeformableBuffer
        {
            set
            {
                SetDeformableBuffer = value;
                if(SetDeformableBuffer == null)
                    SetDeformableBuffer = InternalEngineBridge.SetDeformableBuffer;
            }
        }

        internal GameObject helperGameObject
        {
            get => m_Helper;
        }

        internal void RemoveTransformById(int transformId)
        {
            m_LocalToWorldTransformAccessJob.RemoveTransformById(transformId);
        }

        internal void AddSpriteSkinBoneTransform(SpriteSkin spriteSkin)
        {
            if (spriteSkin == null)
                return;
            if (spriteSkin.boneTransforms != null)
            {
                foreach (var t in spriteSkin.boneTransforms)
                {
                    if(t != null)
                        m_LocalToWorldTransformAccessJob.AddTransform(t);
                }
            }
        }

        internal void AddSpriteSkinRootBoneTransform(SpriteSkin spriteSkin)
        {
            if (spriteSkin == null || spriteSkin.rootBone == null)
                return;
            m_LocalToWorldTransformAccessJob.AddTransform(spriteSkin.rootBone);
        }

        internal void AddSpriteSkin(SpriteSkin spriteSkin)
        {
            if (spriteSkin == null)
                return;
            bool added = m_SpriteSkins.Contains(spriteSkin);
            Debug.Assert(!added, string.Format("SpriteSkin {0} is already added", spriteSkin.gameObject.name));
            if (!added)
            {
                m_SpriteSkins.Add(spriteSkin);
                m_WorldToLocalTransformAccessJob.AddTransform(spriteSkin.transform);
            }
            
        }

        internal void RemoveSpriteSkin(SpriteSkin spriteSkin)
        {
            m_SpriteSkins.Remove(spriteSkin);
            m_WorldToLocalTransformAccessJob.RemoveTransform(spriteSkin.transform);
        }

        internal void AddSpriteSkinForLateUpdate(SpriteSkin spriteSkin)
        {
            if (spriteSkin != null)
            {
                bool added = m_SpriteSkinLateUpdate.Contains(spriteSkin);
                Debug.Assert( !added, string.Format("SpriteSkin {0} is already added", spriteSkin.gameObject.name));
                if(!added)
                    m_SpriteSkinLateUpdate.Add(spriteSkin);
            }
        }

        internal void RemoveSpriteSkinForLateUpdate(SpriteSkin spriteSkin)
        {
            m_SpriteSkinLateUpdate.Remove(spriteSkin);
        }

        void Init()
        {
            if(m_LocalToWorldTransformAccessJob == null)
                m_LocalToWorldTransformAccessJob = new TransformAccessJob();
            if(m_WorldToLocalTransformAccessJob == null)
                m_WorldToLocalTransformAccessJob = new TransformAccessJob();
        }

        internal void ResetComposite()
        {
            foreach (var spriteSkin in m_SpriteSkins)
                spriteSkin.batchSkinning = false;
            m_SpriteSkins.Clear();
            m_LocalToWorldTransformAccessJob.Destory();
            m_WorldToLocalTransformAccessJob.Destory();
            m_LocalToWorldTransformAccessJob = new TransformAccessJob();
            m_WorldToLocalTransformAccessJob = new TransformAccessJob();
        }

        public void OnEnable()
        {
            m_Instance = this;
            if (m_Helper == null)
            {
                m_Helper = new GameObject("SpriteSkinManager");
                m_Helper.hideFlags = HideFlags.HideAndDontSave;
                m_Helper.AddComponent<SpriteSkinManager.SpriteSkinManagerInternal>();
#if !UNITY_EDITOR
                GameObject.DontDestroyOnLoad(m_Helper);
#endif
            }
            m_SpriteSkinBatchProcessData = new SpriteSkinBatchProcessData(1);

            m_DeformedVerticesBuffer = new DeformVerticesBuffer(DeformVerticesBuffer.k_DefaultBufferSize);
            m_FinalBoneTransforms = new NativeArray<float4x4>(1, Allocator.Persistent);
            m_PerSkinJobData = new NativeArray<PerSkinJobData>(1, Allocator.Persistent);
            m_SpriteSkinData = new NativeArray<SpriteSkinData>(1, Allocator.Persistent);
            m_BoneLookupData = new NativeArray<int2>(1, Allocator.Persistent);
            m_VertexLookupData = new NativeArray<int2>(1, Allocator.Persistent);
            Init();
            foreach (var spriteSkin in m_SpriteSkins)
                spriteSkin.batchSkinning = true;
        }

        private void OnDisable()
        {
            m_DeformJobHandle.Complete();
            m_BoundJobHandle.Complete();
            foreach (var spriteSkin in m_SpriteSkins)
                spriteSkin.batchSkinning = false;
            m_DeformedVerticesBuffer.Dispose();
            m_PerSkinJobData.DisposeIfCreated();
            m_SpriteSkinData.DisposeIfCreated();
            m_BoneLookupData.DisposeIfCreated();
            m_VertexLookupData.DisposeIfCreated();
            m_FinalBoneTransforms.DisposeIfCreated();
            m_SpriteSkinBatchProcessData.Destory();
            if (m_Helper != null)
                GameObject.DestroyImmediate(m_Helper);
            m_LocalToWorldTransformAccessJob.Destory();
            m_WorldToLocalTransformAccessJob.Destory();
        }

        internal unsafe void LateUpdate()
        {
            foreach (var ss in m_SpriteSkinLateUpdate)
            {
                if(ss != null)
                    ss.OnLateUpdate();
            }

            int batchCount = 0;
            m_SkinBatch.verticesIndex = int2.zero;
            m_SkinBatch.bindPosesIndex = int2.zero;
            int vertexBufferSize = 0;

            Profiler.BeginSample("SpriteSkinComposite.PrepareData");
            NativeArrayHelpers.ResizeIfNeeded(ref m_PerSkinJobData, m_SpriteSkins.Count);
            NativeArrayHelpers.ResizeIfNeeded(ref m_SpriteSkinData, m_SpriteSkins.Count);
            m_SpriteSkinBatchProcessData.ResizeIfNeeded(m_SpriteSkins.Count);
            for(int i = 0; i < m_SpriteSkins.Count; ++i)
            {
                Profiler.BeginSample("SpriteSkinComposite.GetSpriteSkinBatchData");
                if (m_SpriteSkins[i] != null)
                {
                    if (m_SpriteSkins[i].GetSpriteSkinBatchData(ref m_SpriteSkinData, ref m_SpriteSkinBatchProcessData, ref m_SkinBatch, ref vertexBufferSize, batchCount, i))
                    {
                        m_PerSkinJobData[batchCount] = m_SkinBatch;
                        ++batchCount;
                    }
                }
                Profiler.EndSample();
            }
            Profiler.EndSample();

            if (batchCount > 0)
            {
                Profiler.BeginSample("SpriteSkinComposite.TransformAccessJob");
                var localToWorldJobHandle = m_LocalToWorldTransformAccessJob.StartLocalToWorldJob();
                var worldToLocalJobHandle = m_WorldToLocalTransformAccessJob.StartWorldToLocalJob();
                Profiler.EndSample();

                NativeArrayHelpers.ResizeIfNeeded(ref m_FinalBoneTransforms, m_SkinBatch.bindPosesIndex.y);
                var deformVertices = m_DeformedVerticesBuffer.GetBuffer(vertexBufferSize);
                NativeArrayHelpers.ResizeIfNeeded(ref m_BoneLookupData, m_SkinBatch.bindPosesIndex.y);
                NativeArrayHelpers.ResizeIfNeeded(ref m_VertexLookupData, m_SkinBatch.verticesIndex.y);
                var jobHandle = JobHandle.CombineDependencies(localToWorldJobHandle, worldToLocalJobHandle);

                Profiler.BeginSample("SpriteSkin.Prepare");
                PrepareDeformJob prepareJob = new PrepareDeformJob
                {
                    batchDataSize = batchCount,
                    spriteSkinData = m_SpriteSkinData,
                    perSkinJobData = m_PerSkinJobData,
                    boneLookupData = m_BoneLookupData,
                    vertexLookupData = m_VertexLookupData
                };
                jobHandle = prepareJob.Schedule();
                Profiler.EndSample();

                BoneDeformBatchedJob boneJobBatched = new BoneDeformBatchedJob()
                {
                    boneTransform = m_LocalToWorldTransformAccessJob.transformMatrix,
                    rootTransformId = m_SpriteSkinBatchProcessData.rootTransformId,
                    rootTransform = m_WorldToLocalTransformAccessJob.transformMatrix,
                    spriteSkinData = m_SpriteSkinData,
                    boneLookupData = m_BoneLookupData,
                    finalBoneTransforms = m_FinalBoneTransforms,
                    rootTransformIndex = m_WorldToLocalTransformAccessJob.transformData,
                    boneTransformIndex = m_LocalToWorldTransformAccessJob.transformData
                };

                jobHandle = JobHandle.CombineDependencies(localToWorldJobHandle, jobHandle, worldToLocalJobHandle);
                jobHandle = boneJobBatched.Schedule(m_SkinBatch.bindPosesIndex.y, 8, jobHandle);

                SkinDeformBatchedJob skinJobBatched = new SkinDeformBatchedJob()
                {
                    vertices = deformVertices,
                    vertexLookupData = m_VertexLookupData,
                    spriteSkinData = m_SpriteSkinData,
                    perSkinJobData = m_PerSkinJobData,
                    finalBoneTransforms = m_FinalBoneTransforms,
                };
                m_DeformJobHandle = skinJobBatched.Schedule(m_SkinBatch.verticesIndex.y, 16, jobHandle);
                
                CalculateSpriteSkinAABBJob updateBoundJob = new CalculateSpriteSkinAABBJob
                {
                    vertices = deformVertices,
                    spriteSkinData = m_SpriteSkinData,
                    bounds = m_SpriteSkinBatchProcessData.newSpriteBound,
                };
                m_BoundJobHandle = updateBoundJob.Schedule(batchCount, 16, m_DeformJobHandle);                
                
                JobHandle.ScheduleBatchedJobs();
                m_BoundJobHandle.Complete();
                Profiler.BeginSample("WriteBack");
                byte* ptrVertices = (byte*)deformVertices.GetUnsafeReadOnlyPtr();
                for (int i = 0; i < batchCount; ++i)
                {
                    var skinData = m_SpriteSkinData[i];
                    var vertexBufferLength = skinData.spriteVertexCount * skinData.spriteVertexStreamSize;
                    var copyFrom = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(ptrVertices, vertexBufferLength, Allocator.None);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref copyFrom, NativeArrayUnsafeUtility.GetAtomicSafetyHandle(deformVertices));
#endif
                    SetDeformableBuffer(m_SpriteSkins[skinData.spriteSkinIndex].spriteRenderer, copyFrom);
                    m_SpriteSkins[skinData.spriteSkinIndex].bounds = m_SpriteSkinBatchProcessData.newSpriteBound[i];
                    InternalEngineBridge.SetLocalAABB(m_SpriteSkins[skinData.spriteSkinIndex].spriteRenderer, m_SpriteSkinBatchProcessData.newSpriteBound[i]);
                    ptrVertices = ptrVertices + vertexBufferLength;
                }
                Profiler.EndSample();
            }
        }

        internal unsafe NativeArray<byte> GetDeformableBufferForSprite(int dataIndex)
        {
            if (dataIndex < 0 && m_SpriteSkinData.Length >= dataIndex)
                throw new ArgumentException("Invalid index for deformable buffer");
            
            if (!m_DeformJobHandle.IsCompleted)
                m_DeformJobHandle.Complete();

            var skinData = m_SpriteSkinData[dataIndex];
            var vertexBufferLength = skinData.spriteVertexCount * skinData.spriteVertexStreamSize;
            var deformVertices = m_DeformedVerticesBuffer.GetCurrentBuffer();
            byte* ptrVertices = (byte*)deformVertices.GetUnsafeReadOnlyPtr();
            ptrVertices += skinData.deformVerticesStartPos; 
            var buffer = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(ptrVertices, vertexBufferLength, Allocator.None);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref buffer, NativeArrayUnsafeUtility.GetAtomicSafetyHandle(deformVertices));
#endif
            return buffer;
        }

        // Code for tests
        internal string GetDebugLog()
        {
            var log = "";
            log = "====SpriteSkinLateUpdate===\n";
            log += "Count: " + m_SpriteSkinLateUpdate.Count +"\n";
            foreach (var ss in m_SpriteSkinLateUpdate)
            {
                log += ss == null ? "null" : ss.name;
                log += "\n";
            }
            log += "\n";

            log += "===SpriteSkinBatch===\n";
            log += "Count: " + m_SpriteSkins.Count +"\n";
            foreach (var ss in m_SpriteSkins)
            {
                log += ss == null ? "null" : ss.name;
                log += "\n";
            }

            log += "===LocalToWorldTransformAccessJob===\n";
            log += m_LocalToWorldTransformAccessJob.GetDebugLog();
            log += "\n";
            log += "===WorldToLocalTransformAccessJob===\n";
            log += "\n";
            log += m_WorldToLocalTransformAccessJob.GetDebugLog();
            return log;
        }


        internal SpriteSkin[] GetSpriteSkins()
        {
            return m_SpriteSkins.ToArray();
        }

        internal TransformAccessJob GetWorldToLocalTransformAccessJob()
        {
            return m_WorldToLocalTransformAccessJob;
        }

        internal TransformAccessJob GetLocalToWorldTransformAccessJob()
        {
            return m_LocalToWorldTransformAccessJob;
        }
    }
}
#endif