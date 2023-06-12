using System.ComponentModel.DataAnnotations;

namespace ToDoList.DAL.Models
{
	public class ToDoItem
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Text { get; set; }

		public DateTime? EndDate { get; set; }

		public int CategoryId { get; set; }

		public Category Category { get; set; }

		[Required]
		public bool IsFinished { get; set; }



	}
}
