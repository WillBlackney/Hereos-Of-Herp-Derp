using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TMPro
{
    /// <summary>
    /// 
    /// </summary>
    public class TMP_ResourceManager
    {
        private static TMP_ResourceManager instance { get { return s_instance; } }

        private static Dictionary<int, TMP_FontAsset> s_FontAssetReferenceLookup = new Dictionary<int, TMP_FontAsset>();
        private static readonly TMP_ResourceManager s_instance = new TMP_ResourceManager();

        static TMP_ResourceManager() { }

        // ======================================================
        // FONT ASSET MANAGEMENT - Fields, Properties and Functions
        // ======================================================

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fontAsset"></param>
        public static void AddFontAsset(TMP_FontAsset fontAsset)
        {
            int hashcode = fontAsset.hashCode;

            if (s_FontAssetReferenceLookup.ContainsKey(hashcode))
                return;

            s_FontAssetReferenceLookup.Add(hashcode, fontAsset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hashcode"></param>
        /// <param name="fontAsset"></param>
        /// <returns></returns>
        public static bool TryGetFontAsset(int hashcode, out TMP_FontAsset fontAsset)
        {
            fontAsset = null;

            if (s_FontAssetReferenceLookup.TryGetValue(hashcode, out fontAsset))
                return true;

            return false;
        }
    }
}
