using System;
using System.IO;

namespace FileSystemTransaction
{
	public class WriteCommand : IFileSystemCommand
	{
		private readonly string FileToWrite;
		private readonly string Content;
		
		public WriteCommand (string fileToWrite, string content)
		{
			FileToWrite = fileToWrite;
			Content = content;
		}
		
		public void Execute ()
		{
			if (!File.Exists(FileToWrite))
				throw new FileNotFoundException ();
				
			File.AppendAllText (FileToWrite, Content);
		}
		
		public void Rollback ()
		{
			var fileStream = File.OpenWrite (FileToWrite);
			
			var length = fileStream.Length;
			fileStream.SetLength (length - Content.Length);
			
			fileStream.Close ();
		}
	}
}

