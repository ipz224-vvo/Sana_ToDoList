using Dapper;
using System.Xml;
using ToDoList.DAL.Models;

namespace ToDoList.DAL.Implementations
{
	public class CategoryDAL
	{
		private static StorageType _storageType;


		public static void ChangeStorageType(string storageType)
		{
			if (storageType == "SQL")
				_storageType = StorageType.SQL;
			else if (storageType == "XML")
				_storageType = StorageType.XML;
		}
		public static async Task<List<Category>> GetCategoriesAsync()
		{
			if (_storageType == StorageType.SQL)
			{
				using var connection = DBConnection.CreateConnection();
				return connection.Query<Category>("SELECT * FROM [Categories]").ToList();
			}
			else if (_storageType == StorageType.XML)
			{
				List<Category> categories = new List<Category>();
				XmlDocument xmlCategoryDocument = new XmlDocument();
				xmlCategoryDocument.Load(DBConnection.GetXMLCategoriesPath());
				if (xmlCategoryDocument != null)
				{
					XmlElement xmlCategoryRoot = xmlCategoryDocument.DocumentElement;
					if (xmlCategoryRoot != null)
					{
						foreach (XmlNode node in xmlCategoryRoot.ChildNodes)
						{
							Category item = new Category();
							foreach (XmlNode childNode in node.ChildNodes)
							{
								if (childNode.Name == "Id")
									item.Id = Convert.ToInt32(childNode.InnerText);

								else if (childNode.Name == "Name")
									item.Name = Convert.ToString(childNode.InnerText);
							}
							categories.Add(item);
						}
					}
				}
				return categories;
			}
			return null;
		}
		public static async Task<Category> GetCategoryByIdAsync(int id)
		{
			if (_storageType == StorageType.SQL)
			{
				using var connection = DBConnection.CreateConnection();
				return connection.QueryFirstOrDefault<Category>("SELECT * FROM [Categories] WHERE Id=@Id", new { Id = id });
			}
			else if (_storageType == StorageType.XML)
			{
				XmlDocument xmlCategoryDocument = new XmlDocument();
				xmlCategoryDocument.Load(DBConnection.GetXMLCategoriesPath());
				XmlElement xCategoryRoot = xmlCategoryDocument.DocumentElement;

				XmlNode foundNode = xCategoryRoot.SelectSingleNode($"Category[Id={id}]");

				Category item = new Category();
				foreach (XmlNode childNode in foundNode.ChildNodes)
				{
					if (childNode.Name == "Id")
						item.Id = Convert.ToInt32(childNode.InnerText);

					else if (childNode.Name == "Name")
						item.Name = Convert.ToString(childNode.InnerText);
				}

				return item;
			}
			return null;
		}
		public static async void AddCategoryAsync(Category category)
		{
			if (_storageType == StorageType.SQL)
			{
				using var connection = DBConnection.CreateConnection();
				connection.Query<Category>("INSERT INTO [Categories] (Name) " +
										   "VALUES (@Name)",
					new
					{
						Name = category.Name,
					});
			}
			else if (_storageType == StorageType.XML)
			{
				XmlDocument xDoc = new XmlDocument();
				xDoc.Load(DBConnection.GetXMLCategoriesPath());
				XmlElement xRoot = xDoc.DocumentElement;

				XmlElement xCategory = xDoc.CreateElement("Category");
				if (category.Id != null)
				{
					XmlElement element = xDoc.CreateElement("Id");
					if (category.Id == 0)
					{
						var categories = await GetCategoriesAsync();
						int maxId = categories.Max(x => x.Id);
						element.InnerText = (maxId + 1).ToString();
					}
					else
						element.InnerText = category.Id.ToString();
					xCategory.AppendChild(element);
				}
				if (category.Name != null)
				{
					XmlElement element = xDoc.CreateElement("Name");
					element.InnerText = category.Name;
					xCategory.AppendChild(element);
				}
				xRoot.AppendChild(xCategory);
				xDoc.Save(DBConnection.GetXMLCategoriesPath());
			}
		}
		public static async void DeleteCategoryAsync(int id)
		{
			if (_storageType == StorageType.SQL)
			{
				using var connection = DBConnection.CreateConnection();
				var ent = connection.QueryFirstOrDefault<ToDoItem>("DELETE FROM [Categories] WHERE Id=@Id", new { Id = id });
			}
			else if (_storageType == StorageType.XML)
			{
				XmlDocument xDoc = new XmlDocument();
				xDoc.Load(DBConnection.GetXMLCategoriesPath());
				XmlElement xRoot = xDoc.DocumentElement;


				xRoot.RemoveChild(xRoot.SelectSingleNode($"Category[Id={id}]"));
				xDoc.Save(DBConnection.GetXMLCategoriesPath());
			}
		}

	}
}
