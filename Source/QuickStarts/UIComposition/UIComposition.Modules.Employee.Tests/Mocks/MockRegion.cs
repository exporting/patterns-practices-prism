//===============================================================================
// Microsoft patterns & practices
// Composite WPF (PRISM)
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

using System;
using System.ComponentModel;
using System.Windows;
using Prism.Interfaces;
using System.Collections.ObjectModel;

namespace UIComposition.Modules.Employee.Tests.Mocks
{
    public class MockRegion : IRegion
    {
        public bool ShowCalled;
        public int ViewsCount;
        public string NamedViewAdded;
        public IRegionManager AddReturnValue;

        public IRegionManager Add(object view)
        {
            ViewsCount++;
            return null;
        }

        public void Remove(object view)
        {
            ViewsCount--;
        }

        public ICollectionView Views
        {
            get { return null; }
        }

        public void Show(object view)
        {
            ShowCalled = true;
        }

        public IRegionManager Add(object view, string name)
        {
            ViewsCount++;
            NamedViewAdded = name;
            return null;
        }

        public object GetView(string name)
        {
            if (NamedViewAdded == name)
                return new UIElement();

            return null;
        }

        public IRegionManager RegionManager { get; set; }

        public IRegionManager Add(object view, string name, bool createRegionManagerScope)
        {
            ViewsCount++;
            NamedViewAdded = name;
            return AddReturnValue;
        }
    }
}
