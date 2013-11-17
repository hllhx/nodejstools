﻿/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. 
 *
 * This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
 * copy of the license can be found in the License.html file at the root of this distribution. If 
 * you cannot locate the Apache License, Version 2.0, please send an email to 
 * vspython@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
 * by the terms of the Apache License, Version 2.0.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * ***************************************************************************/

using System.IO;
using Microsoft.NodejsTools.Npm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NpmTests{
    [TestClass]
    public class InstallUninstallPackageTests : AbstractFilesystemPackageJsonTests{
        [TestMethod]
        public void TestAddPackageToSimplePackageJsonThenUninstall(){
            var rootDir = CreateRootPackage(PkgSimple);
            var controller = NpmControllerFactory.Create(rootDir);
            controller.Refresh();
            var rootPackage = controller.RootPackage;
            Assert.IsNotNull(rootPackage, "Root package with no dependencies should not be null.");
            Assert.AreEqual(0, rootPackage.Modules.Count, "Should be no modules before package install.");

            using (var commander = controller.CreateNpmCommander()){
                AsyncHelpers.RunSync(() => commander.InstallPackageByVersionAsync("sax", "*", DependencyType.Standard));
            }

            Assert.AreNotEqual(
                rootPackage,
                controller.RootPackage,
                "Root package should be different after package installed.");

            rootPackage = controller.RootPackage;
            Assert.IsNotNull(rootPackage, "Root package should not be null after package installed.");
            Assert.AreEqual(1, rootPackage.Modules.Count, "Should be one module after package installed.");

            var module = controller.RootPackage.Modules["sax"];
            Assert.IsNotNull(module, "Installed package should not be null.");
            Assert.AreEqual("sax", module.Name, "Module name mismatch.");
            Assert.IsNotNull(module.PackageJson, "Module package.json should not be null.");
            Assert.IsTrue(
                module.IsListedInParentPackageJson,
                "Should be listed as a dependency in parent package.json.");
            Assert.IsFalse(module.IsMissing, "Should not be marked as missing.");
            Assert.IsFalse(module.IsDevDependency, "Should not be marked as dev dependency.");
            Assert.IsFalse(module.IsOptionalDependency, "Should not be marked as optional dependency.");
            Assert.IsFalse(module.IsBundledDependency, "Should not be marked as bundled dependency.");
            Assert.IsTrue(module.HasPackageJson, "Module should have its own package.json");

            using (var commander = controller.CreateNpmCommander()){
                AsyncHelpers.RunSync(() => commander.UninstallPackageAsync("sax"));
            }

            Assert.AreNotEqual(
                rootPackage,
                controller.RootPackage,
                "Root package should be different after package uninstalled.");

            rootPackage = controller.RootPackage;
            Assert.IsNotNull(rootPackage, "Root package should not be null after package uninstalled.");
            Assert.AreEqual(0, rootPackage.Modules.Count, "Should be no modules after package installed.");
        }

        [TestMethod]
        public void TestAddPackageNoPackageJsonThenUninstall(){
            var rootDir = CreateRootPackage(PkgSimple);
            File.Delete(Path.Combine(rootDir, "package.json"));
            var controller = NpmControllerFactory.Create(rootDir);
            controller.Refresh();
            var rootPackage = controller.RootPackage;
            Assert.IsNotNull(rootPackage, "Root package with no dependencies should not be null.");
            Assert.AreEqual(0, rootPackage.Modules.Count, "Should be no modules before package install.");

            using (var commander = controller.CreateNpmCommander()){
                AsyncHelpers.RunSync(() => commander.InstallPackageByVersionAsync("sax", "*", DependencyType.Standard));
            }

            Assert.AreNotEqual(
                rootPackage,
                controller.RootPackage,
                "Root package should be different after package installed.");

            rootPackage = controller.RootPackage;
            Assert.IsNotNull(rootPackage, "Root package should not be null after package installed.");
            Assert.AreEqual(1, rootPackage.Modules.Count, "Should be one module after package installed.");

            var module = controller.RootPackage.Modules["sax"];
            Assert.IsNotNull(module, "Installed package should not be null.");
            Assert.AreEqual("sax", module.Name, "Module name mismatch.");
            Assert.IsNotNull(module.PackageJson, "Module package.json should not be null.");
            Assert.IsFalse(
                module.IsListedInParentPackageJson,
                "Should be listed as a dependency in parent package.json.");
            Assert.IsFalse(module.IsMissing, "Should not be marked as missing.");
            Assert.IsFalse(module.IsDevDependency, "Should not be marked as dev dependency.");
            Assert.IsFalse(module.IsOptionalDependency, "Should not be marked as optional dependency.");
            Assert.IsFalse(module.IsBundledDependency, "Should not be marked as bundled dependency.");
            Assert.IsTrue(module.HasPackageJson, "Module should have its own package.json");

            using (var commander = controller.CreateNpmCommander()){
                AsyncHelpers.RunSync(() => commander.UninstallPackageAsync("sax"));
            }

            Assert.AreNotEqual(
                rootPackage,
                controller.RootPackage,
                "Root package should be different after package uninstalled.");

            rootPackage = controller.RootPackage;
            Assert.IsNotNull(rootPackage, "Root package should not be null after package uninstalled.");
            Assert.AreEqual(0, rootPackage.Modules.Count, "Should be no modules after package installed.");
        }
    }
}