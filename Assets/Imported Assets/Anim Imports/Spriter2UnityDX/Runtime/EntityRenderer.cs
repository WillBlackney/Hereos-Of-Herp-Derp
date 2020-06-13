using UnityEngine;
using System;
using System.Collections.Generic;

namespace Spriter2UnityDX {
	[DisallowMultipleComponent, ExecuteInEditMode, AddComponentMenu("")]
	public class EntityRenderer : MonoBehaviour
    {
		public enum MaskInteractionn { None, VisibleInsideMask, VisibleOutsideMask};

        [SerializeField] public LivingEntity myEntity;
		private SpriteRenderer[] renderers = new SpriteRenderer [0];
		private SortingOrderUpdater[] updaters = new SortingOrderUpdater [0];
		private SpriteRenderer _first;
		private SpriteRenderer first {
			get {
				if (_first == null && renderers.Length > 0)
					_first = renderers [0];
				return _first;
			}
		}
		public Color Color {
			get { return (first != null) ? first.color : default(Color); }
			set { DoForAll (x => x.color = value); }
		}

		public Material Material {
			get { return (first != null) ? first.sharedMaterial : null; }
			set { DoForAll (x => x.sharedMaterial = value); }
		}

		public int SortingLayerID {
			get { return (first != null) ? first.sortingLayerID : 0; }
			set { DoForAll (x => x.sortingLayerID = value); }
		}

		public string SortingLayerName {
			get { return (first != null) ? first.sortingLayerName : null; }
			set { DoForAll (x => x.sortingLayerName = value); }
		}

		[SerializeField, HideInInspector] private int sortingOrder = 0;
		public int SortingOrder {
			get { return sortingOrder; }
			set { 
				sortingOrder = value;
				if (applySpriterZOrder)
					for (var i = 0; i < updaters.Length; i++)
						updaters [i].SortingOrder = value;
				else DoForAll (x => x.sortingOrder = value + x.GetComponent<UniversalCharacterModelElement>().sortingOrderBonus);
			}
		}

		[SerializeField, HideInInspector] private bool visibleOutsideMask;
		public bool VisibleWithinMask
		{
			get { return visibleOutsideMask; }
			set
			{
				visibleOutsideMask = value;
				
				if (visibleOutsideMask)
				{
					DoForAll(x => x.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask);
				}
				else
				{
					DoForAll(x => x.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None);
				}
				

			}
		}

		[SerializeField, HideInInspector] private bool applySpriterZOrder = false;
		public bool ApplySpriterZOrder {
			get { return applySpriterZOrder; }
			set { 
				applySpriterZOrder = value;
				if (applySpriterZOrder) {
					var list = new List<SortingOrderUpdater> ();
					var spriteCount = renderers.Length;
					foreach (var renderer in renderers) {
						var updater = renderer.GetComponent<SortingOrderUpdater> ();
						if (updater == null) updater = renderer.gameObject.AddComponent<SortingOrderUpdater> ();
						updater.SortingOrder = sortingOrder;
						updater.SpriteCount = spriteCount;
						list.Add (updater);
					}
					updaters = list.ToArray ();
				}
				else {
					for (var i = 0; i < updaters.Length; i++) {
						if (Application.isPlaying) Destroy (updaters [i]);
						else DestroyImmediate (updaters [i]);
					}
					updaters = new SortingOrderUpdater [0];
					DoForAll (x => x.sortingOrder = sortingOrder);
				}
			}
		}

		private void Awake () {
			RefreshRenders ();
            myEntity = GetComponentInParent<LivingEntity>();
		}
		private void Start()
		{
			int refreshValue = SortingOrder;
			SortingOrder = refreshValue;
		}

		private void OnEnable () {
			DoForAll (x => x.enabled = true);
		}

		private void OnDisable () {
			DoForAll (x => x.enabled = false);
		}
		
		private void DoForAll (Action<SpriteRenderer> action) {
			for (var i = 0; i < renderers.Length; i++) action (renderers [i]);
		}

		public void RefreshRenders () {
			renderers = GetComponentsInChildren<SpriteRenderer> (true);
			updaters = GetComponentsInChildren<SortingOrderUpdater> (true);
			var length = updaters.Length;
			for (var i = 0; i < length; i++) updaters [i].SpriteCount = length;
			_first = null;

			Debug.Log("EntityRenderer.RefreshRenders() found " + renderers.Length + " sprite renderers on its UC Model...");
		}

        public void SetDeathAnimationAsFinished()
        {
            myEntity.SetDeathAnimAsFinished();
        }
        public void SetRangedAttackAnimAsFinished()
        {
            myEntity.SetRangedAttackAnimAsFinished();
        }
        public void RefreshRangedAttackBool()
        {
            myEntity.RefreshRangedAttackBool();
        }
    }
}
