# File Utilities

- This is a command for Windows Systems to wrap some utilities commands like `mv` and `cp` and `touch`.
- I made this because the ones on PowerShell are too hard to use and I'm dumb.

## Usage

- `fs ls [path]`: List segments of `[path]` or current directory.
- `fs cp [source] [destination]`: Copy from `[source]` to `[destination]`.
- `fs mv [source] [destination]`: Move from `[source]` to `[destination]`.
- `fs rm [path]`: Remove `[path]`.
- `fs info [path]`: Prints the info of `[path]`.
- `fs size [path]`: Prints the size of `[path]`.
- `fs find [pattern] [path]`: Finds all segments matching `[pattern]` in `[path]` or current directory.
- `fs mkdir [path]`: Make a new directory `[path]`.
- `fs rmdir [path]`: Remove an empty directory `[path]`.
- `fs touch [path]`: Touches `[path]` if exists or makes a new file there.

## Building

1. Clone the repository: `git clone https://github.com/yojoecapital/FileUtilities.git`
2. Restore the [CliFramework](https://github.com/yojoecapital/CliFramework) submodule: `git submodule init && git submodule update`
3. Restore the NuGet Packages using the NuGet CLI: `nuget restore`
4. Build the application using the .NET CLI: `dotnet build`
5. Run the executable located in `FileUtilitiesCore/bin`

## Contact

For any inquiries or feedback, contact me at [yousefsuleiman10@gmail.com](mailto:yousefsuleiman10@gmail.com).