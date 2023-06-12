﻿using Dapper;
using System.Xml;
using ToDoList.DAL.Models;

namespace ToDoList.DAL.Implementations
{
	public class ToDoItemDAL
	{
		public static StorageType StorageType;
		public static string DateTimeToString(DateTime? dateTime)
		{
			DateTime temp_date;
			if (dateTime == null) return null;
			temp_date = Convert.ToDateTime(dateTime);

			return temp_date.ToString("yyyy-MM-dd HH:mm");
		}
		public async static Task<ToDoItem> GetToDoItemByIdAsync(int id)
		{
			if (StorageType == StorageType.SQL)
			{
				using var connection = DBConnection.CreateConnection();
				var ent = connection.QueryFirstOrDefault<ToDoItem>("SELECT * FROM [Tasks] WHERE Id=@Id",
					new { Id = id });
				ent.Category = connection.QueryFirstOrDefault<Category>("SELECT * FROM [Categories] WHERE Id=@cat_id",
					new { cat_id = ent.CategoryId });
				return ent;
			}
			else if (StorageType == StorageType.XML)
			{
				XmlDocument xmlToDoDocument = new XmlDocument();
				xmlToDoDocument.Load(DBConnection.GetXMLToDoItemsPath());
				if (xmlToDoDocument == null) return null;
				XmlElement xmlToDoRoot = xmlToDoDocument.DocumentElement;
				if (xmlToDoRoot == null) return null;

				XmlNode foundNode = xmlToDoRoot.SelectSingleNode($"ToDoItem[Id={id}]");
				ToDoItem item = new ToDoItem();
				foreach (XmlNode childNode in foundNode.ChildNodes)
				{
					if (childNode.Name == "Id")
						item.Id = Convert.ToInt32(childNode.InnerText);

					else if (childNode.Name == "Text")
						item.Text = Convert.ToString(childNode.InnerText);

					else if (childNode.Name == "EndDate")
						item.EndDate = Convert.ToDateTime(childNode.InnerText);

					else if (childNode.Name == "CategoryId")
						item.CategoryId = Convert.ToInt32(childNode.InnerText);

					else if (childNode.Name == "IsFinished")
						item.IsFinished = Convert.ToBoolean(childNode.InnerText);
				}
				List<Category> categories = await CategoryDAL.GetCategoriesAsync();
				item.Category = categories.First(x => x.Id == item.CategoryId);
				return item;
			}
			return null;
		}

		public static async Task<List<ToDoItem>> GetToDoItemsAsync()
		{
			if (StorageType == StorageType.SQL)
			{
				using var connection = DBConnection.CreateConnection();
				var entyties = connection.Query<ToDoItem>("SELECT * FROM [Tasks]").ToList();
				foreach (var item in entyties)
				{
					item.Category = connection.QueryFirstOrDefault<Category>(
						"SELECT * FROM [dbo].[Categories] WHERE Id=@cat_id", new { cat_id = item.CategoryId });

				}
				return entyties;
			}
			else if (StorageType == StorageType.XML)
			{
				List<ToDoItem> toDoItems = new List<ToDoItem>();
				XmlDocument xmlToDoDocument = new XmlDocument();
				xmlToDoDocument.Load(DBConnection.GetXMLToDoItemsPath());
				if (xmlToDoDocument == null) return new List<ToDoItem>();
				XmlElement xmlToDoRoot = xmlToDoDocument.DocumentElement;
				if (xmlToDoRoot == null) return new List<ToDoItem>();

				foreach (XmlNode node in xmlToDoRoot.ChildNodes)
				{
					ToDoItem item = new ToDoItem();
					foreach (XmlNode childNode in node.ChildNodes)
					{
						if (childNode.Name == "Id")
							item.Id = Convert.ToInt32(childNode.InnerText);

						else if (childNode.Name == "Text")
							item.Text = Convert.ToString(childNode.InnerText);

						else if (childNode.Name == "EndDate")
							item.EndDate = Convert.ToDateTime(childNode.InnerText);

						else if (childNode.Name == "CategoryId")
							item.CategoryId = Convert.ToInt32(childNode.InnerText);

						else if (childNode.Name == "IsFinished")
							item.IsFinished = Convert.ToBoolean(childNode.InnerText);
					}
					toDoItems.Add(item);
				}
				List<Category> categories = await CategoryDAL.GetCategoriesAsync();
				foreach (ToDoItem toDoItem in toDoItems)
				{
					toDoItem.Category = categories.FirstOrDefault(x => x.Id == toDoItem.CategoryId);
				}

				return toDoItems;
			}

			return null;
		}
		public static async void AddToDoItemAsync(ToDoItem toDoItem)
		{
			if (StorageType == StorageType.SQL)
			{

				using var connection = DBConnection.CreateConnection();
				connection.Query<ToDoItem>("INSERT INTO [Tasks] (Text, EndDate, CategoryId, IsFinished) " +
										   "VALUES (@Text, @EndDate, @CategoryId,@IsFinished)",
					new
					{
						Text = toDoItem.Text,
						EndDate = toDoItem.EndDate,
						CategoryId = toDoItem.CategoryId,
						IsFinished = toDoItem.IsFinished
					});
			}
			else if (StorageType == StorageType.XML)
			{

				XmlDocument xDoc = new XmlDocument();
				xDoc.Load(DBConnection.GetXMLToDoItemsPath());
				XmlElement xRoot = xDoc.DocumentElement;

				XmlElement xToDoItem = xDoc.CreateElement("ToDoItem");
				if (toDoItem.Id != null)
				{
					XmlElement element = xDoc.CreateElement("Id");

					if (toDoItem.Id == 0)
					{
						var toDoItems = await GetToDoItemsAsync();
						int maxId = toDoItems.Max(x => x.Id);
						element.InnerText = (maxId + 1).ToString();
					}
					else
						element.InnerText = toDoItem.Id.ToString();
					xToDoItem.AppendChild(element);
				}
				if (toDoItem.Text != null)
				{
					XmlElement element = xDoc.CreateElement("Text");
					element.InnerText = toDoItem.Text;
					xToDoItem.AppendChild(element);
				}
				if (toDoItem.EndDate != null)
				{
					XmlElement element = xDoc.CreateElement("EndDate");
					element.InnerText = toDoItem.EndDate.ToString();
					xToDoItem.AppendChild(element);
				}
				if (toDoItem.CategoryId != null)
				{
					XmlElement element = xDoc.CreateElement("CategoryId");
					element.InnerText = toDoItem.CategoryId.ToString();
					xToDoItem.AppendChild(element);
				}
				if (toDoItem.IsFinished != null)
				{
					XmlElement element = xDoc.CreateElement("IsFinished");
					element.InnerText = toDoItem.IsFinished.ToString();
					xToDoItem.AppendChild(element);
				}
				xRoot.AppendChild(xToDoItem);
				xDoc.Save(DBConnection.GetXMLToDoItemsPath());
			}
		}
		public static async void EditToDoItemAsync(ToDoItem toDoItem)
		{
			if (StorageType == StorageType.SQL)
			{

				using var connection = DBConnection.CreateConnection();
				connection.Query<ToDoItem>("UPDATE [Tasks] " +
										   "SET Text = @Text, EndDate = @EndDate, CategoryId = @CategoryId, IsFinished = @IsFinished " +
										   "Where Id = @Id",
					new
					{
						Text = toDoItem.Text,
						EndDate = toDoItem.EndDate,
						CategoryId = toDoItem.CategoryId,
						IsFinished = toDoItem.IsFinished,
						Id = toDoItem.Id
					});
			}
			else if (StorageType == StorageType.XML)
			{
				DeleteToDoItemAsync(toDoItem.Id);
				AddToDoItemAsync(toDoItem);
			}
		}
		public static async void DeleteToDoItemAsync(int id)
		{
			if (StorageType == StorageType.SQL)
			{
				using var connection = DBConnection.CreateConnection();
				var ent = connection.QueryFirstOrDefault<ToDoItem>("DELETE FROM [Tasks] WHERE Id=@Id", new { Id = id });
			}
			else if (StorageType == StorageType.XML)
			{
				XmlDocument xDoc = new XmlDocument();
				xDoc.Load(DBConnection.GetXMLToDoItemsPath());
				XmlElement xRoot = xDoc.DocumentElement;

				xRoot.RemoveChild(xRoot.SelectSingleNode($"ToDoItem[Id={id}]"));
				xDoc.Save(DBConnection.GetXMLToDoItemsPath());
			}
		}

	}
}
