namespace ToDoList.DAL.Models
{
	public class ToDoItem
	{
		public int Id { get; set; }

		public string Text { get; set; }

		public DateTime? Due_Date { get; set; }

		public int CategoryId { get; set; }

		public Category Category { get; set; }

		public DateTime? Set_Date { get; set; }

		public bool Is_Finished { get; set; }



	}
}
