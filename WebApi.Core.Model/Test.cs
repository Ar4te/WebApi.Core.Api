﻿using SqlSugar;

namespace WebApi.Core.Model;

[SugarTable("Test")]
public class Test
{
    public Test()
    {

    }
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int TestId { get; set; }

    public string TestName { get; set; }
}
