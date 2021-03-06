//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Regions;

namespace Microsoft.Practices.Prism.MefExtensions.Regions
{
    /// <summary>
    /// Exports the ContentControlRegionAdapter using the Managed Extensibility Framework (MEF).
    /// </summary>
    /// <remarks>
    /// This allows the <see cref="MefBootstrapper.ConfigureContainer" /> to provide this class as a default implementation.
    /// If another implementation is found, this export will not be used.
    /// </remarks>
    [Export(typeof(ContentControlRegionAdapter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MefContentControlRegionAdapter : ContentControlRegionAdapter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MefContentControlRegionAdapter"/> class.
        /// </summary>
        /// <param name="regionBehaviorFactory">The region behavior factory.</param>
        [ImportingConstructor]
        public MefContentControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {
        }
    }
}