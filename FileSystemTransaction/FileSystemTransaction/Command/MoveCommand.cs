using System;
using System.IO;


namespace FileSystemTransaction
{
	public class MoveCommand : IFileSystemCommand
	{
		private readonly string Source;
		private readonly string Target;
		
		public MoveCommand (string source, string target)
		{
			Source = source;
			Target = target;
		}
		
		public void Execute ()
		{
			Directory.CreateDirectory (Path.GetDirectoryName (Target));
			File.Move(Source, Target);
		}
		
		public void Rollback ()
		{
			File.Move(Target, Source);
		}
		
	}
}

