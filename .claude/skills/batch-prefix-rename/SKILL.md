---
name: batch-prefix-rename
description: Batch rename files in a folder by adding a numeric prefix ("1- ", "2- ", etc.) to each file name. Use this skill whenever the user wants to number files, add prefix IDs to file names, organize files with numeric prefixes, or batch rename files in a directory with sequential numbers. Trigger on phrases like "prefix files", "number the files", "add numbers to files", "rename files in batch", or when the user points at a folder and wants files ordered/numbered.
---

# Batch Prefix Rename

Add a unique numeric prefix (`1- `, `2- `, `3- `, …) to every file in a folder that doesn't already have one.

## File Classification

Every file falls into one of three categories:

1. **Canonical prefix** — name matches `^(\d+)- .+` exactly (e.g. `3- introduction.pdf`). Already correct; skip.

2. **Fuzzy prefix** — name starts with digits followed by a non-standard separator before the rest of the name. The number is extracted and the file is renamed to `<N>- <rest>` (normalizing the separator). The number is preserved; no new ID is assigned.

   Fuzzy separators to recognize (after the leading digits):
   - `.<rest>` — e.g. `1.abc.txt` → `1- abc.txt`
   - ` - <rest>` (space-hyphen-space) — e.g. `2 - abc.txt` → `2- abc.txt`
   - `_<rest>` or `_ <rest>` — e.g. `3_abc.txt`, `3_ abc.txt` → `3- abc.txt`
   - `.<space><rest>` — e.g. `4. abc.txt` → `4- abc.txt`
   - Any other `^\d+\W` pattern where the non-digit lead characters are clearly a separator

   The "rest" is everything after stripping the digits and separator, with leading/trailing whitespace trimmed.

3. **No prefix** — doesn't start with digits at all. Assign a new unique ID.

## Rules

- **Canonical files are never touched**, even if their number conflicts with another file.
- **Fuzzy-prefixed files are normalized** to canonical format, preserving their existing number.
- **Un-prefixed files get a new unique ID**: the lowest positive integer not already claimed by any file in the folder (canonical or fuzzy).
- **Only files, not subdirectories** — skip subdirectories unless the user explicitly says to recurse.
- **Dry-run first.** Show the proposed renames before executing. Ask for confirmation unless the user already said "go ahead".

## Algorithm

```
1. List all files in the folder.
2. Classify each file as canonical, fuzzy, or unprefixed.
3. Collect used numbers: extract the number from every canonical and fuzzy file.
4. Build rename plan:
   - Fuzzy files: rename to "<extracted-N>- <rest>"
   - Unprefixed files (alpha order): assign lowest free N, rename to "<N>- <name>"
5. Present plan to user, then execute on confirmation.
```

## Implementation

```python
import os, re

folder = "."  # replace with target folder

CANONICAL = re.compile(r"^(\d+)- (.+)$")
FUZZY     = re.compile(r"^(\d+)[.\-_ ]+(.+)$")  # digits then non-word separator

files = sorted(f for f in os.listdir(folder)
               if os.path.isfile(os.path.join(folder, f)))

used = set()
canonical = set()
fuzzy_files = []   # (filename, n, rest)
plain_files = []   # filename

for f in files:
    m = CANONICAL.match(f)
    if m:
        used.add(int(m.group(1)))
        canonical.add(f)
        continue
    m = FUZZY.match(f)
    if m:
        n, rest = int(m.group(1)), m.group(2).strip()
        used.add(n)
        fuzzy_files.append((f, n, rest))
    else:
        plain_files.append(f)

plan = []

# Normalize fuzzy-prefixed files
for f, n, rest in fuzzy_files:
    new_name = f"{n}- {rest}"
    if new_name != f:
        plan.append((f, new_name))

# Assign new IDs to unprefixed files
for f in plain_files:
    n = 1
    while n in used:
        n += 1
    used.add(n)
    plan.append((f, f"{n}- {f}"))

# Dry-run output
for old, new in plan:
    print(f"  {old!r}  →  {new!r}")
```

Execute after confirmation:
```python
for old, new in plan:
    os.rename(os.path.join(folder, old), os.path.join(folder, new))
```

## Example

Folder contents before:
```
3- introduction.pdf
2 - chapter one.pdf
10_ appendix.pdf
1.overview.pdf
notes.pdf
summary.pdf
```

Classification:
- `3- introduction.pdf` → canonical (skip)
- `2 - chapter one.pdf` → fuzzy, N=2, rest=`chapter one.pdf`
- `10_ appendix.pdf` → fuzzy, N=10, rest=`appendix.pdf`
- `1.overview.pdf` → fuzzy, N=1, rest=`overview.pdf`
- `notes.pdf` → unprefixed
- `summary.pdf` → unprefixed

Used numbers: {1, 2, 3, 10}. Free for new IDs: 4, 5, …

Plan:
- `2 - chapter one.pdf` → `2- chapter one.pdf`
- `10_ appendix.pdf`    → `10- appendix.pdf`
- `1.overview.pdf`      → `1- overview.pdf`
- `notes.pdf`           → `4- notes.pdf`
- `summary.pdf`         → `5- summary.pdf`

Result:
```
1- overview.pdf
2- chapter one.pdf
3- introduction.pdf   ← unchanged
4- notes.pdf
5- summary.pdf
10- appendix.pdf
```
