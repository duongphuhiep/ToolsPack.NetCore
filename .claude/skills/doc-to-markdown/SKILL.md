---
name: doc-to-markdown
description: Convert documents (PDF, DOCX, images, etc.) to Markdown. Use this skill whenever the user wants to convert, parse, extract text from, or transform a document file (PDF, DOCX, DOC, PPTX, XLSX, images) into Markdown format. Trigger even if the user just says "convert this file", "parse this PDF", "extract text from", "turn into markdown", or gives you a file path with a document extension. Always use this skill before attempting any document conversion.
---

# Document to Markdown Conversion

Convert documents to Markdown using the right tool for the job.

## Prerequisites

Both tools must be installed before conversion will work. If a command fails with "not found", direct the user to install the missing tool.

**Method 1 — markitdown** ([docs](https://github.com/microsoft/markitdown)):
```
pipx install 'markitdown[all]'
pipx inject markitdown markitdown-ocr
```

**Method 2 — lit parse** ([docs](https://developers.llamaindex.ai/liteparse/getting_started/)) requires the CLI plus LibreOffice and ImageMagick for [multi-format support](https://developers.llamaindex.ai/liteparse/guides/multi-format/):
```
npm i -g @llamaindex/liteparse
```

LibreOffice:
```
# macOS
brew install --cask libreoffice
# Ubuntu/Debian
apt-get install libreoffice
# Windows
choco install libreoffice-fresh
```

ImageMagick:
```
# macOS
brew install imagemagick
# Ubuntu/Debian
apt-get install imagemagick
# Windows
choco install imagemagick.app
```

## Two Methods

**Method 1 — markitdown** (preferred for text-heavy documents)
```
markitdown --use-plugins "input.pdf" -o "output.md"
```
Best for: PDFs with mostly readable text, DOCX, PPTX, XLSX, HTML — anything where the content is already selectable/copyable text.

**Method 2 — lit parse** (preferred for image/OCR-heavy documents)
```
lit parse --ocr-language <lang> "input.pdf" -o "output.md"
```
Best for: scanned documents, image-heavy PDFs, documents where text extraction would produce garbage, or anything that needs OCR.

## Decision Process

### Step 1 — Estimate the document type

Before converting, look at the first 10–20 pages of the document to judge whether it's text-dominant or image-dominant. Use this heuristic:

- If the document renders as readable, selectable text → Method 1
- If the document is a scan, photo, or mostly diagrams/tables as images → Method 2
- If unsure, lean toward Method 1 and fall back to Method 2 on failure

For DOCX/PPTX/XLSX: always Method 1 (these are structured formats, not scans).

**80% confidence rule**: If you're at least 80% sure about your choice, proceed without asking. If you're genuinely uncertain (50/50), ask the user to choose.

### Step 2 — Choose the OCR language (Method 2 only)

Supported codes: `vie` (Vietnamese), `eng` (English), `fra` (French).

Guess from the file name:
1. Try to interpret the file name as English or French. If it makes sense in either → use `eng` or `fra`.
2. If it doesn't make sense as English or French, it's likely Vietnamese (possibly stripped of accents) → use `vie`.

**Examples:**
- `annual-report-2024.pdf` → English → `eng`
- `rapport-annuel.pdf` → French → `fra`
- `So tay tham phan.pdf` → Doesn't make sense in English/French → Vietnamese → `vie`
- `Luat lao dong.pdf` → Vietnamese → `vie`
- `Guide utilisateur.pdf` → French → `fra`

If you're less than 80% sure about the language, ask the user.

### Step 3 — Determine output path

If the user didn't specify an output path, use the same directory and base name as the input, with `.md` extension:
- `~/Documents/report.pdf` → `~/Documents/report.md`
- `/data/Luat lao dong.docx` → `/data/Luat lao dong.md`

### Step 4 — Run the conversion

Run the chosen command. If it errors, try the other method. If both fail, report the error to the user.

## Quick Reference

| Situation | Choice |
|-----------|--------|
| Regular PDF (mostly text) | Method 1 (markitdown) |
| Scanned PDF / photos | Method 2 (lit parse) |
| DOCX, PPTX, XLSX | Method 1 (markitdown) |
| Vietnamese filename that looks garbled in English | `--ocr-language vie` |
| Method 1 errors out | Retry with Method 2 |
| Both methods fail | Report error to user |
