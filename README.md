# File Utilities

- This is a command for Windows Systems to wrap some utilities commands like `mv` and `cp` and `touch`.
- I made this because the ones on PowerShell are too hard to use and I'm dumb.

## Usage

- `fs clear (cls)`: Clear the console screen.
- `fs help (h)`: Display this message.
- `fs ls -r -i [include] -e [exclude]`: List segments.
  - `-r`: Display recursively.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
- `fs cp [source] [destination] -p -r`: Copy from `[source]` to `[destination]`.
  - `-p`: Use patterns.
  - `-r`: Search recursively.
- `fs mv [source] [destination] -p -r -o`: Move from `[source]` to `[destination]`.
  - `-p`: Use patterns.
  - `-r`: Search recursively.
  - `-o`: Overwrite existing files.
- `fs rm [path] -p -r`: Remove `[path]`.
  - `-p`: Use patterns.
  - `-r`: Search recursively.
- `fs nm [path] [name]`: Rename `[path]` to `[name]`.
- `fs mkdir [path]`: Make a new directory at `[path]`.
- `fs touch [path]`: Update the file at `[path]` if it exists, or create it.
- `fs info [path]`: Display the file information at `[path]`.
- `fs size [path]`: Display the file size in bytes at `[path]`.
- `fs open (o)`: Open the settings JSON.

## Building

1. Clone the repository: `git clone https://github.com/yojoecapital/FileUtilities.git`
2. Restore the [CliFramework](https://github.com/yojoecapital/CliFramework) submodule: `git submodule init && git submodule update`
3. Restore the NuGet Packages using the NuGet CLI: `nuget restore`
4. Build the application using the .NET CLI: `dotnet build`
5. Run the executable located in `FileUtilitiesCore/bin`

## Contact

For any inquiries or feedback, contact me at [yousefsuleiman10@gmail.com](mailto:yousefsuleiman10@gmail.com).