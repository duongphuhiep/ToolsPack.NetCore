---
name: display-pr-remarks
description: Use when the user wants to see unresolved PR comments, review remarks, or outstanding feedback on a pull request.
---

# Display PR Remarks

List all unresolved comments on a pull request as a table.

## Input

Determine the PR number in this order:
1. Infer from the current conversation context.
2. Parse from the current branch name (e.g. `APIF-1234/some-feature` → search for a PR on that branch).
3. If ambiguous, ask the user.

## Process

```bash
# Find PR number from current branch
gh pr list --head "$(git rev-parse --abbrev-ref HEAD)" --json number,url --jq '.[0]'

# Fetch all review comments + issue comments
gh pr view <PR> --json reviews,comments

# Or fetch inline comments
gh api repos/:owner/:repo/pulls/<PR>/comments
gh api repos/:owner/:repo/issues/<PR>/comments
```

Use `gh api` to get:
- **Pull request review comments** (inline, on diff): `GET /repos/{owner}/{repo}/pulls/{pull_number}/comments`
  - These include `path` (file) and `line` (or `original_line`) fields.
- **Issue-level comments** (general): `GET /repos/{owner}/{repo}/issues/{pull_number}/comments`
  - These have no file/line (they are PR-level); leave Location blank.
- **Review threads with resolution state**: `gh pr view <PR> --json reviewThreads`

Filter only **unresolved** threads (`isResolved: false`). For comments not in a thread, include all.

## Output

Render a markdown table:

| # | Author | Category | Message | Location |
|---|--------|----------|---------|----------|
| 1 | @alice | question | "Why is this a ConcurrentDictionary?" | `Foo.cs:42` |
| 2 | @bob | potential bug | "This will cause a null ref in production..." | `Bar.cs:17` |
| 3 | @carol | suggestion | "Consider extracting this to a helper method." | _(PR-level)_ |

**Category** — read the comment body and choose 1–2 words that best capture its nature. There is no fixed list; infer freely. Examples of what good values look like: `question`, `potential bug`, `critical bug`, `must fix`, `warning`, `suggestion`, `recommendation`, `cleanup`, `test issue`, `info` — but any concise label that accurately reflects the comment is valid.

**Location** — for inline diff comments use `` `filename:line` `` (use `line` field; fall back to `original_line`). For PR-level issue comments write `_(PR-level)_`.

Include a summary line below the table:
> **N unresolved comment(s)** on PR #NNN — `<PR title>`
