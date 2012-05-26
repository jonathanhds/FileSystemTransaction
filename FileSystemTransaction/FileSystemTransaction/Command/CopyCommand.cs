using System;
using System.IO;

namespace FileSystemTransaction
{
	public class CopyCommand : IFileSystemCommand
	{
		
		private readonly string Source;
		private readonly string Destiny;
		
		public CopyCommand (string source, string destiny)
		{
			Source = source;
			Destiny = destiny;
		}
		
		public void Execute ()
		{
			Directory.CreateDirectory (Path.GetDirectoryName (Destiny));
			
			File.Copy (Source, Destiny);
		}
		
		public void Rollback ()
		{
			File.Delete (Destiny);
		}
		
	}
}

