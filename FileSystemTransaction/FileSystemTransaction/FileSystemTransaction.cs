using System;
using System.Collections.Generic;

namespace FileSystemTransaction
{
	public class FileSystemTransaction
	{
		private readonly IList<IFileSystemCommand> commands;
		
		public FileSystemTransaction()
		{
			commands = new List<IFileSystemCommand>();
		}
		
		public void Create (string filePath)
		{
			var command = new CreateCommand (filePath);
			
			commands.Add (command);
		}
		
		public void Move (string source, string target)
		{
			var command = new MoveCommand(source, target);

			commands.Add(command);
		}
		
		public void Copy()
		{	
		}
		
		public void Delete()
		{
		}
		
		public void Commit()
		{
		}
		
		public void Rollback()
		{	
		}
	}
}