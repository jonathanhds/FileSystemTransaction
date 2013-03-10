FileSystemTransaction
=====================

Simple file system transaction library for C#


Usage
--------------

Commit:
```csharp
var fileTransaction = new FileSystemTransaction();
fileTransaction.Create("C:/test"); // Create new file/path
fileTransaction.Commit(); // Commit changes
```

Rollback:
```csharp
var fileTransaction = new FileSystemTransaction();
fileTransaction.Create("C:/test"); // Create new file/path
fileTransaction.Write("C:/test/foo.txt", "Hello World"); // Write to file
fileTransaction.Commit(); // Commit changes
fileTransaction.Move("C:/test/foo.txt", "C:/foo.txt"); // Moves foo.txt file
fileTransaction.Rollback(); // Returns foo.txt file to C:/test directory
```
