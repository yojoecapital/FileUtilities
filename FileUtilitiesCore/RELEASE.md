# Release Notes - Version [2.1.1]

## Commands

### Default

- `fs clear (cls)`: Clear the console screen.
- `fs help (h)`: Display the help message.

### List and Filter Segments

- `fs cd`: Prints the current directory. This *does not* set the current directory. 
- `fs ls -r -i [include] -e [exclude]`: List segments of the current directory.
  - `-r`: Display recursively.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
- `fs find [dir] -r -i [include] -e [exclude]`: List segments of `[dir]`.
  - `-r`: Display recursively.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.

### Copy, Move, and Remove

- Note that when globs or the recursive flag are included, they will be considered if the `[src]` is a directory. Moreover, they will only affect *files* (i.e. directories will not be removed).
- `fs cp [src] [dest] -r -o -i [include] -e [exclude]`: Copy from `[src]` to `[dest]`.
  - `-r`: Copy recursively.
  - `-o`: Overwrite existing items.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
  - `-y`: Answer yes to prompt. ✨
- `fs mv [src] [dest] -r -o -i [include] -e [exclude]`: Move from `[src]` to `[dest]`.
  - `-r`: Move recursively.
  - `-o`: Overwrite existing items.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
  - `-y`: Answer yes to prompt. ✨
- `fs rm [path] -r -i [include] -e [exclude]`: Remove from `[path]`.
  - `-r`: Remove recursively.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
  - `-f`: Bypass the default behavior of putting the file in the recycle bin. ✨
  - `-y`: Answer yes to prompt. ✨

### Rename

- `fs nm [path] [name]`: Rename the item at `[path]` to `[name]`.

### Make Files and Directories

- `fs mkdir [dir]`: Make a new directory at `[dir]`.
- `fs touch [file]`: Update the file at `[file]` if it exists. Otherwise, create it.

### Metadata

- `fs info [file]`: Display the file information at `[file]`.
- `fs size [file]`: Display the file size in bytes at `[file]`.

### Scripting 

- `fs exec [name] [...args]`: Execute a script item.
- `fs make script [name]`: Steps to make a new script item.
- `fs remove script [name]`: Remove `[script]`.
- `fs dir scripts -o`: Prints the script items directory.
  - `-o`: Open the directory.
- `fs list scripts`: Lists the available scripts.

### Other

- `fs open (o)`: Open the settings JSON.

## Settings JSON

This is an example of how the settings JSON may look like. You can use `fs open` to open the JSON.

- `"resultsPerPage"`: the number of result displayed in actions like `ls` or `find`.
- `"scriptsPath"`: the path to the the script items. You can use this to save the scripts to a different directory such as a shared directory. Make sure the directory is used exclusively for script items.
- `"endBlock"`: this is the token to end a script when running `make script [name]`.
- `"methods"`: these are methods used to execute a script item.
  - `"path"`: what should execute the script.
  - `"setup"`: pre-arguments such as `\c` for the `cmd.exe`.
  - `"extension"`: what file extension should be used when saving the script item's file.

```json
{
  "resultsPerPage": 15,
  "scriptsPath": "scripts",
  "endBlock":  "</script>",
  "methods": {
    "cmd": {
      "path": "cmd.exe",
      "prefix": ["/c"],
      "extension": "bat" 
    },
    "ps": {
      "path": "pwsh.exe",
      "extension": "ps1" 
    }
  }
}
```