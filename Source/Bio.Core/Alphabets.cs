using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static System.Array;
using static System.String;

using Bio.Algorithms.MUMmer;
using Bio.Extensions;
using Bio.Registration;
using Bio.Util;
using static Bio.Properties.Resource;

namespace Bio
{
    /// <summary>
    ///     The currently supported and built-in alphabets for sequence items.
    /// </summary>
    public static class Alphabets
    {
        /// <summary>
        /// 	Types of recognised alphabets, which provide the value of the
        ///     <see cref="IAlphabet.AlphabetType"/> property.
        /// </summary>
        /// <remarks>
        ///     The <see cref="IAlphabet.AlphabetType"/> property of plug-in alphabets should be assigned 
        ///     the value <see cref="PlugIn"/> OR'd with the value for the appropriate built-in alphabet <br/>
        ///     e.g. <c>AlphabetType = AlphabetTypes.PlugIn | AlphabetTypes.ProteinFragment</c> for a
        ///     peptide alphabet derived from <see cref="ProteinFragmentAlphabet"/>.
        /// </remarks>
        [Flags]
        public enum AlphabetTypes
        {
            None = 0,
            DNA = 1,
            AmbiguousDNA = 2,
            RNA = 4,
            AmbiguousRNA = 8,
            Protein = 16,
            AmbiguousProtein = 32,
            AminoAcids = 64,
            ProteinFragment = 128,
            ProteinScapePeptide = 256,
            PeaksPeptide = 512,
            PlugIn = 1024,

            AllDNA = DNA + AmbiguousDNA,
            AllRNA = RNA + AmbiguousRNA,
            AllNucleotides = AllDNA + AllRNA,
            AllAminoAcids = Protein + AmbiguousProtein + AminoAcids + ProteinFragment + ProteinScapePeptide + PeaksPeptide
        }

        /// <summary>
        /// 	The result of comparing the set of symbols of one alphabet to that of another alphabet.
        /// </summary>
        public enum ComparisonResult
        {
            Unknown,
            Identical,
            Subset,
            Superset,
            Intersects,
            NoOverlap
        }

        /// <summary>
        ///     List of all internal alphabet instances according to their priority in auto detection.
        ///     Auto detection starts from top of the list.
        /// </summary>
        /// <remarks>
        ///     Includes the six original Bio nucleotide and protein alphabets, plus the new alphabets
        ///     that extend the framework's functionality for use with peptides.
        /// </remarks>
        private static readonly List<IAlphabet> AllAlphabetsPriorityList =
            new List<IAlphabet>
            {
                DnaAlphabet.Instance,
                AmbiguousDnaAlphabet.Instance,
                RnaAlphabet.Instance,
                AmbiguousRnaAlphabet.Instance,
                ProteinAlphabet.Instance,
                AmbiguousProteinAlphabet.Instance,
                AminoAcidsAlphabet.Instance,
                ProteinFragmentAlphabet.Instance,
                ProteinScapePeptideAlphabet.Instance,
                PeaksPeptideAlphabet.Instance
            };

        /// <summary>
        ///     List of alphabet instances according to their priority in auto detection.
        ///     Auto detection starts from top of the list.
        /// </summary>
        /// <remarks>
        ///     Only includes the six original Bio alphabets, to prevent breaking
        ///     existing functionality.
        /// </remarks>
        private static readonly List<IAlphabet> AlphabetPriorityList = 
            new List<IAlphabet>
            {
                DnaAlphabet.Instance,
                AmbiguousDnaAlphabet.Instance,
                RnaAlphabet.Instance,
                AmbiguousRnaAlphabet.Instance,
                ProteinAlphabet.Instance,
                AmbiguousProteinAlphabet.Instance,
            };

        /// <summary>
        ///     Mapping between an alphabet type and its corresponding base alphabet type.
        /// </summary>
        public static readonly Dictionary<IAlphabet, IAlphabet> AlphabetToBaseAlphabetMap;

        /// <summary>
        ///     Mapping between an alphabet type and its corresponding ambiguous alphabet type.
        /// </summary>
        public static readonly Dictionary<IAlphabet, IAlphabet> AmbiguousAlphabetMap;

        /// <summary>
        ///     The DNA alphabet including symbols for ambiguous nucleotides.
        /// </summary>
        public static readonly AmbiguousDnaAlphabet AmbiguousDNA = AmbiguousDnaAlphabet.Instance;

        /// <summary>
        ///     The Protein alphabet including symbols for ambiguous amino acids, gaps and terminations.
        /// </summary>
        public static readonly AmbiguousProteinAlphabet AmbiguousProtein = AmbiguousProteinAlphabet.Instance;

        /// <summary>
        ///     The RNA alphabet including symbols for ambiguous nucleotides.
        /// </summary>
        public static readonly AmbiguousRnaAlphabet AmbiguousRNA = AmbiguousRnaAlphabet.Instance;

        /// <summary>
        ///     List of protein alphabet instances according to their priority in auto detection.
        ///     Auto detection starts from top of the list.
        /// </summary>
        /// <remarks>
        ///     Includes the two original Bio Protein alphabets, plus the new alphabets
        ///     that extend the framework's functionality for use with peptides.
        /// </remarks>
        private static readonly List<IAlphabet> AminoAcidsAlphabetPriorityList =
            new List<IAlphabet>
            {
                AminoAcidsAlphabet.Instance,
                ProteinFragmentAlphabet.Instance,
                ProteinScapePeptideAlphabet.Instance,
                PeaksPeptideAlphabet.Instance,
                ProteinAlphabet.Instance,
                AmbiguousProteinAlphabet.Instance
            };

        /// <summary>
        ///     The Amino Acids alphabet containing only unambiguous amino acids, without gap
        ///     or termination symbols.
        /// </summary>
        public static readonly AminoAcidsAlphabet AminoAcids = AminoAcidsAlphabet.Instance;

        /// <summary>
        ///     The DNA alphabet.
        /// </summary>
        public static readonly DnaAlphabet DNA = DnaAlphabet.Instance;

        /// <summary>
        ///     The Protein Fragment alphabet containing symbols used in peptides exported by PEAKS.
        /// </summary>
        public static readonly PeaksPeptideAlphabet PeaksPeptide = PeaksPeptideAlphabet.Instance;

        /// <summary>
        ///     The Protein alphabet excluding ambiguous amino acids, but including gap and termination symbols.
        /// </summary>
        public static readonly ProteinAlphabet Protein = ProteinAlphabet.Instance;

        /// <summary>
        ///     The Protein Fragment (i.e. Peptide) alphabet containing only unambiguous amino acids, without gap
        ///     or termination symbols, but including a separator symbol that delimits the beginning and end of the
        ///     peptide sequence.
        /// </summary>
        public static readonly ProteinFragmentAlphabet ProteinFragment = ProteinFragmentAlphabet.Instance;

        /// <summary>
        ///     The Protein Fragment alphabet containing symbols used in peptides exported by ProteinScape.
        /// </summary>
        public static readonly ProteinScapePeptideAlphabet ProteinScapePeptide = ProteinScapePeptideAlphabet.Instance;

        /// <summary>
        ///     The RNA alphabet.
        /// </summary>
        public static readonly RnaAlphabet RNA = RnaAlphabet.Instance;

        /// <summary>
        /// 	Mapping between an alphabet type and its subset alphabet types.
        /// </summary>
        /// <remarks>
        ///     The list of mapped subset alphabets is in size order, with smallest first.
        ///     It is an empty list for alphabets that have no subsets. 
        /// </remarks>
        public static readonly Dictionary<IAlphabet, IReadOnlyList<IAlphabet>> SubsetAlphabetsMap;

        /// <summary>
        /// 	Mapping between an alphabet type and its superset alphabet types.
        /// </summary>
        public static readonly Dictionary<IAlphabet, IReadOnlyList<IAlphabet>> SupersetAlphabetsMap;

        /// <summary>
        ///     List of all supported alphabets.
        /// </summary>
        /// <remarks>
        ///     Contains four Amino Acids alphabets added to the six alphabets defined in version 3.0.0-alpha.
        /// </remarks>
        private static readonly List<IAlphabet> KnownAlphabets = new List<IAlphabet>
        {
            DNA,
            AmbiguousDNA,
            RNA,
            AmbiguousRNA,
            Protein,
            AmbiguousProtein,
            AminoAcids,
            ProteinFragment,
            PeaksPeptide,
            ProteinScapePeptide
        };

        /// <summary>
        ///     List of all supported Amino Acids alphabets.
        /// </summary>
        private static readonly List<IAminoAcidsAlphabet> KnownAminoAcidsAlphabets =
            new List<IAminoAcidsAlphabet>
            {
                AminoAcids,
                ProteinFragment,
                ProteinScapePeptide,
                PeaksPeptide,
                Protein,
                AmbiguousProtein
            };

        /// <summary>
        ///     List of all supported DNA alphabets.
        /// </summary>
        private static readonly List<IDnaAlphabet> KnownDnaAlphabets = new List<IDnaAlphabet>
        {
            DNA,
            AmbiguousDNA,
        };

        /// <summary>
        ///     List of all supported RNA alphabets.
        /// </summary>
        private static readonly List<IRnaAlphabet> KnownRnaAlphabets = new List<IRnaAlphabet>
        {
            RNA,
            AmbiguousRNA,
        };

        /// <summary>
        ///     Initializes static members of the Alphabets class.
        /// </summary>
        static Alphabets()
        {
		    // get the registered alphabets.
            GetRegisteredAlphabets();

            // Get mappings to ambiguous and base alphabets.
            AmbiguousAlphabetMap = GetAmbiguousAlphabetMappings();
            AlphabetToBaseAlphabetMap = GetBaseAlphabetMappings();

            // Get mappings to subset and superset alphabets.
            SupersetAlphabetsMap = GetSupersetAlphabetMappings();
            SubsetAlphabetsMap = GetSubsetAphabetMappings();
        }

        /// <summary>
        ///  Gets the list of all Alphabets which are supported by the framework.
        /// </summary>
        public static IReadOnlyList<IAlphabet> All
        {
            get { return KnownAlphabets; }
        }

        /// <summary>
        ///  Gets the list of all Amino Acid Sequence alphabets which are supported by the framework.
        /// </summary>
        public static IReadOnlyList<IAminoAcidsAlphabet> AllAminoAcids
        {
            get { return KnownAminoAcidsAlphabets; }
        }

        /// <summary>
        ///  Gets the list of all DNA Alphabets which are supported by the framework.
        /// </summary>
        public static IReadOnlyList<IDnaAlphabet> AllDNA
        {
            get { return KnownDnaAlphabets; }
        }

        /// <summary>
        ///  Gets the list of all DNA Alphabets which are supported by the framework.
        /// </summary>
        public static IReadOnlyList<IRnaAlphabet> AllRNA
        {
            get { return KnownRnaAlphabets; }
        }

        /// <summary>
        /// 	Maps the given alphabet to its subsets or supersets.
        /// </summary>
        /// <param name="mappings">The collection of mappings.</param>
        /// <param name="alphabet">The alphabet.</param>
        /// <param name="mappedAlphabets">The subset/superset alphabets to map to the alphabet.</param>
        private static void AddAlphabetMappings(Dictionary<IAlphabet, IReadOnlyList<IAlphabet>> mappings, 
            IAlphabet alphabet, params IAlphabet[] mappedAlphabets)
        {
            var mappingsForAlphabet = new List<IAlphabet>();
            if (mappedAlphabets.Length > 0)
            {
                mappingsForAlphabet.AddRange(mappedAlphabets);
            }
            
            mappings.Add(alphabet, mappingsForAlphabet);
        }

        /// <summary>
        ///     This methods loops through the six DNA, RNA and Protein alphabet types supported in  
        ///     Bio version 3.0.0-alpha and tries to identify the best alphabet type for the given symbols.
        /// </summary>
        /// <param name="symbols">Symbols on which auto detection should be performed.</param>
        /// <param name="offset">Offset from which the auto detection should start.</param>
        /// <param name="length">Number of symbols to process from the offset position.</param>
        /// <param name="identifiedAlphabetType">
        ///     In case the symbols passed are a sub set of a bigger sequence, 
        ///     provide the already identified alphabet type of the sequence.
        /// </param>
        /// <returns>Returns the detected alphabet type or <c>null</c> if detection fails.</returns>
        /// <remarks>
        ///     This is a version 3.0.0-alpha legacy method which works with nucleotide or protein sequences 
        ///     from standard FASTA files. <br/>
        ///     Returns <see cref="DnaAlphabet"/>, <see cref="AmbiguousDnaAlphabet"/>,
        ///     <see cref="RnaAlphabet"/>, <see cref="AmbiguousRnaAlphabet"/>, <see cref="ProteinAlphabet"/>
        ///     or <see cref="AmbiguousProteinAlphabet"/> as appropriate, or <c>null</c>
        ///     if none of the six alphabets contains all the given symbols.
        /// </remarks>
        public static IAlphabet AutoDetectAlphabet(byte[] symbols, long offset, long length, 
            IAlphabet identifiedAlphabetType) => 
            AutoDetectHelper(symbols, offset, length, identifiedAlphabetType, AlphabetPriorityList, All);

        /// <summary>
        /// 	Helper method that does the work for AutoDetectAlphabet(), AutoDetectAminoAcidsAlphabet()
        ///     and AutoDetectAnyAlphabet().
        /// </summary>
        /// <param name="symbols">Symbols on which auto detection should be performed.</param>
        /// <param name="offset">Offset from which the auto detection should start.</param>
        /// <param name="length">Number of symbols to process from the offset position.</param>
        /// <param name="identifiedAlphabetType">
        ///     In case the symbols passed are a sub set of a bigger sequence, 
        ///     provide the already identified alphabet type of the sequence.
        /// </param>
        /// <param name="priorityList">The list of alphabets to check in order of priority.</param>
        /// <param name="fullList">The full list of alphabets to check, including plug-ins.</param>
        /// <returns>
        ///     Returns the detected alphabet type or <c>null</c> if detection fails.
        /// </returns>
        /// <exception cref="ArgumentException">identifiedAlphabetType</exception>
        private static IAlphabet AutoDetectHelper<T>(in byte[] symbols, long offset,
            long length, IAlphabet identifiedAlphabetType, in List<IAlphabet> priorityList,
            in IReadOnlyList<T> fullList) where T : IAlphabet
        {
            int currentPriorityIndex = 0;

            if (identifiedAlphabetType == null)
            {
                identifiedAlphabetType = priorityList[0];
            }

            while (identifiedAlphabetType != priorityList[currentPriorityIndex])
            {
                // Increment priority index and validate boundary condition
                if (++currentPriorityIndex == priorityList.Count)
                {
                    throw new ArgumentException(CouldNotRecognizeAlphabet, nameof(identifiedAlphabetType));
                }
            }

            // Start validating against alphabet types according to their priority
            while (!priorityList[currentPriorityIndex].ValidateSequence(symbols, offset, length))
            {
                // Increment priority index and validate boundary condition
                if (++currentPriorityIndex == priorityList.Count)
                {
                    // Last ditch effort - look at any other registered alphabets and see if
                    // any contain all the located symbols.
                    for (int i = currentPriorityIndex; i < fullList.Count; i++)
                    {
                        IAlphabet alphabet = fullList[i];

                        // Make sure alphabet supports validation -- if not, ignore it.
                        try
                        {
                            if (alphabet.ValidateSequence(symbols, offset, length))
                                return alphabet;
                        }
                        catch (NotImplementedException)
                        {
                        }
                    }

                    // No alphabet found.
                    return null;
                }
            }

            return priorityList[currentPriorityIndex];
        }

        /// <summary>
        /// 	Auto-detects all the recognised alphabets to which the input symbols belong.
        /// </summary>
        /// <param name="symbols">Symbols on which auto detection should be performed.</param>
        /// <returns>
        ///  	<see cref="IReadOnlyList{IAlphabet}"/>: the recognised alphabets to which the input symbols belong.
        /// </returns>
        public static IReadOnlyList<IAlphabet> AutoDetectAlphabets(byte[] symbols) =>
            GetContainingAlphabets(symbols, 0, GetSymbolCount(symbols), All).ToList();

        /// <summary>
        /// 	Auto-detects all the recognised alphabets to which the input symbols belong.
        /// </summary>
        /// <param name="symbols">Symbols on which auto detection should be performed.</param>
        /// <param name="offset">Offset from which the auto detection should start.</param>
        /// <param name="length">Number of symbols to process from the offset position.</param>
        /// <returns>
        ///  	<see cref="IReadOnlyList{IAlphabet}"/>: the recognised alphabets to which the input symbols belong.
        /// </returns>
        public static IReadOnlyList<IAlphabet> AutoDetectAlphabets(byte[] symbols, long offset, long length) =>
            GetContainingAlphabets(symbols, offset, length, All).ToList();

        /// <summary>
        /// 	Auto-detects all the alphabets in the given collection to which the input symbols belong.
        /// </summary>
        /// <param name="symbols">Symbols on which auto detection should be performed.</param>
        /// <param name="offset">Offset from which the auto detection should start.</param>
        /// <param name="length">Number of symbols to process from the offset position.</param>
        /// <param name="fromAlphabets">The alphabets to search for the containing alphabets</param>
        /// <returns>
        ///  	<see cref="HashSet{IAlphabet}"/>: the alphabets in the given collection to which the input symbols belong.
        /// </returns>
        public static HashSet<IAlphabet> GetContainingAlphabets(byte[] symbols, long offset, long length, 
            IEnumerable<IAlphabet> fromAlphabets)
        {
            if (symbols == null)
                throw new ArgumentNullException(nameof(symbols));

            if ((offset < 0) || (offset >= symbols.Length))
                throw new ArgumentOutOfRangeException(nameof(offset));

            if ((length < 0) || (symbols.Length < offset + length))
                throw new ArgumentOutOfRangeException(nameof(length));

            if (fromAlphabets == null)
                throw new ArgumentNullException(nameof(fromAlphabets));

            var alphabets = new HashSet<IAlphabet>();

            foreach (IAlphabet alphabet in fromAlphabets)
            {
                if (!alphabets.Contains(alphabet))
                {
                    // Make sure alphabet supports validation -- if not, ignore it.
                    try
                    {
                        if (alphabet.ValidateSequence(symbols, offset, length))
                        {
                            alphabets.Add(alphabet);

                            // Any superset alphabets will also contain all the symbols of the alphabet.
                            if (SupersetAlphabetsMap.ContainsKey(alphabet))
                            {
                                alphabets.AddNewOrOldRange(SupersetAlphabetsMap[alphabet]);
                            }
                        }
                    }
                    catch (NotImplementedException)
                    {
                    }
                }
            }

            return alphabets;
        }

        /// <summary>
        ///     This methods loops through supported Amino Acids alphabet types and tries to identify
        ///     the best alphabet type for the given symbols.
        /// </summary>
        /// <param name="symbols">Symbols on which auto detection should be performed.</param>
        /// <returns>
        ///     Returns the detected alphabet type or <c>null</c> if detection fails.
        /// </returns>
        /// <remarks>
        ///     Checks all the symbols in the given sequence, and returns <c>null</c> if detection fails.
        /// </remarks>
        public static IAminoAcidsAlphabet AutoDetectAminoAcidsAlphabet(byte[] symbols) =>
            AutoDetectHelper(symbols, 0, GetSymbolCount(symbols), null, AminoAcidsAlphabetPriorityList,
                AllAminoAcids) as IAminoAcidsAlphabet;

        /// <summary>
        ///     This methods loops through all supported Amino Acid Sequence alphabet types and tries
        ///     to identify the best alphabet type for the given symbols.
        /// </summary>
        /// <param name="symbols">Symbols on which auto detection should be performed.</param>
        /// <param name="identifiedAlphabetType">In case the symbols passed are a sequence of a group
        /// of sequences, provide the already identified alphabet type of the group.</param>
        /// <returns>
        ///     Returns the detected alphabet type or <c>null</c> if detection fails.
        /// </returns>
        /// <remarks>
        ///     Checks all the symbols in the given sequence, and returns <c>null</c> if detection fails.<br/>
        ///     If <paramref name="identifiedAlphabetType"/> is <c>null</c>, all alphabets in the
        ///     <see cref="AllAminoAcids"/> collection are included in the detection.
        ///     Otherwise, detection starts from the specified alphabet.
        /// </remarks>
        public static IAminoAcidsAlphabet AutoDetectAminoAcidsAlphabet(byte[] symbols,
            IAminoAcidsAlphabet identifiedAlphabetType) =>
            AutoDetectHelper(symbols, 0, GetSymbolCount(symbols), identifiedAlphabetType, 
                AminoAcidsAlphabetPriorityList, AllAminoAcids) as IAminoAcidsAlphabet;

        /// <summary>
        ///     This methods loops through supported Amino Acid Sequence alphabet types and
        ///     tries to identify the best alphabet type for the given symbols.
        /// </summary>
        /// <param name="symbols">Symbols on which auto detection should be performed.</param>
        /// <param name="offset">Offset from which the auto detection should start.</param>
        /// <param name="length">Number of symbols to process from the offset position.</param>
        /// <param name="identifiedAlphabetType">In case the symbols passed are a sub set of a bigger sequence, 
        /// provide the already identified alphabet type of the sequence.</param>
        /// <returns>
        ///     Returns the detected alphabet type or <c>null</c> if detection fails.
        /// </returns>
        /// <remarks>
        ///     Checks the specified range of symbols in the given sequence, and returns <c>null</c> if detection fails.<br/>
        ///     If <paramref name="identifiedAlphabetType"/> is <c>null</c>, all alphabets in the
        ///     <see cref="AllAminoAcids"/> collection are included in the detection.
        ///     Otherwise, detection starts from the specified alphabet.
        /// </remarks>
        public static IAminoAcidsAlphabet AutoDetectAminoAcidsAlphabet(byte[] symbols, long offset, 
            long length, IAminoAcidsAlphabet identifiedAlphabetType) =>
            AutoDetectHelper(symbols, offset, length, identifiedAlphabetType, AminoAcidsAlphabetPriorityList,
                AllAminoAcids) as IAminoAcidsAlphabet;

        /// <summary>
        ///     This methods loops through the six DNA, RNA and Protein alphabet types supported in  
        ///     Bio version 3.0.0-alpha followed by new Peptide alphabet types, and tries to
        ///     identify the best alphabet type for the given symbols.
        /// </summary>
        /// <param name="symbols">Symbols on which auto detection should be performed.</param>
        /// <returns>
        ///     Returns the detected alphabet type or <c>null</c> if detection fails.
        /// </returns>
        /// <remarks>
        ///     This method first checks for nucleotide or protein sequences as found in standard 
        ///     FASTA files. If a matching alphabet is not identified, it then looks for a match
        ///     to one of the new peptide alphabets added after Bio version 3.0.0-alpha. Finally
        ///     it checks any other alphabets found in the plug-ins folder.<br/>
        ///     Returns <c>null</c> if no recognised alphabet contains all the given symbols.
        /// </remarks>
        public static IAlphabet AutoDetectAnyAlphabet(byte[] symbols) =>
            AutoDetectHelper(symbols, 0, GetSymbolCount(symbols), null, AllAlphabetsPriorityList, All);

        /// <summary>
        ///     This methods loops through the six DNA, RNA and Protein alphabet types supported in  
        ///     Bio version 3.0.0-alpha followed by new Peptide alphabet types, and tries to
        ///     identify the best alphabet type for the given symbols.
        /// </summary>
        /// <param name="symbols">Symbols on which auto detection should be performed.</param>
        /// <param name="identifiedAlphabetType">
        ///     In case the symbols passed are a sub set of a bigger sequence, 
        ///     provide the already identified alphabet type of the sequence.
        /// </param>
        /// <returns>
        ///     Returns the detected alphabet type or <c>null</c> if detection fails.
        /// </returns>
        /// <remarks>
        ///     This method first checks for nucleotide or protein sequences as found in standard 
        ///     FASTA files. If a matching alphabet is not identified, it then looks for a match
        ///     in the new peptide alphabets added after Bio version 3.0.0-alpha. Finally
        ///     it checks any other alphabets found in the plug-ins folder.<br/>
        ///     Returns <c>null</c> if no recognised alphabet contains all the given symbols.
        /// </remarks>
        public static IAlphabet AutoDetectAnyAlphabet(byte[] symbols,
            IAminoAcidsAlphabet identifiedAlphabetType) =>
            AutoDetectHelper(symbols, 0, GetSymbolCount(symbols), identifiedAlphabetType,
                AllAlphabetsPriorityList, All);

        /// <summary>
        ///     This methods loops through the six DNA, RNA and Protein alphabet types supported in  
        ///     Bio version 3.0.0-alpha followed by new Peptide alphabet types, and tries to
        ///     identify the best alphabet type for the given symbols.
        /// </summary>
        /// <param name="symbols">Symbols on which auto detection should be performed.</param>
        /// <param name="offset">Offset from which the auto detection should start.</param>
        /// <param name="length">Number of symbols to process from the offset position.</param>
        /// <param name="identifiedAlphabetType">In case the symbols passed are a sub set of a bigger sequence, 
        /// provide the already identified alphabet type of the sequence.</param>
        /// <returns>Returns the detected alphabet type or <c>null</c> if detection fails.</returns>
        /// <remarks>
        ///     The method first checks for nucleotide or protein sequences as found in standard 
        ///     FASTA files. If a matching alphabet is not identified, it then looks for a match
        ///     in the new peptide alphabets added after Bio version 3.0.0-alpha. Finally
        ///     it checks any other alphabets found in the plug-ins folder.<br/>
        ///     Returns <c>null</c> if no recognised alphabet contains all the given symbols.
        /// </remarks>
        public static IAlphabet AutoDetectAnyAlphabet(byte[] symbols, long offset,
            long length, IAlphabet identifiedAlphabetType) =>
            AutoDetectHelper(symbols, offset, length, identifiedAlphabetType, AllAlphabetsPriorityList, All);

        /// <summary>
        /// Verifies if two given alphabets comes from the same base alphabet.
        /// </summary>
        /// <param name="alphabetA">First alphabet to compare.</param>
        /// <param name="alphabetB">Second alphabet to compare.</param>
        /// <returns>True if both alphabets comes from the same base class.</returns>
        public static bool CheckIsFromSameBase(IAlphabet alphabetA, IAlphabet alphabetB)
        {
            if (alphabetA == alphabetB)
                return true;

            IAlphabet innerAlphabetA = alphabetA, innerAlphabetB = alphabetB;

            if (AlphabetToBaseAlphabetMap.Keys.Contains(alphabetA))
                innerAlphabetA = AlphabetToBaseAlphabetMap[alphabetA];

            if (AlphabetToBaseAlphabetMap.Keys.Contains(alphabetB))
                innerAlphabetB = AlphabetToBaseAlphabetMap[alphabetB];

            return innerAlphabetA == innerAlphabetB;
        }

        /// <summary>
        /// Gets all registered alphabets in core folder and addins (optional) folders.
        /// </summary>
        /// <returns>List of registered alphabets.</returns>
        private static IEnumerable<IAlphabet> GetAlphabets()
        {
            IEnumerable<Type> implementations = BioRegistrationService.LocateRegisteredParts<IAlphabet>();
            List<IAlphabet> registeredAlphabets = new List<IAlphabet>();

            foreach (Type impl in implementations)
            {
                try
                {
                    IAlphabet alpha = Activator.CreateInstance(impl) as IAlphabet;
                    if (alpha != null)
                        registeredAlphabets.Add(alpha);
                }
                catch
                {
                    // Cannot create - no default ctor?
                }
            }

            return registeredAlphabets;
        }

        /// <summary>
        /// Gets the ambiguous alphabet
        /// </summary>
        /// <param name="currentAlphabet">Alphabet to validate</param>
        /// <returns></returns>
        public static IAlphabet GetAmbiguousAlphabet(IAlphabet currentAlphabet)
        {
            if (currentAlphabet == DnaAlphabet.Instance ||
                currentAlphabet == RnaAlphabet.Instance ||
                currentAlphabet == AminoAcidsAlphabet.Instance ||
                currentAlphabet == ProteinAlphabet.Instance)
            {
                return AmbiguousAlphabetMap[currentAlphabet];
            }

            return currentAlphabet;
        }

        /// <summary>
        /// 	Gets mappings of each non-ambiguous alphabet to its ambiguous alphabet.
        /// </summary>
        /// <returns>
        ///  	<see cref="Dictionary{IAlphabet,IAlphabet}"/>:
        ///     mappings of each non-ambiguous alphabet to its ambiguous alphabet.
        /// </returns>
        /// <remarks>
        ///     Peptide alphabets (<see cref="ProteinFragment"/>, <see cref="ProteinScapePeptide"/>
        ///     and <see cref="PeaksPeptide"/>) do not have corresponding ambiguous alphabets.
        /// </remarks>
        private static Dictionary<IAlphabet, IAlphabet> GetAmbiguousAlphabetMappings()
        {
            // Don't map peptide alphabets, which don't have ambiguous supersets.
            var mappings = new Dictionary<IAlphabet, IAlphabet>
            {
                { DnaAlphabet.Instance, AmbiguousDnaAlphabet.Instance },
                { RnaAlphabet.Instance, AmbiguousRnaAlphabet.Instance },
                { AminoAcidsAlphabet.Instance, AmbiguousProteinAlphabet.Instance },
                { ProteinAlphabet.Instance, AmbiguousProteinAlphabet.Instance },
                { AmbiguousDnaAlphabet.Instance, AmbiguousDnaAlphabet.Instance },
                { AmbiguousRnaAlphabet.Instance, AmbiguousRnaAlphabet.Instance },
                { AmbiguousProteinAlphabet.Instance, AmbiguousProteinAlphabet.Instance }
            };

            return mappings;
        }

        /// <summary>
        /// 	Gets mappings of each version 3.0.0-alpha alphabet to its base alphabet.
        /// </summary>
        /// <returns>
        ///  	<see cref="Dictionary{IAlphabet,IAlphabet}"/>:
        ///     mappings of each version 3.0.0-alpha alphabet to its base alphabet.
        /// </returns>
        /// <remarks>
        ///     Maintains the original mappings of ambiguous alphabets to base alphabets, even though
        ///     AminoAcidsAlphabet has been added as the base class for ProteinAlphabet, to
        ///     prevent breaking any functionality of the v3.0.0-alpha framework.
        /// </remarks>
        private static Dictionary<IAlphabet, IAlphabet> GetBaseAlphabetMappings()
        {
            var mappings = new Dictionary<IAlphabet, IAlphabet>
            {
                { AmbiguousDnaAlphabet.Instance, DnaAlphabet.Instance },
                { AmbiguousRnaAlphabet.Instance, RnaAlphabet.Instance },
                { AmbiguousProteinAlphabet.Instance, ProteinAlphabet.Instance },
                { MummerDnaAlphabet.Instance, DnaAlphabet.Instance },
                { MummerRnaAlphabet.Instance, RnaAlphabet.Instance },
                { MummerProteinAlphabet.Instance, ProteinAlphabet.Instance },
                { ProteinFragmentAlphabet.Instance, AminoAcidsAlphabet.Instance },
                { ProteinScapePeptideAlphabet.Instance, PeaksPeptideAlphabet.Instance },
                { PeaksPeptideAlphabet.Instance, ProteinFragmentAlphabet.Instance }
            };

            return mappings;
        }

        /// <summary>
        /// 	Loops through the sequences in the given collection and tries to the find
        ///     the smallest consensus alphabet of all recognised alphabets that contains
        ///     all of the symbols in the sequences.
        /// </summary>
        /// <param name="sequences">The sequences for which to find a consensus alphabet.</param>
        /// <returns>
        ///  	<see cref="IAminoAcidsAlphabet"/>: a consensus alphabet that contains all of the symbols in the sequences.
        /// </returns>
        /// <exception cref="ArgumentNullException">sequences - A collection of sequences is required.</exception>
        /// <remarks>
        ///     Returns <c>null</c> if a single alphabet cannot be found that contains all the symbols
        ///     in all the supplied sequences.<br/>
        ///     NOTE: can erroneously return a DNA or RNA alphabet if passed amino acid sequences that do
        ///     not contain the full range of amino acid symbols, since nucleotide alphabets are smaller than
        ///     amino acids alphabets. To avoid this, use the GetConsensusAminoAcidsAlphabet() method
        ///     if the sequences are known to only contain amino acids.
        /// </remarks>
        public static IAlphabet GetConsensusAlphabet(IEnumerable<byte[]> sequences) =>
            GetConsensusAlphabet(sequences, null);

        /// <summary>
        /// 	Loops through the sequences in the given collection and tries to the find
        ///     the smallest consensus alphabet in the supplied collection that contains
        ///     all of the symbols in the sequences.
        /// </summary>
        /// <param name="sequences">The sequences for which to find a consensus alphabet.</param>
        /// <param name="fromAlphabets">
        ///     The alphabets to search for the containing alphabets. If this is <c>null</c>, all
        ///     recognised alphabets are searched.
        /// </param>
        /// <returns>
        ///  	<see cref="IAminoAcidsAlphabet"/>: a consensus alphabet that contains all of the symbols in the sequences.
        /// </returns>
        /// <remarks>
        ///     Returns <c>null</c> if a single alphabet cannot be found that contains all the symbols
        ///     in all the supplied sequences.<br/>
        ///     NOTE: can erroneously return a DNA or RNA alphabet if passed amino acid sequences that do
        ///     not contain the full range of amino acid symbols, since nucleotide alphabets are smaller than
        ///     amino acids alphabets. To avoid this, use the GetConsensusAminoAcidsAlphabet() method
        ///     if the sequences are known to only contain amino acids.
        /// </remarks>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IAlphabet GetConsensusAlphabet(IEnumerable<byte[]> sequences, IEnumerable<IAlphabet> fromAlphabets)
        {
            if ((sequences == null) || !sequences.Any())
            {
                throw new ArgumentException(SequencesCannotBeNullOrEmpty);
            }

            if (fromAlphabets == null)
            {
                fromAlphabets = All;
            }

            // Get all the alphabets that contain all the symbols of all the sequences.
            HashSet<IAlphabet> consensusAlphabets = null;
            foreach (byte[] sequence in sequences)
            {
                HashSet<IAlphabet> alphabets = GetContainingAlphabets(sequence, 0, GetSymbolCount(sequence), fromAlphabets);

                if (consensusAlphabets == null)
                {
                    consensusAlphabets = alphabets;
                }
                else
                {
                    consensusAlphabets.IntersectWith(alphabets);
                    if (consensusAlphabets.Count == 0)
                    {
                        return null;
                    }
                }
            }

            // This should never happen.
            if (consensusAlphabets == null)
            {
                return null;
            }

            // Find and return the consensus alphabet with the fewest symbols.
            IAlphabet consensusAlphabet = null;
            foreach (IAlphabet alphabet in consensusAlphabets)
            {
                if (consensusAlphabet == null  || (alphabet.Count() < consensusAlphabet.Count()))
                {
                    consensusAlphabet = alphabet;
                }
            }

            return consensusAlphabet;
        }

        /// <summary>
        /// 	Loops through the sequences in the given collection and tries to the detect
        ///     a consensus Amino Acids alphabet that contains all of the symbols in the sequences.
        /// </summary>
        /// <param name="sequences">The sequences for which to find a consensus alphabet.</param>
        /// <returns>
        ///  	<see cref="IAminoAcidsAlphabet"/>: a consensus alphabet that contains all of the symbols in the sequences.
        /// </returns>
        /// <remarks>
        ///     Returns <c>null</c> if a single alphabet cannot be found that contains all the symbols
        ///     in all the supplied sequences.
        /// </remarks>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IAminoAcidsAlphabet GetConsensusAminoAcidsAlphabet(IEnumerable<byte[]> sequences) =>
            GetConsensusAlphabet(sequences, KnownAminoAcidsAlphabets) as IAminoAcidsAlphabet;

        /// <summary>
        /// 	Adds all registered alphabets to appropriate collection properties.
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        private static void GetRegisteredAlphabets()
        {
            IEnumerable<IAlphabet> registeredAlphabets = GetAlphabets();
            if (null == registeredAlphabets) return;

            foreach (IAlphabet alphabet in registeredAlphabets.Where(
                alphabet => alphabet != null &&
                            KnownAlphabets.All(ra => Compare(ra.Name, alphabet.Name, StringComparison.OrdinalIgnoreCase) != 0)))
            {
                KnownAlphabets.Add(alphabet);

                if (alphabet.HasAminoAcidSymbols())
                    KnownAminoAcidsAlphabets.Add(alphabet as IAminoAcidsAlphabet);
                else if (alphabet.HasDnaSymbols())
                    KnownDnaAlphabets.Add(alphabet as IDnaAlphabet);
                else if (alphabet.HasRnaSymbols())
                    KnownRnaAlphabets.Add(alphabet as IRnaAlphabet);
                else
                    throw new NotSupportedException(UnsupportedAlphabetType);
            }
        }

        /// <summary>
        /// 	Gets mappings of each alphabet to its subset alphabet(s) (if any).
        /// </summary>
        private static Dictionary<IAlphabet, IReadOnlyList<IAlphabet>> GetSubsetAphabetMappings()
        {
            var mappings = new Dictionary<IAlphabet, IReadOnlyList<IAlphabet>>();

            AddAlphabetMappings(mappings, AminoAcidsAlphabet.Instance);

            AddAlphabetMappings(mappings, ProteinFragmentAlphabet.Instance,
                AminoAcidsAlphabet.Instance);

            AddAlphabetMappings(mappings, ProteinScapePeptideAlphabet.Instance,
                AminoAcidsAlphabet.Instance,
                ProteinFragmentAlphabet.Instance);

            AddAlphabetMappings(mappings, PeaksPeptideAlphabet.Instance,
                AminoAcidsAlphabet.Instance,
                ProteinFragmentAlphabet.Instance);

            AddAlphabetMappings(mappings, ProteinAlphabet.Instance,
                DnaAlphabet.Instance,
                RnaAlphabet.Instance,
                AminoAcidsAlphabet.Instance);

            AddAlphabetMappings(mappings, AmbiguousProteinAlphabet.Instance,
                DnaAlphabet.Instance,
                AmbiguousDnaAlphabet.Instance,
                RnaAlphabet.Instance,
                AmbiguousRnaAlphabet.Instance,
                AminoAcidsAlphabet.Instance,
                ProteinAlphabet.Instance);

            AddAlphabetMappings(mappings, DnaAlphabet.Instance);

            AddAlphabetMappings(mappings, AmbiguousDnaAlphabet.Instance,
                DnaAlphabet.Instance);

            AddAlphabetMappings(mappings, RnaAlphabet.Instance);

            AddAlphabetMappings(mappings, AmbiguousRnaAlphabet.Instance,
                RnaAlphabet.Instance);

            return mappings;
        }

        /// <summary>
        /// 	Gets mappings of each alphabet to its superset alphabet(s) (if any).
        /// </summary>
        private static Dictionary<IAlphabet, IReadOnlyList<IAlphabet>> GetSupersetAlphabetMappings()
        {
            var mappings = new Dictionary<IAlphabet, IReadOnlyList<IAlphabet>>();

            AddAlphabetMappings(mappings, AminoAcidsAlphabet.Instance, 
                ProteinFragmentAlphabet.Instance,
                ProteinScapePeptideAlphabet.Instance,
                PeaksPeptideAlphabet.Instance,
                ProteinAlphabet.Instance,
                AmbiguousProteinAlphabet.Instance);

            AddAlphabetMappings(mappings, ProteinFragmentAlphabet.Instance,
                ProteinScapePeptideAlphabet.Instance,
                PeaksPeptideAlphabet.Instance);

            AddAlphabetMappings(mappings, ProteinScapePeptideAlphabet.Instance);

            AddAlphabetMappings(mappings, PeaksPeptideAlphabet.Instance);

            AddAlphabetMappings(mappings, ProteinAlphabet.Instance,
                AmbiguousProteinAlphabet.Instance);

            AddAlphabetMappings(mappings, AmbiguousProteinAlphabet.Instance);

            AddAlphabetMappings(mappings, DnaAlphabet.Instance,
                AmbiguousDnaAlphabet.Instance,
                ProteinAlphabet.Instance,
                AmbiguousProteinAlphabet.Instance);

            AddAlphabetMappings(mappings, AmbiguousDnaAlphabet.Instance,
                AmbiguousProteinAlphabet.Instance);

            AddAlphabetMappings(mappings, RnaAlphabet.Instance,
                AmbiguousRnaAlphabet.Instance,
                ProteinAlphabet.Instance,
                AmbiguousProteinAlphabet.Instance);

            AddAlphabetMappings(mappings, AmbiguousRnaAlphabet.Instance,
                AmbiguousProteinAlphabet.Instance);

            return mappings;
        }

        /// <summary>
        /// 	Gets the number of symbols in the byte array.
        /// </summary>
        /// <param name="symbols">The byte array of symbols.</param>
        /// <returns>
        ///  	<see cref="long"/>: the number of symbols in the byte array.
        /// </returns>
        /// <remarks>
        ///     Ignores any zero bytes that occur after the symbols.
        /// </remarks>
        public static long GetSymbolCount(byte[] symbols)
        {
            if (symbols == null)
            {
                throw new ArgumentNullException(nameof(symbols));
            }

            long symbolCount = FindIndex(symbols, s => s == 0);
            if (symbolCount < 0)
            {
                symbolCount = symbols.LongLength;
            }

            return symbolCount;
        }

        /// <summary>
        /// Maps the alphabet to its ambiguous alphabet.
        /// For example: DnaAlphabet to AmbiguousDnaAlphabet.
        /// </summary>
        /// <param name="alphabet">Alphabet to map.</param>
        /// <param name="ambiguousAlphabet">Ambiguous alphabet to map.</param>
        private static void MapAlphabetToAmbiguousAlphabet(IAlphabet alphabet, IAlphabet ambiguousAlphabet)
        {
            AmbiguousAlphabetMap.Add(alphabet, ambiguousAlphabet);
        }

        /// <summary>
        /// Maps the alphabet to its base alphabet.
        /// For example: AmbiguousDnaAlphabet to DnaAlphabet
        /// </summary>
        /// <param name="alphabet">Alphabet to map.</param>
        /// <param name="baseAlphabet">Base alphabet to map.</param>
        private static void MapAlphabetToBaseAlphabet(IAlphabet alphabet, IAlphabet baseAlphabet)
        {
            AlphabetToBaseAlphabetMap.Add(alphabet, baseAlphabet);
        }
    }
}
