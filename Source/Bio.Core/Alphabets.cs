using Bio.Algorithms.MUMmer;
using Bio.Registration;
using static Bio.Properties.Resource;
using static System.String;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bio
{
    /// <summary>
    /// The currently supported and built-in alphabets for sequence items.
    /// </summary>
    public static class Alphabets
    {
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
        ///     List of protein alphabet instances according to their priority in auto detection.
        ///     Auto detection starts from top of the list, and proceeds in order of strictness.
        /// </summary>
        /// <remarks>
        ///     Includes the two original Bio protein alphabets, plus the new alphabets
        ///     that extend the framework's functionality for use with peptides and proteins.
        /// </remarks>
        private static readonly List<IStrictProteinAlphabet> ProteinAlphabetPriorityList =
            new List<IStrictProteinAlphabet>
            {
                ProteinFragmentAlphabet.Instance,
                ProteinScapePeptideAlphabet.Instance,
                PeaksPeptideAlphabet.Instance,
                StrictProteinAlphabet.Instance,
                ProteinAlphabet.Instance,
                AmbiguousProteinAlphabet.Instance
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
        ///     The DNA alphabet.
        /// </summary>
        public static readonly DnaAlphabet DNA = DnaAlphabet.Instance;

        /// <summary>
        ///     The Protein alphabet excluding ambiguous amino acids, but including gap and termination symbols.
        /// </summary>
        public static readonly ProteinAlphabet Protein = ProteinAlphabet.Instance;

        /// <summary>
        ///     The RNA alphabet.
        /// </summary>
        public static readonly RnaAlphabet RNA = RnaAlphabet.Instance;

        /// <summary>
        ///     The Protein Fragment (i.e. Peptide) alphabet containing only unambiguous amino acids, without gap
        ///     or termination symbols, but including a separator symbol that delimits the beginning and end of the
        ///     peptide sequence.
        /// </summary>
        public static readonly ProteinFragmentAlphabet ProteinFragment = ProteinFragmentAlphabet.Instance;

        /// <summary>
        ///     The strict Protein alphabet containing only unambiguous amino acids, without gap
        ///     or termination symbols.
        /// </summary>
        public static readonly StrictProteinAlphabet StrictProtein = StrictProteinAlphabet.Instance;

        /// <summary>
        ///     The Protein Fragment alphabet containing symbols used in peptides exported by PEAKS.
        /// </summary>
        public static readonly PeaksPeptideAlphabet PeaksPeptide = PeaksPeptideAlphabet.Instance;

        /// <summary>
        ///     The Protein Fragment alphabet containing symbols used in peptides exported by ProteinScape.
        /// </summary>
        public static readonly ProteinScapePeptideAlphabet ProteinScapePeptide = ProteinScapePeptideAlphabet.Instance;

        /// <summary>
        ///     List of all supported alphabets.
        /// </summary>
        private static readonly List<IAlphabet> KnownAlphabets = new List<IAlphabet> 
        {
            DNA,
            AmbiguousDNA,
            RNA,
            AmbiguousRNA,
            StrictProtein,
            Protein,
            AmbiguousProtein,
            ProteinFragment,
            PeaksPeptide,
            ProteinScapePeptide
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
        ///     List of all supported Protein alphabets.
        /// </summary>
        private static readonly List<IStrictProteinAlphabet> KnownProteinAlphabets = new List<IStrictProteinAlphabet>
        {
            ProteinFragment,
            ProteinScapePeptide,
            PeaksPeptide,
            StrictProtein,
            Protein,
            AmbiguousProtein
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
            IEnumerable<IAlphabet> registeredAlphabets = GetAlphabets();
            if (null != registeredAlphabets)
            {
                foreach (IAlphabet alphabet in registeredAlphabets.Where(
                    alphabet => alphabet != null && 
                                KnownAlphabets.All(ra => Compare(ra.Name, alphabet.Name, StringComparison.OrdinalIgnoreCase) != 0)))
                {
                    KnownAlphabets.Add(alphabet);
                    if (alphabet.IsDna)
                        KnownDnaAlphabets.Add(alphabet as IDnaAlphabet);
                    else if (alphabet.IsProtein)
                        KnownProteinAlphabets.Add(alphabet as IStrictProteinAlphabet);
                    else if (alphabet.IsRna)
                        KnownRnaAlphabets.Add(alphabet as IRnaAlphabet);
                    else
                        throw new NotSupportedException(UnsupportedAlphabetType);
                }
            }

            // Don't map peptide alphabets.
            AmbiguousAlphabetMap = new Dictionary<IAlphabet, IAlphabet>();
            MapAlphabetToAmbiguousAlphabet(DnaAlphabet.Instance, AmbiguousDnaAlphabet.Instance);
            MapAlphabetToAmbiguousAlphabet(RnaAlphabet.Instance, AmbiguousRnaAlphabet.Instance);
            MapAlphabetToAmbiguousAlphabet(StrictProteinAlphabet.Instance, AmbiguousProteinAlphabet.Instance);
            MapAlphabetToAmbiguousAlphabet(ProteinAlphabet.Instance, AmbiguousProteinAlphabet.Instance);
            MapAlphabetToAmbiguousAlphabet(AmbiguousDnaAlphabet.Instance, AmbiguousDnaAlphabet.Instance);
            MapAlphabetToAmbiguousAlphabet(AmbiguousRnaAlphabet.Instance, AmbiguousRnaAlphabet.Instance);
            MapAlphabetToAmbiguousAlphabet(AmbiguousProteinAlphabet.Instance, AmbiguousProteinAlphabet.Instance);

            // Maintain original mappings of ambiguous alphabets to base alphabets, even though
            // StrictProteinAlphabet has been added as the base class for ProteinAlphabet, to
            // prevent breaking any existing functionality of the framework.
            AlphabetToBaseAlphabetMap = new Dictionary<IAlphabet, IAlphabet>();
            MapAlphabetToBaseAlphabet(AmbiguousDnaAlphabet.Instance, DnaAlphabet.Instance);
            MapAlphabetToBaseAlphabet(AmbiguousRnaAlphabet.Instance, RnaAlphabet.Instance);
            MapAlphabetToBaseAlphabet(AmbiguousProteinAlphabet.Instance, ProteinAlphabet.Instance);

		    MapAlphabetToBaseAlphabet(MummerDnaAlphabet.Instance, DnaAlphabet.Instance);
            MapAlphabetToBaseAlphabet(MummerRnaAlphabet.Instance, RnaAlphabet.Instance);
            MapAlphabetToBaseAlphabet(MummerProteinAlphabet.Instance, ProteinAlphabet.Instance);  
        }

        /// <summary>
        ///  Gets the list of all Alphabets which are supported by the framework.
        /// </summary>
        public static IReadOnlyList<IAlphabet> All
        {
            get { return KnownAlphabets; }
        }

        /// <summary>
        ///  Gets the list of all DNA Alphabets which are supported by the framework.
        /// </summary>
        public static IReadOnlyList<IDnaAlphabet> AllDna
        {
            get { return KnownDnaAlphabets; }
        }

        /// <summary>
        ///  Gets the list of all Protein Alphabets which are supported by the framework.
        /// </summary>
        public static IReadOnlyList<IStrictProteinAlphabet> AllProtein
        {
            get { return KnownProteinAlphabets; }
        }

        /// <summary>
        ///  Gets the list of all DNA Alphabets which are supported by the framework.
        /// </summary>
        public static IReadOnlyList<IRnaAlphabet> AllRna
        {
            get { return KnownRnaAlphabets; }
        }

        /// <summary>
        ///     This methods loops through the six alphabet types supported in the original 
        ///     version of Bio and tries to identify the best alphabet type for the given symbols.
        /// </summary>
        /// <param name="symbols">Symbols on which auto detection should be performed.</param>
        /// <param name="offset">Offset from which the auto detection should start.</param>
        /// <param name="length">Number of symbols to process from the offset position.</param>
        /// <param name="identifiedAlphabetType">In case the symbols passed are a sub set of a bigger sequence, 
        /// provide the already identified alphabet type of the sequence.</param>
        /// <returns>Returns the detected alphabet type or null if detection fails.</returns>
        /// <remarks>
        ///     Returns <see cref="DnaAlphabet"/>, <see cref="AmbiguousDnaAlphabet"/>,
        ///     <see cref="RnaAlphabet"/>, <see cref="AmbiguousRnaAlphabet"/>, <see cref="ProteinAlphabet"/>
        ///     or <see cref="AmbiguousProteinAlphabet"/> as appropriate, or <c>null</c>
        ///     if none contains all the given symbols.
        /// </remarks>
        public static IAlphabet AutoDetectAlphabet(byte[] symbols, long offset, long length, IAlphabet identifiedAlphabetType)
        {
            int currentPriorityIndex = 0;

            if (identifiedAlphabetType == null)
            {
                identifiedAlphabetType = AlphabetPriorityList[0];
            }

            while (identifiedAlphabetType != AlphabetPriorityList[currentPriorityIndex])
            {
                // Increment priority index and validate boundary condition
                if (++currentPriorityIndex == AlphabetPriorityList.Count)
                {
                    throw new ArgumentException(CouldNotRecognizeAlphabet, nameof(identifiedAlphabetType));
                }
            }

            // Start validating against alphabet types according to their priority
            while (!AlphabetPriorityList[currentPriorityIndex].ValidateSequence(symbols, offset, length))
            {
                // Increment priority index and validate boundary condition
                if (++currentPriorityIndex == AlphabetPriorityList.Count)
                {
                    // Last ditch effort - look at all registered alphabets and see if any contain all the located symbols.
                    foreach (IAlphabet alphabet in All)
                    {
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

            return AlphabetPriorityList[currentPriorityIndex];
        }

        /// <summary>
        ///     This methods loops through supported Protein alphabet types and tries to identify
        ///     the best alphabet type for the given symbols.
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
        ///     Returns <see cref="StrictProteinAlphabet"/>, <see cref="ProteinAlphabet"/>,
        ///     <see cref="AmbiguousProteinAlphabet"/>, <see cref="PeaksPeptideAlphabet"/>, 
        ///     or <see cref="ProteinScapePeptideAlphabet"/> as appropriate, or <c>null</c>
        ///     if none contains all the given symbols.
        /// </remarks>
        public static IStrictProteinAlphabet AutoDetectProteinAlphabet(byte[] symbols, long offset, long length, 
            IStrictProteinAlphabet identifiedAlphabetType)
        {
            int currentPriorityIndex = 0;

            if (identifiedAlphabetType == null)
            {
                identifiedAlphabetType = ProteinAlphabetPriorityList[0];
            }

            while (identifiedAlphabetType != ProteinAlphabetPriorityList[currentPriorityIndex])
            {
                // Increment priority index and validate boundary condition
                if (++currentPriorityIndex == ProteinAlphabetPriorityList.Count)
                {
                    throw new ArgumentException(CouldNotRecognizeAlphabet, nameof(identifiedAlphabetType));
                }
            }

            // Start validating against alphabet types according to their priority
            while (!ProteinAlphabetPriorityList[currentPriorityIndex].ValidateSequence(symbols, offset, length))
            {
                // Increment priority index and validate boundary condition
                if (++currentPriorityIndex == ProteinAlphabetPriorityList.Count)
                {
                    // Last ditch effort - look at all registered alphabets and see if any contain all the located symbols.
                    foreach (IAlphabet alphabet in All)
                    {
                        // Make sure alphabet supports validation -- if not, ignore it.
                        try
                        {
                            if (alphabet.IsProtein && alphabet.ValidateSequence(symbols, offset, length))
                                return alphabet as IStrictProteinAlphabet;
                        }
                        catch (NotImplementedException)
                        {
                        }
                    }

                    // No alphabet found.
                    return null;
                }
            }

            return ProteinAlphabetPriorityList[currentPriorityIndex];
        }

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
                currentAlphabet == StrictProteinAlphabet.Instance ||
                currentAlphabet == ProteinAlphabet.Instance)
            {
                return AmbiguousAlphabetMap[currentAlphabet];
            }

            return currentAlphabet;
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
