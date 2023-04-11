using SqlSugar;
using WebApi.Core.Model.ViewModels;

namespace WebApi.Core.Model.Models;

[SugarTable("User")]
public class User
{
    public User()
    {

    }
    public User(UserVM uvm)
    {
        UserAge = uvm.UserAge;
        UserName = uvm.UserName;
        Password = uvm.Password;
    }
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int UserId { get; set; }

    public string UserName { get; set; }

    public int UserAge { get; set; }

    public string Password { get; set; }
}
