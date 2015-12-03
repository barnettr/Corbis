//===============================================================================
// Enterprise Library 2.0 Extensions
//===============================================================================
// Copyright © 2006 Erwyn van der Meer.
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

using LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration.Design;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration.Design")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("LogicaCMG")]
[assembly: AssemblyProduct("LogicaCMG.EnterpriseLibraryExtensions")]
[assembly: AssemblyCopyright("Copyright © 2006 Erwyn van der Meer")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

[assembly: CLSCompliant(true)]

[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]

[assembly: ConfigurationDesignManager(typeof(LoggingConfigurationDesignManager))]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("8FF46DD8-CB94-4cf7-8174-5BEDE2B3B3BD")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("0.5.2.0")]
[assembly: AssemblyFileVersion("0.5.2.0")]
