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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Presentation.Regions.Behaviors;
using Microsoft.Practices.Composite.Presentation.Tests.Mocks;
using Microsoft.Practices.Composite.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Presentation.Tests.Regions.Behaviors
{
    [TestClass]
    public class DelayedRegionCreationBehaviorFixture
    {
        private DelayedRegionCreationBehavior GetBehavior(DependencyObject control, MockRegionManagerAccessor accessor, MockRegionAdapter adapter)
        {
            var mappings = new RegionAdapterMappings();
            mappings.RegisterMapping(control.GetType(), adapter);
            var behavior = new DelayedRegionCreationBehavior(mappings);
            behavior.RegionManagerAccessor = accessor;
            behavior.TargetElement = control;
            return behavior;
        }


        private DelayedRegionCreationBehavior GetBehavior(DependencyObject control, MockRegionManagerAccessor accessor)
        {
            return GetBehavior(control, accessor, new MockRegionAdapter());
        }

        [TestMethod]
        public void RegionWillNotGetCreatedTwiceWhenThereAreMoreRegions()
        {
            var control1 = new MockFrameworkElement();
            var control2 = new MockFrameworkElement();

            var accessor = new MockRegionManagerAccessor
                               {
                                   GetRegionName = d => d == control1 ? "Region1" : "Region2"
                               };

            var adapter = new MockRegionAdapter();
            adapter.Accessor = accessor;

            var behavior1 = this.GetBehavior(control1, accessor, adapter);
            var behavior2 = this.GetBehavior(control2, accessor, adapter);

            behavior1.Attach();
            behavior2.Attach();

            accessor.UpdateRegions();

            Assert.IsTrue(adapter.CreatedRegions.Contains("Region1"));
            Assert.IsTrue(adapter.CreatedRegions.Contains("Region2"));
            Assert.AreEqual(1, adapter.CreatedRegions.Count((name) => name == "Region2"));

        }


        [TestMethod]
        public void RegionGetsCreatedWhenAccessingRegions()
        {
            var control = new MockFrameworkElement();

            var accessor = new MockRegionManagerAccessor
                               {
                                   GetRegionName = d => "myRegionName"
                               };

            var behavior = this.GetBehavior(control, accessor);
            behavior.Attach();

            accessor.UpdateRegions();

            Assert.IsNotNull(RegionManager.GetObservableRegion(control).Value);
            Assert.IsInstanceOfType(RegionManager.GetObservableRegion(control).Value, typeof(IRegion));
        }

        [TestMethod]
        public void RegionDoesNotGetCreatedTwiceWhenUpdatingRegions()
        {
            var control = new MockFrameworkElement();

            var accessor = new MockRegionManagerAccessor
            {
                GetRegionName = d => "myRegionName"
            };

            var behavior = this.GetBehavior(control, accessor);
            behavior.Attach();
            accessor.UpdateRegions();
            IRegion region = RegionManager.GetObservableRegion(control).Value;

            accessor.UpdateRegions();

            Assert.AreSame(region, RegionManager.GetObservableRegion(control).Value);
        }

        [TestMethod]
        public void BehaviorDoesNotPreventControlFromBeingGarbageCollected()
        {
            var control = new MockFrameworkElement();
            WeakReference controlWeakReference = new WeakReference(control);

            var accessor = new MockRegionManagerAccessor
                               {
                                   GetRegionName = d => "myRegionName"
                               };

            var behavior = this.GetBehavior(control, accessor);
            behavior.Attach();

            Assert.IsTrue(controlWeakReference.IsAlive);
            GC.KeepAlive(control);

            control = null;
            GC.Collect();

            Assert.IsFalse(controlWeakReference.IsAlive);
        }

        [TestMethod]
        public void BehaviorDoesNotPreventControlFromBeingGarbageCollectedWhenRegionWasCreated()
        {
            var control = new MockFrameworkElement();
            WeakReference controlWeakReference = new WeakReference(control);

            var accessor = new MockRegionManagerAccessor
            {
                GetRegionName = d => "myRegionName"
            };

            var behavior = this.GetBehavior(control, accessor);
            behavior.Attach();
            accessor.UpdateRegions();
            
            Assert.IsTrue(controlWeakReference.IsAlive);
            GC.KeepAlive(control);

            control = null;
            GC.Collect();

            Assert.IsFalse(controlWeakReference.IsAlive);
        }

        [TestMethod]
        public void BehaviorShouldUnhookEventWhenDetaching()
        {
            var control = new MockFrameworkElement();

            var accessor = new MockRegionManagerAccessor
                               {
                                   GetRegionName = d => "myRegionName",
                               };
            var behavior = this.GetBehavior(control, accessor);
            behavior.Attach();

            int startingCount = accessor.GetSubscribersCount();

            behavior.Detach();

            Assert.AreEqual<int>(startingCount - 1, accessor.GetSubscribersCount());
        }

        [TestMethod]
        public void ShouldCleanupBehaviorOnceRegionIsCreated()
        {
            var control = new MockFrameworkElement();

            var accessor = new MockRegionManagerAccessor
            {
                GetRegionName = d => "myRegionName"
            };

            var behavior = this.GetBehavior(control, accessor);
            WeakReference behaviorWeakReference = new WeakReference(behavior);
            behavior.Attach();
            accessor.UpdateRegions();
            Assert.IsTrue(behaviorWeakReference.IsAlive);
            GC.KeepAlive(behavior);

            behavior = null;
            GC.Collect();

            Assert.IsFalse(behaviorWeakReference.IsAlive);
        }
    }
}