﻿using System;
using System.Collections;
using System.Collections.Generic;

using Bio.Core.Extensions;
using Bio.Properties;
using Bio.Util;

namespace Bio.IO.Wiggle
{
    /// <summary>
    ///     Wiggle annotation type.
    /// </summary>
    public enum WiggleAnnotationType
    {
        /// <summary>
        ///     Fixed Step.
        /// </summary>
        FixedStep,

        /// <summary>
        ///     Variable Step.
        /// </summary>
        VariableStep
    }

    /// <summary>
    ///     Wiggle annotation class to represent sequence annotation data in wiggle format.
    ///     Supports fixed/variable step wiggle data.
    /// </summary>
    public class WiggleAnnotation : IEnumerable<KeyValuePair<long, float>>
    {
        // Annotation data if format is fixed step.
        private readonly List<string> annotationComments = new List<string>();

        private float[] fixedStepValues;

        // Annotation data if format is variable step.
        private KeyValuePair<long, float>[] variableStepValues;

        /// <summary>
        ///     Initializes a new instance of the WiggleAnnotation class.
        /// </summary>
        /// <param name="data">Annotation data.</param>
        /// <param name="chromosome">Chromosome name.</param>
        /// <param name="start">Start or Base position.</param>
        /// <param name="step">Step size.</param>
        public WiggleAnnotation(float[] data, string chromosome, long start, int step)
            : this(data, chromosome, start, step, -1)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the WiggleAnnotation class.
        /// </summary>
        /// <param name="data">Annotation data.</param>
        /// <param name="chromosome">Chromosome name.</param>
        /// <param name="start">Start or Base position.</param>
        /// <param name="step">Step size.</param>
        /// <param name="span">Span window.</param>
        public WiggleAnnotation(float[] data, string chromosome, long start, int step, int span)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (chromosome == null)
            {
                throw new ArgumentNullException(nameof(chromosome));
            }

            SetFixedStepAnnotationData(data);
            Chromosome = chromosome;
            BasePosition = start;
            Step = step;
            Span = span;
            Metadata = new Dictionary<string, string> { { WiggleSchema.Type, WiggleSchema.Wiggle0 } };
        }

        /// <summary>
        ///     Initializes a new instance of the WiggleAnnotation class.
        /// </summary>
        /// <param name="data">Annotation data.</param>
        /// <param name="chromosome">Chromosome name.</param>
        public WiggleAnnotation(KeyValuePair<long, float>[] data, string chromosome)
            : this(data, chromosome, -1)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the WiggleAnnotation class.
        /// </summary>
        /// <param name="data">Annotation data.</param>
        /// <param name="chromosome">Chromosome name.</param>
        /// <param name="span">Span window.</param>
        public WiggleAnnotation(KeyValuePair<long, float>[] data, string chromosome, int span)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (chromosome == null)
            {
                throw new ArgumentNullException(nameof(chromosome));
            }

            SetVariableStepAnnotationData(data);
            Chromosome = chromosome;
            Span = span;
            Metadata = new Dictionary<string, string> { { WiggleSchema.Type, WiggleSchema.Wiggle0 } };
        }

        /// <summary>
        ///     Initializes a new instance of the WiggleAnnotation class.
        /// </summary>
        internal WiggleAnnotation()
        {
            Span = -1;
            Metadata = new Dictionary<string, string> { { WiggleSchema.Type, WiggleSchema.Wiggle0 } };
        }

        /// <summary>
        ///     Gets the list of comments on this annotation.
        /// </summary>
        public List<string> Comments
        {
            get
            {
                return annotationComments;
            }
        }

        /// <summary>
        ///     Gets the type of annotation (fixed/variable).
        /// </summary>
        public WiggleAnnotationType AnnotationType { get; internal set; }

        /// <summary>
        ///     Gets the chromosome to which this annotation applies.
        /// </summary>
        public string Chromosome { get; internal set; }

        /// <summary>
        ///     Gets the base position or start in case of fixed step annotation.
        /// </summary>
        public long BasePosition { get; internal set; }

        /// <summary>
        ///     Gets the step in case of fixed step annotation.
        /// </summary>
        public int Step { get; internal set; }

        /// <summary>
        ///     Gets the span. -1 if not applicable.
        /// </summary>
        public int Span { get; internal set; }

        /// <summary>
        ///     Gets the total count of annotation values in this object.
        /// </summary>
        public long Count { get; private set; }

        /// <summary>
        ///     Gets the metadata from the track line of wiggle file.
        /// </summary>
        public Dictionary<string, string> Metadata { get; internal set; }

        /// <summary>
        ///     Gets the location and value of an annotation item at the given index.
        ///     Index is zero based and is the index of the item in the annotation items list,
        ///     not to be confused with the base pair index.
        ///     Base pair index can be calculated as BasePosition + index (fixed step).
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns>Annotation location and value.</returns>
        public KeyValuePair<long, float> this[long index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                if (AnnotationType == WiggleAnnotationType.FixedStep)
                {
                    return new KeyValuePair<long, float>(
                        BasePosition + (index * Step),
                        fixedStepValues[index]);
                }
                return variableStepValues[index];
            }
        }

        /// <summary>
        ///     Gets an enumerator to loop through all the annotation values.
        /// </summary>
        /// <returns>Annotation items enumerator.</returns>
        public IEnumerator<KeyValuePair<long, float>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        /// <summary>
        ///     Gets an enumerator to loop through all the annotation values.
        /// </summary>
        /// <returns>Annotation items enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Gets the annotation values at a given range.
        ///     Supported only for fixed step annotations.
        /// </summary>
        /// <param name="startIndex">Start location.</param>
        /// <param name="length">Total number of values to extract.</param>
        /// <returns>Sub set of annotation data.</returns>
        public float[] GetValueArray(long startIndex, long length)
        {
            if (startIndex < 0 || startIndex + length > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (AnnotationType == WiggleAnnotationType.VariableStep)
            {
                throw new NotSupportedException(Resource.WiggleNotSupportedOnVariableStep);
            }

            float[] result = new float[length];
            Helper.Copy(fixedStepValues, startIndex, result, 0, length);

            return result;
        }

        /// <summary>
        ///     Sets the fixed step annotation values of the current annotation object.
        /// </summary>
        /// <param name="values">Annotation data.</param>
        internal void SetFixedStepAnnotationData(float[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            AnnotationType = WiggleAnnotationType.FixedStep;
            fixedStepValues = values;
            Count = values.GetLongLength();
        }

        /// <summary>
        ///     Sets the variable step annotation values of the current annotation object.
        /// </summary>
        /// <param name="values">Annotation data.</param>
        internal void SetVariableStepAnnotationData(KeyValuePair<long, float>[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            AnnotationType = WiggleAnnotationType.VariableStep;
            variableStepValues = values;
            Count = values.GetLongLength();
        }
    }
}