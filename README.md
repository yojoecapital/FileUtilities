# File Utilities

- This is a command for Windows Systems to wrap some utilities commands like `mv` and `cp` and `touch`.
- I made this because the ones on PowerShell are too hard to use and I'm dumb.

## Usage

### Default

- `quit (q) (exit)`: Exit the program.
- `clear (cls)`: Clear the console screen.
- `help (h)`: Display this message.
- `cd`: Print the current directory.
- `open (o)`: Open the settings JSON.

### List and Filter Segments

- `ls -r -i [include] -e [exclude]`: List segments in the current directory.
  - `-r`: Display recursively.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
- `find [dirs...] -r -cd -i [include] -e [exclude]`: List segments in `[dirs...]`. 
  - `-r`: Display recursively.
  - `-cd`: Display relative to the current directory.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.

### Copy, Move, and Remove 

- `cp [src...] [dest] -r -o -y -i [include] -e [exclude]`: Copy from `[src...]` to `[dest]`.
  - Use `*` in `[dest]` as a macro for the file name of `[src]`.
  - `-r`: Copy recursively.
  - `-o`: Overwrite existing items.
  - `-y`: Skip the prompt.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
- `mv [src...] [dest] -r -o -y -i [include] -e [exclude]`: Move from `[src...]` to `[dest]`.
  - Use `*` in `[dest]` as a macro for the file name of `[src]`.
  - `-r`: Move recursively.
  - `-o`: Overwrite existing items.
  - `-y`: Skip the prompt.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
- `rm [paths...] -r -f -y -i [include] -e [exclude]`: Remove from `[paths...]`.
  - `-r`: Remove recursively.
  - `-f`: Bypass recycle bin.
  - `-y`: Skip the prompt.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.

### Make Files and Directories

- `mkdir [dirs...]`: Make new directories at `[dirs...]`.
- `touch [files...]`: Update the files at `[files...]` if they exist. Otherwise, create them.

### Metadata

- `info [paths...]`: Display the directory or file information at `[paths...]`.

### Scripting

- `exec (e) [name] [args...] -nw`: Execute a script item.
  - `-nw`:  Create a new window for the process.
- `make script [name]`: Steps to make a new script item.
- `remove script [name]`: Remove a script item.
- `dir scripts -o`: Print the script items directory.
  - `-o`: Open the directory.
- `list scripts`: List the available scripts.
- `open script [name]`: Open the file for a script item.

## Building

1. Clone the repository: `git clone https://github.com/yojoecapital/FileUtilities.git`
2. Restore the [CliFramework](https://github.com/yojoecapital/CliFramework) submodule: `git submodule init && git submodule update`
3. Restore the NuGet Packages using the NuGet CLI: `nuget restore`
4. Build the application using the .NET CLI: `dotnet build`
5. Run the executable located in `FileUtilitiesCore/bin`

## Contact

For any inquiries or feedback, contact me at [yousefsuleiman10@gmail.com](mailto:yousefsuleiman10@gmail.com).