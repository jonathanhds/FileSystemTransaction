using System;
using System.IO;
using NUnit.Core;
using NUnit.Framework;


namespace FileSystemTransaction.Test
{
	[TestFixture]
	public class WriteCommandTest
	{
		
		private readonly string FileToWrite = @"c:\temp\test\write.txt";
		
		[SetUp]
		public void CreateCenario ()
		{
			File.Create (FileToWrite).Close();
		}
		
		[Test]
		public void ShouldWriteToFile ()
		{
			// Given
			Assert.AreEqual("", File.ReadAllText(FileToWrite));
			
			// When
			var command = new WriteCommand (FileToWrite, "foo bar");
			command.Execute ();
			
			// Then
			Assert.AreEqual ("foo bar", File.ReadAllText (FileToWrite));
		}
		
		[Test]
		public void ShouldRollbackFileWrite ()
		{
			// Given
			var command = new WriteCommand (FileToWrite, "test");
			command.Execute ();
			
			// When
			command.Rollback ();
			
			// Then
			Assert.AreEqual ("", File.ReadAllText (FileToWrite));
		}
		
		[Test]
		public void ShouldRollbackFileWriteWithoutChangingPredefinedText ()
		{
			// Given
			File.WriteAllText (FileToWrite, "foo");
			var command = new WriteCommand (FileToWrite, "bar");
			command.Execute ();
			
			// When
			command.Rollback ();
			
			// Then
			Assert.AreEqual ("foo", File.ReadAllText (FileToWrite));
		}

		[TearDown]
		public void TearDown ()
		{
			if (File.Exists (FileToWrite))
				File.Delete (FileToWrite);
		}
		
	}
}

