using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using WebApplication1.Pages.ToDo;

namespace Todolist.Pages.ToDo
{
    public class CreateModel : PageModel
    {
        public TodoList todolist = new TodoList();
        public String errorMessage = "";
        public String succMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            todolist.name = Request.Form["name"];
            todolist.task = Request.Form["task"];

            if (todolist.name.Length == 0 || todolist.task.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            //save the data into the database
            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=\"To Do List\";Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO todolist " +
                                    "(name, task) VALUES " +
                                    "(@name, @task);";
                    
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", todolist.name);
                        command.Parameters.AddWithValue("@task", todolist.task);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            todolist.name = ""; todolist.task = "";
            succMessage = "New task added";

            Response.Redirect("/ToDo/Index");
        }
    }
}
