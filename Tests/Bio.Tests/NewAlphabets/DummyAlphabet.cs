using System;
using System.Collections;
using System.Collections.Generic;

namespace Bio.Tests.NewAlphabets
{
    internal class DummyAlphabet : IAlphabet
    {
        static DummyAlphabet()
        {
            Instance=new DummyAlphabet();
        }

        protected DummyAlphabet()
        {
            Name = "DummyAlphabet";
            AlphabetType = Alphabets.AlphabetTypes.DNA | Alphabets.AlphabetTypes.PlugIn;
            HasGaps = false;
            HasAmbiguity = false;
            HasTerminations = false;
            IsCaseSensitive = false;
            IsComplementSupported = false;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Alphabets.AlphabetTypes AlphabetType { get; }

        public int Count { get; } = 2;

        public bool HasAmbiguity { get; }

        public bool HasGaps { get; }

        public bool HasTerminations { get; }

        /// <summary>
        ///     Static instance of this class.
        /// </summary>
        public static readonly DummyAlphabet Instance;

        public bool IsCaseSensitive { get; }

        public bool IsComplementSupported { get; }

        public string Name { get; }

        public byte this[int index] => throw new NotImplementedException();

        public bool CheckIsAmbiguous(byte item)
        {
            throw new NotImplementedException();
        }

        public bool CheckIsGap(byte item)
        {
            throw new NotImplementedException();
        }

        public bool CompareSymbols(byte x, byte y)
        {
            throw new NotImplementedException();
        }

        public HashSet<byte> GetAmbiguousSymbols()
        {
            throw new NotImplementedException();
        }

        public byte GetConsensusSymbol(HashSet<byte> symbols)
        {
            throw new NotImplementedException();
        }

        public string GetFriendlyName(byte item)
        {
            throw new NotImplementedException();
        }

        public byte[] GetSymbolValueMap()
        {
            throw new NotImplementedException();
        }

        public HashSet<byte> GetValidSymbols()
        {
            return new HashSet<byte> { (byte) '$', (byte) '%' };
        }

        public bool TryGetAmbiguousSymbol(HashSet<byte> symbols, out byte ambiguousSymbol)
        {
            throw new NotImplementedException();
        }

        public bool TryGetBasicSymbols(byte ambiguousSymbol, out HashSet<byte> basicSymbols)
        {
            throw new NotImplementedException();
        }

        public bool TryGetComplementSymbol(byte symbol, out byte complementSymbol)
        {
            throw new NotImplementedException();
        }

        public bool TryGetComplementSymbol(byte[] symbols, out byte[] complementSymbols)
        {
            throw new NotImplementedException();
        }

        public bool TryGetDefaultGapSymbol(out byte defaultGapSymbol)
        {
            throw new NotImplementedException();
        }

        public bool TryGetDefaultTerminationSymbol(out byte defaultTerminationSymbol)
        {
            throw new NotImplementedException();
        }

        public bool TryGetGapSymbols(out HashSet<byte> gapSymbols)
        {
            throw new NotImplementedException();
        }

        public bool TryGetTerminationSymbols(out HashSet<byte> terminationSymbols)
        {
            throw new NotImplementedException();
        }

        public bool ValidateSequence(byte[] symbols, long offset, long length)
        {
            throw new NotImplementedException();
        }
    }
}
