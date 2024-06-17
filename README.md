# File Utilities

- This is a command for Windows Systems to wrap some utilities commands like `mv` and `cp` and `touch`.
- I made this because the ones on PowerShell are too hard to use and I'm dumb.

## Usage

### Default

- `fs quit (q) (exit)`: Exit the program.
- `fs clear (cls)`: Clear the console screen.
- `fs help (h) [filter]`: Display the help message. Use an optional `[filter]` on commands. ✨
- `fs cd`: Prints the current directory. This *does not* set the current directory.

### List and Filter Segments

- `fs ls -r -i [include] -e [exclude]`: List segments of the current directory.
  - `-r`: Display recursively.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
- `fs find [dir] -r -cd -i [include] -e [exclude]`: List segments of `[dir]`. 
  - `-r`: Display recursively.
  - `-cd`:  Display relative to the current directory. ✨
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.

### Copy, Move, and Remove ✨

- Note that you can include multiple sources in `[src...]`.
- Note that when globs or the recursive flag are included, they will be considered if a source is a directory. Moreover, they will only affect *files* (i.e., directories will not be removed).
- `fs cp [src...] [dest] -r -o -y -i [include] -e [exclude]`: Copy from `[src...]` to `[dest]`.
  - Use `*` in `[dest]` as a macro for the file name of a source.
  - `-r`: Copy recursively.
  - `-o`: Overwrite existing items.
  - `-y`: Answer yes to prompt.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
- `fs mv [src...] [dest] -r -o -y -i [include] -e [exclude]`: Move from `[src...]` to `[dest]`.
  - Use `*` in `[dest]` as a macro for the file name of a source.
  - `-r`: Move recursively.
  - `-o`: Overwrite existing items.
  - `-y`: Answer yes to prompt.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
- `fs rm [path...] -r -f -y -i [include] -e [exclude]`: Remove from `[src...]`.
  - `-r`: Remove recursively.
  - `-f`: Bypass the default behavior of putting the file in the recycle bin.
  - `-y`: Answer yes to prompt.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.

### Rename

- `fs nm [path] [name]`: Rename the item at `[path]` to `[name]`.

### Make Files and Directories ✨

- `fs mkdir [dirs...]`: Make new directories at `[dir...]`. 
- `fs touch [files...]`: Update the files at `[files...]` if it exists. Otherwise, create it.

### Metadata ✨

- `fs info [path]`: Display the directory or file information at `[path]`.
- `fs size [path]`: Display the directory or file size in bytes at `[path]`.

### Scripting

- `fs exec [name] [args...]`: Execute a script item.
- `fs make script [name]`: Steps to make a new script item.
- `fs remove script [name]`: Remove a script item.
- `fs open script [name]`: Opens the file for a script item.
- `fs dir scripts -o`: Prints the script items directory.
  - `-o`: Open the directory.
- `fs list scripts`: Lists the available scripts.

### Other

- `fs open (o)`: Open the settings JSON.

## Building

1. Clone the repository: `git clone https://github.com/yojoecapital/FileUtilities.git`
2. Restore the [CliFramework](https://github.com/yojoecapital/CliFramework) submodule: `git submodule init && git submodule update`
3. Restore the NuGet Packages using the NuGet CLI: `nuget restore`
4. Build the application using the .NET CLI: `dotnet build`
5. Run the executable located in `FileUtilitiesCore/bin`

## Contact

For any inquiries or feedback, contact me at [yousefsuleiman10@gmail.com](mailto:yousefsuleiman10@gmail.com).