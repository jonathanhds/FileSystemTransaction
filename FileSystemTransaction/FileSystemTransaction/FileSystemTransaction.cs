using System;
using System.Collections.Generic;

namespace FileSystemTransaction
{
	public class FileSystemTransaction
	{
		private readonly IList<IFileSystemCommand> commands;
		private int transactionStart;
		private int transactionEnd;
		
		public FileSystemTransaction ()
		{
			commands = new List<IFileSystemCommand> ();
			transactionStart = 0;
			transactionEnd = 0;
		}
		
		public void Create (string filePath)
		{
			var command = new CreateCommand (filePath);
			
			Add (command);
		}
		
		public void Move (string source, string target)
		{
			var command = new MoveCommand (source, target);

			Add (command);
		}
		
		public void Copy (string source, string destiny)
		{
			var command = new CopyCommand (source, destiny);
			
			Add (command);
		}
		
		public void Delete (string filePath)
		{
			var command = new DeleteCommand (filePath);
			
			Add (command);
		}
		
		public void Write (string fileToWrite, string content)
		{
			var command = new WriteCommand (fileToWrite, content);
			
			Add (command);
		}
		
		public void Commit ()
		{
			for (var i = transactionStart; i < transactionEnd; i++) {
				commands [i].Execute ();
			}
			
			transactionStart = transactionEnd;
		}
		
		public void Rollback ()
		{
			for (var i = transactionEnd - 2; i >= transactionStart; i--) {
				commands[i].Rollback ();
			}
		}

		public bool HasCommands ()
		{
			return transactionStart != transactionEnd;
		}
		
		private void Add (IFileSystemCommand command)
		{
			commands.Add (command);
			
			transactionEnd++;
		}
	}
}