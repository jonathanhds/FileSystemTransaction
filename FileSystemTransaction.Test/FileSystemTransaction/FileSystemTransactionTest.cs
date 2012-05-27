using System;
using System.IO;
using NUnit.Core;
using NUnit.Framework;

namespace FileSystemTransaction.Test
{
	[TestFixture]
	public class FileSystemTransactionTest
	{
		[Test]
		public void ShouldExecuteCommandOnlyWhenCommitIsCalled ()
		{
			var filePath = @"c:\temp\my_file.txt";
			DeleteIfExists (filePath);
			
			// Given
			var transaction = new FileSystemTransaction ();
			
			// When
			transaction.Create (filePath);
			
			// Then
			Assert.IsFalse (File.Exists (filePath));
		}
		
		[Test]
		public void ShouldExecuteCommands ()
		{
			var filePath = @"c:\temp\test\csharp.txt";
			DeleteIfExists (filePath);
			
			// Given
			var transaction = new FileSystemTransaction ();
			
			// When
			transaction.Create (filePath);
			transaction.Write (filePath, "hello");
			transaction.Write (filePath, " world!");
			transaction.Commit ();
			
			// Then
			Assert.IsTrue (File.Exists (filePath));
			Assert.AreEqual ("hello world!", File.ReadAllText (filePath));
		}
		
		[Test]
		public void ShouldRollbackCommands ()
		{
			var filePath = @"c:\temp\test\abc.txt";
			DeleteIfExists (filePath);
			
			var newFilePath = @"c:\temp\test\xyz.txt";
			DeleteIfExists (newFilePath);

			// Given
			var transaction = new FileSystemTransaction ();

			// When
			try {
				transaction.Create (filePath);
				transaction.Move (filePath, newFilePath);
				transaction.Write (filePath, "hi!"); // Should throw exception (file doesn't exist)
				transaction.Commit (); // Apply all commands
			} catch (Exception) {
				transaction.Rollback ();
			}

			// Then
			Assert.IsFalse (File.Exists (filePath));
			Assert.IsFalse (File.Exists (newFilePath));
		}
		
		[Test]
		public void ShouldCommitManyTimesOnSameTransaction ()
		{
			var filePath = @"c:\temp\bar\foo\dummy.txt";
			DeleteIfExists (filePath);
			
			var copyFilePath = @"c:\temp\bar\foo2\dummy.txt";
			DeleteIfExists (copyFilePath);

			// Given
			var transaction = new FileSystemTransaction ();
			
			// When
			transaction.Create (filePath);
			transaction.Commit ();
			transaction.Write (filePath, "foo");
			transaction.Commit ();
			transaction.Copy (filePath, copyFilePath);
			transaction.Commit ();
			
			// Then
			Assert.IsTrue (File.Exists (filePath));
			Assert.AreEqual ("foo", File.ReadAllText (filePath));
			Assert.IsTrue (File.Exists (copyFilePath));
			Assert.AreEqual ("foo", File.ReadAllText (copyFilePath));
		}
		
		[Test]
		public void ShouldRollbackOnlyNotCommitedCommands ()
		{
			var filePath = @"c:\temp\bar\foo\dummy.txt";
			DeleteIfExists (filePath);
			
			// Given
			var transaction = new FileSystemTransaction ();
			
			// When
			try {
				transaction.Create (filePath);
				transaction.Write (filePath, "test");
				transaction.Commit ();
				transaction.Delete (filePath);
				transaction.Write (filePath, "abc");
				transaction.Commit ();
			} catch (Exception) {
				transaction.Rollback ();
			}
			
			// Then
			Assert.IsTrue (File.Exists (filePath));
			Assert.AreEqual ("test", File.ReadAllText (filePath));
		}
		
		[Test]
		public void ShouldCheckIfHasCommandsToExecute ()
		{
			// Given
			var transaction = new FileSystemTransaction ();
			
			// When
			transaction.Create ("foo.txt");
			
			// Then
			Assert.IsTrue (transaction.HasCommands ());
		}
		
		[Test]
		public void ShouldCheckIfHasCommandsToExecuteAfterCommit ()
		{
			// Given
			var transaction = new FileSystemTransaction ();
			
			// When
			transaction.Create (@"c:\temp\my_file.txt");
			transaction.Commit ();
			
			// Then
			Assert.IsFalse (transaction.HasCommands ());
		}
		
		[Test]
		public void ShouldCheckIfHasNoCommandsToExecute ()
		{
			// Given
			var transaction = new FileSystemTransaction ();
			
			// Then
			Assert.IsFalse (transaction.HasCommands ());
		}
		
		#region helpers
		private void DeleteIfExists(string filePath)
		{
			if (File.Exists (filePath))
				File.Delete (filePath);
			
		}
		#endregion
	}
}