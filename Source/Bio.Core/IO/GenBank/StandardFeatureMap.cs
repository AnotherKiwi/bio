using System;
using System.Collections.Generic;

namespace Bio.IO.GenBank
{
    /// <summary>
    /// Class to map each standard feature key to the class which can hold that feature.
    /// Note that the classes which can hold feature has to be derived from FeatureItem class.
    /// </summary>
    public static class StandardFeatureMap
    {
        // Dictionary hold feature key to class map.
        private static Dictionary<string, Type> featureMap;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static StandardFeatureMap()
        {
            featureMap = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
            {
                { StandardFeatureKeys.Minus10Signal, typeof(Minus10Signal) },
                { StandardFeatureKeys.Minus35Signal, typeof(Minus35Signal) },
                { StandardFeatureKeys.ThreePrimeUtr, typeof(ThreePrimeUtr) },
                { StandardFeatureKeys.FivePrimeUtr, typeof(FivePrimeUtr) },
                { StandardFeatureKeys.Attenuator, typeof(Attenuator) },
                { StandardFeatureKeys.CaaTSignal, typeof(CaatSignal) },
                { StandardFeatureKeys.CodingSequence, typeof(CodingSequence) },
                { StandardFeatureKeys.DisplacementLoop, typeof(DisplacementLoop) },
                { StandardFeatureKeys.Enhancer, typeof(Enhancer) },
                { StandardFeatureKeys.Exon, typeof(Exon) },
                { StandardFeatureKeys.GcSingal, typeof(GcSingal) },
                { StandardFeatureKeys.Gene, typeof(Gene) },
                { StandardFeatureKeys.InterveningDna, typeof(InterveningDna) },
                { StandardFeatureKeys.Intron, typeof(Intron) },
                { StandardFeatureKeys.LongTerminalRepeat, typeof(LongTerminalRepeat) },
                { StandardFeatureKeys.MaturePeptide, typeof(MaturePeptide) },
                { StandardFeatureKeys.MiscBinding, typeof(MiscBinding) },
                { StandardFeatureKeys.MiscDifference, typeof(MiscDifference) },
                { StandardFeatureKeys.MiscFeature, typeof(MiscFeature) },
                { StandardFeatureKeys.MiscRecombination, typeof(MiscRecombination) },
                { StandardFeatureKeys.MiscRna, typeof(MiscRna) },
                { StandardFeatureKeys.MiscSignal, typeof(MiscSignal) },
                { StandardFeatureKeys.MiscStructure, typeof(MiscStructure) },
                { StandardFeatureKeys.ModifiedBase, typeof(ModifiedBase) },
                { StandardFeatureKeys.MessengerRna, typeof(MessengerRna) },
                { StandardFeatureKeys.NonCodingRna, typeof(NonCodingRna) },
                { StandardFeatureKeys.OperonRegion, typeof(OperonRegion) },
                { StandardFeatureKeys.PolyASignal, typeof(PolyASignal) },
                { StandardFeatureKeys.PolyASite, typeof(PolyASite) },
                { StandardFeatureKeys.PrecursorRna, typeof(PrecursorRna) },
                { StandardFeatureKeys.Promoter, typeof(Promoter) },
                { StandardFeatureKeys.ProteinBindingSite, typeof(ProteinBindingSite) },
                { StandardFeatureKeys.RibosomeBindingSite, typeof(RibosomeBindingSite) },
                { StandardFeatureKeys.ReplicationOrigin, typeof(ReplicationOrigin) },
                { StandardFeatureKeys.RepeatRegion, typeof(RepeatRegion) },
                { StandardFeatureKeys.RibosomalRna, typeof(RibosomalRna) },
                { StandardFeatureKeys.SignalPeptide, typeof(SignalPeptide) },
                { StandardFeatureKeys.StemLoop, typeof(StemLoop) },
                { StandardFeatureKeys.TataSignal, typeof(TataSignal) },
                { StandardFeatureKeys.Terminator, typeof(Terminator) },
                { StandardFeatureKeys.TransferMessengerRna, typeof(TransferMessengerRna) },
                { StandardFeatureKeys.TransitPeptide, typeof(TransitPeptide) },
                { StandardFeatureKeys.TransferRna, typeof(TransferRna) },
                { StandardFeatureKeys.UnsureSequenceRegion, typeof(UnsureSequenceRegion) },
                { StandardFeatureKeys.Variation, typeof(Variation) }
            };
        }

        /// <summary>
        /// Returns standard feature class instance, if the key in the specified feature item is found 
        /// in the map; otherwise returns the specified feature item itself.
        /// For example:
        /// If the specified feature item has the key "Gene" then this method returns instance of the Gene class
        /// with data copied from the specified item.
        /// </summary>
        /// <param name="item">Feature item instance to which the standard feature item instance is needed.</param>
        /// <returns>If found returns appropriate class instance for the specified feature item, otherwise returns 
        /// the specified item itself.</returns>
        public static FeatureItem GetStandardFeatureItem(FeatureItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Type type = null;
            if (featureMap.ContainsKey(item.Key))
            {
                type = featureMap[item.Key];
            }

            if (type != null)
            {
                FeatureItem newItem = (FeatureItem)Activator.CreateInstance(type, item.Location);

                foreach (KeyValuePair<string, List<string>> kvp in item.Qualifiers)
                {
                    newItem.Qualifiers.Add(kvp.Key, kvp.Value);
                }

                item = newItem;
            }

            return item;
        }
    }
}
