using System;
using System.IO;

namespace FileSystemTransaction
{
	public class DeleteCommand : IFileSystemCommand
	{
		private readonly string FilePath;
		private byte[] fileContent;
		
		public DeleteCommand (string filePath)
		{
			FilePath = filePath;
		}
		
		public void Execute ()
		{
			fileContent = ReadFile (FilePath);

			File.Delete (FilePath);
		}
		
		public void Rollback ()
		{
			var fileStream = File.Create (FilePath);
			fileStream.Write (fileContent, 0, fileContent.Length);
			fileStream.Close ();
		}
		
		private byte[] ReadFile (string filePath)
		{
			return File.ReadAllBytes (filePath);
		}
		
	}
}

