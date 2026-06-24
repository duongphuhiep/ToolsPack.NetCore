---
name: answer-review-remark
description: Use when the user wants to respond to a code review remark by adding a clarifying comment to the source code — so the reviewer understands the reasoning and won't raise the same remark again. Triggers on "answer remark #N", "add comment to address reviewer concern", "document why we did X", or any request to resolve a review finding through in-code explanation rather than a code change.
---

# Answer Review Remark

Resolve a code review remark by inserting a clarifying comment at the right place in the source, capturing the reasoning so future reviewers won't raise the same concern.

## Inputs

Infer both inputs from the conversation context. Ask only if ambiguous.

**Input 1 — The remark.**
A reviewer finding: usually a table row with a location (`file:line`) and a description of the concern.

**Input 2 — The explanation.**
The developer's response: why the code is correct as-is, the design decision behind it, the trade-off accepted, or the big-picture context the reviewer was missing. This may not directly rebut the remark — it may just shift the reviewer's perspective.

## Process

1. Read the file at the location from Input 1.
2. Identify the exact construct the remark targets (method, block, field, line).
3. Draft a comment that:
   - Captures the essence of **Input 2** (the reasoning / decision / context).
   - Is concise — one to four lines max.
   - Does **not** mention the reviewer, the PR, or the remark number (comments outlive reviews).
   - Does **not** repeat what the code already says — only add the WHY that isn't obvious from reading.
   - Addresses the remark's concern if doing so makes the explanation clearer, but this is optional.
4. Place the comment immediately above the targeted construct, or inline if a trailing comment fits naturally.
5. Apply the edit.

## Output

Confirm the file and line where the comment was added. One sentence is enough.

## What NOT to do

- Do not rewrite the code unless the user explicitly asks.
- Do not write a comment that merely restates what the code does.
- Do not reference "the reviewer said…" or "PR #N" inside the comment.
- Do not add a comment longer than ~4 lines; if the explanation needs more, it belongs in a design doc or commit message, not inline.
