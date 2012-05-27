using System;
using System.IO;
using NUnit.Core;
using NUnit.Framework;


namespace FileSystemTransaction.Test
{
	[TestFixture]
	public class MoveCommandTest
	{
		private readonly string OriginFilePath = @"c:\temp\bar.txt";
		private readonly string DestinyFilePath = @"c:\temp\test\bar.txt";
		
		[SetUp]
		public void CreateCenario ()
		{
			File.Create (OriginFilePath).Close();
		}
		
		[Test]
		public void ShouldMoveFile ()
		{
			// Given
			Assert.IsTrue (File.Exists (OriginFilePath));
			Assert.IsFalse (File.Exists (DestinyFilePath));
			
			// When
			var command = new MoveCommand (OriginFilePath, DestinyFilePath);
			command.Execute ();
			
			// Then
			Assert.IsFalse (File.Exists (OriginFilePath));
			Assert.IsTrue (Directory.Exists (Path.GetDirectoryName (DestinyFilePath)));
			Assert.IsTrue (File.Exists (DestinyFilePath));
		}
		
		[Test]
		public void ShouldRollbackFileMove ()
		{
			// Given
			var command = new MoveCommand (OriginFilePath, DestinyFilePath);
			command.Execute ();
			
			// When
			command.Rollback ();
			
			// Then
			Assert.IsTrue (File.Exists (OriginFilePath));
			Assert.IsFalse (File.Exists (DestinyFilePath));
		}
		
		[TearDown]
		public void ClearCenario ()
		{
			if (File.Exists (OriginFilePath)) {
				File.Delete (OriginFilePath);
			}
			
			if (File.Exists (DestinyFilePath)) {
				File.Delete (DestinyFilePath);	
			}
		}
	}
}