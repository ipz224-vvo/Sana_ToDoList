using Microsoft.Build.Framework;

namespace ToDoList.DAL.Models
{
	public class Category
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public override string ToString()
		{
			return $"Name";
		}

	}
}
