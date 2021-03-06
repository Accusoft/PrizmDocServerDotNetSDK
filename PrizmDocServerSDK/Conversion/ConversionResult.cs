using System;
using System.Collections.Generic;

namespace Accusoft.PrizmDocServer.Conversion
{
    /// <summary>
    /// Represents a conversion result, either a successful result which can be downloaded as a file or an error result if a page or set of pages could not be converted.
    /// </summary>
    public class ConversionResult
    {
        private readonly RemoteWorkFile remoteWorkFile;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Only used internally.")]
        internal ConversionResult(RemoteWorkFile remoteWorkFile, int pageCount, IEnumerable<ConversionSourceDocument> sources)
        {
            this.remoteWorkFile = remoteWorkFile ?? throw new ArgumentNullException("remoteWorkFile");
            this.PageCount = pageCount;
            this.Sources = sources ?? throw new ArgumentNullException("sources");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Only used internally.")]
        internal ConversionResult(string errorCode, IEnumerable<ConversionSourceDocument> sources)
        {
            this.ErrorCode = errorCode;
            this.Sources = sources;
        }

        /// <summary>
        /// Gets a value indicating whether this result represents a successfully-produced output document.
        /// When <c>true</c>, this is a successful result and you can use <see cref="RemoteWorkFile"/> to download this particular output file.
        /// When <c>false</c>, this is an error result. See the <see cref="ErrorCode"/> for more information.
        /// <value><c>true</c> if this result was successful and has an associated <see cref="RemoteWorkFile"/>; <c>false</c> if
        /// this result represents an error and has an associated <see cref="ErrorCode"/>.</value>
        /// </summary>
        public bool IsSuccess
        {
            get { return !this.IsError; }
        }

        /// <summary>
        /// Gets a value indicating whether this result represents an error.
        /// When <c>true</c>, this is an error result. See the <see cref="ErrorCode"/> for more information.
        /// When <c>false</c>, this is a successful result and you can use <see cref="RemoteWorkFile"/> to download this particular output file.
        /// <value><c>true</c> if this result represents an error and has an associated <see cref="ErrorCode"/>; <c>false</c> if
        /// this result was successful and has an associated <see cref="RemoteWorkFile"/>.</value>
        /// </summary>
        public bool IsError
        {
            get { return this.ErrorCode != null; }
        }

        /// <summary>
        /// Gets the specific error code if this is an error result, or <c>null</c> if this is a successful result.
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// Gets the <see cref="RemoteWorkFile"/> for a successful result.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IsError"/> was true; this result has no associated <see cref="RemoteWorkFile"/>.</exception>
        public RemoteWorkFile RemoteWorkFile
        {
            get
            {
                // We want to throw if someone tries to get the RemoteWorkFile from an error result,
                // because we don't want them storing null to some var and then use it much later
                // only to discover they can't. So we fail fast.
                if (this.IsError)
                {
                    throw new InvalidOperationException("This result represents an error instead of a success. It has no RemoteWorkFile. Make sure you check IsSuccess is true before accessing the RemoteWorkFile property.");
                }

                return this.remoteWorkFile;
            }
        }

        /// <summary>
        /// Gets the total number of pages for a successful result, or
        /// <c>null</c> for an error result.
        /// </summary>
        public int? PageCount { get; }

        /// <summary>
        /// Gets the collection of source documents which contributed to this specific result.
        /// </summary>
        public IEnumerable<ConversionSourceDocument> Sources { get; }
    }
}
