using System;
using System.IO;
using NUnit.Framework;


namespace FileSystemTransaction.Test
{
	[TestFixture]
	public class DeleteCommandTest
	{
		
		private readonly string FileToDelete = @"c:\temp\delete_me.txt";

		[SetUp]
		public void CreateCenario ()
		{
			File.Create (FileToDelete).Close();
		}
		
		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void ShouldThrowExceptionIfFileDoesntExist ()
		{
			// When
			var command = new DeleteCommand (@"c:\temp\abc.txt");
			command.Execute ();
		}
		
		[Test]
		public void ShouldDeleteFile ()
		{
			// Given
			Assert.IsTrue (File.Exists (FileToDelete));
			
			// When
			var command = new DeleteCommand (FileToDelete);
			command.Execute ();
			
			// Then
			Assert.IsFalse(File.Exists(FileToDelete));
		}
		
		[Test]
		public void ShouldRollbackFileDeletion ()
		{
			// Given
			var command = new DeleteCommand (FileToDelete);
			command.Execute ();
			
			// When
			command.Rollback ();
			
			// Then
			Assert.IsTrue(File.Exists(FileToDelete));
		}
		
		[Test]
		public void ShouldReturnFileContent ()
		{
			// Given
			File.WriteAllText (FileToDelete, "Hello World!");
			var command = new DeleteCommand (FileToDelete);
			command.Execute ();
			
			// When
			command.Rollback ();
			
			// Then
			Assert.AreEqual ("Hello World!", File.ReadAllText (FileToDelete));
		}
		
		[TearDown]
		public void TearDown ()
		{
			if (File.Exists (FileToDelete))
				File.Delete (FileToDelete);
		}
		
	}
}

