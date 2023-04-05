using SqlSugar;

namespace WebApi.Core.Model
{
    [SugarTable("user")]
    public class User
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int UserAge { get; set; }
    }
}
