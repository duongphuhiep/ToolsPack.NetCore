---
name: prefix-conflict-resolver
description: Find and resolve numeric prefix ID conflicts in a folder — where two or more files share the same leading number (e.g. "3- intro.pdf" and "3- summary.pdf"). Use this skill when the user says "fix prefix conflicts", "there are duplicate prefixes", "resolve numbering conflicts", "two files have the same number", or asks to audit/clean up file numbering in a directory.
---

# Prefix Conflict Resolver

Find files that share the same numeric prefix ID and let the user choose which conflicts to fix.

## What counts as a conflict

Two or more files in the same folder whose names match `^(\d+)- ` with the **same** leading number. For example:
```
3- introduction.pdf
3- summary.pdf        ← conflict on 3
```
Files with fuzzy prefixes (`3_`, `3.`, `3 - `) that haven't been normalized yet are out of scope — run the `batch-prefix-rename` skill first to normalize them, then come back here.

## Step 1 — Scan and report

List every conflict group. Present them clearly so the user can see what needs fixing:

```
Conflicts found:

  [3]  3- introduction.pdf
       3- summary.pdf

  [7]  7- chapter one.pdf
       7- notes.pdf
       7- appendix.pdf
```

If no conflicts exist, say so and stop.

## Step 2 — Ask which to resolve

Ask the user which conflict groups to fix. They may say:
- "all" — resolve every group
- a number or list of numbers — e.g. "3" or "3 and 7"
- a specific file name — rename that one file

## Step 3 — Resolve

For each selected conflict group, keep the **first file alphabetically** at its current prefix and assign new unique IDs to all the others.

"Unique" means not used by **any** file currently in the folder (including files outside the conflict groups).

Assign new IDs in alphabetical order of the files being renamed, using the lowest available integers first.

Present the rename plan before executing:
```
Rename plan:

  [3]  3- introduction.pdf  ← kept
       3- summary.pdf       → 11- summary.pdf

  [7]  7- chapter one.pdf   ← kept
       7- notes.pdf         → 12- notes.pdf
       7- appendix.pdf      → 13- appendix.pdf
```

Ask for confirmation, then execute.

## Implementation

```python
import os, re
from collections import defaultdict

folder = "."  # replace with target folder
CANONICAL = re.compile(r"^(\d+)- (.+)$")

files = sorted(f for f in os.listdir(folder)
               if os.path.isfile(os.path.join(folder, f)))

# Group files by prefix number
groups = defaultdict(list)
for f in files:
    m = CANONICAL.match(f)
    if m:
        groups[int(m.group(1))].append(f)

conflicts = {n: fs for n, fs in groups.items() if len(fs) > 1}

if not conflicts:
    print("No conflicts found.")
else:
    for n, fs in sorted(conflicts.items()):
        print(f"  [{n}]")
        for f in fs:
            print(f"       {f}")
```

After the user selects groups to resolve:

```python
# All numbers currently in use (across all files, not just conflicts)
used = set()
for f in files:
    m = CANONICAL.match(f)
    if m:
        used.add(int(m.group(1)))

plan = []
for n in selected_groups:
    fs = conflicts[n]
    keep, *to_rename = fs  # first alphabetically is kept
    for f in to_rename:
        new_n = 1
        while new_n in used:
            new_n += 1
        used.add(new_n)
        m = CANONICAL.match(f)
        rest = m.group(2)
        plan.append((f, f"{new_n}- {rest}"))

# Show plan, then on confirmation:
for old, new in plan:
    os.rename(os.path.join(folder, old), os.path.join(folder, new))
```
