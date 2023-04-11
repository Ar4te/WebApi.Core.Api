using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace WebApi.Core.Model.Models
{
    ///<summary>
    ///产线信息表
    ///</summary>
    [SugarTable( "Line")]
[Tenant("{ConnId}")]
    public class Line
    {
           public Line()
           {
           }
           /// <summary>
           /// Desc:ID
           /// Default:
           /// Nullable:False
           /// </summary>
           [SugarColumn(IsPrimaryKey=true)]
           public string bl_Id { get; set; }
           /// <summary>
           /// Desc:产线编号
           /// Default:
           /// Nullable:False
           /// </summary>
           public string bl_No { get; set; }
           /// <summary>
           /// Desc:产线名称
           /// Default:
           /// Nullable:False
           /// </summary>
           public string bl_Name { get; set; }
           /// <summary>
           /// Desc:产线类型
           /// Default:
           /// Nullable:True
           /// </summary>
           public int? bl_Type { get; set; }
           /// <summary>
           /// Desc:创建人ID
           /// Default:
           /// Nullable:True
           /// </summary>
           public string bl_CreateId { get; set; }
           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:True
           /// </summary>
           public DateTime? bl_CreateTime { get; set; }
           /// <summary>
           /// Desc:修改人ID
           /// Default:
           /// Nullable:True
           /// </summary>
           public string bl_ModifyId { get; set; }
           /// <summary>
           /// Desc:修改时间
           /// Default:
           /// Nullable:True
           /// </summary>
           public DateTime? bl_ModifyTime { get; set; }
    }
}