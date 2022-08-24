using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using WebApplication1.Pages.ToDo;

namespace Todolist.Pages.ToDo
{
    public class EditModel : PageModel
    {
        public TodoList todolist = new TodoList();
        public String errorMessage = "";
        public String succMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=\"To Do List\";Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM todolist WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                todolist.id = "" + reader.GetInt32(0);
                                todolist.name = reader.GetString(1);
                                todolist.task = reader.GetString(2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            todolist.id = "SELECT id FROM todolist WHERE id = @row_id";
            todolist.name = Request.Form["name"];
            todolist.task = Request.Form["task"];

            if (todolist.name.Length == 0 || todolist.task.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=\"To Do List\";Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE todolist SET name = @name, task = @task WHERE id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", todolist.id);
                        command.Parameters.AddWithValue("@name", todolist.name);
                        command.Parameters.AddWithValue("@task", todolist.task);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/ToDo/Index");
        }
    }
}
