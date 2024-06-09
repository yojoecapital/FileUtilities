# File Utilities

- This is a command for Windows Systems to wrap some utilities commands like `mv` and `cp` and `touch`.
- I made this because the ones on PowerShell are too hard to use and I'm dumb.

## Usage

### Default Commands

- `fs clear (cls)`: Clear the console screen.
- `fs help (h)`: Display the help message.

### List and Filter Segments

- `fs ls -r -i [include] -e [exclude]`: List segments of the current directory.
  - `-r`: Display recursively.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
- `fs find [dir] -r -i [include] -e [exclude]`: List segments of `[dir]`.
  - `-r`: Display recursively.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.

### Copy, Move, and Remove

- `fs cp [src] [dest] -r -o -i [include] -e [exclude]`: Copy from `[src]` to `[dest]`.
  - `-r`: Copy recursively.
  - `-o`: Overwrite existing items.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
  - `-y`: Answer yes to prompt.
- `fs mv [src] [dest] -r -o -i [include] -e [exclude]`: Move from `[src]` to `[dest]`.
  - `-r`: Move recursively.
  - `-o`: Overwrite existing items.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
  - `-y`: Answer yes to prompt.
- `fs rm [path] -r -i [include] -e [exclude]`: Remove from `[path]`.
  - `-r`: Remove recursively.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
  - `-f`: Bypass the default behavior of putting the file in the recycle bin.
  - `-y`: Answer yes to prompt.

### Rename

- `fs nm [path] [name]`: Rename the item at `[path]` to `[name]`.

### Make Files and Directories

- `fs mkdir [dir]`: Make a new directory at `[dir]`.
- `fs touch [file]`: Update the file at `[file]` if it exists. Otherwise, create it.

### Metadata

- `fs info [file]`: Display the file information at `[file]`.
- `fs size [file]`: Display the file size in bytes at `[file]`.

### Batch Scripting

- `fs exec [script] [...args]`: Execute a script item.
- `fs make script`: Steps to make a new script item.
- `fs remove script [script]`: Remove `[script]`.

### Internal JSON Files

- `fs open scripts`: Open the script items directory.
- `fs open (o)`: Open the settings JSON.

## Building

1. Clone the repository: `git clone https://github.com/yojoecapital/FileUtilities.git`
2. Restore the [CliFramework](https://github.com/yojoecapital/CliFramework) submodule: `git submodule init && git submodule update`
3. Restore the NuGet Packages using the NuGet CLI: `nuget restore`
4. Build the application using the .NET CLI: `dotnet build`
5. Run the executable located in `FileUtilitiesCore/bin`

## Contact

For any inquiries or feedback, contact me at [yousefsuleiman10@gmail.com](mailto:yousefsuleiman10@gmail.com).