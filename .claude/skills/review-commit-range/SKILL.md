---
name: review-commit-range
description: Use when the user wants to review code changes between two commits and get a table of remarks. Triggers on "review from X to Y", "review changes between commitA and commitB", or any request to analyze a diff range.
---

# Review Commit Range

Analyze the diff between two commits and produce a table of code review remarks.

## Input

Determine `commitA` and `commitB`:
1. Take them from the user's message (SHA, branch name, or tag).
2. If only one ref is provided, treat `commitA` as the merge-base with `main` and `commitB` as HEAD.
3. If none provided, ask the user.

## Process

```bash
# Show the diff between the two commits
git diff commitA commitB

# List files changed
git diff --name-only commitA commitB

# Show commit log in range (for context)
git log commitA..commitB --oneline
```

Read the diff carefully. For each finding:
- Identify the **file** and **line number** from the diff hunk header (`@@ -old +new @@`).
- Use the **new** (right-side) line number; fall back to the old line number if the line was deleted.
- Write a concise **message** describing the concern.
- Assign a **category** (see below).

## Output

Render a markdown table:

| # | Author | Category | Message | Location |
|---|--------|----------|---------|----------|
| 1 | @claude | question | "Why is this a ConcurrentDictionary?" | `Foo.cs:42` |
| 2 | @claude | potential bug | "This will cause a null ref if X is null." | `Bar.cs:17` |
| 3 | @claude | suggestion | "Consider extracting this to a helper method." | `Baz.cs:88` |

**Author** — always `@claude` (you are the reviewer).

**Category** — read the diff and choose 1–2 words that best capture the remark's nature. No fixed list; infer freely. Good examples: `question`, `potential bug`, `critical bug`, `must fix`, `warning`, `suggestion`, `recommendation`, `cleanup`, `test issue`, `info`.

**Location** — use `` `filename:line` `` for inline remarks. If a remark applies to the whole change (no single line), write `_(change-level)_`.

Include a summary line below the table:
> **N remark(s)** on diff `commitA..commitB`
