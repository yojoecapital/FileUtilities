# Release Notes - Version [3.0.0]

- `fs cp`, `fs mv`, and `fs rm` allow you use multiple sources.
  - For example, using `fs mv a b c d` on directories `a`, `b`, `c`, and `d` will move (by merging) `a`, `b`, and `c` to`d`.

- You can use `*` in the `[dest]` argument of `fs cp` and `fs mv` to use the same file name of the corresponding source.
  - For example, using `fs cp a.txt b\*` will copy `a.txt` to `b\a.txt`.

- `fs info` and `fs size` now gives information and size for directories as well.
- `fs rm` will now show a dialogue box if the file or directory size is greater than the MB specified in `"dialogueSize"` of the settings JSON.
- `fs help [filter]` can now filter commands with `[filter]`.
- `fs` no longer uses pagination. I prefer to pipe to PowerShell's `more` command.
  - `"resultsPerPage"` was removed and will be ignored from the settings JSON.


## Commands

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

### Make Files and Directories

- `fs mkdir [dir]`: Make a new directory at `[dir]`.
- `fs touch [file]`: Update the file at `[file]` if it exists. Otherwise, create it.

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

## Settings JSON

This is an example of how the settings JSON may look like. You can use `fs open` to open the JSON.

```json
{
  "dialogueSize": 100,
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

- `"dialogueSize"`: Specifies the minimum size in MB for when `fs rm` should use the dialogue box. This will not be done if `-f` is passed.
- `"scriptsPath"`: The path to the script items. You can use this to save the scripts to a different directory, such as a shared directory. Make sure the directory is used exclusively for script items.
- `"endBlock"`: This is the token to end a script when running `make script [name]`.
- `"methods"`: These are methods used to execute a script item.
  - `"path"`: What should execute the script.
  - `"setup"`: Pre-arguments such as `/c` for `cmd.exe`.
  - `"extension"`: What file extension should be used when saving the script item's file.