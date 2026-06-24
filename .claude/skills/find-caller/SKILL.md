---
name: find-caller
description: Find the caller tree of a specific C# method in the codebase
---

# Find Caller

Given a C# method, use the LSP `incomingCalls` operation to find all callers recursively and print the call tree. Read-only — never modify any file.

## Requirement

Claude plugin `csharp-lsp`

## Process

1. Use the LSP `prepareCallHierarchy` operation at the given file + line + character to get the call hierarchy item for the target method.
2. Use the LSP `incomingCalls` operation on that item to get direct callers.
3. If LSP returns no results on the first attempt, the `csharp-ls` server may still be indexing the workspace. Wait ~5 seconds and retry up to 5 times before giving up.
4. For each caller found, record its file and line, then recursively call `incomingCalls` on it to find its own callers.
5. Repeat until no new callers are found or a depth limit of 10 is reached.
6. Note any ambiguous cases (overloads, interface implementations) in the output.

## Output format

- Root = user-supplied method; children = callers (4-space indent per level)
- Each entry: `MethodName() - relative/path/to/File.cs:line`

Example:
```txt
GenerateMtlsKvms() - Apif.Platform.Business/Factories/PolicyFactory/Policies/VerifyMtls.cs:65
    BuildKvmsAsync() - Apif.Platform.Business/Factories/PolicyFactory/PolicyFactory.Iac.cs:430
        DeployKvmMapsToApigeeAsync() - Apif.Platform.Business/Features/Deployment/Handlers/DeploymentExecutor.cs:108
    ApplyMtlsCertificates() - Apif.Platform.Business/Factories/PolicyFactory/PolicyFactory.Iac.cs:592
        GenerateKvmFile() - Apif.Platform.Business/Services/Classes/Deployment/DeploymentOrchestrator.cs:229
            OrderDeployApi() - Apif.Platform.Business/Services/Classes/Deployment/DeploymentOrchestrator.cs:170
```

