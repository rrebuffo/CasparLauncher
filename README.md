# CasparLauncher

Frontend app for CasparCG Server.
For more information visit CasparCG's forum: https://casparcgforum.org/

---

## What does this app do?

This app launches and keeps track of the configured executables, restarting them when they crash and if wanted avoiding running multiple instances of the same executable.  
It has a focus on CasparCG server processes and dependencies (`casparcg.exe`, `scanner.exe` and `casparcg.config`) and it provides a UI for editing options for the latter, but it can be used to launch and keep alive any program.

## Installing

*This is a Windows desktop app requiring the .NET 8 **Desktop** Runtime in case the framework independent binaries are used. It can be downloaded here: https://dotnet.microsoft.com/en-us/download/dotnet/8.0*

The simplest way of running CasparCG server is to place this executable inside the server folder and run it with the default executables.  
It can also be installed in a common folder (like `%PROGRAMFILES%`) and point to the path to the executables manually.  
To run the app at startup simply check "Open at login" from the Options menu.


## Compiling

* Open `CasparLauncher.sln` in Visual Studio 2022 and select the CasparLauncher project then click Build > Publish.
* Inside the 'CasparLauncher: Publish' window, select either `FrameworkIndpendent.pubxml` or `SelfContained_x64.pubxml` then click Publish.
  This will build the project into the bin\Release folder and pack all the assemblies into a single exe.
  The framework independent publish will generate a binary that uses the system's latest .NET Desktop framework (requiring at least .NET 8 to be installed) and the self contained will pack the latest framework version available to Visual Studio at build time.

----

## License

Copyright © 2024 Mauro Rebuffo

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program.  If not, see <https://www.gnu.org/licenses/>.

---

This app makes use of the following open source projects:
- rrebuffo.BaseUISupport (https://github.com/rrebuffo/BaseUISupport)  
  A custom UI theme and framework for building desktop WPF apps.