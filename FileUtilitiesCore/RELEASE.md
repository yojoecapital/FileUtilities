# Release Notes - Version [3.1.0]

- `fs size` was removed. Just use `fs info` to get size information which is labeled as `bytes` in the output.
  
- `fs info` can now take multiple paths.
  
- `fs find` can now take multiple directories.
  

## Commands

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
- `find [dirs...] -r -cd -i [include] -e [exclude]`: List segments in `[dirs...]`. ✨
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

### Metadata ✨

- `info [paths...]`: Display the directory or file information at `[paths...]`.

### Scripting

- `exec (e) [name] [args...]`: Execute a script item.
- `make script [name]`: Steps to make a new script item.
- `remove script [name]`: Remove a script item.
- `dir scripts -o`: Print the script items directory.
  - `-o`: Open the directory.
- `list scripts`: List the available scripts.
- `open script [name]`: Open the file for a script item.

## Settings JSON

This is an example of how the settings JSON may look like. You can use `fs open` to open the JSON.

```json
{
  "scriptsPath": "scripts",
  "endBlock":  "</script>",
  "methods": {
    "cmd": {
      "path": "cmd.exe",
      "setup": ["/c"],
      "extension": "bat"
    },
    "ps": {
      "path": "pwsh.exe",
      "extension": "ps1"
    },
    "py": {
      "path": "python.exe",
      "extension": "py"
    }
  }
}
```

### JSON Fields

- `"scriptsPath"`: The path to the script items. You can use this to save the scripts to a different directory, such as a shared directory. Make sure the directory is used exclusively for script items.
- `"endBlock"`: This is the token to end a script when running `make script [name]`.
- `"methods"`: These are methods used to execute a script item.
  - `"path"`: What should execute the script.
  - `"setup"`: Pre-arguments such as `/c` for `cmd.exe`.
  - `"extension"`: What file extension should be used when saving the script item's file.