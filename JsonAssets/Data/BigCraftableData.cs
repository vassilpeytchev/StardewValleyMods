using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text;

using JsonAssets.Framework;
using JsonAssets.Framework.Internal;

using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SpaceShared;

namespace JsonAssets.Data
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = DiagnosticMessages.IsPublicApi)]
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = DiagnosticMessages.IsPublicApi)]
    [DebuggerDisplay("name = {Name}, id = {Id}")]
    public class BigCraftableData : DataNeedsIdWithTexture, ITranslatableItem
    {
        [JsonIgnore]
        internal static HashSet<int> HasHoneyInName { get; } = new();

        /*********
        ** Accessors
        *********/
        [JsonIgnore]
        public Texture2D[] ExtraTextures { get; set; }

        public bool ReserveNextIndex { get; set; } = false; // Deprecated
        public int ReserveExtraIndexCount { get; set; } = 0;

        /// <inheritdoc />
        public string Description { get; set; }

        public int Price { get; set; }

        public bool ProvidesLight { get; set; } = false;

        public BigCraftableRecipe Recipe { get; set; }

        public bool CanPurchase { get; set; } = false;
        public int PurchasePrice { get; set; }
        public string PurchaseFrom { get; set; } = "Pierre";
        public IList<string> PurchaseRequirements { get; set; } = new List<string>();
        public IList<PurchaseData> AdditionalPurchaseData { get; set; } = new List<PurchaseData>();

        /// <inheritdoc />
        public Dictionary<string, string> NameLocalization { get; set; } = new();

        /// <inheritdoc />
        public Dictionary<string, string> DescriptionLocalization { get; set; } = new();

        /// <inheritdoc />
        public string TranslationKey { get; set; }


        /*********
        ** Public methods
        *********/
        public int GetCraftableId()
        {
            return this.Id;
        }

        internal string GetCraftableInformation()
        {
            StringBuilder str = StringBuilderCache.Acquire();

            str.Append(this.Name).Append('/')
                .Append(this.Price).Append("/-300/Crafting -9/")
                .Append(this.LocalizedDescription())
                .Append("/true/true/0");
            if (this.ProvidesLight)
                str.Append("/true");
            str.Append($"/{this.LocalizedName()}");
            return StringBuilderCache.GetStringAndRelease(str);
        }

        /*********
        ** Private methods
        *********/
        /// <summary>Normalize the model after it's deserialized.</summary>
        /// <param name="context">The deserialization context.</param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            this.ExtraTextures ??= Array.Empty<Texture2D>();
            this.PurchaseRequirements ??= new List<string>();
            this.AdditionalPurchaseData ??= new List<PurchaseData>();
            this.NameLocalization ??= new();
            this.DescriptionLocalization ??= new();

            this.PurchaseRequirements.FilterNulls();
            this.AdditionalPurchaseData.FilterNulls();
        }
    }
}
