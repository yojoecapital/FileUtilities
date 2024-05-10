# Release Notes - Version [2.0.0]

Easy batch script creation was added with safe parameter matching.

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
- `fs mv [src] [dest] -r -o -i [include] -e [exclude]`: Move from `[src]` to `[dest]`.
  - `-r`: Move recursively.
  - `-o`: Overwrite existing items.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.
- `fs rm [path] -r -i [include] -e [exclude]`: Remove from `[path]`.
  - `-r`: Remove recursively.
  - `-i [include]`: Include glob patterns.
  - `-e [exclude]`: Exclude glob patterns.

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