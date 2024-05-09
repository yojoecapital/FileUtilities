# File Utilities

- This is a command for Windows Systems to wrap some utilities commands like `mv` and `cp` and `touch`.
- I made this because the ones on PowerShell are too hard to use and I'm dumb.

## Usage

- `fs help (h)`: Display this message.
- `fs ls -r -i [include] -e [exclude]`: List segments of the current directory.
  - `-r`: Display recursively.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
- `fs find [dir] -r -i [include] -e [exclude]`: List segments of `[dir]`.
  - `-r`: Display recursively.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
- `fs cp [path] [dir] -r -o -i [include] -e [exclude]`: Copy from `[path]` into `[dir]`.
  - `-r`: Copy recursively.
  - `-o`: Overwrite existing items.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
- `fs mv [path] [dir] -r -o -i [include] -e [exclude]`: Move from `[path]` into `[dir]`.
  - `-r`: Move recursively.
  - `-o`: Overwrite existing items.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
- `fs rm [path] -r -i [include] -e [exclude]`: Remove from `[path]`.
  - `-r`: Remove recursively.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
- `fs nm [path] [name]`: Rename the item at `[path]` to `[name]`.
- `fs mkdir [dir]`: Make a new directory at `[dir]`.
- `fs touch [file]`: Update the file at `[file]` if it exists. Otherwise, create it.
- `fs info [file]`: Display the file information at `[file]`.
- `fs size [file]`: Display the file size in bytes at `[file]`.
- `fs open (o)`: Open the settings JSON.

## Building

1. Clone the repository: `git clone https://github.com/yojoecapital/FileUtilities.git`
2. Restore the [CliFramework](https://github.com/yojoecapital/CliFramework) submodule: `git submodule init && git submodule update`
3. Restore the NuGet Packages using the NuGet CLI: `nuget restore`
4. Build the application using the .NET CLI: `dotnet build`
5. Run the executable located in `FileUtilitiesCore/bin`

## Contact

For any inquiries or feedback, contact me at [yousefsuleiman10@gmail.com](mailto:yousefsuleiman10@gmail.com).