using SqlSugar;

namespace WebApi.Core.Model;

[SugarTable("Test2")]
public class Test2
{
    public Test2()
    {

    }
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int TestId { get; set; }

    public string TestName { get; set; }
}
