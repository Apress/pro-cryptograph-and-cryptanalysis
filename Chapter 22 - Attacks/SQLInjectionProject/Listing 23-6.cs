using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Login : System.Web.UI.Page
{
    DataTable data_table;
    SqlDataAdapter adapter;
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void MyLogin_Authenticate(object sender, 
AuthenticateEventArgs e)
    {
        SqlConnection connection = new SqlConnection(@"Data 
Source=SERVER_NAME;Initial Catalog=Apress_ProCrypto_SQLInjectionDB;Integrated Security=True");
        string query="select * from LoginUserData where 
Email='"+ MyLogin.UserName+"'and Password='"+ 
MyLogin.Password+"' ";
        adapter = new SqlDataAdapter(query, connection);
        data_table = new DataTable();
        adapter.Fill(data_table);
        if (data_table.Rows.Count >= 1)
        {
            Response.Redirect("index.aspx");
        }
    }
}
