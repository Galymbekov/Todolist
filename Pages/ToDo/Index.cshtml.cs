using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace WebApplication1.Pages.ToDo
{
    public class IndexModel : PageModel
    {
        public List<TodoList> Todotasks = new List<TodoList>();

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=\"To Do List\";Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM todolist";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TodoList todoList = new TodoList
                                {
                                    id = "" + reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    task = reader.GetString(2),
                                    created = reader.GetDateTime(3).ToString()
                                };

                                Todotasks.Add(todoList);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class TodoList
    {
        public String id;
        public String name;
        public String task;
        public String created;
    }
}
