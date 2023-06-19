using ToDoList.DAL.Implementations;
using ToDoList.DAL.Interfaces;
using ToDoList.GraphQL;

namespace ToDoList
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services.AddSingleton<IToDoItemDAL, ToDoItemDAL>();
			builder.Services.AddSingleton<ICategoryDAL, CategoryDAL>();

			builder.Services.AddGraphQL(x => SchemaBuilder.New()
			.AddServices(x)
			.AddType<CategoryType>()
			.AddType<ToDoItemType>()
			.AddQueryType<Query>()
			.AddMutationType<Mutation>()
			.Create()
			);
			//builder.Services.AddGraphQLServer().AddMutationType<Mutation>().AddQueryType<Query>();




			var app = builder.Build();


			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			//app.UsePlayground(new PlaygroundOptions { QueryPath = "/api", Path = "/playground" });


			app.UseHttpsRedirection();
			app.UseStaticFiles();



			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				app.MapGraphQL();
			});
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=ToDoItem}/{action=Index}/{id?}");


			app.Run();
		}
	}
}
