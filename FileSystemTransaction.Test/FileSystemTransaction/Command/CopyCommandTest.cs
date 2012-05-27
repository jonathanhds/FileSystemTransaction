using System;
using NUnit.Core;
using NUnit.Framework;
using System.IO;


namespace FileSystemTransaction.Test
{
	[TestFixture]
	public class CopyCommandTest
	{
		
		private readonly string FilePath = @"c:\temp\hello.txt";
		private readonly string DestinyFilePath = @"c:\temp\foo\bar\hello.txt";
		
		[SetUp]
		public void CreateCenario ()
		{
			File.Create (FilePath).Close();
		}
		
		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void ShouldThrowExceptionIfFileDoesntExist ()
		{
			// When
			var command = new CopyCommand (@"c:\temp\abc.txt", DestinyFilePath);
			command.Execute ();
		}
		
		[Test]
		public void ShouldCopyFile ()
		{
			// Given
			Assert.IsTrue (File.Exists (FilePath));
			Assert.IsFalse (File.Exists (DestinyFilePath));
			
			// When
			var command = new CopyCommand (FilePath, DestinyFilePath);
			command.Execute ();
			
			// Then
			Assert.IsTrue (File.Exists (FilePath));
			Assert.IsTrue (File.Exists (DestinyFilePath));
		}
		
		[Test]
		public void ShouldRollbackFileCopy ()
		{
			// Given
			var command = new CopyCommand (FilePath, DestinyFilePath);
			command.Execute ();
			
			// When
			command.Rollback ();
			
			// Then
			Assert.IsTrue (File.Exists (FilePath));
			Assert.IsFalse (File.Exists (DestinyFilePath));
		}
		
		[TearDown]
		public void TearDown ()
		{
			if (File.Exists (FilePath))
				File.Delete (FilePath);
			
			if (File.Exists(DestinyFilePath))
				File.Delete(DestinyFilePath);
		}
	}
}

