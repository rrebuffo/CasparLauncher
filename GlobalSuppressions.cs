// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

// This suppression is added to avoid problems with LibraryImportAttribute and 'Any CPU' builds.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time", Justification = "<Pending>", Scope = "member", Target = "~M:CasparLauncher.Launcher.Job.CreateJobObject(System.IntPtr,System.String)~System.IntPtr")]
