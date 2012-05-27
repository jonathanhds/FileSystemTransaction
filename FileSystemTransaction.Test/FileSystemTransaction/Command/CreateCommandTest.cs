using System;
using System.IO;
using NUnit.Core;
using NUnit.Framework;

namespace FileSystemTransaction.Test
{
	[TestFixture]
	public class CreateCommandTest
	{
		private readonly string FilePath = @"c:\temp\test_sample\foo.txt";
		
		[Test]
		[ExpectedException(typeof(FileAlreadyExistsException))]
		public void ShouldThrowExceptionIFileAlreadyExists ()
		{
			// Given
			File.Create (FilePath).Close ();
			
			// When
			var command = new CreateCommand (FilePath);
			command.Execute ();
		}
		
		[Test]
		public void ShouldCreateFile()
		{
			// When
			var command = new CreateCommand (FilePath);
			command.Execute ();
			
			// Then
			Assert.IsTrue (File.Exists (FilePath));
		}
		
		[Test]
		public void ShouldRollbackFileCreation ()
		{
			// Given
			var command = new CreateCommand (FilePath);
			command.Execute ();
			
			// When
			command.Rollback ();
			
			// Then
			Assert.IsFalse (File.Exists (FilePath));
		}

		[TearDown]
		[SetUp]
		public void ClearCenario ()
		{
			if (File.Exists (FilePath))
			{
				File.Delete (FilePath);
			}
		}
		
	}
}