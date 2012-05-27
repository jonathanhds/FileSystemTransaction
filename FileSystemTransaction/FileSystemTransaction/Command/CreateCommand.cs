using System;
using System.IO;

namespace FileSystemTransaction
{
	public class CreateCommand : IFileSystemCommand
	{
		private readonly string FilePath;
		
		public CreateCommand (string filePath)
		{
			FilePath = filePath;
		}
		
		public void Execute ()
		{
			if (File.Exists (FilePath))
				throw new FileAlreadyExistsException ();
			
			Directory.CreateDirectory (Path.GetDirectoryName (FilePath));
			File.Create (FilePath).Close();
		}
		
		public void Rollback ()
		{
			File.Delete (FilePath);
		}
		
	}
}