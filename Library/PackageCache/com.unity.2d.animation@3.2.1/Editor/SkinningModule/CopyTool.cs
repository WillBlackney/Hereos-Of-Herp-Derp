using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEditor.U2D.Layout;

namespace UnityEditor.U2D.Animation
{
    internal interface ICopyToolStringStore
    {
        string stringStore
        {
            get;
            set;
        }
    }

    internal class SystemCopyBufferStringStore : ICopyToolStringStore
    {
        public string stringStore
        {
            get { return EditorGUIUtility.systemCopyBuffer; }
            set { EditorGUIUtility.systemCopyBuffer = value; }
        }
    }

    internal class CopyTool : MeshToolWrapper
    {
        public class NewBonesStore
        {
            public BoneCache[] newBones;
            public Dictionary<string, string> newBoneNameDict;
            public NewBonesStore()
            {
                newBones = null;
                newBoneNameDict = new Dictionary<string, string>();
            }

            public void MapAllExistingBones()
            {
                foreach (var bone in newBones)
                    newBoneNameDict.Add(bone.name, bone.name);                
            }
        }
        
        private ICopyToolStringStore m_CopyToolStringStore;
        private CopyToolView m_CopyToolView;

        public float pixelsPerUnit
        {
            private get;
            set;
        }

        public ICopyToolStringStore copyToolStringStore
        {
            set { m_CopyToolStringStore = value; }
        }

        internal override void OnCreate()
        {
            m_CopyToolView = new CopyToolView();
            m_CopyToolView.onPasteActivated += OnPasteActivated;
            m_CopyToolStringStore = new SystemCopyBufferStringStore();
        }

        public override void Initialize(LayoutOverlay layout)
        {
            m_CopyToolView.Initialize(layout);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            m_CopyToolView.Show();
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
            m_CopyToolView.Hide();
        }

        private void CopyMeshFromSpriteCache(SpriteCache sprite, SkinningCopySpriteData skinningSpriteData)
        {
            if (meshTool == null)
                return;

            meshTool.SetupSprite(sprite);
            skinningSpriteData.vertices = meshTool.mesh.vertices;
            skinningSpriteData.indices = meshTool.mesh.indices;
            skinningSpriteData.edges = meshTool.mesh.edges;
            skinningSpriteData.boneWeightNames = new List<string>();
            foreach (var bone in meshTool.mesh.bones)
            {
                skinningSpriteData.boneWeightNames.Add(bone.name);
            }
        }

        public void OnCopyActivated()
        {
            SkinningCopyData skinningCopyData;
            var selectedSprite = skinningCache.selectedSprite;
            if (selectedSprite == null)
            {
                skinningCopyData = CopyAll();
            }
            else
            {
                skinningCopyData = CopySingle(selectedSprite);
            }

            if (skinningCopyData != null)
                m_CopyToolStringStore.stringStore = SkinningCopyUtility.SerializeSkinningCopyDataToString(skinningCopyData);
            skinningCache.events.copy.Invoke();
        }

        public SkinningCopyData CopyAll()
        {
            var skinningCopyData = new SkinningCopyData();
            skinningCopyData.pixelsPerUnit = pixelsPerUnit;

            var sprites = skinningCache.GetSprites();
            foreach (var sprite in sprites)
            {
                var skinningSpriteData = new SkinningCopySpriteData();
                skinningSpriteData.spriteName = sprite.name;

                var skeleton = skinningCache.GetEffectiveSkeleton(sprite);
                if (skeleton != null && skeleton.BoneCount > 0)
                {
                    if (skinningCache.hasCharacter)
                        skinningSpriteData.spriteBones = skeleton.bones.ToSpriteBone(Matrix4x4.identity).ToList();
                    else
                    {
                        skinningSpriteData.spriteBones = new List<UnityEngine.U2D.SpriteBone>();
                        var bones = skeleton.bones.FindRoots();
                        foreach (var bone in bones)
                            GetSpriteBoneDataRecursively(skinningSpriteData.spriteBones, bone);
                    }
                }
                if (meshTool != null)
                {
                    CopyMeshFromSpriteCache(sprite, skinningSpriteData);
                }
                skinningCopyData.copyData.Add(skinningSpriteData);
            }

            if (meshTool != null)
            {
                meshTool.SetupSprite(null);
            }

            return skinningCopyData;
        }

        public SkinningCopyData CopySingle(SpriteCache sprite)
        {
            var skinningCopyData = new SkinningCopyData();
            skinningCopyData.pixelsPerUnit = pixelsPerUnit;

            // Mesh
            var skinningSpriteData = new SkinningCopySpriteData();
            skinningCopyData.copyData.Add(skinningSpriteData);

            CopyMeshFromSpriteCache(sprite, skinningSpriteData);

            // Bones
            var rootBones = new List<BoneCache>();
            if (skinningCache.hasCharacter)
            {
                var characterPart = skinningCache.GetCharacterPart(sprite);
                if (characterPart != null && characterPart.bones != null)
                {
                    var bones = characterPart.bones.FindRoots();
                    foreach (var bone in bones)
                        rootBones.Add(bone);
                }
            }
            else
            {
                var skeleton = skinningCache.GetEffectiveSkeleton(sprite);
                if (skeleton != null && skeleton.BoneCount > 0)
                {
                    var bones = skeleton.bones.FindRoots();
                    foreach (var bone in bones)
                        rootBones.Add(bone);
                }
            }

            if (rootBones.Count > 0)
            {
                skinningSpriteData.spriteBones = new List<UnityEngine.U2D.SpriteBone>();
                foreach (var rootBone in rootBones)
                {
                    var rootBoneIndex = skinningSpriteData.spriteBones.Count;
                    GetSpriteBoneDataRecursively(skinningSpriteData.spriteBones, rootBone);
                    if (skinningCache.hasCharacter)
                    {
                        // Offset the bones based on the currently selected Sprite in Character mode
                        var characterPart = sprite.GetCharacterPart();
                        if (characterPart != null)
                        {
                            var offset = characterPart.position;
                            var rootSpriteBone = skinningSpriteData.spriteBones[rootBoneIndex];
                            rootSpriteBone.position = rootSpriteBone.position - offset;
                            skinningSpriteData.spriteBones[rootBoneIndex] = rootSpriteBone;
                        }
                    }
                }
            }

            return skinningCopyData;
        }

        private void GetSpriteBoneDataRecursively(List<UnityEngine.U2D.SpriteBone> bones, BoneCache rootBone)
        {
            AppendSpriteBoneDataRecursively(bones, rootBone, -1);
        }

        private void AppendSpriteBoneDataRecursively(List<UnityEngine.U2D.SpriteBone> spriteBones, BoneCache bone, int parentIndex)
        {
            int currentParentIndex = spriteBones.Count;

            var spriteBone = new UnityEngine.U2D.SpriteBone();
            spriteBone.name = bone.name;
            spriteBone.parentId = parentIndex;
            if (parentIndex == -1 && bone.parentBone != null)
            {
                spriteBone.position = bone.position;
                spriteBone.rotation = bone.rotation;
            }
            else
            {
                spriteBone.position = bone.localPosition;
                spriteBone.rotation = bone.localRotation;
            }
            spriteBone.position = new Vector3(spriteBone.position.x, spriteBone.position.y, bone.depth);

            spriteBone.length = bone.localLength;
            spriteBones.Add(spriteBone);

            foreach (var child in bone)
            {
                var childBone = child as BoneCache;
                if (childBone != null)
                    AppendSpriteBoneDataRecursively(spriteBones, childBone, currentParentIndex);
            }
        }

        public void OnPasteActivated(bool bone, bool mesh, bool flipX, bool flipY)
        {
            var copyBuffer = m_CopyToolStringStore.stringStore;
            if (!SkinningCopyUtility.CanDeserializeStringToSkinningCopyData(copyBuffer))
            {
                Debug.LogError(TextContent.copyError1);
                return;
            }

            var skinningCopyData = SkinningCopyUtility.DeserializeStringToSkinningCopyData(copyBuffer);
            if (skinningCopyData == null || skinningCopyData.copyData.Count == 0)
            {
                Debug.LogError(TextContent.copyError2);
                return;
            }

            var scale = 1f;
            if (skinningCopyData.pixelsPerUnit > 0f)
                scale = pixelsPerUnit / skinningCopyData.pixelsPerUnit;

            var sprites = skinningCache.GetSprites();
            var copyMultiple = skinningCopyData.copyData.Count > 1;
            if (copyMultiple && skinningCopyData.copyData.Count != sprites.Length && mesh)
            {
                Debug.LogError(String.Format(TextContent.copyError3, sprites.Length, skinningCopyData.copyData.Count));
                return;
            }

            using (skinningCache.UndoScope(TextContent.pasteData))
            {
                NewBonesStore newBonesStore = null;
                if (bone && copyMultiple && skinningCache.hasCharacter)
                {
                    newBonesStore = new NewBonesStore();
                    var skinningSpriteData = skinningCopyData.copyData[0];
                    newBonesStore.newBones = skinningCache.CreateBoneCacheFromSpriteBones(skinningSpriteData.spriteBones.ToArray(), scale);
                    if (flipX || flipY)
                    {
                        var characterRect = new Rect(Vector2.zero, skinningCache.character.dimension);
                        var newPositions = new Vector3[newBonesStore.newBones.Length];
                        var newRotations = new Quaternion[newBonesStore.newBones.Length];
                        for (var i = 0; i < newBonesStore.newBones.Length; ++i)
                        {
                            newPositions[i] = GetFlippedBonePosition(newBonesStore.newBones[i], Vector2.zero, characterRect, flipX, flipY);
                            newRotations[i] = GetFlippedBoneRotation(newBonesStore.newBones[i], flipX, flipY);
                        }
                        for (var i = 0; i < newBonesStore.newBones.Length; ++i)
                        {
                            newBonesStore.newBones[i].position = newPositions[i];
                            newBonesStore.newBones[i].rotation = newRotations[i];
                        }
                    }
                    newBonesStore.MapAllExistingBones();
                    var skeleton = skinningCache.character.skeleton;
                    skeleton.SetBones(newBonesStore.newBones);
                    skinningCache.events.skeletonTopologyChanged.Invoke(skeleton);
                }

                foreach (var skinningSpriteData in skinningCopyData.copyData)
                {
                    SpriteCache sprite = null;
                    if (!String.IsNullOrEmpty(skinningSpriteData.spriteName))
                    {
                        sprite = sprites.FirstOrDefault(x => x.name == skinningSpriteData.spriteName);
                    }
                    if (sprite == null && (skinningCopyData.copyData.Count == 1 || String.IsNullOrEmpty(skinningSpriteData.spriteName)))
                    {
                        sprite = skinningCache.selectedSprite;
                    }
                    if (sprite == null)
                        continue;

                    if (bone && (!skinningCache.hasCharacter || !copyMultiple))
                    {
                        newBonesStore = PasteSkeletonBones(sprite, skinningSpriteData.spriteBones, flipX, flipY, scale);
                    }

                    if (mesh && meshTool != null)
                    {
                        PasteMesh(sprite, skinningSpriteData, flipX, flipY, scale, newBonesStore);
                    }
                }

                if (newBonesStore != null && newBonesStore.newBones != null)
                {
                    skinningCache.skeletonSelection.elements = newBonesStore.newBones;
                    skinningCache.events.boneSelectionChanged.Invoke();
                }
            }
            skinningCache.events.paste.Invoke(bone, mesh, flipX, flipY);
        }

        private Vector3 GetFlippedBonePosition(BoneCache bone, Vector2 startPosition, Rect spriteRect
            , bool flipX, bool flipY)
        {
            Vector3 position = startPosition;
            if (flipX)
            {
                position.x += spriteRect.width - bone.position.x;
            }
            else
            {
                position.x += bone.position.x;
            }

            if (flipY)
            {
                position.y += spriteRect.height - bone.position.y;
            }
            else
            {
                position.y += bone.position.y;
            }

            position.z = bone.position.z;
            return position;
        }

        private Quaternion GetFlippedBoneRotation(BoneCache bone, bool flipX, bool flipY)
        {
            var euler = bone.rotation.eulerAngles;
            if (flipX)
            {
                if (euler.z <= 180)
                {
                    euler.z = 180 - euler.z;
                }
                else
                {
                    euler.z = 540 - euler.z;
                }
            }
            if (flipY)
            {
                euler.z = 360 - euler.z;
            }
            return Quaternion.Euler(euler);
        }

        public NewBonesStore PasteSkeletonBones(SpriteCache sprite, List<UnityEngine.U2D.SpriteBone> spriteBones, bool flipX, bool flipY, float scale = 1.0f)
        {
            NewBonesStore newBonesStore = new NewBonesStore();
            newBonesStore.newBones = skinningCache.CreateBoneCacheFromSpriteBones(spriteBones.ToArray(), scale);
            if (newBonesStore.newBones.Length == 0)
                return null;

            if (sprite == null || (skinningCache.mode == SkinningMode.SpriteSheet && skinningCache.hasCharacter))
                return null;

            var spriteRect = sprite.textureRect;
            var skeleton = skinningCache.GetEffectiveSkeleton(sprite);

            var rectPosition = spriteRect.position;
            if (skinningCache.mode == SkinningMode.Character)
            {
                var characterPart = sprite.GetCharacterPart();
                if (characterPart == null)
                    return null;
                rectPosition = characterPart.position;
            }

            var newPositions = new Vector3[newBonesStore.newBones.Length];
            var newRotations = new Quaternion[newBonesStore.newBones.Length];
            for (var i = 0; i < newBonesStore.newBones.Length; ++i)
            {
                newPositions[i] = GetFlippedBonePosition(newBonesStore.newBones[i], rectPosition, spriteRect, flipX, flipY);
                newRotations[i] = GetFlippedBoneRotation(newBonesStore.newBones[i], flipX, flipY);
            }
            for (var i = 0; i < newBonesStore.newBones.Length; ++i)
            {
                newBonesStore.newBones[i].position = newPositions[i];
                newBonesStore.newBones[i].rotation = newRotations[i];
            }

            if (skinningCache.mode == SkinningMode.SpriteSheet)
            {
                newBonesStore.MapAllExistingBones();
                skeleton.SetBones(newBonesStore.newBones);
            }
            else
            {
                var existingBoneNames = skeleton.bones.Select(x => x.name).ToList();

                skeleton.AddBones(newBonesStore.newBones);

                var bones = skeleton.bones;

                // Update names of all newly pasted bones
                foreach (var bone in newBonesStore.newBones)
                {
                    if (existingBoneNames.Contains(bone.name))
                    {
                        var oldBoneName = bone.name;
                        bone.name = SkeletonController.AutoBoneName(bone.parentBone, bones);
                        existingBoneNames.Add(bone.name);
                        newBonesStore.newBoneNameDict.Add(oldBoneName, bone.name);
                    }
                    else
                    {
                        newBonesStore.newBoneNameDict.Add(bone.name, bone.name);
                    }
                }

                skeleton.SetDefaultPose();
            }

            skinningCache.events.skeletonTopologyChanged.Invoke(skeleton);
            return newBonesStore;
        }

        public void PasteMesh(SpriteCache sprite, SkinningCopySpriteData skinningSpriteData, bool flipX, bool flipY, float scale, NewBonesStore newBonesStore)
        {
            if (sprite == null)
                return;

            meshTool.SetupSprite(sprite);
            meshTool.mesh.vertices = skinningSpriteData.vertices;
            if (!Mathf.Approximately(scale, 1f) || flipX || flipY)
            {
                var spriteRect = sprite.textureRect;
                foreach (var vertex in meshTool.mesh.vertices)
                {
                    var position = vertex.position;
                    if (!Mathf.Approximately(scale, 1f))
                        position = position * scale;
                    if (flipX)
                        position.x = spriteRect.width - vertex.position.x;
                    if (flipY)
                        position.y = spriteRect.height - vertex.position.y;
                    vertex.position = position;
                }
            }
            meshTool.mesh.indices = skinningSpriteData.indices;
            meshTool.mesh.edges = skinningSpriteData.edges;

            int[] copyBoneToNewBones = new int[skinningSpriteData.boneWeightNames.Count];
            BoneCache[] setBones = null;

            if (newBonesStore != null && newBonesStore.newBones != null)
            {
                // Update bone weights with new bone indices
                var setBonesList = new List<BoneCache>();
                copyBoneToNewBones = new int[skinningSpriteData.boneWeightNames.Count];
                int index = 0;
                for (int i = 0; i < skinningSpriteData.boneWeightNames.Count; ++i)
                {
                    string oldBoneName = skinningSpriteData.boneWeightNames[i];
                    string newBoneName;
                    newBonesStore.newBoneNameDict.TryGetValue(oldBoneName, out newBoneName);
                    var newBone = newBonesStore.newBones.FirstOrDefault(bone => bone.name == newBoneName);
                    copyBoneToNewBones[i] = -1;
                    if (newBone == null)
                        continue;

                    for (int j = 0; j < skinningSpriteData.spriteBones.Count; ++j)
                    {
                        if (skinningSpriteData.spriteBones[j].name == oldBoneName)
                        {
                            copyBoneToNewBones[i] = index++;
                            setBonesList.Add(newBone);
                            break;
                        }
                    }
                }
                setBones = setBonesList.ToArray();
            }
            else
            {
                // Attempt to link weights based on existing bone names
                var skeleton = skinningCache.GetEffectiveSkeleton(sprite);
                var characterBones = new List<BoneCache>();
                for (int i = 0; i < skinningSpriteData.boneWeightNames.Count; ++i)
                {
                    copyBoneToNewBones[i] = -1;
                    var boneName = skinningSpriteData.boneWeightNames[i];
                    for (int j = 0; j < skeleton.bones.Length; ++j)
                    {
                        if (skeleton.bones[j].name == boneName)
                        {
                            copyBoneToNewBones[i] = characterBones.Count;
                            characterBones.Add(skeleton.bones[j]);
                            break;
                        }
                    }
                }
                setBones = characterBones.ToArray();
            }

            // Remap new bone indexes from copied bone indexes
            foreach (var vertex in meshTool.mesh.vertices)
            {
                var editableBoneWeight = vertex.editableBoneWeight;

                for (var i = 0; i < editableBoneWeight.Count; ++i)
                {
                    if (!editableBoneWeight[i].enabled)
                        continue;

                    var boneIndex = copyBoneToNewBones[editableBoneWeight[i].boneIndex];
                    if (boneIndex != -1)
                        editableBoneWeight[i].boneIndex = boneIndex;
                }
            }

            // Update associated bones for mesh
            meshTool.mesh.SetCompatibleBoneSet(setBones);
            meshTool.mesh.bones = setBones; // Fixes weights for bones that do not exist                

            // Update associated bones for character
            if (skinningCache.hasCharacter)
            {
                var characterPart = sprite.GetCharacterPart();
                if (characterPart != null)
                {
                    characterPart.bones = setBones;
                    skinningCache.events.characterPartChanged.Invoke(characterPart);
                }
            }

            meshTool.UpdateMesh();
        }
    }

    internal class CopyToolView
    {
        private PastePanel m_PastePanel;

        public event Action<bool, bool, bool, bool> onPasteActivated = (bone, mesh, flipX, flipY) => {};

        public void Show()
        {
            m_PastePanel.SetHiddenFromLayout(false);
        }

        public void Hide()
        {
            m_PastePanel.SetHiddenFromLayout(true);
        }

        public void Initialize(LayoutOverlay layoutOverlay)
        {
            m_PastePanel = PastePanel.GenerateFromUXML();
            BindElements();
            layoutOverlay.rightOverlay.Add(m_PastePanel);
            m_PastePanel.SetHiddenFromLayout(true);
        }

        void BindElements()
        {
            m_PastePanel.onPasteActivated += OnPasteActivated;
        }

        void OnPasteActivated(bool bone, bool mesh, bool flipX, bool flipY)
        {
            onPasteActivated(bone, mesh, flipX, flipY);
        }
    }
}
